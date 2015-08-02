using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace LinqQueries
{
    static class Constants
    {
        public static readonly string[] FirstNames = new string[] {
            "Peter", "Joe", "Frank", "Max", "Alex", "Ann", "Alice", "Josephine", "Christy", "Merie"
        };
        public static readonly string[] LastNames = new string[] {
            "Spradlin", "Davison", "West", "Choe", "Meier", "Bervig", "Cudlipp", "Kynaston", "Brown", "Mercury"
        };
    }

    class Form
    {
        private static readonly IEnumerable<Form> _forms = new List<Form>
            {
                new Form { ID = 1, Letter = 'A', Number = 6 },
                new Form { ID = 2, Letter = 'A', Number = 7 },
                new Form { ID = 3, Letter = 'B', Number = 7 },
            };
        public static IEnumerable<Form> GetForms()
        {
            return _forms;
        }

        public int ID { get; set; }
        public char Letter { get; set; }
        public int Number { get; set; }
    }

    class Student
    {
        private static readonly IEnumerable<Student> _students;
        public static IEnumerable<Student> GetStudents()
        {
            return _students;
        }
        static Student() {
            Random r = new Random();
            List<Student> students = new List<Student>();
            for (int i = 0; i < 10; i++)
            {
                students.Add(new Student {
                    Name = Constants.FirstNames[r.Next(10)] + " " + Constants.LastNames[r.Next(10)],
                    FormID = r.Next(3) + 1,
                    ID = (i + 1)
                });
            }
            _students = students;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int FormID { get; set; }
    }

    class Subject
    {
        private static readonly IEnumerable<Subject> _subjects = new List<Subject>
            {
                new Subject { ID = 1, Name = "Math" },
                new Subject { ID = 2, Name = "Chemistry" },
                new Subject { ID = 3, Name = "Biology" },
                new Subject { ID = 4, Name = "Music" },
                new Subject { ID = 5, Name = "Arts" }
            };
        public static IEnumerable<Subject> GetSubjects()
        {
            return _subjects;
        }

        public int ID { get; set; }
        public string Name { get; set; }
    }

    class Report
    {
        private static readonly IEnumerable<Report> _reports;
        public static IEnumerable<Report> GetReports()
        {
            return _reports;
        }
        static Report()
        {
            Random r = new Random();
            List<Report> reports = new List<Report>();
            for (int i = 0; i < 100; i++)
            {
                reports.Add(new Report
                {
                    StudentID = r.Next(10) + 1,
                    Mark = r.Next(12) + 1,
                    Date = new DateTime(2015, r.Next(12) + 1, r.Next(28) + 1),
                    SubjectID = r.Next(4) + 1
                });
            }
            _reports = reports;
        }

        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public DateTime Date { get; set; }
        public int Mark { get; set; }

        public override string ToString()
        {
            return string.Format("StudentID: {0} SubjectID: {1} Date: {2} Mark: {3}", StudentID, SubjectID, Date.ToShortDateString(), Mark);
        }
    }

    class Program
    {
        
        
        static void Main(string[] args)
        {

            var newReport = Report.GetReports();
            var newList = newReport.Where(n => n.Mark > 5 && IsSecondQuarter(n))
                .OrderBy(n => n.Date)
                .Select(n => new {n.Date,n.Mark,n.StudentID});

            var t = from r in Report.GetReports()
                    where r.Mark > 5 && IsSecondQuarter(r)
                    orderby r.Date 
                    select new { r.Date, r.Mark, r.StudentID };

            
            //var result = Report.GetReports()
            //    .Join(Student.GetStudents(), (n => n.StudentID), (n => n.ID), ((a, b) => new { b.FormID, b.Name }))
            //    .Join(Form.GetForms(), (n => n.FormID),(n=>n.ID), ((a, b) => new {a.Name,b.Number }))
            //    .Where(n => n.Number == 7 );

            //Report.GetReports().GroupBy(n => n.SubjectID, n => n.Mark);
            //foreach (var item in res)
            //{
            //    PrintAnonymousEnumerable(item);
            //}

            var result = Subject.GetSubjects().GroupJoin(
                Report.GetReports().GroupBy(n => n.SubjectID, n => n.Mark),
                subj => subj.ID,
                subjReps => subjReps.Key,
                (subj, subjReps) => new { subj.Name, Avg = subjReps.Single().DefaultIfEmpty(0).Average() });

            //var result = Report.GetReports()
            //    .GroupBy(n => n.SubjectID);

            List<int> lst = new List<int>();
            foreach (var elem in lst.DefaultIfEmpty())
            {
                //Console.WriteLine(elem);
            }

            PrintAnonymousEnumerable(result);
        }

        private static bool IsSecondQuarter(Report n)
        {
            return n.Date.Month > 3 && n.Date.Month < 7;
        }

        static void PrintAnonymousEnumerable<T>(IEnumerable<T> lst)
        {
            foreach (var item in lst)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
