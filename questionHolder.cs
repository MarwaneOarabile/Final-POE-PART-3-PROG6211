using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberBotPart3
{
    class questionHolder
    {
        // Multiple Choice: (Question, OptionA, AnswerA, OptionB, AnswerB, OptionC, AnswerC, OptionD, AnswerD, CorrectIndex, Explanation)
        public List<(string Question, string OptionA, string AnswerA, string OptionB, string AnswerB,
                     string OptionC, string AnswerC, string OptionD, string AnswerD, string CorrectIndex, string Explanation)> MultipleChoiceQuestions
            = new()
        {
            ("What is the safest way to create a strong password?",
             "A)", "Use your birthdate and name",
             "B)", "Use 'password123' for all accounts",
             "C)", "Use a long mix of letters, numbers, and symbols",
             "D)", "Reuse the same password everywhere",
             "Answer: C", "A strong password is long and unpredictable with mixed characters."),

            ("Which of these is a sign of a phishing email?",
             "A)", "Professional language",
             "B)", "From a known contact",
             "C)", "Contains spelling errors and urgent requests",
             "D)", "Sent during working hours",
             "Answer: C", "Phishing emails often use urgency and poor grammar to trick users."),

            ("Which network is safest to use for online banking?",
             "A)", "Public Wi-Fi at a coffee shop",
             "B)", "Hotel guest network",
             "C)", "Mobile hotspot",
             "D)", "Any free Wi-Fi",
             "Answer: C", "Mobile hotspots are private and more secure than public networks."),

            ("What does two-factor authentication (2FA) add?",
             "A)", "A backup of your password",
             "B)", "A second device or code to verify identity",
             "C)", "A firewall",
             "D)", "A browser extension",
             "Answer: B", "2FA adds a second step, usually via code or device, to confirm your identity."),

            ("Which file extension is most likely to carry malware?",
             "A)", ".docx",
             "B)", ".pdf",
             "C)", ".jpg",
             "D)", ".exe",
             "Answer: D", ".exe files are executable and can run malicious code.")
        };

        // True/False: (Question, OptionA, AnswerA, OptionB, AnswerB, CorrectIndex, Explanation)
        public List<(string Question, string OptionA, string AnswerA, string OptionB, string AnswerB,
                     string CorrectIndex, string Explanation)> TrueFalseQuestions
            = new()
        {
            ("Using the same password on multiple accounts is safe if it's strong.",
             "A)", "True",
             "B)", "False",
             "Answer: B", "Reusing passwords puts all accounts at risk if one is compromised."),

            ("You should click on links in emails if they look official.",
             "A)", "True",
             "B)", "False",
             "Answer: B", "Always verify links; phishing emails often look official."),

            ("A secure website URL starts with 'https://'.",
             "A)", "True",
             "B)", "False",
             "Answer: A", "HTTPS indicates data encryption and a secure connection."),

            ("It’s safe to install software from unknown websites if your antivirus is on.",
             "A)", "True",
             "B)", "False",
             "Answer: B", "Avoid unknown sources — antivirus can’t catch everything."),

            ("Social engineering tricks users into revealing sensitive information.",
             "A)", "True",
             "B)", "False",
             "Answer: A", "Social engineering manipulates people to give up personal info.")
        };
    }
}
