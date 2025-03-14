using System;
using System.Data;
using System.Data.SqlClient;

namespace Task_Master.Data
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString =
            "Server=DESKTOP-70CU9IO;Database=Quản lý công việc;Integrated Security=True;";

        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public static DataTable GetLists(int board_id)
        {
            string query = "SELECT id, Name FROM list WHERE board_id = @board_id";
            SqlParameter[] parameters = { new SqlParameter("@board_id", board_id) };
            return ExecuteQuery(query, parameters);
        }

    }
}
