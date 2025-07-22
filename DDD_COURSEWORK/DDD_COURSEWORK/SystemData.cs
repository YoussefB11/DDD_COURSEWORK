using System;
using DDD_COURSEWORK;
namespace DDD_COURSEWORK
{
    // This class stores all the data for the whole system in one place
    public class SystemData
    {
        public List<Student> Students { get; set; } = new();           // List of all students (in this case only one but ca add more)
        public List<PersonalSupervisor> Supervisors { get; set; } = new();  // List of all personal supervisors (in this case only one but can add more)
        public SeniorTutor SeniorTutor { get; set; }                   // The senior tutor (only one)
    }
}
