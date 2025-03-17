using System;

namespace Task_Master.Models
{
    public class BoardModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createdat { get; set; }
    }
}