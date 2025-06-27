using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CyberBotPart3
{
    public partial class cyberMinigameWindow : Window
    {
        private questionHolder questionBank = new questionHolder();
        private int currentIndex = 0;
        private int score = 0;
        private bool isMCQ = true; // true = multiple choice, false = true/false
        private string correctAnswer = "";
        private List<RadioButton> radioButtons = new();
        private List<TextBox> answerBoxes = new();


        public cyberMinigameWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                radioButtons = new List<RadioButton>
                {
                    (RadioButton)this.FindName("optionA_RadioBtn"),
                    (RadioButton)this.FindName("optionB_RadioBtn"),
                    (RadioButton)this.FindName("optionC_RadioBtn"),
                    (RadioButton)this.FindName("optionD_RadioBtn")
                };

                answerBoxes = new List<TextBox>
                {
                    answerATxtBx,
                    answerBTxtBx,
                    answerCTxtBx,
                    answerDTxtBx
                };

                LoadNextQuestion();
            };
        }

        private void LoadNextQuestion()
        {
            resultsRichTxtBx.Document.Blocks.Clear();

            if (currentIndex < questionBank.MultipleChoiceQuestions.Count)
            {
                var q = questionBank.MultipleChoiceQuestions[currentIndex];
                questionTxtbx.Text = q.Question;
                answerBoxes[0].Text = q.AnswerA;
                answerBoxes[1].Text = q.AnswerB;
                answerBoxes[2].Text = q.AnswerC;
                answerBoxes[3].Text = q.AnswerD;

                radioButtons[0].Content = q.OptionA;
                radioButtons[1].Content = q.OptionB;
                radioButtons[2].Content = q.OptionC;
                radioButtons[3].Content = q.OptionD;

                for (int i = 0; i < 4; i++)
                {
                    answerBoxes[i].Visibility = Visibility.Visible;
                    radioButtons[i].Visibility = Visibility.Visible;
                    radioButtons[i].IsChecked = false;
                }

                correctAnswer = q.CorrectIndex;
                isMCQ = true;
            }
            else if (currentIndex - questionBank.MultipleChoiceQuestions.Count < questionBank.TrueFalseQuestions.Count)
            {
                var tfIndex = currentIndex - questionBank.MultipleChoiceQuestions.Count;
                var q = questionBank.TrueFalseQuestions[tfIndex];
                questionTxtbx.Text = q.Question;

                radioButtons[0].Content = q.OptionA;
                radioButtons[1].Content = q.OptionB;


                answerBoxes[0].Text = q.AnswerA;
                answerBoxes[1].Text = q.AnswerB;

                // Show only A and B
                answerBoxes[0].Visibility = Visibility.Visible;
                answerBoxes[1].Visibility = Visibility.Visible;
                radioButtons[0].Visibility = Visibility.Visible;
                radioButtons[1].Visibility = Visibility.Visible;

                // Hide C and D
                answerBoxes[2].Visibility = Visibility.Collapsed;
                answerBoxes[3].Visibility = Visibility.Collapsed;
                radioButtons[2].Visibility = Visibility.Collapsed;
                radioButtons[3].Visibility = Visibility.Collapsed;

                radioButtons[0].IsChecked = false;
                radioButtons[1].IsChecked = false;

                correctAnswer = q.CorrectIndex;
                isMCQ = false;
            }
            else
            {
                ShowFinalScore();
            }


        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {

            // Check if none of the real radio buttons are selected
            if (!(radioButtons[0].IsChecked == true ||
                  radioButtons[1].IsChecked == true ||
                  (isMCQ && radioButtons[2].IsChecked == true) ||
                  (isMCQ && radioButtons[3].IsChecked == true)))
            {
                MessageBox.Show("Please select an option before submitting.", "No Answer Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selected = "";

            if (radioButtons[0].IsChecked == true) selected = "Answer: A";
            else if (radioButtons[1].IsChecked == true) selected = "Answer: B";
            else if (isMCQ && radioButtons[2].IsChecked == true) selected = "Answer: C";
            else if (isMCQ && radioButtons[3].IsChecked == true) selected = "Answer: D";


            var block = new Paragraph();

            if (selected == correctAnswer)
            {
                block.Inlines.Add(new Run("✅ Correct!"));
                score++;
            }
            else
            {
                block.Inlines.Add(new Run("❌ Incorrect."));
            }

            string explanation = isMCQ
                ? questionBank.MultipleChoiceQuestions[currentIndex].Explanation
                : questionBank.TrueFalseQuestions[currentIndex - questionBank.MultipleChoiceQuestions.Count].Explanation;

            block.Inlines.Add(new LineBreak());
            block.Inlines.Add(new Run("Explanation: " + explanation));
            resultsRichTxtBx.Document.Blocks.Clear();
            resultsRichTxtBx.Document.Blocks.Add(block);

            // Move to next question only when button is clicked
            nextQuestionBtn.Visibility = Visibility.Visible;
            submitBtn.IsEnabled = false;
        }


        private void ShowFinalScore()
        {
            resultsRichTxtBx.Document.Blocks.Clear();
            questionTxtbx.Text = "Quiz Completed! 🎉";

            foreach (var box in answerBoxes)
                box.Visibility = Visibility.Collapsed;

            foreach (var rb in radioButtons)
                rb.Visibility = Visibility.Collapsed;

            string result = $"You scored {score} out of {questionBank.MultipleChoiceQuestions.Count + questionBank.TrueFalseQuestions.Count}.";
            resultsRichTxtBx.Document.Blocks.Add(new Paragraph(new Run(result)));
        }

        private void NextQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            dummyRadioBtn.IsChecked = true; // Uncheck all visible options
            nextQuestionBtn.Visibility = Visibility.Collapsed;
            submitBtn.IsEnabled = true;
            currentIndex++; // ✅ move to next question
            LoadNextQuestion();
        }

        private void BackToChatBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the mini-game window
                          // If you hid the chatbot earlier, it will reappear automatically
        }



    }
}
