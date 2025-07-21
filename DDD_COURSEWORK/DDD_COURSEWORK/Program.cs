using System;
using System.Linq;
using DDD_COURSEWORK;
using DDD_COURSEWORK.Models;

class Program
{
    // Program.cs with empty switch logic (a case for every command or break when a command is unknown)
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            RunInteractiveMenu();
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

    static void RunInteractiveMenu()
    {
        var data = FileManager.LoadData();

        Console.WriteLine("-- Student Support System --");
        Console.WriteLine("");
        Console.WriteLine("Select your role: ");
        Console.WriteLine("1. Student");
        Console.WriteLine("2. Personal Supervisor");
        Console.WriteLine("3. Senior Tutor");
        Console.Write("Enter choice: ");

        string choice = Console.ReadLine()?.Trim();

        switch (choice)
        {
            case "1":
                RunStudentMenu(data);
                break;
            case "2":
                RunSupervisorMenu(data);
                break;
            case "3":
                RunTutorMenu(data);
                break;
            default:
                Console.WriteLine("Error");
                break;
        }

        FileManager.SaveData(data);
    }

    static void RunStudentMenu(SystemData data)
    {
        Student student = null;
        string studentId = "";

        while (student == null)
        {
            Console.Write("Enter your Student ID: ");
            studentId = Console.ReadLine()?.Trim();
            student = data.Students.Find(s => s.Id == studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found so try again.");
            }
        }


        while (true)
        {
            Console.WriteLine($"Student Menu for {student.Name}");
            Console.WriteLine("1. Submit Check-In");
            Console.WriteLine("2. Request Meeting");
            Console.WriteLine("3. View My Info");
            Console.WriteLine("0. Close");
            Console.Write("Choice: ");
            string input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    Console.Write("Enter your check-in message: ");
                    string message = Console.ReadLine();
                    CheckInCommand(data, studentId, message);
                    break;
                case "2":
                    Console.Write("Enter the meeting date/time (e.g. 2025-07-25 10:00): ");
                    string dateStr = Console.ReadLine();
                    RequestMeetingCommand(data, studentId, dateStr);
                    break;
                case "3":
                    ViewStudentCommand(data, studentId);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }


    static void RunSupervisorMenu(SystemData data)
    {
        PersonalSupervisor supervisor = null;
        string psId = "";

        while (supervisor == null)
        {
            Console.Write("Enter your Supervisor ID: ");
            psId = Console.ReadLine()?.Trim();
            supervisor = data.Supervisors.Find(ps => ps.Id == psId);

            if (supervisor == null)
            {
                Console.WriteLine("PS id not found sotry again.");
            }
        }


        while (true)
        {
            Console.WriteLine($"Supervisor Menu for {supervisor.Name}");
            Console.WriteLine("1. Book Meeting with Student");
            Console.WriteLine("2. View Student Info");
            Console.WriteLine("0. Close");
            Console.Write("Choose: ");
            string input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    Console.Write("Enter Student ID: ");
                    string studentId = Console.ReadLine();
                    Console.Write("Enter meeting date/time: ");
                    string dateStr = Console.ReadLine();
                    Console.Write("Write a note: ");
                    string notes = Console.ReadLine();
                    BookMeetingCommand(data, studentId, dateStr, notes);
                    break;
                case "2":
                    Console.Write("Enter Student ID: ");
                    string viewId = Console.ReadLine();
                    ViewStudentCommand(data, viewId);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Inavlid option");
                    break;
            }
        }
    }


    static void RunTutorMenu(SystemData data)
    {
        while (true)
        {
            Console.WriteLine("Senior Tutor Menu");
            Console.WriteLine("1. View Supervisors/Students Engagements");
            Console.WriteLine("0. Close");
            Console.Write("Choice: ");
            string input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    ViewSummaryCommand(data);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option  ");
                    break;
            }
        }
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
