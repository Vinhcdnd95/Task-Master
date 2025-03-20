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
                Panel listPanel = CreateListPanel(listName);
                boardPanel.Controls.Add(listPanel);
            }

            boardPanel.Controls.Add(addListButton);
        }

        private void SetupUI()
        {
            this.Text = "Task Master";
            this.Size = new Size(1000, 600);

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
                Width = 150,
                Height = 40
            };
            addListButton.Click += AddListButton_Click;
            boardPanel.Controls.Add(addListButton);
        }

        private void AddListButton_Click(object sender, EventArgs e)
        {
            Panel listPanel = CreateListPanel("New List");
            boardPanel.Controls.Add(listPanel);
            boardPanel.Controls.SetChildIndex(addListButton, boardPanel.Controls.Count - 1);
            listPanels.Add(listPanel);
        }

        private Panel CreateListPanel(string title)
        {
            Panel panel = new Panel()
            {
                Width = 240,
                Height = 400,
                BackColor = Color.LightGray,
                Padding = new Padding(5)
            };

            // Tiêu đề danh sách
            TextBox titleBox = new TextBox()
            {
                Text = title,
                Width = 180,
                Font = new Font("Arial", 10, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                Location = new Point(10, 5)
            };
            panel.Controls.Add(titleBox);

            // Nút xóa danh sách
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
                    boardPanel.Controls.Remove(panel);
                    listPanels.Remove(panel);
                }
            };
            panel.Controls.Add(deleteButton);

            // FlowLayoutPanel để chứa các task
            FlowLayoutPanel taskPanel = new FlowLayoutPanel()
            {
                Location = new Point(10, 40),
                Width = 220,
                Height = 300,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            panel.Controls.Add(taskPanel);

            // Nút thêm task
            Button addTaskButton = new Button()
            {
                Text = "+ Add Task",
                Width = 100,
                Height = 30,
                Location = new Point(10, 350),
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            addTaskButton.Click += (s, e) =>
            {
                TextBox newTask = new TextBox()
                {
                    Text = "New Task",
                    Width = 200,
                    Margin = new Padding(5)
                };
                taskPanel.Controls.Add(newTask);
            };
            panel.Controls.Add(addTaskButton);

            return panel;
        }
    }
}