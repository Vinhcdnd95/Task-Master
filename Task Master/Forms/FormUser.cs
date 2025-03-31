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
                dgvUser.DataSource = dt;
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

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDel_Click(object sender, EventArgs e)
        {

        }
    }
}
