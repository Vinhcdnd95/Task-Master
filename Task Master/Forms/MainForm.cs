using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Task_Master.Data;

namespace Task_Master
{
    public partial class MainForm : Form
    {
        private FlowLayoutPanel boardPanel;
        private Button addListButton;
        private List<Panel> listPanels = new List<Panel>();
        private Panel draggedTaskPanel = null;

        public MainForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            int boardId = 1;
            LoadLists(boardId);
        }

        private void LoadLists(int boardId)
        {
            try
            {
                DataTable lists = DatabaseHelper.GetLists(boardId);
                boardPanel.Controls.Clear();

                foreach (DataRow row in lists.Rows)
                {
                    int listId = Convert.ToInt32(row["Id"]);
                    string listName = row["Name"].ToString();
                    Panel listPanel = CreateListPanel(listName, listId);
                    boardPanel.Controls.Add(listPanel);
                }

                boardPanel.Controls.Add(addListButton);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupUI()
        {
            this.Text = "Task Master";
            this.Size = new Size(1500, 900);

            boardPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                HorizontalScroll = { Enabled = false, Visible = false },
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            this.Controls.Add(boardPanel);

            addListButton = new Button()
            {
                Text = "+ Thêm danh sách",
                Width = 170,
                Height = 50
            };
            addListButton.Click += AddListButton_Click;
            boardPanel.Controls.Add(addListButton);
        }

        private void AddListButton_Click(object sender, EventArgs e)
        {
            try
            {
                int newListId = DatabaseHelper.InsertList(1, "New List");
                Panel listPanel = CreateListPanel("New List", newListId);
                boardPanel.Controls.Add(listPanel);
                boardPanel.Controls.SetChildIndex(addListButton, boardPanel.Controls.Count - 1);
                listPanels.Add(listPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm danh sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CreateListPanel(string title, int listId = -1)
        {
            Panel panel = new Panel()
            {
                Width = 350,
                Height = 790,
                BackColor = Color.LightGray,
                Padding = new Padding(5),
                Tag = listId
            };

            TextBox titleBox = new TextBox()
            {
                Text = title,
                Width = 240,
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                Location = new Point(10, 5)
            };
            panel.Controls.Add(titleBox);

            Button deleteButton = new Button()
            {
                Text = "X",
                Width = 40,
                Height = 40,
                Location = new Point(panel.Width - 50, 5),
                BackColor = Color.Red,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            deleteButton.Click += (s, e) =>
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa danh sách này?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int listIdToDelete = (int)panel.Tag;
                        DatabaseHelper.DeleteList(listIdToDelete);
                        boardPanel.Controls.Remove(panel);
                        listPanels.Remove(panel);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa danh sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };
            panel.Controls.Add(deleteButton);

            FlowLayoutPanel taskPanel = new FlowLayoutPanel()
            {
                Location = new Point(5, 50),
                Width = 300,
                Height = 600,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AllowDrop = true
            };
            taskPanel.DragEnter += TaskPanel_DragEnter;
            taskPanel.DragOver += TaskPanel_DragOver;
            taskPanel.DragDrop += TaskPanel_DragDrop;
            panel.Controls.Add(taskPanel);

            if (listId != -1)
            {
                try
                {
                    DataTable tasks = DatabaseHelper.GetTasks(listId);
                    foreach (DataRow row in tasks.Rows)
                    {
                        int taskId = Convert.ToInt32(row["id"]);
                        string taskNameText = row["name"] != DBNull.Value ? row["name"].ToString() : "";
                        string taskDescriptionText = row["description"] != DBNull.Value ? row["description"].ToString() : "";
                        bool isActived = row["is_actived"] != DBNull.Value ? Convert.ToBoolean(row["is_actived"]) : false;
                        DateTime deadline = row["deadline"] != DBNull.Value ? Convert.ToDateTime(row["deadline"]) : DateTime.Now;

                        Panel taskContainer = CreateTaskPanel(taskId, taskNameText, taskDescriptionText, isActived, deadline);
                        taskPanel.Controls.Add(taskContainer);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải task: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Button addTaskButton = new Button()
            {
                Text = "+ Thêm Task",
                Width = 120,
                Height = 40,
                Location = new Point(10, 740),
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            addTaskButton.Click += (s, e) =>
            {
                Panel taskContainer = new Panel()
                {
                    Width = 290,
                    Height = 180,
                    Margin = new Padding(5),
                    Tag = -1,
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label taskNameLabel = new Label()
                {
                    Text = "Nội dung:",
                    Location = new Point(0, 0),
                    Width = 220
                };

                TextBox taskName = new TextBox()
                {
                    Text = "",
                    Width = 220,
                    Location = new Point(0, 20),
                    MaxLength = 50
                };

                Label taskDescriptionLabel = new Label()
                {
                    Text = "Người thực hiện:",
                    Location = new Point(0, 50),
                    Width = 220
                };

                TextBox taskDescription = new TextBox()
                {
                    Text = "",
                    Width = 220,
                    Location = new Point(0, 70),
                    MaxLength = 200
                };

                CheckBox isActiveCheckbox = new CheckBox()
                {
                    Text = "Active",
                    Location = new Point(230, 5),
                    Checked = true
                };

                Label deadlineLabel = new Label()
                {
                    Text = "Thời hạn:",
                    Location = new Point(0, 100),
                    Width = 220
                };

                DateTimePicker deadlinePicker = new DateTimePicker()
                {
                    Location = new Point(0, 120),
                    Width = 260,
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy HH:mm"
                };

                Button saveButton = new Button()
                {
                    Text = "Lưu",
                    Width = 60,
                    Height = 25,
                    Location = new Point(0, 150),
                    BackColor = Color.Blue,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                saveButton.Click += (senderSave, eSave) =>
                {
                    int taskId = (int)taskContainer.Tag;
                    if (taskId == -1)
                    {
                        try
                        {
                            taskId = DatabaseHelper.InsertTask(listId, taskName.Text, taskDescription.Text, isActiveCheckbox.Checked, deadlinePicker.Value);
                            taskContainer.Tag = taskId;
                            MessageBox.Show("Task đã thêm: " + taskName.Text, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Panel newTaskContainer = CreateTaskPanel(taskId, taskName.Text, taskDescription.Text, isActiveCheckbox.Checked, deadlinePicker.Value);
                            taskPanel.Controls.Add(newTaskContainer);

                            taskName.Text = "";
                            taskDescription.Text = "";
                            deadlinePicker.Value = DateTime.Now;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi thêm task: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                };

                taskContainer.Controls.Add(taskNameLabel);
                taskContainer.Controls.Add(taskName);
                taskContainer.Controls.Add(taskDescriptionLabel);
                taskContainer.Controls.Add(taskDescription);
                taskContainer.Controls.Add(isActiveCheckbox);
                taskContainer.Controls.Add(deadlineLabel);
                taskContainer.Controls.Add(deadlinePicker);
                taskContainer.Controls.Add(saveButton);
                taskPanel.Controls.Add(taskContainer);
            };
            panel.Controls.Add(addTaskButton);

            return panel;
        }

        private Panel CreateTaskPanel(int taskId, string taskNameText, string taskDescriptionText, bool isActived, DateTime deadline)
        {
            Panel taskContainer = new Panel()
            {
                Width = 290,
                Height = 180,
                Margin = new Padding(5),
                Tag = taskId,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = isActived ? Color.LightGreen : Color.LightGray
            };

            Label taskNameLabel = new Label()
            {
                Text = "Nội dung:",
                Location = new Point(0, 0),
                Width = 220
            };

            TextBox taskName = new TextBox()
            {
                Text = taskNameText,
                Width = 220,
                Location = new Point(0, 20),
                MaxLength = 50,
                ReadOnly = true
            };

            Label taskDescriptionLabel = new Label()
            {
                Text = "Người thực hiện:",
                Location = new Point(0, 50),
                Width = 220
            };

            TextBox taskDescription = new TextBox()
            {
                Text = taskDescriptionText,
                Width = 220,
                Location = new Point(0, 70),
                MaxLength = 200,
                ReadOnly = true
            };

            CheckBox isActiveCheckbox = new CheckBox()
            {
                Text = "Active",
                Location = new Point(230, 5),
                Checked = isActived
            };
            isActiveCheckbox.CheckedChanged += (sender, e) =>
            {
                try
                {
                    DatabaseHelper.UpdateTask(taskId, taskName.Text, taskDescription.Text, isActiveCheckbox.Checked, deadline);
                    taskContainer.BackColor = isActiveCheckbox.Checked ? Color.LightGreen : Color.LightGray;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật trạng thái task: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            Label deadlineLabel = new Label()
            {
                Text = "Thời hạn:",
                Location = new Point(0, 100),
                Width = 220
            };

            DateTimePicker deadlinePicker = new DateTimePicker()
            {
                Location = new Point(0, 120),
                Width = 260,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm",
                Value = deadline,
                Enabled = false
            };

            Button editButton = new Button()
            {
                Text = "Sửa",
                Width = 60,
                Height = 25,
                Location = new Point(0, 150),
                BackColor = Color.Blue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            Button cancelButton = new Button()
            {
                Text = "Hủy",
                Width = 60,
                Height = 25,
                Location = new Point(70, 150),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Visible = false
            };

            Button deleteTaskButton = new Button()
            {
                Text = "X",
                Width = 60,
                Height = 25,
                Location = new Point(140, 150),
                BackColor = Color.Red,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            editButton.Click += (s, e) =>
            {
                if (editButton.Text == "Sửa")
                {
                    taskName.ReadOnly = false;
                    taskDescription.ReadOnly = false;
                    deadlinePicker.Enabled = true;
                    editButton.Text = "Lưu";
                    cancelButton.Visible = true;
                    deleteTaskButton.Visible = false; // Ẩn nút xóa khi đang sửa
                }
                else
                {
                    try
                    {
                        DatabaseHelper.UpdateTask(taskId, taskName.Text, taskDescription.Text, isActiveCheckbox.Checked, deadlinePicker.Value);
                        taskName.ReadOnly = true;
                        taskDescription.ReadOnly = true;
                        deadlinePicker.Enabled = false;
                        editButton.Text = "Sửa";
                        cancelButton.Visible = false;
                        deleteTaskButton.Visible = true;
                        MessageBox.Show("Task đã được cập nhật!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật task: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            cancelButton.Click += (s, e) =>
            {
                taskName.Text = taskNameText;
                taskDescription.Text = taskDescriptionText;
                deadlinePicker.Value = deadline;
                taskName.ReadOnly = true;
                taskDescription.ReadOnly = true;
                deadlinePicker.Enabled = false;
                editButton.Text = "Sửa";
                cancelButton.Visible = false;
                deleteTaskButton.Visible = true;
            };

            deleteTaskButton.Click += (senderDelete, eDelete) =>
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa task này?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        DatabaseHelper.DeleteTask(taskId);
                        ((FlowLayoutPanel)taskContainer.Parent).Controls.Remove(taskContainer);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa task: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            taskContainer.Controls.Add(taskNameLabel);
            taskContainer.Controls.Add(taskName);
            taskContainer.Controls.Add(taskDescriptionLabel);
            taskContainer.Controls.Add(taskDescription);
            taskContainer.Controls.Add(isActiveCheckbox);
            taskContainer.Controls.Add(deadlineLabel);
            taskContainer.Controls.Add(deadlinePicker);
            taskContainer.Controls.Add(editButton);
            taskContainer.Controls.Add(cancelButton);
            taskContainer.Controls.Add(deleteTaskButton);

            taskContainer.MouseDown += TaskContainer_MouseDown;
            taskContainer.MouseMove += TaskContainer_MouseMove;

            return taskContainer;
        }

        private void TaskContainer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                draggedTaskPanel = (Panel)sender;
                draggedTaskPanel.DoDragDrop(draggedTaskPanel, DragDropEffects.Move);
            }
        }

        private void TaskContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && draggedTaskPanel != null)
            {
                draggedTaskPanel.DoDragDrop(draggedTaskPanel, DragDropEffects.Move);
            }
        }

        private void TaskPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Panel)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void TaskPanel_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Panel)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void TaskPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Panel)))
            {
                Panel taskPanel = (Panel)e.Data.GetData(typeof(Panel));
                FlowLayoutPanel targetPanel = (FlowLayoutPanel)sender;
                Point dropPoint = targetPanel.PointToClient(new Point(e.X, e.Y));
                int index = targetPanel.Controls.Count;

                foreach (Control control in targetPanel.Controls)
                {
                    if (dropPoint.Y < control.Bottom)
                    {
                        index = targetPanel.Controls.GetChildIndex(control);
                        break;
                    }
                }

                if (taskPanel.Parent != targetPanel)
                {
                    try
                    {
                        int taskId = (int)taskPanel.Tag;
                        int newListId = (int)targetPanel.Parent.Tag;
                        DatabaseHelper.MoveTaskToList(taskId, newListId);
                        taskPanel.Parent.Controls.Remove(taskPanel);
                        targetPanel.Controls.Add(taskPanel);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi di chuyển task: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                targetPanel.Controls.SetChildIndex(taskPanel, index);
            }
        }
    }
}