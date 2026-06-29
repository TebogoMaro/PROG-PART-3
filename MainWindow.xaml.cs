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

//allows the program to use speech synthesis(text-to-speech)
using System.Speech.Synthesis;

namespace CyberSecurityBotGUI1
{
    //summary>
    // Interaction logic for MainWindow.xaml
    // </summary>
 
    public partial class MainWindow : Window
    {
        private void TestDatabase()
        {
            TaskModel task = new TaskModel();

            task.Title = "Password Safety";
            task.Description = "Review passwords.";
            task.Reminder = "Friday";
            task.Status = "Pending";

            DatabaseHelper db = new DatabaseHelper();

            db.AddTask(task);

            MessageBox.Show("Task added!");
        }

        //creates a speech synthesizer object for voice output
        SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        //creates an instance of the chatbot class
        Chatbot bot = new Chatbot();

        /// <summary>
        /// constructor for the MainWindow class
        /// initializes all GUI components
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            
        }

        /// <summary>
        /// runs when the Send button is clicked,
        /// gets user input, sends it to the chatbot,
        /// displays both messages, and speaks the response
        /// </summary>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //store the text entered by the user
            string userInput = InputBox.Text;

            //display the user's message in the chat window
            AddMessage("You", userInput);

            //get a response from the chatbot
            string botResponse = bot.GetResponse(userInput);

            //display the chatbot's response in the chat window
            AddMessage("Bot", botResponse);
            synthesizer.Speak(botResponse);

            //clear the input box after sending
            InputBox.Clear();
        }

        /// <summary>
        /// adds a new message to the chat display area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void AddMessage(string sender, string message)
        {
            //add a paragraph containing the sender name and message
            ChatBox.Document.Blocks.Add(
                new Paragraph(
                    new Run(sender + ": " + message)
                    )
                );
        }
    }
}