using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task_Master.Data;

namespace Task_Master.Forms
{
    public partial class FormUser : Form
    {
        public FormUser()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO users (username, password) VALUES (@username, @password)";
            SqlParameter[] parameters = {
                new SqlParameter("@username", txtUsername.Text),
                new SqlParameter("@password", txtPassword.Text)
            };
            DatabaseHelper.ExecuteNonQuery(query, parameters);
            LoadData();
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'quản_lý_công_việcDataSet.users' table. You can move, or remove it, as needed.
            this.usersTableAdapter.Fill(this.quản_lý_công_việcDataSet.users);
            dgvUsers.CellClick += dgvUsers_CellClick;
            LoadData(); 

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                int id = (int)dgvUsers.SelectedRows[0].Cells[0].Value;
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

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                int id = (int)dgvUsers.SelectedRows[0].Cells[0].Value;
                string query = "DELETE FROM users WHERE id = @id";
                SqlParameter[] parameters = { new SqlParameter("@id", id) };
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                LoadData();
            }
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // đảm bảo không phải header
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];

                //txtUsername.Text = row.Cells["username"].Value?.ToString();
                //txtPassword.Text = row.Cells["password"].Value?.ToString();

                txtUsername.Text = row.Cells[1].Value?.ToString(); // username
                txtPassword.Text = row.Cells[2].Value?.ToString(); // password

            }
        }
        

    }
}
