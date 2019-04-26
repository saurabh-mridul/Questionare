using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanskeITChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var outputs = new List<List<Subject>>();
                Console.WriteLine("Enter the number of days: ");
                var days = Convert.ToInt32(Console.ReadLine());

                for (int i = 1; i <= days; i++)
                {
                    Console.WriteLine($"Number of subject for the {i} day followed by start time end time");
                    int sNumbers = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine($"Enter subjects");

                    var subjects = new List<Subject>();
                    for (int j = 1; j <= sNumbers; j++)
                    {
                        var sub = Console.ReadLine();
                        subjects.Add(ParseSubject(sub));
                    }

                    outputs.Add(GetSubjectsCanBeTakenForTheDay(subjects));
                }

                Console.WriteLine("Output : Max class can be attended each day with overlap");
                foreach (var subjects in outputs)
                {
                    Console.WriteLine($"maximum subjects can be taken: {subjects.Count}");
                    foreach (var subject in subjects)
                    {
                        Console.WriteLine($"{subject.Name} {subject.StartTime} {subject.EndTime}");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private static List<Subject> GetSubjectsCanBeTakenForTheDay(List<Subject> subjects)
        {
            subjects = subjects.OrderBy(s => s.StartTime).ToList();
            var finalSubjects = GetConsiderableSubjects(subjects);
            return finalSubjects;
        }

        private static List<Subject> GetConsiderableSubjects(List<Subject> subjects)
        {
            var isLastItem = false;
            for (int k = 0; k <= subjects.Count; k++)
            {
                var currSub = subjects[k];
                if (k == subjects.Count - 1)
                {
                    isLastItem = true;
                    break;
                }
                var nextSub = subjects[k + 1];

                var isOverLap = currSub.EndTime > nextSub.StartTime;
                if (isOverLap)
                {
                    var rejectedSub = (currSub.Duration > nextSub.Duration) ? currSub : nextSub;
                    subjects.Remove(rejectedSub);
                    break; // stop the iteration and update the source
                }
            }
            if (!isLastItem)
            {
                GetConsiderableSubjects(subjects);
            }
            return subjects;
        }

        private static Subject ParseSubject(string subject)
        {
            if (string.IsNullOrEmpty(subject))
                return null;

            var parts = subject.Split(' ');
            return new Subject { Name = parts[0], StartTime = TimeSpan.Parse(parts[1]), EndTime = TimeSpan.Parse(parts[2]) };
        }
    }

    public class Subject
    {
        public string Name { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan Duration => EndTime.Subtract(StartTime).Duration();
    }
}
