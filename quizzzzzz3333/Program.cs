using System;
using System.Collections.Generic;

/*
    quizzzzzz3333 - A simple console quiz application in C#
    Version: 1.0.0
 */

namespace quizzzzzz3333
{
    enum Section
    {
        GeneralScience = 1,
        Biology,
        Geography
    }

    class Question
    {
        public string Text { get; }
        public string[] Options { get; }
        public char CorrectOption { get; }

        public Question(string text, string[] options, char correctOption)
        {
            Text = text;
            Options = options;
            CorrectOption = char.ToUpper(correctOption);
        }

        public bool CheckAnswer(char input) =>
            char.ToUpper(input) == CorrectOption;
    }

    class PlayerResult
    {
        public string Name { get; }
        public string SectionName { get; }
        public int Score { get; }
        public int Total { get; }
        public DateTime TimeStamp { get; }

        public PlayerResult(string name, string sectionName, int score, int total)
        {
            Name = name;
            SectionName = sectionName;
            Score = score;
            Total = total;
            TimeStamp = DateTime.Now;
        }

        public override string ToString() => $"{Name} | {SectionName} | {Score}/{Total} | {TimeStamp}";

        public void SaveToDesktop(string fileName)
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string path = Path.Combine(desktop, fileName);
            File.AppendAllText(path, ToString() + Environment.NewLine);
        }
    }

    class Quiz
    {
        // Questions grouped by section
        private readonly Dictionary<Section, List<Question>> allQuestions;

        public Quiz()
        {
            allQuestions = new Dictionary<Section, List<Question>>
            {
                [Section.GeneralScience] = new List<Question>
            {
                new Question("What planet is known as the Red Planet?",
                    new[]{"Mars","Venus","Jupiter","Mercury"}, 'A'),
                new Question("What is the chemical symbol for water?",
                    new[]{"H2O","O2","CO2","HO"}, 'A'),
                new Question("How many continents are there?",
                    new[]{"5","6","7","8"}, 'C'),
                new Question("What gas do plants absorb from the atmosphere?",
                    new[]{"Oxygen","Nitrogen","Carbon Dioxide","Hydrogen"}, 'C'),
                new Question("What is the hardest natural substance on Earth?",
                    new[]{"Gold","Iron","Diamond","Platinum"}, 'C'),
                new Question("What part of the atom has no electric charge?",
                    new[]{"Proton","Neutron","Electron","Nucleus"}, 'B'),
                new Question("What is the main gas found in the air we breathe?",
                    new[]{"Oxygen","Nitrogen","Carbon Dioxide","Hydrogen"}, 'B'),
                new Question("What force keeps us on the ground?",
                    new[]{"Magnetism","Friction","Gravity","Inertia"}, 'C'),
                new Question("Which organ in the human body is primarily responsible for detoxification?",
                    new[]{"Kidneys","Liver","Lungs","Heart"}, 'B'),
                new Question("What is the phenomenon where a moving object emits a sound that increases in frequency as it approaches an observer and decreases as it moves away?",
                    new[]{"Doppler Effect","Refraction","Diffraction","Interference"}, 'A'),
                new Question("If a train travels 60 kilometers in 1 hour, how many minutes does it take to travel 180 kilometers?",
                    new[]{"60 mins","120 mins","180 mins","90 mins"}, 'C'),
            },
                [Section.Biology] = new List<Question>
            {
                new Question("What is the powerhouse of the cell?",
                    new[]{"Ribosome","Mitochondria","Nucleus","Golgi"}, 'B'),
                new Question("Human blood type O is often called?",
                    new[]{"Universal donor","Universal recipient", "Rare","None"}, 'A'),
                new Question("Why do your fingers and toes get wrinkled when you spend a long time in water?",
                    new[]{"The outer layer of your skin absorbs water and swells up.", "It is an automatic nervous system response that is thought to have evolved to help with gripping wet objects.",
                        "The skin contracts to prevent water loss from the body.", "The cold water causes the blood vessels to constrict, wrinkling the skin." }, 'B')
            },
                [Section.Geography] = new List<Question>
            {
                new Question("What is the capital of Australia?",
                    new[]{"Sydney","Melbourne","Canberra","Brisbane"}, 'C'),
                new Question("Which is the largest ocean?",
                    new[]{"Atlantic","Pacific","Indian","Arctic"}, 'B'),
                new Question("The Nile river flows into which sea?",
                    new[]{"Red Sea","Mediterranean","Black Sea","Arabian"}, 'B'),
                new Question("What is the name of the tallest mountain in the world?",
                    new[]{"Mount Fuji", "Mount Kilimanjaro", "Mount Everest", "Mount Olympus " }, 'C'),
            }
            };
        }

        public PlayerResult Start()
        {
            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine() ?? "Anonymous";

            // 1️⃣  Ask for section
            Console.WriteLine("\nChoose a section:");
            foreach (Section s in Enum.GetValues(typeof(Section)))
                Console.WriteLine($"{(int)s}. {s}");

            Section chosen = ReadSection();

            //  Run quiz only for that section
            var questions = allQuestions[chosen];
            int score = 0;
            foreach (var q in questions)
            {
                Console.WriteLine("\n" + q.Text);
                Console.WriteLine($"A) {q.Options[0]}");
                Console.WriteLine($"B) {q.Options[1]}");
                Console.WriteLine($"C) {q.Options[2]}");
                Console.WriteLine($"D) {q.Options[3]}");
                if (q.CheckAnswer(ReadOption())) { Console.WriteLine("Correct!"); score++; }
                else Console.WriteLine($"Wrong. Correct answer: {q.CorrectOption}");
            }

            return new PlayerResult(playerName, chosen.ToString(), score, questions.Count);
        }

        private Section ReadSection()
        {
            while (true)
            {
                Console.Write("Enter number of your choice: ");
                string input = Console.ReadLine() ?? "";
                if (int.TryParse(input, out int n) &&
                    Enum.IsDefined(typeof(Section), n))
                    return (Section)n;

                Console.WriteLine("Invalid section. Try again.");
            }
        }

        private char ReadOption()
        {
            while (true)
            {
                Console.Write("Choose A, B, C, or D: ");
                var key = Console.ReadKey(true).KeyChar;
                char c = char.ToUpper(key);
                if ("ABCD".Contains(c))
                {
                    Console.WriteLine(c);
                    return c;
                }
                Console.WriteLine("  (Invalid choice, try again)");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Quiz quiz = new Quiz();
            PlayerResult result = quiz.Start();
            Console.WriteLine($"\n{result}");
            result.SaveToDesktop("results.txt");
            Console.WriteLine("Result saved to your Desktop.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

}
