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
        private Button addListButton;
        private List<Panel> listPanels = new List<Panel>();
        private int boardId = 1;

        public MainForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
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
            string newListName = "New List";
            string query = "INSERT INTO list (board_id, Name) OUTPUT INSERTED.id VALUES (@board_id, @name)";
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
                listPanels.Add(listPanel);
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
                Height = 400,
                BackColor = Color.LightGray,
                Padding = new Padding(5),
                Tag = listId
            };

            TextBox titleBox = new TextBox()
            {
                Text = title,
                Width = 180,
                Font = new Font("Arial", 10, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                Location = new Point(10, 5)
            };

            titleBox.Leave += (s, e) =>
            {
                string updateQuery = "UPDATE list SET Name = @name WHERE id = @id";
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
                    string deleteQuery = "DELETE FROM list WHERE id = @id";
                    SqlParameter[] deleteParams = { new SqlParameter("@id", listId) };

                    if (DatabaseHelper.ExecuteNonQuery(deleteQuery, deleteParams) > 0)
                    {
                        boardPanel.Controls.Remove(panel);
                        listPanels.Remove(panel);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi xóa danh sách!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            panel.Controls.Add(deleteButton);
            return panel;
        }
    }
}
