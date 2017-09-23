using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PCLStorage;

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
        //Temporarily store the quiz for the selected past result.
        public static RootQuiz reference_Quiz;

        //Temporarily store the saved state of the ongoing quiz.
        public static Results ongoing_Quiz;
        //Temporarily store the current quiz being solved.
        public static RootQuiz selected_Quiz;
        //Temporarily store the user's answers for review
        public static List<string> all_user_answers;
        //Temporarily store the user's results for review
        public static List<int> all_user_results;
        //Temporarily store the user's scores for review
        public static int review_total_score;
        //Temporarily store the user's scores for review
        public static int review_user_score;

        //Check whether a quiz is currently ongoing
        public static bool ongoingQuiz = false;

        //Check whether the user has an account or is a 'Guest'.
        public static bool isGuest = false;

        //The base 'root_folder' during operations.
        public static IFolder root_folder = FileSystem.Current.LocalStorage;

        /* */

    }

}
