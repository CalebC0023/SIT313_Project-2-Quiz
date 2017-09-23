using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT313_Project_2_Quiz
{

    /* A static class used to store temporary 'global' data while the app is running (including in the background).
     * The following code is referenced from this URL.
     * URL: {https://stackoverflow.com/questions/2445436/global-variables-in-c-net}
     */
    public static class Current_Data
    {

        /* Data taken from the files online (or locally)
         */

        //Temporarily store the details of all users.
        public static List<User> all_users;
        //Store whether the 'users' item can be reached during HTTP request.
        public static bool user_list_status = true;

        //Temporarily store the details of all quizzes.
        public static List<RootQuiz> all_quizzes;
        //Store whether the 'quizzes' item can be reached during HTTP request.
        public static bool quiz_list_status = true;

        //Temporarily store the details of all results.
        public static List<Results> all_results;
        //Store whether the 'results' item can be reached during HTTP request.
        public static bool result_list_status = true;

        /* */


        /* Data used to handle miscellaneous tasks (e.g. display the current user in the profile page, save data for registered users but not guest, etc.)
         */

        //Temporarily store the user's username.
        public static string Username;

        //Temporarily store the user's details which is being edited.
        public static string edit_username;
        public static string edit_secure_question;
        public static string edit_secure_answer;

        //Temporarily store the user selected result for viewing.
        public static Results selected_Results;

        //Check whether the user has an account or is a 'Guest'.
        public static bool isGuest;

        /* */

    }

}
