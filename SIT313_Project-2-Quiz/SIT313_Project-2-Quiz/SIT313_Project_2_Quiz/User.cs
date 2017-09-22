using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT313_Project_2_Quiz
{

    //The base 'user' format.
    public class User
    {

        public string username { get; set; } //Stores the username.
        public string password { get; set; } //Stores the password.
        public string security_question { get; set; } //Stores the security question.
        public string security_answer { get; set; } //Stores the security answer.

    }
}
