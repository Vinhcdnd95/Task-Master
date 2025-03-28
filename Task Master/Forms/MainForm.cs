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

        private void SetupUI()
        {
            this.Text = "Task Master";
            this.Size = new Size(1200, 700); // Tăng kích thước của form

            boardPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
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
            int newListId = DatabaseHelper.InsertList(1, "New List");
            Panel listPanel = CreateListPanel("New List", newListId);
            boardPanel.Controls.Add(listPanel);
            boardPanel.Controls.SetChildIndex(addListButton, boardPanel.Controls.Count - 1);
            listPanels.Add(listPanel);
        }

        private Panel CreateListPanel(string title, int listId = -1)
        {
            Panel panel = new Panel()
            {
                Width = 300, // Tăng chiều rộng của list
                Height = 625, // Tăng chiều cao của list
                BackColor = Color.LightGray,
                Padding = new Padding(5),
                Tag = listId // Lưu listId vào Tag
            };

            // Tiêu đề danh sách
            TextBox titleBox = new TextBox()
            {
                Text = title,
                Width = 240, // Tăng chiều rộng của TextBox
                Font = new Font("Arial", 12, FontStyle.Bold), // Tăng kích thước font
                TextAlign = HorizontalAlignment.Center,
                Location = new Point(10, 5)
            };
            panel.Controls.Add(titleBox);

            // Nút xóa danh sách
            Button deleteButton = new Button()
            {
                Text = "X",
                Width = 40, // Tăng chiều rộng của Button
                Height = 40, // Tăng chiều cao của Button
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
                    int listIdToDelete = (int)panel.Tag;
                    DatabaseHelper.DeleteList(listIdToDelete); // Xóa khỏi DB
                    boardPanel.Controls.Remove(panel); // Xóa khỏi giao diện
                    listPanels.Remove(panel);
                }
            };
            panel.Controls.Add(deleteButton);

            // FlowLayoutPanel để chứa các task
            FlowLayoutPanel taskPanel = new FlowLayoutPanel()
            {
                Location = new Point(10, 50), // Điều chỉnh vị trí
                Width = 280, // Tăng chiều rộng của FlowLayoutPanel
                Height = 380, // Tăng chiều cao của FlowLayoutPanel
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AllowDrop = true
            };
            taskPanel.DragEnter += TaskPanel_DragEnter;
            taskPanel.DragDrop += TaskPanel_DragDrop;
            panel.Controls.Add(taskPanel);

            // Tải các task đã lưu từ cơ sở dữ liệu
            if (listId != -1) // Chỉ tải nếu listId hợp lệ
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

            // Nút thêm task
            Button addTaskButton = new Button()
            {
                Text = "+ Add Task",
                Width = 120, // Tăng chiều rộng của Button
                Height = 40, // Tăng chiều cao của Button
                Location = new Point(10, 575), // Điều chỉnh vị trí
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            addTaskButton.Click += (s, e) =>
            {
                Panel taskContainer = new Panel()
                {
                    Width = 260, // Tăng chiều rộng của taskContainer
                    Height = 150, // Tăng chiều cao để chứa thêm DateTimePicker và nút
                    Margin = new Padding(5),
                    Tag = -1 // Chưa có trong DB
                };

                TextBox taskName = new TextBox()
                {
                    Text = "",
                    Width = 220, // Tăng chiều rộng của TextBox
                    Location = new Point(0, 5),
                    MaxLength = 50
                };

                TextBox taskDescription = new TextBox()
                {
                    Text = "",
                    Width = 220, // Tăng chiều rộng của TextBox
                    Location = new Point(0, 35),
                    MaxLength = 200
                };

                CheckBox isActiveCheckbox = new CheckBox()
                {
                    Text = "Active",
                    Location = new Point(230, 5),
                    Checked = true // Default new task to active
                };

                DateTimePicker deadlinePicker = new DateTimePicker()
                {
                    Location = new Point(0, 65),
                    Width = 260,
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy HH:mm"
                };

                Button saveButton = new Button()
                {
                    Text = "Save",
                    Width = 60,
                    Height = 25,
                    Location = new Point(0, 100),
                    BackColor = Color.Blue,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                saveButton.Click += (senderSave, eSave) =>
                {
                    int taskId = (int)taskContainer.Tag;
                    if (taskId == -1) // Chỉ thêm mới
                    {
                        taskId = DatabaseHelper.InsertTask(listId, taskName.Text, taskDescription.Text, isActiveCheckbox.Checked, deadlinePicker.Value);
                        taskContainer.Tag = taskId;
                        MessageBox.Show("Task added: " + taskName.Text, "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Tạo task mới và thêm vào taskPanel
                        Panel newTaskContainer = CreateTaskPanel(taskId, taskName.Text, taskDescription.Text, isActiveCheckbox.Checked, deadlinePicker.Value);
                        taskPanel.Controls.Add(newTaskContainer);

                        // Xóa nội dung trong các TextBox
                        taskName.Text = "";
                        taskDescription.Text = "";
                        deadlinePicker.Value = DateTime.Now;
                    }
                };

                taskContainer.Controls.Add(taskName);
                taskContainer.Controls.Add(taskDescription);
                taskContainer.Controls.Add(isActiveCheckbox);
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
                Width = 260, // Tăng chiều rộng của taskContainer
                Height = 150, // Tăng chiều cao để chứa thêm DateTimePicker và nút
                Margin = new Padding(5),
                Tag = taskId // Lưu taskId
            };

            TextBox taskName = new TextBox()
            {
                Text = taskNameText,
                Width = 220, // Tăng chiều rộng của TextBox
                Location = new Point(0, 5),
                MaxLength = 50,
                ReadOnly = true // Hiển thị readonly
            };

            TextBox taskDescription = new TextBox()
            {
                Text = taskDescriptionText,
                Width = 220, // Tăng chiều rộng của TextBox
                Location = new Point(0, 35),
                MaxLength = 200,
                ReadOnly = true // Hiển thị readonly
            };

            CheckBox isActiveCheckbox = new CheckBox()
            {
                Text = "Active",
                Location = new Point(230, 5),
                Checked = isActived
            };
            isActiveCheckbox.CheckedChanged += (sender, e) =>
            {
                DatabaseHelper.UpdateTask(taskId, taskName.Text, taskDescription.Text, isActiveCheckbox.Checked, deadline);
            };

            DateTimePicker deadlinePicker = new DateTimePicker()
            {
                Location = new Point(0, 65),
                Width = 260,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm",
                Value = deadline, // Hiển thị giá trị thời gian tới hạn nếu có
                Enabled = false // Hiển thị readonly
            };

            Button deleteTaskButton = new Button()
            {
                Text = "X",
                Width = 60,
                Height = 25,
                Location = new Point(70, 100),
                BackColor = Color.Red,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
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
                    DatabaseHelper.DeleteTask(taskId); // Xóa khỏi DB
                    ((FlowLayoutPanel)taskContainer.Parent).Controls.Remove(taskContainer); // Xóa khỏi giao diện
                }
            };

            taskContainer.Controls.Add(taskName);
            taskContainer.Controls.Add(taskDescription);
            taskContainer.Controls.Add(isActiveCheckbox);
            taskContainer.Controls.Add(deadlinePicker);
            taskContainer.Controls.Add(deleteTaskButton);

            // Kéo thả sự kiện
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

        private void TaskPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Panel)))
            {
                Panel taskPanel = (Panel)e.Data.GetData(typeof(Panel));
                FlowLayoutPanel targetPanel = (FlowLayoutPanel)sender;

                if (taskPanel.Parent != targetPanel)
                {
                    // Cập nhật cơ sở dữ liệu để di chuyển task sang danh sách mới
                    int taskId = (int)taskPanel.Tag;
                    int newListId = (int)targetPanel.Parent.Tag;
                    DatabaseHelper.MoveTaskToList(taskId, newListId);

                    // Thêm task vào danh sách mới
                    targetPanel.Controls.Add(taskPanel);
                }
            }
        }
    }
}