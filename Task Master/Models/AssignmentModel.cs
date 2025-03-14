using System;

namespace Task_Master.Models
{
    internal class AssignmentModel
    {
        public int user_id { get; set; }
        public int task_id { get; set; }
        public DateTime? due_date { get; set; }
    }
}
