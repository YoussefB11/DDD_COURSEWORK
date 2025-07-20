using System;
namespace DDD_COURSEWORK.Models
{
	public class PersonalSupervisor
	{
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> AssignedStudentIds { get; set; } = new();
    }
}

