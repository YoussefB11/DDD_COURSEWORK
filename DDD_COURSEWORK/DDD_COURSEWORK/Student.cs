using System;
using System.Collections.Generic;

namespace DDD_COURSEWORK
{
    // This class represents a student in the system
    public class Student
    {
        public string Id { get; set; }              // Unique student ID
        public string Name { get; set; }            // Full name of the student
        public string SupervisorId { get; set; }    // ID of the student's assigned personal supervisor

        public List<CheckIn> CheckIns { get; set; } = new();   // List of the student's check-ins
        public List<Meeting> Meetings { get; set; } = new();   // List of meetings the studetn had
    }
}


