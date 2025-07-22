using System;

namespace DDD_COURSEWORK
{
    // This class represents a check-in from a student
    public class CheckIn
    {
        public DateTime Date { get; set; }  // This saves the date and time the check-in was made
        public string Message { get; set; } // This stores the message the student typed during their check-in
    }
}
