using System;
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
        private int boardId = 1;
        private readonly Color[] listColors = { Color.LightBlue, Color.LightGreen, Color.LightCoral, Color.LightPink, Color.LightSalmon, Color.LightSkyBlue, Color.LightSteelBlue, Color.LightGoldenrodYellow };
        private int colorIndex = 0;

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
            deleteButton.Click += (s, e) => DeleteList(panel, listId);
            panel.Controls.Add(deleteButton);
            EnableDragAndDrop(panel);
            return panel;
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
                string deleteQuery = "DELETE FROM list WHERE id = @id";
                SqlParameter[] deleteParams = { new SqlParameter("@id", listId) };
                if (DatabaseHelper.ExecuteNonQuery(deleteQuery, deleteParams) > 0)
                {
                    boardPanel.Controls.Remove(panel);
                }
            }
        }
    }
}
