using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// ======================
// class holds various tips and responses
// ======================
namespace CyberBotPart3
{
    public class TipsHolder
    {
        
        public List<string> generalTips = new List<string>
        {
            "Always keep your software updated to protect against vulnerabilities.",
            "Use strong, unique passwords for each of your accounts.",
            "Be cautious of unsolicited emails or messages asking for personal information.",
            "Regularly back up your data to prevent loss in case of an attack."
        };

        
        public List<string> phishingTips = new List<string>
        {
            "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
            "Check the sender's email address—phishers often use fake but similar addresses.",
            "Never click on suspicious links. Hover over them first to see the actual URL.",
            "Look out for grammar mistakes or urgent messages—they are common signs of phishing."
        };

        public List<string> passwordTips = new List<string>
        {
            "Don't reuse passwords across sites.",
            "Use a password manager to generate and store secure passwords.",
            "Enable two-factor authentication where possible."
        };

        public List<string> browsingTips = new()
            {
                "Keep your browser and software up to date.",
                "Avoid clicking on unknown links or ads.",
                   "Use secure websites with HTTPS."
            };

        public List<string> privacyTips = new()
            {
                "Limit the personal information you share online.",
                "Review privacy settings on social media platforms.",
                "Be cautious when using public Wi-Fi networks."
            };

        public List<string> scamTips = new()
            {
                 "Always double-check the sender’s identity.",
                 "Don’t respond to messages requesting urgent payment or personal info.",
                 "Legitimate companies don’t ask for sensitive info over email."
            };

        public List<string> cybersecurityTips = new()
            {
                "Install antivirus software and keep it updated.",
                "Use strong, unique passwords for each account.",
                "Avoid using public Wi-Fi for sensitive transactions."
            };

    

        
    }

}
