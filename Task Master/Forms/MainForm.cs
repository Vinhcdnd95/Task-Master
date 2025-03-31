
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Task_Master.Data;

namespace Task_Master
{
    public partial class MainForm : Form
    {
        private FlowLayoutPanel boardPanel;
        private Button addUserButton;
        private Button addListButton;
        private int boardId;
        private readonly Color[] listColors = { Color.LightBlue, Color.LightGreen, Color.LightCoral, Color.LightPink, Color.LightSalmon, Color.LightSkyBlue, Color.LightSteelBlue, Color.LightGoldenrodYellow };
        private int colorIndex = 0;
        private List<Panel> listPanels = new List<Panel>();
        private Panel draggedTaskPanel = null;
        public MainForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            boardId = 1;
            LoadLists(boardId);
        }

        private void LoadLists(int boardId)
        {
            DataTable list = DatabaseHelper.GetLists(boardId);
            if (list == null)
            {
                MessageBox.Show("Lỗi khi lấy danh sách từ cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            boardPanel.Controls.Clear();
            foreach (DataRow row in list.Rows)
            {
                int listId = Convert.ToInt32(row["id"]);
                string listName = row["Name"].ToString();
                Panel listPanel = CreateListPanel(listName, listId);
                boardPanel.Controls.Add(listPanel);
            }
            boardPanel.Controls.Add(addListButton);
        }

        private void SetupUI()
        {
            this.Text = "Task Master";
            this.Size = new Size(1600, 900);

            boardPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AllowDrop = true
            };
            boardPanel.DragEnter += BoardPanel_DragEnter;
            boardPanel.DragDrop += BoardPanel_DragDrop;
            this.Controls.Add(boardPanel);

            addListButton = new Button()
            {
                Text = "+ Thêm danh sách",
                Width = 150,
                Height = 40,
            };
            addListButton.Click += AddListButton_Click;
            boardPanel.Controls.Add(addListButton);

            addUserButton = new Button()
            {
                Text = "+ Thêm User",
                Width = 150,
                Height = 40,
                Location = new Point(this.ClientSize.Width - 170, 10), // Góc trên cùng bên phải
                Anchor = AnchorStyles.Top | AnchorStyles.Right // Cố định ở góc phải
            };
            addUserButton.Click += AddUserButton_Click;
            this.Controls.Add(addUserButton);
            addUserButton.BringToFront();

        }

        private void AddListButton_Click(object sender, EventArgs e)
        {
            string newListName = "New List";
            string query = "INSERT INTO lists (board_id, Name) OUTPUT INSERTED.id VALUES (@board_id, @name)";
            SqlParameter[] parameters = {
                new SqlParameter("@board_id", boardId),
                new SqlParameter("@name", newListName)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);
            if (result != null && result.Rows.Count > 0)
            {
                int newListId = Convert.ToInt32(result.Rows[0]["id"]);
                Panel listPanel = CreateListPanel(newListName, newListId);
                boardPanel.Controls.Add(listPanel);
                boardPanel.Controls.SetChildIndex(addListButton, boardPanel.Controls.Count - 1);
            }
            else
            {
                MessageBox.Show("Lỗi khi thêm danh sách vào cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CreateListPanel(string title, int listId)
        {
            Panel panel = new Panel()
            {
                Width = 240,
                Height = 800,
                BackColor = listColors[colorIndex % listColors.Length],
                Padding = new Padding(5),
                Tag = listId
            };
            colorIndex++;

            TextBox titleBox = new TextBox()
            {
                Text = title,
                Width = 180,
                Font = new Font("Arial", 10, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                Location = new Point(10, 5)
            };
            titleBox.Leave += (s, e) => {
                string updateQuery = "UPDATE lists SET Name = @name WHERE id = @id";
                SqlParameter[] updateParams = {
                    new SqlParameter("@name", titleBox.Text),
                    new SqlParameter("@id", listId)
                };
                DatabaseHelper.ExecuteNonQuery(updateQuery, updateParams);
            };
            panel.Controls.Add(titleBox);

            Button deleteButton = new Button()
            {
                Text = "X",
                Width = 30,
                Height = 30,
                Location = new Point(panel.Width - 40, 5),
                BackColor = Color.Red,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            deleteButton.Click += (s, e) => DeleteList(panel, listId);
            panel.Controls.Add(deleteButton);
            EnableDragAndDrop(panel);

            FlowLayoutPanel taskPanel = new FlowLayoutPanel()
            {
                Location = new Point(5, 50), // Điều chỉnh vị trí
                Width = 300, // Tăng chiều rộng của FlowLayoutPanel
                Height = 600, // Tăng chiều cao của FlowLayoutPanel
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AllowDrop = true
            };
            taskPanel.DragEnter += TaskPanel_DragEnter;
            taskPanel.DragOver += TaskPanel_DragOver; // Thêm sự kiện DragOver
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
                Text = "+ Thêm Task",
                Width = 100, // Tăng chiều rộng của Button
                Height = 25, // Tăng chiều cao của Button
                Location = new Point(10, 30), // Điều chỉnh vị trí
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,

            };
            addTaskButton.Click += (s, e) =>
            {
                Panel taskContainer = new Panel()
                {
                    Width = 220, // Tăng chiều rộng của taskContainer
                    Height = 220, // Tăng chiều cao để chứa thêm DateTimePicker và nút
                    Margin = new Padding(5),
                    Tag = -1, // Chưa có trong DB
                    BorderStyle = BorderStyle.FixedSingle // Thêm khung viền cho task
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
                    Width = 200, // Tăng chiều rộng của TextBox
                    Location = new Point(0, 20),
                    MaxLength = 50
                };

                Label taskDescriptionLabel = new Label()
                {
                    Text = "Người thực hiện:",
                    Location = new Point(0, 50),
                    Width = 220
                };

                ComboBox userComboBox = new ComboBox()
                {
                    Width = 200,
                    Location = new Point(0, 70),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                LoadUsers(userComboBox);

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

                CheckBox isActiveCheckbox = new CheckBox()
                {
                    Text = "Active",
                    Location = new Point(0, 150),
                    Checked = true
                };

                Button saveButton = new Button()
                {
                    Text = "Lưu",
                    Width = 60,
                    Height = 25,
                    Location = new Point(0, 170),
                    BackColor = Color.Blue,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                saveButton.Click += (senderSave, eSave) =>
                {
                    int taskId = (int)taskContainer.Tag;
                    if (taskId == -1) // Chỉ thêm mới
                    {
                        int selectedUserId = (int)userComboBox.SelectedValue;
                        taskId = DatabaseHelper.InsertTask(listId, taskName.Text, selectedUserId, isActiveCheckbox.Checked, deadlinePicker.Value);
                        taskContainer.Tag = taskId;
                        MessageBox.Show("Task đã thêm: " + taskName.Text, "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Tạo task mới và thêm vào taskPanel
                        Panel newTaskContainer = CreateTaskPanel(taskId, taskName.Text, userComboBox.Text, isActiveCheckbox.Checked, deadlinePicker.Value);
                        taskPanel.Controls.Add(newTaskContainer);

                        // Xóa nội dung trong TextBox
                        taskName.Text = "";
                        userComboBox.Text = "";
                        deadlinePicker.Value = DateTime.Now;
                    }
                };

                taskContainer.Controls.Add(taskNameLabel);
                taskContainer.Controls.Add(taskName);
                taskContainer.Controls.Add(taskDescriptionLabel);
                taskContainer.Controls.Add(userComboBox);
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
                Width = 220, // Tăng chiều rộng
                Height = 220, // Tăng chiều cao để bố cục đẹp hơn
                Margin = new Padding(10),
                Padding = new Padding(5),
                Tag = taskId,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label taskNameLabel = new Label()
            {
                Text = "Nội dung:",
                Location = new Point(10, 10),
                Width = 80,
                Height = 20

            };

            TextBox taskName = new TextBox()
            {
                Text = taskNameText,
                Width = 200,
                Location = new Point(10, 30),
                MaxLength = 50,
                ReadOnly = true
            };

            Label taskDescriptionLabel = new Label()
            {
                Text = "Người thực hiện:",
                Location = new Point(10, 60),
                Width = 100,
                Height = 20
            };

            ComboBox userComboBox = new ComboBox()
            {
                Width = 200,
                Location = new Point(10, 80),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            LoadUsers(userComboBox); // Load danh sách người dùng vào ComboBox

            // Đặt giá trị được chọn theo user của task (nếu có)
            if (!string.IsNullOrEmpty(taskDescriptionText))
            {
                userComboBox.SelectedIndex = userComboBox.FindStringExact(taskDescriptionText);
            }

            Label deadlineLabel = new Label()
            {
                Text = "Thời hạn:",
                Location = new Point(10, 110),
                Width = 80,
                Height = 20
            };

            DateTimePicker deadlinePicker = new DateTimePicker()
            {
                Location = new Point(10, 130),
                Width = 200,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm",
                Value = deadline,
                Enabled = false
            };

            CheckBox isActiveCheckbox = new CheckBox()
            {
                Text = "Active",
                Location = new Point(10, 150),
                Checked = isActived
            };
            isActiveCheckbox.CheckedChanged += (sender, e) =>
            {
                DatabaseHelper.UpdateTask(taskId, taskName.Text, userComboBox.Text, isActiveCheckbox.Checked, deadline);
            };

            PictureBox dragHandle = new PictureBox()
            {
                Size = new Size(30, 30),
                Location = new Point(190, 5),
                Image = null, // Thay bằng icon kéo thả của bạn
                SizeMode = PictureBoxSizeMode.StretchImage,
                Cursor = Cursors.Hand
            };

            dragHandle.MouseDown += TaskContainer_MouseDown;


            Button deleteTaskButton = new Button()
            {
                Text = "X",
                Width = 60,
                Height = 30,
                Location = new Point((taskContainer.Width - 60) / 2, 190),
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
                    DatabaseHelper.DeleteTask(taskId);
                    ((FlowLayoutPanel)taskContainer.Parent).Controls.Remove(taskContainer);
                }
            };

            taskContainer.Controls.Add(taskNameLabel);
            taskContainer.Controls.Add(taskName);
            taskContainer.Controls.Add(taskDescriptionLabel);
            taskContainer.Controls.Add(userComboBox);
            taskContainer.Controls.Add(deadlineLabel);
            taskContainer.Controls.Add(deadlinePicker);
            taskContainer.Controls.Add(isActiveCheckbox);
            taskContainer.Controls.Add(deleteTaskButton);
            taskContainer.Controls.Add(dragHandle);

            return taskContainer;
        }

        private void TaskContainer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox dragHandle = sender as PictureBox;
                if (dragHandle != null)
                {
                    Panel taskPanel = dragHandle.Parent as Panel;
                    if (taskPanel != null)
                    {
                        draggedTaskPanel = taskPanel;
                        draggedTaskPanel.DoDragDrop(draggedTaskPanel, DragDropEffects.Move);
                    }
                }
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

        private void EnableDragAndDrop(Panel listPanel)
        {
            listPanel.MouseDown += (s, e) => {
                if (e.Button == MouseButtons.Left)
                {
                    listPanel.DoDragDrop(listPanel, DragDropEffects.Move);
                }
            };
        }

        private void BoardPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Panel)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void BoardPanel_DragDrop(object sender, DragEventArgs e)
        {
            Panel draggedPanel = (Panel)e.Data.GetData(typeof(Panel));
            Point dropPoint = boardPanel.PointToClient(new Point(e.X, e.Y));
            int insertIndex = boardPanel.Controls.GetChildIndex(boardPanel.GetChildAtPoint(dropPoint));

            if (boardPanel.Controls[insertIndex] == addListButton)
            {
                insertIndex--;
            }

            if (insertIndex >= 0)
            {
                boardPanel.Controls.SetChildIndex(draggedPanel, insertIndex);
            }
        }

        private void DeleteList(Panel panel, int listId)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa danh sách này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string deleteQuery = "DELETE FROM lists WHERE id = @id";
                SqlParameter[] deleteParams = { new SqlParameter("@id", listId) };
                if (DatabaseHelper.ExecuteNonQuery(deleteQuery, deleteParams) > 0)
                {
                    boardPanel.Controls.Remove(panel);
                }
            }
        }

        private void LoadUsers(ComboBox comboBox)
        {
            string query = "SELECT id, username FROM users";
            DataTable users = DatabaseHelper.ExecuteQuery(query);

            if (users != null && users.Rows.Count > 0)
            {
                comboBox.DataSource = users;
                comboBox.DisplayMember = "username"; // Hiển thị tên user
                comboBox.ValueMember = "id";        // Lưu ID user
            }
            else
            {
                comboBox.Items.Clear();
                comboBox.Items.Add("Không có người dùng");
                comboBox.SelectedIndex = 0;
            }
        }

        private void AddUserButton_Click(object sender, EventArgs e)
        {
            UserForm userForm = new UserForm();
            userForm.ShowDialog(); // Mở form thêm người dùng
        }

    }
}
