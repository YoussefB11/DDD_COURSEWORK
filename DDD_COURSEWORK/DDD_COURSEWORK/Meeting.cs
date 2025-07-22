using System;

namespace DDD_COURSEWORK
{
    // This class represents a meeting between a student and their supervisor
    public class Meeting
    {
        public DateTime Date { get; set; }    // When the meeting is scheduled
        public string With { get; set; }      // Who the meeting is with (supervisor's name)
        public string Notes { get; set; }     // Any notes or reason for the meeting
    }
}
