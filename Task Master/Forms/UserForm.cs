using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Task_Master.Data;
using Task_Master.Models;

namespace Task_Master
{
    public partial class UserForm : Form
    {
        private DataGridView dgvUsers;
        private TextBox txtUsername, txtPassword;
        private Button btnAdd, btnEdit, btnDelete;

        public UserForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.txtUsername = new TextBox { Top = 210, Left = 10, Width = 200, Text = "Username", ForeColor = System.Drawing.Color.Gray };
            this.txtPassword = new TextBox { Top = 210, Left = 220, Width = 200, Text = "Password", ForeColor = System.Drawing.Color.Gray };

            this.txtUsername.GotFocus += RemoveText;
            this.txtUsername.LostFocus += AddText;
            this.txtPassword.GotFocus += RemoveText;
            this.txtPassword.LostFocus += AddText;

            dgvUsers = new DataGridView { Top = 10, Left = 10, Width = 600, Height = 200 };
            btnAdd = new Button { Top = 250, Left = 10, Text = "Add" };
            btnEdit = new Button { Top = 250, Left = 100, Text = "Edit" };
            btnDelete = new Button { Top = 250, Left = 190, Text = "Delete" };

            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            dgvUsers.SelectionChanged += dgvUsers_SelectionChanged;

            this.Controls.Add(dgvUsers);
            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
        }

        private void RemoveText(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.ForeColor == System.Drawing.Color.Gray)
            {
                txt.Text = "";
                txt.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void AddText(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                txt.Text = txt == txtUsername ? "Username" : "Password";
                txt.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void LoadData()
        {
            string query = "SELECT id, username, password FROM users";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            if (dt != null)
            {
                dgvUsers.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO users (username, password) VALUES (@username, @password)";
            SqlParameter[] parameters = {
                new SqlParameter("@username", txtUsername.Text),
                new SqlParameter("@password", txtPassword.Text)
            };
            DatabaseHelper.ExecuteNonQuery(query, parameters);
            LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                int id = (int)dgvUsers.SelectedRows[0].Cells["id"].Value;
                string query = "UPDATE users SET username = @username, password = @password WHERE id = @id";
                SqlParameter[] parameters = {
                    new SqlParameter("@id", id),
                    new SqlParameter("@username", txtUsername.Text),
                    new SqlParameter("@password", txtPassword.Text)
                };
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                int id = (int)dgvUsers.SelectedRows[0].Cells["id"].Value;
                string query = "DELETE FROM users WHERE id = @id";
                SqlParameter[] parameters = { new SqlParameter("@id", id) };
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                LoadData();
            }
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                txtUsername.Text = dgvUsers.SelectedRows[0].Cells["username"].Value.ToString();
                txtPassword.Text = dgvUsers.SelectedRows[0].Cells["password"].Value.ToString();
            }
        }
    }
}
