//st10436124 POE Part 1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Threading.Channels;
using System.Globalization;
using System.IO;


namespace CyberBotPart3
{
    internal class Chatbackend
    {

        // q1 add voice greeting 


        public void PlayGreeting()
        {
            // relative path to the audio file
            string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "greetings.wav");

            if (File.Exists(fileLocation))
            {
                SoundPlayer player = new SoundPlayer(fileLocation);

                player.PlaySync();

            }
            else
            {
                Console.WriteLine("Greeting audio file not found: " + fileLocation);
            }


        }



        // q2 add a method to display the logo
        public void imageDisplay()
        {
            string asciiArtLogo = @"                                                 @@@@@@                                             
                                               @@@@@@@@@@@                                          
                                            @@@@@@@@@@@@@@@@                                        
                                   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                               
                                   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                               
                                   @@@@@@@@@@@@@@      @@@@@@@@@@@@@@                               
                                   @@@@@@@@@@@@   @@@@   @@@@@@@@@@@@                               
                                   @@@@@@@@@@@  @@@@@@@@  @@@@@@@@@@@                               
                                   @@@@@@@@@@@  @@@@@@@@  @@@@@@@@@@@                               
                                   @@@@@@@@@@@  @@@@@@@@  @@@@@@@@@@@                               
                                   @@@@@@@@@@@  @@@@@@@@  @@@@@@@@@@@@                              
                                   @@@@@@@                    @@@@@@@@                              
                                   @@@@@@@  @@@@@@@@@@@@@@@@  @@@@@@@@                              
                                   @@@@@@@  @@@@@@@@@@@@@@@@  @@@@@@@@                              
                                   @@@@@@@  @@@@@@@  @@@@@@@  @@@@@@@@                              
                                   @@@@@@@  @@@@@@@@@@@@@@@@  @@@@@@@@                              
                                   @@@@@@@      @@@@@@@@@     @@@@@@@                               
                                   @@@@@@@@@@@@          @@@@@@@@@@@@                               
                                    @@@@@@@@@@@@@@@  @@@@@@@@@@@@@@@                                
                                     @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                                 
                                       @@@@@@@@@@@@@@@@@@@@@@@@@@                                   
                                          @@@@@@@@@@@@@@@@@@@@                                      
                                             @@@@@@@@@@@@@@                                         
                                                @@@@@@@@";


            Console.WriteLine("\n ========= CYBERSECURITY AWARENESS BOT =========\n");
            Console.WriteLine(asciiArtLogo);
        }

        // q3 Display a text-based welcome message with decorative boarderd 

        private string name;
        bool cancel = false;
        //method gets name
        public void GetName()
        {
            cancel = false;

            do
            {

                TypingEffect("Please enter your name: ");
                name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    TypingEffect("Please enter a valid name.");
                }
                else
                {
                    cancel = true;
                }

            } while (!cancel);
        }

        // this method displays a welcome message with name
        public void WelcomeMessage()
        {
            GetName();

            PrintDivider();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine($"║             WELCOME {name.ToUpper(),-25} ║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
            Console.ResetColor();
            TypingEffect($"Welcome {name} to the CyberBot, the best cybersecurity chatbot!");
        }




        // this methods is used to display a menu


        public void StartMenu()
        {
            PrintDivider();
            PrintHeader("Main Menu");

            Console.WriteLine("\n1. Start Chat");
            Console.WriteLine("2. Exit Console App");
            Console.Write("\nPlease select a number option from the menu above: ");
            cancel = false;

            do
            {
                string input = Console.ReadLine();

                // Check if input is not empty and contains only digits
                if (string.IsNullOrWhiteSpace(input))
                {
                    TypingEffect("\nPlease enter a valid OPTION 1 or 2:");
                }
                else
                {
                    // Manually check if the input is a valid integer and within range
                    if (input.Length == 1 && input[0] >= '1' && input[0] <= '2')
                    {
                        int inputConvertInt = input[0] - '0';  // Convert char to integer

                        if (inputConvertInt == 1)
                        {
                            Console.Clear();
                            TypingEffect("\nStarting chat...");

                            Reponses();
                            cancel = true;
                        }
                        else if (inputConvertInt == 2)
                        {
                            TypingEffect("\nExiting...");

                            cancel = true;
                        }
                    }
                    else
                    {
                        // Invalid input; prompt for retry
                        TypingEffect("\nInvalid input. Please enter OPTION 1 or 2:");
                    }
                }

            } while (!cancel);
        }


        // q4 Basic  general Responses System

        private void Reponses()
        {
            cancel = false;

            do
            {


                Console.Clear();
                PrintDivider();
                PrintHeader("Cyber Questions");
                Console.WriteLine("1. Hello, I want to type directly to cyberbot helper.");
                Console.WriteLine("2. What is your purpose?");
                Console.WriteLine("3. What can I ask you about?");
                Console.WriteLine("4. What makes a password safe?");
                Console.WriteLine("5. What is phishing?");
                Console.WriteLine("6. How do I safely browse the internet?");
                Console.WriteLine("7. exit application");

                Console.Write("\nPlease select an option from the menu above: ");

                int userInput = 0;

                try
                {
                    userInput = int.Parse(Console.ReadLine());
                    if (userInput < 1 || userInput > 7)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        TypingEffect("CyberBot: Please select a valid option.");
                        Console.ResetColor();
                        continue;
                    }
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    TypingEffect("CyberBot: I didn’t quite understand that. Could you rephrase?");
                    Console.ResetColor();
                    continue;
                }

                switch (userInput)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        TypingEffect("CyberBot: Hello");

                        Console.ResetColor();
                        userQuestion();
                        break;

                    case 2:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        TypingEffect("CyberBot: I am here to help you with your questions");
                        Console.ResetColor();
                        Console.WriteLine("\nPress Enter to continue...");
                        Console.ReadLine();
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        TypingEffect("CyberBot: You can ask me about cybersecurity topics.");
                        Console.ResetColor();
                        Console.WriteLine("\nPress Enter to continue...");
                        Console.ReadLine();
                        break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        TypingEffect("CyberBot: A safe password should be at least 8 characters long and include a mix of letters, numbers, and symbols.");
                        Console.ResetColor();
                        Console.WriteLine("\nPress Enter to continue...");
                        Console.ReadLine();
                        break;
                    case 5:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        TypingEffect("CyberBot: Phishing is a type of cyber attack where an attacker tries to trick you into giving them your personal information.");
                        Console.ResetColor();
                        Console.WriteLine("\nPress Enter to continue...");
                        Console.ReadLine();
                        break;
                    case 6:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        TypingEffect("CyberBot: To safely browse the internet, use a VPN, avoid suspicious links, and keep your software updated.");
                        Console.ResetColor();
                        Console.WriteLine("\nPress Enter to continue...");
                        Console.ReadLine();
                        break;
                    case 7:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        TypingEffect("CyberBot: Exiting application...");
                        Console.ResetColor();
                        cancel = true;
                        break;
                }

            } while (!cancel);
        }


        //allows user to ask their own questions
        public void userQuestion()
        {

            string currentTopic = string.Empty;

            bool talkedAboutPhishing = false;
            bool talkedAboutPassword = false;
            bool talkedAboutBrowsing = false;
            bool talkedAboutPrivacy = false;
            bool talkedAboutScam = false;
            bool talkedAboutCybersecurity = false;



            List<string> phishingTips = new List<string>
            {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "Check the sender’s email address—phishers often use fake but similar addresses.",
                "Never click on suspicious links. Hover over them first to see the actual URL.",
                "Look out for grammar mistakes or urgent messages—they are common signs of phishing."
            };

            List<string> passwordTips = new()
            {
                "Don’t reuse passwords across sites.",
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


            List<(string keyword, string response)> sentimentKeywords = new List<(string, string)>
                    {
                        ("worried", "CyberBot: It's okay to feel worried. Cybersecurity can be tricky, but I'm here to help guide you."),
                        ("stressed", "CyberBot: I understand that cybersecurity can feel overwhelming. You're not alone—ask me anything, and we’ll take it step by step."),
                        ("scared", "CyberBot: Don’t worry, you're doing the right thing by learning. I'm here to help you stay safe online."),
                        ("confused", "CyberBot: If something seems confusing, feel free to ask. I’ll explain it in a simpler way."),
                        ("frustrated", "CyberBot: I know it can be frustrating. Let’s work through it together!"),
                        ("curious", "CyberBot: Curiosity is the first step to better cybersecurity. What would you like to learn about?")
};



            string userQuestion;
            Console.WriteLine("CyberBot: You can ask me a cybersecurity question, or type 'exit' to go back to the main menu.");

            do
            {
                Console.Write("\nPlease ask your question: ");
                userQuestion = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userQuestion))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    TypingEffect("CyberBot: I didn’t quite understand that. Could you rephrase?");
                    Console.ResetColor();
                    continue;
                }

                userQuestion = userQuestion.ToLower().Trim();
                Console.WriteLine("You asked: " + userQuestion);

                Console.ForegroundColor = ConsoleColor.Magenta;



                if (userQuestion.Contains("exit"))
                {
                    TypingEffect($"CyberBot: Exiting Q&A mode. Goodbye for now, {name}!");
                    Console.ResetColor();
                    break;
                }

                // Sentiment detection
                bool sentimentDetected = false;
                foreach (var (keyword, response) in sentimentKeywords)
                {
                    if (userQuestion.Contains(keyword))
                    {
                        TypingEffect(response);
                        sentimentDetected = true;
                        break;
                    }
                }


                if (!sentimentDetected)
                {
                    // Small talk
                    if (userQuestion.Contains("hello"))
                    {
                        TypingEffect($"CyberBot: Hello {name}, as you know I am CyberBot.");
                    }
                    else if (userQuestion.Contains("how are you"))
                    {
                        TypingEffect($"CyberBot: I am fine, thank you for asking, {name}.");
                    }


                    // Topic responses

                    else if (userQuestion.Contains("phishing"))
                    {
                        currentTopic = "phishing";
                        TypingEffect("CyberBot: Phishing is when someone tries to trick you into giving personal info by pretending to be someone you trust.");

                        if (talkedAboutPhishing)
                        {
                            Random rand = new Random();
                            int index = rand.Next(phishingTips.Count);
                            TypingEffect($"CyberBot: Here's a phishing tip - {phishingTips[index]}");

                        }


                        talkedAboutPhishing = true;
                    }
                    

                    else if (userQuestion.Contains("password"))
                    {
                        currentTopic = "password";
                        TypingEffect("CyberBot: A strong password must be longer than 8 characters and include a mix of uppercase, lowercase, numbers, and symbols.");

                        if (talkedAboutPassword)
                        {
                            Random rand = new();
                            TypingEffect($"CyberBot: Here's a password tip - {passwordTips[rand.Next(passwordTips.Count)]}");
                            
                        }
                        talkedAboutPassword = true;
                    }


                    else if (userQuestion.Contains("browsing"))
                    {
                        currentTopic = "browsing";
                        TypingEffect("CyberBot: To browse safely: keep your software updated, use antivirus, and don’t click on suspicious links.");

                        if (talkedAboutBrowsing)
                        {
                            Random rand = new();
                            TypingEffect($"CyberBot: Here's a browsing tip - {browsingTips[rand.Next(browsingTips.Count)]}");
                            talkedAboutBrowsing = true;
                        }
                        talkedAboutBrowsing = true;
                    }
                    else if (userQuestion.Contains("privacy"))
                    {
                        currentTopic = "privacy";
                        TypingEffect("CyberBot: Protect your privacy by limiting the personal information you share online.");

                        if (talkedAboutPrivacy)
                        {
                            Random rand = new();
                            TypingEffect($"CyberBot: Here's a privacy tip - {privacyTips[rand.Next(privacyTips.Count)]}");
                            
                        }
                        talkedAboutPrivacy = true;
                    }



                    else if (userQuestion.Contains("scam"))
                    {
                        currentTopic = "scam";
                        TypingEffect("CyberBot: Scams often try to trick you with urgent messages. Always verify links and senders.");

                        if (talkedAboutScam)
                        {
                            TypingEffect($"CyberBot: Tip - {scamTips[new Random().Next(scamTips.Count)]}");
                            
                        }
                        talkedAboutScam = true;
                    }
                    else if (userQuestion.Contains("cybersecurity"))
                    {
                        currentTopic = "cybersecurity";
                        TypingEffect("CyberBot: Cybersecurity is the practice of protecting systems, networks, and programs from cyber attacks.");

                        if (talkedAboutCybersecurity)
                        {
                            TypingEffect($"CyberBot: Tip - {cybersecurityTips[new Random().Next(cybersecurityTips.Count)]}");
                            
                        }
                        talkedAboutCybersecurity = true;
                    }

                    else if (userQuestion.Contains("more"))
                    {
                        Random rand = new();

                        if (currentTopic == "phishing" && talkedAboutPhishing)
                            TypingEffect($"CyberBot: Here's another phishing tip - {phishingTips[rand.Next(phishingTips.Count)]}");
                        else if (currentTopic == "password" && talkedAboutPassword)
                            TypingEffect($"CyberBot: Here's another password tip - {passwordTips[rand.Next(passwordTips.Count)]}");
                        else if (currentTopic == "browsing" && talkedAboutBrowsing)
                            TypingEffect($"CyberBot: Here's another browsing tip - {browsingTips[rand.Next(browsingTips.Count)]}");
                        else if (currentTopic == "privacy" && talkedAboutPrivacy)
                            TypingEffect($"CyberBot: Here's another privacy tip - {privacyTips[rand.Next(privacyTips.Count)]}");
                        else if (currentTopic == "scam" && talkedAboutScam)
                            TypingEffect($"CyberBot: Here's another scam tip - {scamTips[rand.Next(scamTips.Count)]}");
                        else if (currentTopic == "cybersecurity" && talkedAboutCybersecurity)
                            TypingEffect($"CyberBot: Here's another cybersecurity tip - {cybersecurityTips[rand.Next(cybersecurityTips.Count)]}");
                        else
                            TypingEffect("CyberBot: I'm not sure what topic you'd like more information on. Please ask a question first.");
                    }

                    else
                    {
                        TypingEffect($"CyberBot: I'm sorry {name}, I don't have an answer for that yet.");
                    }



                }


                Console.ResetColor();

            } while (true);

            Console.ForegroundColor = ConsoleColor.Magenta;
            TypingEffect("Returning to main menu...");
            Console.ResetColor();

            StartMenu();
        }


        // q6 add visual elements to the chatbot

        // this method is used to format colour and and divider

        public void PrintDivider()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n════════════════════════════════════════════════════════════");
            Console.ResetColor();
        }

        // this method is used to add typing effect to the text
        public void TypingEffect(string message, int delay = 15)
        {
            foreach (char c in message)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(c);
                Thread.Sleep(delay);
                Console.ResetColor();
            }
            Console.WriteLine(); // Move to next line after message
        }

        public void PrintHeader(string header)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n===== " + header.ToUpper() + " =====");
            Console.ResetColor();
        }

    }
}
