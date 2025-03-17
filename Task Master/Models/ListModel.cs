using System;

namespace Task_Master.Models
{
    public class ListModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int board_id { get; set; }
        public DateTime createdAt { get; set; }
    }
}