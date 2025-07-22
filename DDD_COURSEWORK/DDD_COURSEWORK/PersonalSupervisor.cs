using System;

namespace DDD_COURSEWORK
{
    // This class represents a personal supervisor (PS)
    public class PersonalSupervisor
    {
        public string Id { get; set; }  // Unique ID for the supervisor
        public string Name { get; set; }  // Full name of the supervisor

        public List<string> AssignedStudentIds { get; set; } = new();  // A list of student IDs that are assigned to this supervisor
    }
}
