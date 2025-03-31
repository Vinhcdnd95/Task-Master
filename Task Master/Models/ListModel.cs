using System;

namespace Task_Master.Models
{
    public class ListsModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int board_id { get; set; }
        public int position { get; set; }
        public DateTime createdAt { get; set; }
    }
}