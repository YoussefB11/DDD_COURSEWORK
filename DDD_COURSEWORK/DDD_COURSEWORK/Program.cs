using System;
using System.Linq;
using DDD_COURSEWORK;

class Program
{
    // Program.cs with empty switch logic (a case for every command or break when a command is unknown)
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No command provided.");
            return;
        }

        var data = FileManager.LoadData();

        string command = args[0].ToLower();

        switch (command)
        {
            case "checkin":
                if (args.Length >= 3)
                {
                    string studentId = args[1];
                    string message = string.Join(" ", args[2..]);
                    CheckInCommand(data, studentId, message);
                }
                else
                {
                    Console.WriteLine("Usage: checkin <StudentID> <Message>");
                }
                break;

            case "viewstudent":
                if (args.Length >= 2)
                {
                    string studentId = args[1];
                    ViewStudentCommand(data, studentId);
                }
                else
                {
                    Console.WriteLine("Usage: viewstudent <StudentID>");
                }
                break;

            case "requestmeeting":
                if (args.Length >= 3)
                {
                    string studentId = args[1];
                    string dateStr = string.Join(" ", args[2..]);
                    RequestMeetingCommand(data, studentId, dateStr);
                }
                else
                {
                    Console.WriteLine("Usage: requestmeeting <StudentID> <DateTime>");
                }
                break;


            case "bookmeeting":
                if (args.Length >= 4)
                {
                    string studentId = args[1];
                    string dateStr = args[2];
                    string notes = string.Join(" ", args[3..]);
                    BookMeetingCommand(data, studentId, dateStr, notes);
                }
                else
                {
                    Console.WriteLine("Usage: bookmeeting <StudentID> <DateTime> <Notes>");
                }
                break;


            case "viewsummary":
                ViewSummaryCommand(data);
                break;


            default:
                Console.WriteLine("Unknown command.");
                break;
        }

        FileManager.SaveData(data);
    }

    static void CheckInCommand(SystemData data, string studentId, string message)
    {
        var student = data.Students.Find(s => s.Id == studentId);
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        student.CheckIns.Add(new CheckIn
        {
            Date = DateTime.Now,
            Message = message
        });

        Console.WriteLine($"Check-in submitted for {student.Name}");
    }

    static void ViewStudentCommand(SystemData data, string studentId)
    {
        var student = data.Students.Find(s => s.Id == studentId);
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        Console.WriteLine($"Name: {student.Name}");
        Console.WriteLine("Check-Ins:");
        if (student.CheckIns.Count == 0)
        {
            Console.WriteLine("- (none)");
        }
        else
        {
            foreach (var checkIn in student.CheckIns)
            {
                Console.WriteLine($"- [{checkIn.Date:yyyy-MM-dd}] {checkIn.Message}");
            }
        }

        Console.WriteLine("Meetings:");
        if (student.Meetings.Count == 0)
        {
            Console.WriteLine("- (none)");
        }
        else
        {
            foreach (var meeting in student.Meetings)
            {
                Console.WriteLine($"- [{meeting.Date:yyyy-MM-dd HH:mm}] With: {meeting.With} | Notes: {meeting.Notes}");
            }
        }
    }

    static void RequestMeetingCommand(SystemData data, string studentId, string dateStr)
    {
        var student = data.Students.Find(s => s.Id == studentId);
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        if (!DateTime.TryParse(dateStr, out DateTime meetingDate))
        {
            Console.WriteLine("Invalid date format. Try something like: 2025-07-23 10:00");
            return;
        }

        var supervisor = data.Supervisors.Find(ps => ps.Id == student.SupervisorId);
        if (supervisor == null)
        {
            Console.WriteLine("Supervisor not found.");
            return;
        }

        student.Meetings.Add(new Meeting
        {
            Date = meetingDate,
            With = supervisor.Name,
            Notes = "(Requested by student)"
        });

        Console.WriteLine($"Meeting requested with {supervisor.Name} on {meetingDate:g}");
    }


    static void BookMeetingCommand(SystemData data, string studentId, string dateStr, string notes)
    {
        var student = data.Students.Find(s => s.Id == studentId);
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        if (!DateTime.TryParse(dateStr, out DateTime meetingDate))
        {
            Console.WriteLine("Invalid date format. Use something like: 2025-07-24 14:00");
            return;
        }

        var supervisor = data.Supervisors.Find(ps => ps.Id == student.SupervisorId);
        if (supervisor == null)
        {
            Console.WriteLine("Supervisor not found.");
            return;
        }

        student.Meetings.Add(new Meeting
        {
            Date = meetingDate,
            With = supervisor.Name,
            Notes = notes
        });

        Console.WriteLine($"Meeting booked with {student.Name} on {meetingDate:g} with notes: {notes}");
    }

    static void ViewSummaryCommand(SystemData data)
    {
        foreach (var ps in data.Supervisors)
        {
            var students = data.Students.FindAll(s => s.SupervisorId == ps.Id);
            int total = students.Count;
            int withCheckIns = students.Count(s => s.CheckIns.Any());
            int withMeetings = students.Count(s => s.Meetings.Any());

            Console.WriteLine($"\nSupervisor: {ps.Name}");
            Console.WriteLine($"- Students assigned: {total}");
            Console.WriteLine($"- Students with check-ins: {withCheckIns}");
            Console.WriteLine($"- Students with meetings: {withMeetings}");
        }
    }

}
