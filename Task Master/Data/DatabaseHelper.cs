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

        public static DataTable GetLists(int boardId)
        {
            string query = "SELECT Id, Name FROM List WHERE board_id = @boardId";
            SqlParameter[] parameters = { new SqlParameter("@boardId", boardId) };
            return ExecuteQuery(query, parameters);
        }

        public static int InsertList(int boardId, string name)
        {
            string query = "INSERT INTO List (board_id, Name) VALUES (@boardId, @name); " +
                           "SELECT SCOPE_IDENTITY();";
            SqlParameter[] parameters = {
                new SqlParameter("@boardId", boardId),
                new SqlParameter("@name", name)
            };
            return ExecuteScalar(query, parameters);
        }

        public static int InsertTask(int listId, string name, string description, bool isActived)
        {
            string query = "INSERT INTO [task] (list_id, name, description, is_actived) " +
                           "VALUES (@listId, @name, @description, @isActived); " +
                           "SELECT SCOPE_IDENTITY();";
            SqlParameter[] parameters = {
                new SqlParameter("@listId", listId),
                new SqlParameter("@name", name ?? (object)DBNull.Value),
                new SqlParameter("@description", description ?? (object)DBNull.Value),
                new SqlParameter("@isActived", isActived)
            };
            return ExecuteScalar(query, parameters);
        }

        public static void UpdateTask(int taskId, string name, string description, bool isActived)
        {
            string query = "UPDATE [task] SET name = @name, description = @description, " +
                           "is_actived = @isActived WHERE id = @taskId";
            SqlParameter[] parameters = {
                new SqlParameter("@taskId", taskId),
                new SqlParameter("@name", name ?? (object)DBNull.Value),
                new SqlParameter("@description", description ?? (object)DBNull.Value),
                new SqlParameter("@isActived", isActived)
            };
            ExecuteNonQuery(query, parameters);
        }

        public static void DeleteTask(int taskId)
        {
            string query = "DELETE FROM [task] WHERE id = @taskId";
            SqlParameter[] parameters = { new SqlParameter("@taskId", taskId) };
            ExecuteNonQuery(query, parameters);
        }

        // Thêm phương thức để lấy danh sách task theo list_id
        public static DataTable GetTasks(int listId)
        {
            string query = "SELECT id, name, description FROM [task] WHERE list_id = @listId";
            SqlParameter[] parameters = { new SqlParameter("@listId", listId) };
            return ExecuteQuery(query, parameters);
        }

        // Thêm phương thức để di chuyển task giữa các danh sách
        public static void MoveTaskToList(int taskId, int newListId)
        {
            string query = "UPDATE [task] SET list_id = @newListId WHERE id = @taskId";
            SqlParameter[] parameters = {
                new SqlParameter("@taskId", taskId),
                new SqlParameter("@newListId", newListId)
            };
            ExecuteNonQuery(query, parameters);
        }
    }
}