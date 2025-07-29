using Microsoft.VisualStudio.TestTools.UnitTesting;
using DDD_COURSEWORK;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace DDD_COURSEWORK.Tests
{
    [TestClass]
    public class StudentTests
    {
        [TestMethod]
        public void CheckInCommand_AddsCheckInToStudent()
        {
            
            var student = new Student { Id = "s001", Name = "Youssef Baya", SupervisorId = "ps001" };
            var data = new SystemData();
            data.Students.Add(student);
            string message = "Feeling better today.";

            
            Program.CheckInCommand(data, "s001", message);

            
            Assert.AreEqual(1, student.CheckIns.Count);
            Assert.AreEqual(message, student.CheckIns[0].Message);
        }

        [TestMethod]
        public void CheckInCommand_InvalidStudent_PrintsError()
        {
            
            var data = new SystemData(); 
            using var sw = new StringWriter();
            Console.SetOut(sw);

           
            Program.CheckInCommand(data, "invalid123", "Test message");

          
            string output = sw.ToString().Trim();
            Assert.IsTrue(output.Contains("Student not found."));
        }

        [TestMethod]
        public void CheckInCommand_AllowsMultipleCheckIns()
        {
            var student = new Student { Id = "s001", Name = "Youssef", SupervisorId = "ps001" };
            var data = new SystemData();
            data.Students.Add(student);

            Program.CheckInCommand(data, "s001", "First check-in");
            Program.CheckInCommand(data, "s001", "Second check-in");

            Assert.AreEqual(2, student.CheckIns.Count);
            Assert.AreEqual("Second check-in", student.CheckIns[1].Message);
        }

        [TestMethod]
        public void BookMeetingCommand_AddsMeeting()
        {
            var student = new Student { Id = "s002", Name = "Kris", SupervisorId = "ps001" };
            var supervisor = new PersonalSupervisor { Id = "ps001", Name = "Peter Robinson" };

            var data = new SystemData();
            data.Students.Add(student);
            data.Supervisors.Add(supervisor);

            Program.BookMeetingCommand(data, "s002", "2025-08-01 15:00", "Discuss project");

            Assert.AreEqual(1, student.Meetings.Count);
            Assert.AreEqual("Discuss project", student.Meetings[0].Notes);
        }

        [TestMethod]
        public void BookMeetingCommand_InvalidDate_ShowsError()
        {
            var student = new Student { Id = "s002", Name = "Kris", SupervisorId = "ps001" };
            var supervisor = new PersonalSupervisor { Id = "ps001", Name = "Peter Robinson" };

            var data = new SystemData();
            data.Students.Add(student);
            data.Supervisors.Add(supervisor);

            using var sw = new StringWriter();
            Console.SetOut(sw);

            Program.BookMeetingCommand(data, "s002", "not-a-date", "invalid test");

            string output = sw.ToString();
            Assert.IsTrue(output.Contains("Invalid date format"));
        }


    }
    


}