using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT313_Project_2_Quiz
{

    //The base 'result' format.
    public class Results
    {

        public string user { get; set; } //Stores the user of this quiz result.
        public string quiz_id { get; set; } //Stores the quiz attempted.
        public List<string> quiz_answers { get; set; } //Stores the list of user answers.
        public List<int> quiz_results { get; set; } //Stores the result of each question.
        public int quiz_total_score { get; set; } //Stores the total available score for that quiz.
        public int quiz_user_score { get; set; } //Stores the user's total score.

    }

}
