
using System;
using System.Data;
using System.Data.SqlClient;

namespace Task_Master.Data
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString =
            "Server=.\\SQLEXPRESS;Database=Quản lý công việc;Integrated Security=True;";

        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            try
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

                            if (dt.Rows.Count == 0)
                            {
                                Console.WriteLine("Không có dữ liệu trả về từ truy vấn!");
                            }
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi truy vấn SQL: {ex.Message}");
                return null;
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

        public static int ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static DataTable GetLists(int board_id)
        {
            string query = "SELECT id, name FROM lists WHERE board_id = @board_id";
            SqlParameter[] parameters = { new SqlParameter("@board_id", board_id) };
            return ExecuteQuery(query, parameters);
        }

        public static int InsertTask(int listId, string name, int userId, bool isActive, DateTime deadline)
        {
            string query = "INSERT INTO tasks (list_id, name, user_id, is_actived, deadline) OUTPUT INSERTED.id VALUES (@list_id, @name, @user_id, @is_actived, @deadline)";
            SqlParameter[] parameters = {
                new SqlParameter("@list_id", listId),
                new SqlParameter("@name", name),
                new SqlParameter("@user_id", userId),
                new SqlParameter("@is_actived", isActive),
                new SqlParameter("@deadline", deadline)
            };
            DataTable result = ExecuteQuery(query, parameters);
            return (result != null && result.Rows.Count > 0) ? Convert.ToInt32(result.Rows[0]["id"]) : -1;
        }

        public static void UpdateTask(int taskId, string name, int userId, bool isActived, DateTime deadline)
        {
            string query = "UPDATE tasks SET name = @name, user_id = @user_id, " +
                           "is_actived = @isActived, deadline = @deadline WHERE id = @taskId";

            SqlParameter[] parameters = {
        new SqlParameter("@taskId", taskId),
        new SqlParameter("@name", name ?? (object)DBNull.Value),
        new SqlParameter("@user_id", userId),
        new SqlParameter("@isActived", isActived),
        new SqlParameter("@deadline", deadline),
    };

            ExecuteNonQuery(query, parameters);
        }


        public static void DeleteTask(int taskId)
        {
            string query = "DELETE FROM tasks WHERE id = @taskId";
            SqlParameter[] parameters = { new SqlParameter("@taskId", taskId) };
            ExecuteNonQuery(query, parameters);
        }

        public static DataTable GetTasks(int listId)
        {
            string query = "SELECT id, name, description, is_actived, deadline, user_id FROM tasks WHERE list_id = @listId";
            SqlParameter[] parameters = { new SqlParameter("@listId", listId) };
            return ExecuteQuery(query, parameters);
        }

        public static void MoveTaskToList(int taskId, int newListId)
        {
            string query = "UPDATE tasks SET list_id = @newListId WHERE id = @taskId";
            SqlParameter[] parameters = {
                new SqlParameter("@taskId", taskId),
                new SqlParameter("@newListId", newListId)
            };
            ExecuteNonQuery(query, parameters);
        }
    }
}
