//st10436124 POE Part 1

namespace CyberBotPart2
{
    internal class UseChat
    {
        // runs the code
        static void Main(string[] args)
        {
            // plays the audio file when the application starts
            Chatbackend myObjChat = new Chatbackend();
            myObjChat.PlayGreeting();

            // displaying an ASCII representation of a logo
            myObjChat.imageDisplay();

            // displays the welcome message with name 
            myObjChat.WelcomeMessage();

            // displays the menu
            myObjChat.StartMenu();






        }
    }
}
