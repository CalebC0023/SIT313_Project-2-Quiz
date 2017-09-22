using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT313_Project_2_Quiz
{

    //The base 'question' format.
    public class Question
    {

        public string text { get; set; } //The Question itself.
        public string type { get; set; } //The type of answer (e.g. Entry, RadioBox, etc).
        public List<string> options { get; set; } //List all available options (for multiple choice questions).
        public string answer { get; set; } //The Question's answer.

    }

    //The base 'quiz' format.
    public class RootQuiz
    {

        public string id { get; set; } //The Quiz ID.
        public string title { get; set; } //The Quiz title.
        public List<Question> questions { get; set; } //The list of Questions in this Quiz

    }

}
