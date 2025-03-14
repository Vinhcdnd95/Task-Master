using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Task_Master
{
    public partial class assignment : Form
    {
        // Chuỗi kết nối đến SQL Server
        private string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=Quản lý công việc;Integrated Security=True";

        public assignment()
        {
            InitializeComponent();
            this.Load += new EventHandler(assignment_Load);
        }

        private void assignment_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM dbo.assignment";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO dbo.assignment (user_id, task_id, due_date) VALUES (@UserId, @TaskId, @DueDate)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", txtUserId.Text);
                    cmd.Parameters.AddWithValue("@TaskId", txtTaskId.Text);
                    cmd.Parameters.AddWithValue("@DueDate", DateTime.Parse(txtDueDate.Text));
                    cmd.ExecuteNonQuery();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE dbo.assignment SET user_id = @UserId, task_id = @TaskId, due_date = @DueDate WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", txtId.Text);
                    cmd.Parameters.AddWithValue("@UserId", txtUserId.Text);
                    cmd.Parameters.AddWithValue("@TaskId", txtTaskId.Text);
                    cmd.Parameters.AddWithValue("@DueDate", DateTime.Parse(txtDueDate.Text));
                    cmd.ExecuteNonQuery();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM dbo.assignment WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", txtId.Text);
                    cmd.ExecuteNonQuery();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtId.Text = row.Cells["id"].Value.ToString();
                txtUserId.Text = row.Cells["user_id"].Value.ToString();
                txtTaskId.Text = row.Cells["task_id"].Value.ToString();
                txtDueDate.Text = row.Cells["due_date"].Value.ToString();
            }
        }
    }
}