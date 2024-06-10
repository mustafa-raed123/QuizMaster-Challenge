using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;


namespace QuizMaster_Challenge
{
    internal class Program
    {
        
        static  int score = 0;


        static void Main(string[] args)
        {
            Console.WriteLine("Welcome");
            Console.WriteLine("for each question you have 10 seconds ");
            Console.WriteLine("Press Enter to start the quiz");
            Console.ReadKey();

            try
            {
                StartQuiz();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Finished quiz");
            }
        }

        public static async void StartQuiz()
        {
            int score = 0;
            int i = 0;
            List<string> questions = new List<string> {
                "What is the capital of Jordan:",
                "What is 5 * 6:",  
                "What is 100/4:",
                "What is 10*9:",
                "What is 4 + 10:",
                "What is 5 - 6:",
                "What is 20 - 10:",
                "What is 10/2:",
                "What is 15*10:"
            };

            List<string> answers = new List<string> {

                "AMMAN",
                "30",               
                "25",
                "90",
                "14",
                "-1",
                "10",
                "5",
                "150"
            };

            while (i < questions.Count)
            {
                 

                try
                {
                    Console.WriteLine(questions[i]);
                    string userAnswer = GetUserAnswerWithTimeout().Result;
                    

                    if (userAnswer == "TIME'S UP")
                    {
                        Console.WriteLine("You took too long to answer.");
                    }
                    else
                    {
                        bool isParsed = int.TryParse(answers[i], out int correctNumber);
                        if (isParsed)
                        {
                            if (int.TryParse(userAnswer, out int userNumber))
                            {
                                IsTrue(userNumber == correctNumber, answers[i],ref score);
                            }
                            else
                            {
                                throw new FormatException("Invalid input. Expected a number.");
                            }
                        }
                        else if(!(String.IsNullOrWhiteSpace(userAnswer)))
                        {
                            IsTrue(userAnswer == answers[i], answers[i], ref score);
                        }else
                        { 
                               throw new FormatException("Invalid input. Expected a number.");
                        }
                    }
                    i++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Do you want to try the question again? (y/n)");
                    string retry = Console.ReadLine().ToUpper();
                    if (retry != "Y" && retry != "YES")
                    {
                        i++;
                    }
                }
            }
            Console.WriteLine($"Your score is: {score}");
        }

        private static void IsTrue(bool isCorrect, string correctAnswer, ref int score)
        {
            if (isCorrect)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Correct answer");
                Console.ResetColor();
                score++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not correct");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"The correct answer is: {correctAnswer.ToLower()}");
                Console.ResetColor();
            }

        }
        private static async Task<string> GetUserAnswerWithTimeout()
        {
            var timer = new Stopwatch();
            timer.Start();

            var inputTask = Task.Run(() => Console.ReadLine().ToUpper());
            var delayTask = Task.Delay(10000); 

            var completedTask = await Task.WhenAny(inputTask, delayTask);

            timer.Stop();

            if (completedTask == inputTask)
            {
                TimeSpan timeTaken = timer.Elapsed;
                return inputTask.Result;
            }
            else
            {
                return "TIME'S UP";
            }
        }
    }
}