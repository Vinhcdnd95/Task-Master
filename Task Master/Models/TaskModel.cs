using System;

namespace Task_Master.Models
{
    public class TaskModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int list_id { get; set; }
        public bool is_active { get; set; }
        public DateTime? deadline { get; set; }
        public int user_id { get; set; }
    }
}
