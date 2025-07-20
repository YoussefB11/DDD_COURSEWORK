using System;
using System.Collections.Generic;
namespace DDD_COURSEWORK.Models
{
	public class Student
	{
        public string Id { get; set; }
        public string Name { get; set; }
        public string SupervisorId { get; set; }
        public List<CheckIn> CheckIns { get; set; } = new();
        public List<Meeting> Meetings { get; set; } = new();
    }
}

