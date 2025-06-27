using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CyberBotPart3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChatBotWindow : Window
    {
        // Conversation state
        private string currentTopic = string.Empty;
        private string userName = "User";
        private bool cancel = false;
        private bool isAwaitingInput = false;
        private bool isTyping = false;

        private static readonly string BaseTextFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "txt files");

        private static readonly string TaskFilePath = System.IO.Path.Combine(BaseTextFolder, "tasks.txt");
        private static readonly string ActivityLogPath = System.IO.Path.Combine(BaseTextFolder, "activity_log.txt");



        // Topic memory flags
        private bool talkedAboutPhishing = false;
        private bool talkedAboutPassword = false;
        private bool talkedAboutBrowsing = false;
        private bool talkedAboutPrivacy = false;
        private bool talkedAboutScam = false;
        private bool talkedAboutCybersecurity = false;

        // Tip collections
        private List<string> phishingTips = new List<string>
        {
            "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
            "Check the sender's email address—phishers often use fake but similar addresses.",
            "Never click on suspicious links. Hover over them first to see the actual URL.",
            "Look out for grammar mistakes or urgent messages—they are common signs of phishing."
        };

        private List<string> passwordTips = new List<string>
        {
            "Don't reuse passwords across sites.",
            "Use a password manager to generate and store secure passwords.",
            "Enable two-factor authentication where possible."
        };

        List<string> browsingTips = new()
            {
                "Keep your browser and software up to date.",
                "Avoid clicking on unknown links or ads.",
                   "Use secure websites with HTTPS."
            };

        List<string> privacyTips = new()
            {
                "Limit the personal information you share online.",
                "Review privacy settings on social media platforms.",
                "Be cautious when using public Wi-Fi networks."
            };

        List<string> scamTips = new()
            {
                 "Always double-check the sender’s identity.",
                 "Don’t respond to messages requesting urgent payment or personal info.",
                 "Legitimate companies don’t ask for sensitive info over email."
            };

        List<string> cybersecurityTips = new()
            {
                "Install antivirus software and keep it updated.",
                "Use strong, unique passwords for each account.",
                "Avoid using public Wi-Fi for sensitive transactions."
            };

        // Other tip lists (browsingTips, privacyTips, etc.) would go here...

        // Sentiment detection
        private List<(string keyword, string response)> sentimentKeywords = new List<(string, string)>
                    {
                        ("worried", "CyberBot: It's okay to feel worried. Cybersecurity can be tricky, but I'm here to help guide you."),
                        ("stressed", "CyberBot: I understand that cybersecurity can feel overwhelming. You're not alone—ask me anything, and we’ll take it step by step."),
                        ("scared", "CyberBot: Don’t worry, you're doing the right thing by learning. I'm here to help you stay safe online."),
                        ("confused", "CyberBot: If something seems confusing, feel free to ask. I’ll explain it in a simpler way."),
                        ("frustrated", "CyberBot: I know it can be frustrating. Let’s work through it together!"),
                        ("curious", "CyberBot: Curiosity is the first step to better cybersecurity. What would you like to learn about?")
};


        public ChatBotWindow(string name)
        {
            InitializeComponent();
            userName = name;

            // Initial greeting
            AddBotMessage($"Hello {userName}! I'm CyberBot. Type 'help' for options.");
        }

        // ======================
        // Core Chat Methods
        // ======================

        private void AddUserMessage(string message)
        {
            ConversationHolderTxtBx.AppendText($"You: {message}\n");
            LogActivity("User", message);
            ScrollToBottom();
        }

        private async void AddBotMessage(string message)
        {
            if (isTyping)
                return; // Prevent multiple bot messages from overlapping

            isTyping = true;

            ConversationHolderTxtBx.AppendText("CyberBot: ");
            LogActivity("CyberBot", message);

            foreach (char c in message)
            {
                ConversationHolderTxtBx.AppendText(c.ToString());
                await Task.Delay(15);
            }

            ConversationHolderTxtBx.AppendText("\n");
            ScrollToBottom();

            isTyping = false;
        }



        private void ScrollToBottom()
        {
            ConversationHolderTxtBx.ScrollToEnd();
        }

        // ======================
        // Response System
        // ======================

        private void ProcessUserInput(string input)
        {
            string lowerInput = input.ToLower();

            // Task-related
            if (lowerInput.ContainsAny("task", "remind", "todo", "action", "reminder"))
            {
                HandleTaskCreation(input);
                return;
            }

            // Direct commands
            if (lowerInput.ContainsAny("help"))
            {
                ShowHelpMenu();
                return;
            }
            if (lowerInput.ContainsAny("exit", "quit", "close"))
            {
                AddBotMessage("Goodbye! Stay safe online.");
                Close();
                return;
            }

            // Sentiment
            foreach (var (keyword, response) in sentimentKeywords)
            {
                if (lowerInput.Contains(keyword))
                {
                    AddBotMessage(response);
                    return;
                }
            }

            // 🔐 Topic matching (must come before small talk)
            if (lowerInput.ContainsAny("phish", "scam email", "email trick", "fake message"))
            {
                HandlePhishingTopic();
                return;
            }
            if (lowerInput.ContainsAny("password", "strong password", "secure login"))
            {
                HandlePasswordTopic();
                return;
            }
            if (lowerInput.ContainsAny("browsing", "internet", "web", "surf", "online safety"))
            {
                HandleBrowsingTopic();
                return;
            }
            if (lowerInput.ContainsAny("privacy", "private", "data leak", "personal info"))
            {
                HandlePrivacyTopic();
                return;
            }
            if (lowerInput.ContainsAny("scam", "fraud", "fake", "deceive", "spoof"))
            {
                HandleScamTopic();
                return;
            }
            if (lowerInput.ContainsAny("cybersecurity", "cyber security", "cyber safety"))
            {
                HandleCybersecurityTopic();
                return;
            }

            // ✅ Small talk — LAST priority
            if (lowerInput.ContainsAny("hello", "hi", "hey"))
            {
                AddBotMessage($"Hello {userName}, I'm here to help with your cybersecurity questions!");
                return;
            }
            if (lowerInput.ContainsAny("how are you", "how's it going", "how do you feel"))
            {
                AddBotMessage("I'm just a bot, but thanks for asking! I'm running securely and ready to chat. 😊");
                return;
            }
            if (lowerInput.ContainsAny("thank you", "thanks"))
            {
                AddBotMessage("You're welcome! Stay safe online. 🔐");
                return;
            }
            if (lowerInput.ContainsAny("what's your name", "who are you"))
            {
                AddBotMessage("I'm CyberBot, your cybersecurity assistant. 🤖");
                return;
            }

            // Fallback
            AddBotMessage("I'm not sure about that. Try asking about 'phishing' or 'passwords'.");
        }



        private void HandlePhishingTopic()
        {
            if (!(currentTopic.Equals("phishing")))
            {
                currentTopic = "phishing";
                AddBotMessage("Phishing is when someone tries to trick you into giving personal info by pretending to be someone you trust.");
            }else if (talkedAboutPhishing)
            {
                Random rand = new Random();
                AddBotMessage($"Tip: {phishingTips[rand.Next(phishingTips.Count)]}");
            }

            talkedAboutPhishing = true;
        }

        private void HandlePasswordTopic()
        {
            if (!(currentTopic.Equals("password")))
            {
                currentTopic = "password";
                AddBotMessage("A strong password must be longer than 8 characters and include a mix of uppercase, lowercase, numbers, and symbols.");
            }else if (talkedAboutPassword)
            {
                Random rand = new Random();
                AddBotMessage($"Tip: {passwordTips[rand.Next(passwordTips.Count)]}");
            }

            talkedAboutPassword = true;
        }


        private void HandleBrowsingTopic()
        {
            if (!(currentTopic.Equals("browsing")))
            {
                currentTopic = "browsing";
                AddBotMessage("To browse safely: keep your software updated, use antivirus, and avoid clicking suspicious links.");
            }else if (talkedAboutBrowsing)
            {
                Random rand = new Random();
                AddBotMessage($"Tip: {browsingTips[rand.Next(browsingTips.Count)]}");
            }

            talkedAboutBrowsing = true;
        }


        private void HandlePrivacyTopic()
        {
            if (!(currentTopic.Equals("privacy")))
            {
                currentTopic = "privacy";
                AddBotMessage("Protect your privacy by limiting the personal information you share online.");

            }else if (talkedAboutPrivacy)
            {
                Random rand = new Random();
                AddBotMessage($"Tip: {privacyTips[rand.Next(privacyTips.Count)]}");
            }

            talkedAboutPrivacy = true;
        }


        private void HandleScamTopic()
        {
            if (!(currentTopic.Equals("scam")))
            {
                currentTopic = "scam";
                AddBotMessage("Scams often try to trick you with urgent messages. Always verify the sender before clicking on links or sharing info.");
            }else if (talkedAboutScam)
            {
                AddBotMessage($"Tip: {scamTips[new Random().Next(scamTips.Count)]}");
            }

            talkedAboutScam = true;
        }


        private void HandleCybersecurityTopic()
        {
            if (!(currentTopic.Equals("cybersecurity")))
            {
                currentTopic = "cybersecurity";
                AddBotMessage("Cybersecurity is the practice of protecting systems, networks, and data from cyber threats.");
            }else if (talkedAboutCybersecurity)
            {
                AddBotMessage($"Tip: {cybersecurityTips[new Random().Next(cybersecurityTips.Count)]}");
            }

            talkedAboutCybersecurity = true;
        }


        


        private void ShowHelpMenu()
        {
            string helpText = "I can help with:\n" +
                            "- Phishing scams\n" +
                            "- Password safety\n" +
                            "- Secure browsing\n\n" +
                            "Commands:\n" +
                            "- 'help': Show this menu\n" +
                            "- 'exit': End chat";
            AddBotMessage(helpText);
        }

        // ======================
        // task handler
        // ======================
        // File operations
        

        private List<CyberSecurityTask> LoadTasks()
        {
            var tasks = new List<CyberSecurityTask>();
            if (File.Exists(TaskFilePath))
            {
                foreach (string line in File.ReadAllLines(TaskFilePath))
                {
                    var parts = line.Split('|');
                    tasks.Add(new CyberSecurityTask
                    {
                        Title = parts[0],
                        Description = parts[1],
                        Reminder = parts[2] == "null" ? null : DateTime.Parse(parts[2]),
                        IsComplete = bool.Parse(parts[3])
                    });
                }
            }
            return tasks;
        }

        private async void HandleTaskCreation(string userInput)
        {
            AddBotMessage("Let's create a new cybersecurity task!" + "\nWhat should we call this task? (e.g. 'Enable 2FA')");

           
            string title = await WaitForUserInputAsync();

            AddBotMessage("Please describe the task:");
            string description = await WaitForUserInputAsync();

            AddBotMessage("When should I remind you? (e.g. 'tomorrow', 'in 3 days', or 'no')");
            string reminderInput = await WaitForUserInputAsync();

            DateTime? reminder = ParseReminder(reminderInput);
            SaveNewTask(title, description, reminder);

        }

        private async Task<string> WaitForUserInputAsync()
        {
            isAwaitingInput = true;

            var tcs = new TaskCompletionSource<string>();

            void Handler(object s, RoutedEventArgs e)
            {
                var text = userInputTxt.Text.Trim();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    submitBtn.Click -= Handler;
                    userInputTxt.Clear();
                    isAwaitingInput = false;
                    tcs.TrySetResult(text);
                }
                else
                {
                    AddBotMessage("Please enter a value before submitting.");
                }
            }

            submitBtn.Click += Handler;

            string result = await tcs.Task;

            return result;
        }


        private DateTime? ParseReminder(string input)
        {
            input = input.ToLower().Trim();

            if (input == "no") return null;
            if (input == "today") return DateTime.Today;
            if (input == "tomorrow") return DateTime.Today.AddDays(1);

            if (input.StartsWith("in "))
            {
                if (int.TryParse(input.Substring(3).Split(' ')[0], out int days))
                {
                    return DateTime.Today.AddDays(days);
                }
            }

            if (DateTime.TryParse(input, out DateTime exactDate))
            {
                return exactDate;
            }

            return null;
        }

        private void SaveNewTask(string title, string description, DateTime? reminder)
        {
            var tasks = LoadTasks();
            tasks.Add(new CyberSecurityTask
            {
                Title = title,
                Description = description,
                Reminder = reminder
            });
            SaveTasks(tasks);

            AddBotMessage($"✅ Task saved: {title}" +
                (reminder.HasValue ? $" (Reminder set for {reminder:dd-MMM-yyyy})" : ""));
        }

        private void SaveTasks(List<CyberSecurityTask> tasks)
        {
            File.WriteAllLines(TaskFilePath, tasks.Select(t => t.ToString()));
        }



        // ======================
        // Button Event Handlers
        // ======================

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isAwaitingInput)
            {
                // Ignore this click because WaitForUserInputAsync handles it
                return;
            }

            string userInput = userInputTxt.Text.Trim();
            if (!string.IsNullOrEmpty(userInput))
            {
                AddUserMessage(userInput);
                ProcessUserInput(userInput);
                userInputTxt.Clear();
            }
        }

        private void minigameBtn_Click(object sender, RoutedEventArgs e)
        {
            // Open the mini-game window and pass the user's name (if needed)
            var miniGameWindow = new cyberMinigameWindow();

            // Optional: Pass data (e.g., user name) to personalize the game
            // miniGameWindow.UserName = userName; 

            miniGameWindow.Show();  // Show as a standalone window

            // Optional: Hide the chatbot window while playing
            this.Hide();
            miniGameWindow.Closed += (s, args) => this.Show(); // Re-show chatbot when game closes

        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the chatbot window
        }

        private void taskManagerBtn_Click(object sender, RoutedEventArgs e)
        {
            var tasks = LoadTasks();
            var taskList = new StringBuilder("Your Cybersecurity Tasks:\n\n");

            foreach (var task in tasks)
            {
                taskList.AppendLine($"• {task.Title}");
                taskList.AppendLine($"  Description: {task.Description}");
                if (task.Reminder.HasValue)
                    taskList.AppendLine($"  ⏰ Reminder: {task.Reminder:dd-MM-yyyy}");
                taskList.AppendLine();
            }

            AddBotMessage(taskList.ToString());
        }

       

        private void LogActivity(string sender, string message)
        {
            string logEntry = $"{DateTime.Now:dd/MM/yyyy HH:mm} | {sender}: {message}";
            File.AppendAllLines(ActivityLogPath, new[] { logEntry });
        }

        private void activityLogBtn_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(ActivityLogPath))
            {
                string[] lines = File.ReadAllLines(ActivityLogPath);
                AddBotMessage("📒 Activity Log:\n" + string.Join("\n", lines));
            }
            else
            {
                AddBotMessage("No activity has been logged yet.");
            }
        }




    }
}