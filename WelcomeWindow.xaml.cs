//st10436124 POE Part 2
using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace CyberBotPart3
{
    public partial class WelcomeWindow : Window 
    {
        

        Chatbackend myObj = new Chatbackend();

        public WelcomeWindow()
        {
            InitializeComponent();

            Loaded += WelcomeWindow_Loaded;
            
        }

        private async void WelcomeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => myObj.PlayGreeting());
            await Task.Delay(300);

            outputBox.Text = myObj.imageDisplay();
            outputBox.AppendText(myObj.WelcomeMessage());

            UserName = myObj.userName;

            startBtn.IsEnabled = true;
        }


        public String UserName
        {
            get { return myObj.userName; }
            set { myObj.userName = value; }
        }




        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            ChatBotWindow chatWindow = new ChatBotWindow(UserName);
            Application.Current.MainWindow = chatWindow;
            this.Close();
            chatWindow.Show();
        }

        
    }
}
