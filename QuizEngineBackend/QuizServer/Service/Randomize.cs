//using QuizServer.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;

//namespace QuizServer.Service
//{
//    public class Randomize
//    {
//        static void randomiseList(List<Question> questionBank)
//        {
//            List<Question> QuestionBank = new List<Question>()
//        {
//            new Question()
//            {
//                QuestionId = "1",
//                QuestionText = "Who is the CM of WestBengal?",
//                Options = new List<string>()
//                    {
//                        "Siddu",
//                        "Didi",
//                        "Modi",
//                        "RaGa"
//                    }
//            },


//            new Question()
//            {
//                QuestionId = "2",
//                QuestionText = "Who is the CM of Karnataka?",
//                Options = new List<string>()
//                    {
//                        "deepu",
//                        "shashaidar",
//                        "Deepika",
//                        "deepthi"
//                    }
//            },

//            new Question()
//            {
//                QuestionId = "3",
//                QuestionText = "Who is the CM of Kerala?",
//                Options = new List<string>()
//                    {
//                        "Siddu",
//                        "Didi",
//                        "Modi",
//                        "RaGa"
//                    }
//            },
//            new Question()
//            {
//                QuestionId = "4",
//                QuestionText = "Who is the best ----- developer in stack route?",
//                Options = new List<string>()
//                    {
//                        "deepu",
//                        "deepu",
//                        "deepu",
//                        "FE"
//                    }
//            }
//        };



//            //var numbers = new List<int>(Enumerable.Range(1, 10));
//            questionBank.Shuffle();
//            Console.WriteLine("The winning numbers are: {0}", string.Join(",  ", questionBank));
//            questionBank.Shuffle();
//            Console.WriteLine("The winning numbers are: {0}", string.Join(",  ", questionBank));
//            questionBank.Shuffle();
//            Console.WriteLine("The winning numbers are: {0}", string.Join(",  ", questionBank));
//            questionBank.Shuffle();
//            Console.WriteLine("The winning numbers are: {0}", string.Join(",  ", questionBank));

//            //randomiseList(numbers);
//            randomiseList(QuestionBank);
//        }
//    }


//    public static class ThreadSafeRandom
//    {
//        [ThreadStatic] private static Random Local;

//        public static Random ThisThreadsRandom
//        {
//            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
//        }
//    }

//    static class MyExtensions
//    {
//        public static void Shuffle<T>(this List<T> list)
//        {
//            int n = list.Count;
//            while (n > 1)
//            {
//                n--;
//                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
//                T value = list[k];
//                list[k] = list[n];
//                list[n] = value;
//            }
//        }
//    }


//}


