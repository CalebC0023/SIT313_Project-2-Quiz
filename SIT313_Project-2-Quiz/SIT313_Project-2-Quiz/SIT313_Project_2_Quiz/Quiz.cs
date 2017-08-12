using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT313_Project_2_Quiz
{
    /* The code in this file is to help encapsulate the items in the JSON file.
     * They following code is referenced from the link below
     * URL: {https://stackoverflow.com/questions/38743280/deserialize-json-object-xamarin-android-c-sharp}
     */

    //The base 'question' format.
    public class Question
    {

        public int id { get; set; } //Question ID.
        public string text { get; set; } //The Question itself.
        public string type { get; set; } //The type of answer (e.g. Entry, RadioBox, etc).
        public List<string> options { get; set; } //List all available options (for multiple choice questions).

        /* The following lines of commented code below will mainly be used to validate the given
         * answers to the questions (used in Project 2). Therefore, I have decided not to use
         * the variables below as they will not be necessary for Project 1, which is just displaying
         * the UI.
         */

        public string help { get; set; } //The Question's hint.
        public List<string> answer { get; set; } //The Question's answer.
        public string validate { get; set; }
        public int weighting { get; set; }

    }

    //The base 'quiz' format.
    public class RootQuiz
    {

        public string id { get; set; } //The Quiz ID.
        public string title { get; set; } //The Quiz title.
        public List<Question> questions { get; set; } //The list of Questions in this Quiz
        public int score { get; set; } //The total available score for that quiz

        /* Due to my design for questions (using 'CarouselPage' class to display 1 question per page),
         * the code below for 'questions per page' is left out as it will be needed.
         */

        public List<int> questionsPerPage { get; set; } //The number of Questions per Page.

    }

}
