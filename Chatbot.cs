using System.Text.RegularExpressions;
namespace CyberSecurityBotGUI1
{
    //Main Chatbot class
    public class Chatbot
    {
        private DatabaseHelper db;
        private List<string> actionHistory;
        private List<Question> questions;
        private List<string> activityLog = new List<string>();

        private int score = 0;
        private int currentQuestion = 0;
        private bool quizMode = false;

        private Random random;

        public Chatbot()
        {
            db = new DatabaseHelper();
            random = new Random();

            actionHistory = new List<string>();

            questions = new List<Question>()
            {
                new Question()
                {
                  Text = "1. What should you do if you receive an email asking for your password?\n" +
                         "A) Reply with your password\n" +
                         "B) Click the link in the email\n" +
                         "C) Report it as phishing\n" +
                         "D) Ignore it",

                    Answer = "C",

                    Explanation = "Correct! Reporting phishing emails helps prevent scams."
                },

                new Question()
                {
                    Text = "2. True or False: Passwords should be shared with friends.\n",
                    Answer = "False",
                    Explanation = "Correct! Passwords should never be shared to maintain security."
                },

                new Question()
                {
                    Text = "3. What is the purpose of two-factor authentication (2FA)?\n" +
                           "A) To make logging in faster\n" +
                           "B) To provide an extra layer of security\n" +
                           "C) To remember your password\n" +
                           "D) To allow multiple users to log in",
                    Answer = "B",
                    Explanation = "Correct! 2FA adds an extra layer of security by requiring a second form of verification."
                },

                new Question()
                {
                    Text = "4. What is a common sign of a phishing attempt?\n" +
                           "A) A well-known sender\n" +
                           "B) A request for personal information\n" +
                           "C) A secure website\n" +
                           "D) A professional email signature",
                    Answer = "B",
                    Explanation = "Correct! Phishing attempts often ask for personal information."
                },

                new Question()
                {
                      Text = "5. True or False: Using the same password for multiple accounts is safe.\n",
                    Answer = "False",
                    Explanation = "Correct! Using the same password for multiple accounts increases the risk of compromise."
                },

                new Question()
                {
                    Text = "6. What is the best way to protect your online accounts?\n" +
                           "A) Use strong, unique passwords\n" +
                           "B) Share your passwords with friends\n" +
                           "C) Use the same password for all accounts\n" +
                           "D) Avoid using two-factor authentication",
                    Answer = "A",
                    Explanation = "Correct! Strong, unique passwords help protect your online accounts."
                },

                new Question()
                {
                    Text = "7. What should you do if you suspect a website is not secure?\n" +
                           "A) Enter your personal information\n" +
                           "B) Leave the website immediately\n" +
                           "C) Ignore the warning and continue\n" +
                           "D) Share the website with friends",
                    Answer = "B",
                    Explanation = "Correct! Leaving a suspicious website helps protect your personal information."
                },

                new Question()
                {
                    Text = "8. True or False: It's safe to click on links from unknown sources.\n",
                    Answer = "False",
                    Explanation = "Correct! Clicking on links from unknown sources can lead to malware or phishing attacks."
                },

                new Question()
                {
                    Text = "9. What is the purpose of a firewall?\n" +
                           "A) To block unauthorized access to a network\n" +
                           "B) To speed up internet connection\n" +
                           "C) To store passwords securely\n" +
                           "D) To monitor social media activity",
                    Answer = "A",
                    Explanation = "Correct! A firewall helps protect a network by blocking unauthorized access."
                },

                new Question()
                {
                    Text = "10. True or False: Regularly updating your software helps protect against security vulnerabilities.\n",
                    Answer = "True",
                    Explanation = "Correct! Keeping software up to date helps protect against known security vulnerabilities."
                }


            };



        }

        //Random object for generating random responses
        Random rand = new Random();

        //Last topic discussed
        string lastTopic = "";

        //Dictionary of cybersecurity topics and similar responses
        Dictionary<string, List<string>> responses =
            new Dictionary<string, List<string>>()
            {
                {
                    "password",
                    new List<string>()
                    {
                        "Use strong passwords.",
                        "Enable two-factor authentication.",
                        "Avoid reusing passwords."
                    }
                },

                {
                    "phishing",
                    new List<string>()
                    {
                        "Do not click on suspicious links.",
                        "Verify email senders carefully.",
                        "Be careful of fake websites."
                    }

                }
            };


        //Main method to get chatbot response based on user input

        public string GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please type something.";
            }

            if (Regex.IsMatch(input,
                @"\b(remind me|remember to|set a reminder)\b"))
            {
                return "I can help you set a reminder.";
            }

            if (input.Contains("remind me")
                || input.Contains("remember to")
                || input.Contains("set a reminder"))
            {
                string taskText =
                     input.Replace("remind me", "")
                     .Replace("remember to", "")
                     .Replace("set a reminder", "")
                     .Trim();

                actionHistory.Add(
                    $"Reminder set for '{taskText}' tomorrow.");
                AddtoLog("Reminder set for: " + taskText);

                return $"Reminder set for '{taskText}' tomorrow's date.";
            }

            // Start quiz
            AddtoLog("Quiz started.");
            if (input.Contains("quiz")
                || input.Contains("start quiz")
                || input.Contains("test me")
                || input.Contains("ask me questions"))

            {
                quizMode = true;
                score = 0;
                currentQuestion = 0;

                return "Cybersecurity Quiz Started!\n\n" +
                       questions[currentQuestion].Text;
            }

            if (quizMode)
            {
                bool correct =
                    input.Equals(questions[currentQuestion].Answer,
                    StringComparison.OrdinalIgnoreCase);

                string response;

                if (correct)
                {
                    score++;

                    response = "Correct!\n " +
                               questions[currentQuestion].Explanation;
                }
                else
                {
                    response = "Incorrect.\nCorrect Answer: " +
                        questions[currentQuestion].Answer + "\n" +
                        questions[currentQuestion].Explanation;
                }

                currentQuestion++;

                if (currentQuestion >= questions.Count)
                {
                    quizMode = false;

                    return response + "\n\nQuiz Completed!\nYour score: {score} /{ questions.Count}";
                  
                }
                return response + "\n\nNext Question:\n\n" +
                       questions[currentQuestion].Text;

            }

            if (input.Contains("add task")
                || input.Contains("add a task")
                || input.Contains("create task")
                || input.Contains("create a task")
                || input.Contains("new task"))
            {
                string taskText = input.Replace("add task", "")
                                       .Replace("add a task", "")
                                       .Replace("create task", "")
                                       .Replace("create a task", "")
                                       .Replace("new task", "")
                                       .Trim();

                TaskModel task = new TaskModel();

                task.Title = taskText;
                task.Description = taskText;
                task.Reminder = "None";
                task.Status = "Pending";

                db.AddTask(task);
                AddtoLog("Task added: " + taskText);

                actionHistory.Add($"Task added: '{taskText}'.");

                return $"Task added successfully: '{taskText}'. Would you like to set a reminder for this task?";
            }

            if (input == "show tasks")
            {
                List<TaskModel> tasks = db.GetTasks();

                if (tasks.Count == 0)
                {
                    return "No tasks found.";
                }

                string result = "Tasks:\n";

                foreach (TaskModel task in tasks)
                {
                    result += task.Id + ". " + task.Title + " - " + task.Status + "\n";
                }

                return result;
            }

            if (input.StartsWith("complete task"))
            {
                string number = input.Replace("complete task", "").Trim();

                int id = Convert.ToInt32(number);

                db.CompleteTask(id);

                return "Task marked as completed.";
            }

            if (input.StartsWith("delete task"))
            {
                string number = input.Replace("delete task", "").Trim();
                int id = Convert.ToInt32(number);
                db.DeleteTask(id);
                AddtoLog("Task deleted: " + id);
                return "Task deleted successfully.";
            }

            input = input.Trim();




            //Error Handling
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please type something.";
            }

            //Sentiment Detection
            string mood = SentimentAnalyzer.Detect(input);

            if (mood == "worried")
            {
                return "I understand your concern. Let me help you stay safe online.";
            }

            if (mood == "confused")
            {
                return "No problem. I will explain it in a simplified way";
            }

            //Password responses
            if (input.Contains("password")
                || input.Contains("passwords")
                || input.Contains("strong password")
                || input.Contains("secure password"))
            {
                AddtoLog("Password assistance required.");
                return "Passwords are the first line of defense. Use strong, unique passwords for each account.";
            }

            //Phishing responses
            if (input.Contains("phishing")
                || input.Contains("scam")
                || input.Contains("fake email")
                || input.Contains("suspicious email"))
            {

                AddtoLog("Phishing information requested.");
                return "Phishing scams trick users into sharing sensitive information. Always verify suspicious emails carefully.";
            }

            //Safe Browsing responses
            if (input.Contains("safe browsing"))
            {
                lastTopic = "safe browsing";
                return "Use secure browsers and keep them updated to protect against online threats.";
            }



            //Memory Recall
            if (input.Contains("tip"))
            {

                return "Here's a cybersecurity tip: Use strong passwords and enable two-factor authentication for better security.";
            }

            //Conversation Flow
            if (input.Contains("tell me more"))
            {
                if (lastTopic == "phishing")
                {
                    return "Never rush to open emails that seem sketchy, especially those flagged as potential spam.";
                }

                if (lastTopic == "password")
                {
                    return "Avoid using personal information in your passwords.";
                }

                if (lastTopic == "safe browsing")
                {
                    return "Be cautious when downloading files from the internet.";
                }
            }



            //Loop through keywords in the dictionary
            foreach (var keyword in responses.Keys)
            {

                //Check if the input contains the keyword
                if (input.Contains(keyword))
                {
                    var list = responses[keyword];

                    int index = rand.Next(list.Count);

                    return list[index];

                }
            }

            if (input.Contains("what have you done?")
                || input.Contains("summary")
                || input.Contains("recent actions"))

            {
                if (actionHistory.Count == 0)
                {
                    return "No recent actions found.";
                }

                string result = "Here's a summary of recent actions:\n";

                for (int i = 0; i < actionHistory.Count; i++)
                {
                    result += (i + 1) + ". " + actionHistory[i] + "\n";
                }

                return result;
            }

            if (input.Contains("show activity")
                || input.Contains("activity log")
                || input.Contains("what have you done"))

            {
                return ShowActivityLog();
            }





            //Default response if no keywords match
            return "You can ask me about passowrds, phishing, tasks, reminders, or the cybersecurity quiz.";




        }

        private void AddtoLog(string action)
        {
            string entry = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + action;

            activityLog.Add(entry);

            if (activityLog.Count > 10)
            {
                activityLog.RemoveAt(0);
            }
        }

        private string ShowActivityLog()
        {
            if (activityLog.Count == 0)
            {
                return "No activity has been recorded yet.";
            }
            string result = "Recent Actions:\n";
            foreach (string action in activityLog)
            {
                result += action + "\n";
            }
            return result;
        }
    }
}



