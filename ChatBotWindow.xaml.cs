using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CyberBotPart3
{
    public partial class ChatBotWindow : Window
    {
        // Conversation state variables
        private string currentTopic = string.Empty;
        private string userName = "User";
        private bool cancel = false;
        private bool isAwaitingInput = false;
        private bool isTyping = false;

        private TipsHolder tips = new TipsHolder();

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

        // Sentiment detection keywords and responses
        public System.Collections.Generic.List<(string keyword, string response)> sentimentKeywords = new System.Collections.Generic.List<(string, string)>
        {
            ("worried", "CyberBot: It's okay to feel worried. Cybersecurity can be tricky, but I'm here to help guide you."),
            ("stressed", "CyberBot: I understand that cybersecurity can feel overwhelming. You're not alone—ask me anything, and we’ll take it step by step."),
            ("scared", "CyberBot: Don’t worry, you're doing the right thing by learning. I'm here to help you stay safe online."),
            ("confused", "CyberBot: If something seems confusing, feel free to ask. I’ll explain it in a simpler way."),
            ("frustrated", "CyberBot: I know it can be frustrating. Let’s work through it together!"),
            ("curious", "CyberBot: Curiosity is the first step to better cybersecurity. What would you like to learn about?")
        };
        private Dictionary<string, List<string>> intentMap = new Dictionary<string, List<string>>
        {
            { "add_task", new List<string> { "add task", "new task", "to-do", "set a task", "create reminder", "make a task" } },
            { "reminder", new List<string> { "remind me", "reminder", "set reminder", "alert me" } },
            { "log", new List<string> { "show history", "activity log", "what did i do", "what have you done", "log" } },
            { "help", new List<string> { "help", "show help", "assist me" } },
            { "exit", new List<string> { "exit", "quit", "close", "bye" } },
            { "minigame", new List<string> { "minigame", "play game", "start game" } }
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

        private async Task AddBotMessage(string message)
        {
            if (isTyping)
                return; // Prevent overlapping bot messages

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

        

            private async Task ProcessUserInput(string input)
        {
            string lowerInput = input.ToLower();

            // Use fuzzy NLP matching
            string detectedIntent = LinguistischeDistance.LinguistiDistance(lowerInput, intentMap);

            switch (detectedIntent)
            {
                case "add_task":
                case "reminder":
                    HandleTaskCreation(input);
                    return;

                case "log":
                    await ShowActivityLogAsync();
                    LogStructuredActivity("User requested to view activity log.");
                    return;

                case "minigame":
                    var miniGameWindow = new cyberMinigameWindow();
                    miniGameWindow.Show();
                    this.Hide();
                    miniGameWindow.Closed += (s, args) => this.Show();
                    return;

                case "help":
                    await ShowHelpMenu();
                    return;

                case "exit":
                    await AddBotMessage("Goodbye! Stay safe online.");
                    Close();
                    return;
            }

            // Sentiment detection
            foreach (var (keyword, response) in sentimentKeywords)
            {
                if (lowerInput.Contains(keyword))
                {
                    await AddBotMessage(response);
                    LogActivity("NLP", $"Keyword '{keyword}' detected. Responded accordingly.");
                    LogStructuredActivity($"NLP interpreted keyword: '{keyword}' → responded with guidance.");

                    return;
                }
            }

            // Topic matching (priority order)
            if (lowerInput.ContainsAny("phish", "scam email", "email trick", "fake message"))
            {
                await HandlePhishingTopic();
                return;
            }
            if (lowerInput.ContainsAny("password", "strong password", "secure login"))
            {
                await HandlePasswordTopic();
                return;
            }
            if (lowerInput.ContainsAny("browsing", "internet", "web", "surf", "online safety"))
            {
                await HandleBrowsingTopic();
                return;
            }
            if (lowerInput.ContainsAny("privacy", "private", "data leak", "personal info"))
            {
                await HandlePrivacyTopic();
                return;
            }
            if (lowerInput.ContainsAny("scam", "fraud", "fake", "deceive", "spoof"))
            {
                await HandleScamTopic();
                return;
            }
            if (lowerInput.ContainsAny("cybersecurity", "cyber security", "cyber safety"))
            {
                await HandleCybersecurityTopic();
                return;
            }

            // Small talk last priority
            if (lowerInput.ContainsAny("hello", "hi", "hey"))
            {
                await AddBotMessage($"Hello {userName}, I'm here to help with your cybersecurity questions!");
                
                return;
            }
            if (lowerInput.ContainsAny("how are you", "how's it going", "how do you feel"))
            {
                await AddBotMessage("I'm just a bot, but thanks for asking! I'm running securely and ready to chat. 😊");
                return;
            }
            if (lowerInput.ContainsAny("thank you", "thanks"))
            {
                await AddBotMessage("You're welcome! Stay safe online. 🔐");
                return;
            }
            if (lowerInput.ContainsAny("what's your name", "who are you"))
            {
                await AddBotMessage("I'm CyberBot, your cybersecurity assistant. 🤖");
                return;
            }

            // Fallback
            await AddBotMessage("I'm not sure about that. Try asking about 'phishing' or 'passwords'.");
        }

        // ======================
        // Topic Handlers (unchanged)
        // ======================

        private async Task HandlePhishingTopic()
        {
            if (!(currentTopic.Equals("phishing")))
            {
                currentTopic = "phishing";
                await AddBotMessage("Phishing is when someone tries to trick you into giving personal info by pretending to be someone you trust.");
            }
            else if (talkedAboutPhishing)
            {
                var rand = new Random();
                await AddBotMessage($"Tip: {tips.phishingTips[rand.Next(tips.phishingTips.Count)]}");
            }
            talkedAboutPhishing = true;
        }

        private async Task HandlePasswordTopic()
        {
            if (!(currentTopic.Equals("password")))
            {
                currentTopic = "password";
                await AddBotMessage("A strong password must be longer than 8 characters and include a mix of uppercase, lowercase, numbers, and symbols.");
            }
            else if (talkedAboutPassword)
            {
                var rand = new Random();
                await AddBotMessage($"Tip: {tips.passwordTips[rand.Next(tips.passwordTips.Count)]}");
            }
            talkedAboutPassword = true;
        }

        private async Task HandleBrowsingTopic()
        {
            if (!(currentTopic.Equals("browsing")))
            {
                currentTopic = "browsing";
                await AddBotMessage("To browse safely: keep your software updated, use antivirus, and avoid clicking suspicious links.");
            }
            else if (talkedAboutBrowsing)
            {
                var rand = new Random();
                await AddBotMessage($"Tip: {tips.browsingTips[rand.Next(tips.browsingTips.Count)]}");
            }
            talkedAboutBrowsing = true;
        }

        private async Task HandlePrivacyTopic()
        {
            if (!(currentTopic.Equals("privacy")))
            {
                currentTopic = "privacy";
                await AddBotMessage("Protect your privacy by limiting the personal information you share online.");
            }
            else if (talkedAboutPrivacy)
            {
                var rand = new Random();
                await AddBotMessage($"Tip: {tips.privacyTips[rand.Next(tips.privacyTips.Count)]}");
            }
            talkedAboutPrivacy = true;
        }

        private async Task HandleScamTopic()
        {
            if (!(currentTopic.Equals("scam")))
            {
                currentTopic = "scam";
                await AddBotMessage("Scams often try to trick you with urgent messages. Always verify the sender before clicking on links or sharing info.");
            }
            else if (talkedAboutScam)
            {
                var rand = new Random();
                await AddBotMessage($"Tip: {tips.scamTips[rand.Next(tips.scamTips.Count)]}");
            }
            talkedAboutScam = true;
        }

        private async Task HandleCybersecurityTopic()
        {
            if (!(currentTopic.Equals("cybersecurity")))
            {
                currentTopic = "cybersecurity";
                await AddBotMessage("Cybersecurity is the practice of protecting systems, networks, and data from cyber threats.");
            }
            else if (talkedAboutCybersecurity)
            {
                var rand = new Random();
                await AddBotMessage($"Tip: {tips.cybersecurityTips[rand.Next(tips.cybersecurityTips.Count)]}");
            }
            talkedAboutCybersecurity = true;
        }

        // ======================
        // Help Menu
        // ======================
        private async Task ShowHelpMenu()
        {
            string helpText = "I can help with:\n" +
                              "- Phishing scams\n" +
                              "- Password safety\n" +
                              "- Secure browsing\n" +
                              "- Privacy protection\n" +
                              "- Recognizing scams and fraud\n" +
                              "- General cybersecurity tips\n\n" +
                              "Commands:\n" +
                              "- 'help': Show this menu\n" +
                              "- 'manage' or 'tasks': Manage your tasks (add, delete, complete)\n" +
                              "- 'minigame': Play a cybersecurity mini-game\n" +
                              "- 'show activity log' or 'what have you done': View recent actions I've taken\n" +
                              "- 'exit': End chat\n\n" +
                              "Try asking me about phishing, passwords, browsing safety, privacy, scams, or cybersecurity.";

            await AddBotMessage(helpText);
        }


        // ======================
        // Task Handling
        // ======================

        private System.Collections.Generic.List<CyberSecurityTask> LoadTasks()
        {
            var tasks = new System.Collections.Generic.List<CyberSecurityTask>();

            if (File.Exists(TaskFilePath))
            {
                foreach (string line in File.ReadAllLines(TaskFilePath))
                {
                    try
                    {
                        var parts = line.Split('|');
                        if (parts.Length != 4) continue;

                        var task = new CyberSecurityTask
                        {
                            Title = parts[0],
                            Description = parts[1],
                            Reminder = parts[2] == "null" ? (DateTime?)null : DateTime.Parse(parts[2]),
                            IsComplete = bool.Parse(parts[3])
                        };

                        tasks.Add(task);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return tasks;
        }

        private async void HandleTaskCreation(string userInput)
        {
            await AddBotMessage("Let's create a new cybersecurity task!\nWhat should we call this task? (e.g. 'Enable 2FA')");
            string title = await WaitForUserInputAsync();

            await AddBotMessage("Please describe the task:");
            string description = await WaitForUserInputAsync();

            await AddBotMessage("When should I remind you? (e.g. 'tomorrow', 'in 3 days', or 'no')");
            string reminderInput = await WaitForUserInputAsync();

            DateTime? reminder = ParseReminder(reminderInput);
            await SaveNewTask(title, description, reminder);
        }

        private async Task<string> WaitForUserInputAsync()
        {
            isAwaitingInput = true;

            var tcs = new TaskCompletionSource<string>();

            async void Handler(object s, RoutedEventArgs e)
            {
                var text = userInputTxt.Text.Trim();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    submitBtn.Click -= Handler;
                    AddUserMessage(text);
                    isAwaitingInput = false;
                    userInputTxt.Clear();
                    tcs.TrySetResult(text);
                }
                else
                {
                    await AddBotMessage("Nope cant leave it blank. ;)");
                }
            }

            submitBtn.Click += Handler;

            string result = await tcs.Task;
            return result;
        }

        private async Task HandleTaskManagement()
        {
            var tasks = LoadTasks();

            if (tasks.Count == 0)
            {
                await AddBotMessage("📭 You don't have any tasks.");
                return;
            }

            // Show tasks with index
            await AddBotMessage("Here are your tasks:");
            for (int i = 0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                string status = task.IsComplete ? "✅ Completed" : "🕒 Pending";
                string reminderText = task.Reminder.HasValue ? $" (Remind on {task.Reminder.Value:dd-MMM-yyyy})" : "";
                await AddBotMessage($"{i + 1}. {task.Title} - {status}{reminderText}");
            }

            await AddBotMessage("Enter the task number you want to manage (or type 'cancel'):");
            string choice = (await WaitForUserInputAsync()).Trim().ToLower();

            if (choice == "cancel")
            {
                await AddBotMessage("❌ Task management cancelled.");
                return;
            }

            if (int.TryParse(choice, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
            {
                var selectedTask = tasks[taskNumber - 1];
                await AddBotMessage($"📝 You selected: {selectedTask.Title}\nType 'delete' to remove it or 'complete' to mark it as done:");

                string action = (await WaitForUserInputAsync()).Trim().ToLower();

                if (action == "delete")
                {
                    LogActivity("Task", $"Deleted task '{selectedTask.Title}'");
                    tasks.RemoveAt(taskNumber - 1);
                    SaveTasks(tasks);
                    await AddBotMessage("🗑️ Task deleted.");
                    LogStructuredActivity($"Task deleted: '{selectedTask.Title}'");

                }
                else if (action == "complete")
                {
                    selectedTask.IsComplete = true;
                    SaveTasks(tasks);
                    LogActivity("Task", $"Marked task '{selectedTask.Title}' as completed");
                    await AddBotMessage("✅ Task marked as completed.");
                    LogStructuredActivity($"Task completed: '{selectedTask.Title}'");


                }
                else if (action == "cancel")
                {
                    await AddBotMessage("❌ Action cancelled.");
                }
                else
                {
                    await AddBotMessage("⚠️ Action not recognized. Please try again.");
                }
            }
            else
            {
                await AddBotMessage("❌ Invalid task number. Try again or type 'cancel'.");
            }
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

            if (int.TryParse(input, out int directDays))
            {
                return DateTime.Today.AddDays(directDays);
            }

            if (DateTime.TryParse(input, out DateTime exactDate))
            {
                return exactDate;
            }

            return null;
        }

        private async Task SaveNewTask(string title, string description, DateTime? reminder)
        {

            var tasks = LoadTasks();
            tasks.Add(new CyberSecurityTask
            {
                Title = title,
                Description = description,
                Reminder = reminder
            });
            SaveTasks(tasks);

            await AddBotMessage($"✅ Task saved: {title}" + (reminder.HasValue ? $" (Reminder set for {reminder:dd-MMM-yyyy})" : ""));
            LogActivity("Task", $"Added task '{title}'");
            if (reminder.HasValue)
            {
                LogActivity("Reminder", $"Reminder set for task '{title}' on {reminder:dd-MMM-yyyy}");
            }

            LogStructuredActivity($"Task added: '{title}'" + (reminder.HasValue ? $" (Reminder set for {reminder:dd-MMM-yyyy})" : ""));



        }

        private void SaveTasks(System.Collections.Generic.List<CyberSecurityTask> tasks)
        {
            File.WriteAllLines(TaskFilePath, tasks.Select(t => t.ToString()));
        }

        // ======================
        // Logging and Activity Log Viewing
        // ======================

        private void LogStructuredActivity(string summary)
        {
            string logEntry = $"{DateTime.Now:dd/MM/yyyy HH:mm} | {summary}";
            File.AppendAllLines(ActivityLogPath, new[] { logEntry });
        }

        private void LogActivity(string category, string description)
        {
            string logEntry = $"{DateTime.Now:dd/MM/yyyy HH:mm} | {category}: {description}";
            File.AppendAllLines(ActivityLogPath, new[] { logEntry });
        }


        private async Task ShowActivityLogAsync()
        {
            if (File.Exists(ActivityLogPath))
            {
                string[] lines = File.ReadAllLines(ActivityLogPath);
                var recentEntries = lines.Reverse().Take(10).Reverse(); // Last 10 entries
                var formattedLog = "📒 Here's a summary of recent actions:\n";

                int count = 1;
                foreach (var line in recentEntries)
                {
                    formattedLog += $"{count}. {line.Split('|')[1].Trim()}\n";
                    count++;
                }

                await AddBotMessage(formattedLog);
            }
            else
            {
                await AddBotMessage("No activity has been logged yet.");
            }
        }

        // ======================
        // Button Event Handlers
        // ======================

        private async void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isAwaitingInput)
            {
                // Ignore because handled by WaitForUserInputAsync
                return;
            }

            string userInput = userInputTxt.Text.Trim();
            if (!string.IsNullOrEmpty(userInput))
            {
                AddUserMessage(userInput);
                await ProcessUserInput(userInput);
                userInputTxt.Clear();
            }
        }

        private void minigameBtn_Click(object sender, RoutedEventArgs e)
        {
            var miniGameWindow = new cyberMinigameWindow();
            miniGameWindow.Show();
            this.Hide();
            miniGameWindow.Closed += (s, args) => this.Show();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void taskManagerBtn_Click(object sender, RoutedEventArgs e)
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

            await AddBotMessage(taskList.ToString());
        }

        private async void activityLogBtn_Click(object sender, RoutedEventArgs e)
        {
            await ShowActivityLogAsync();
        }

    }
}
