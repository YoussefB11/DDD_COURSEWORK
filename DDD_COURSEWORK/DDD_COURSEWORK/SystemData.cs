using System;
using DDD_COURSEWORK.Models;

namespace DDD_COURSEWORK
{
    public class SystemData
    {
        public List<Student> Students { get; set; } = new();
        public List<PersonalSupervisor> Supervisors { get; set; } = new();
        public SeniorTutor SeniorTutor { get; set; }
    }

}