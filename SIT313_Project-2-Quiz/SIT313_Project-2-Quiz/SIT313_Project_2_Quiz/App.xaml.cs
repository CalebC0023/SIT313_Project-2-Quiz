using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using PCLStorage;

namespace SIT313_Project_2_Quiz
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //Root page of the app.
            MainPage = new NavigationPage(new SIT313_Project_2_Quiz.MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts

            //Try to load all the necessary files.
            LoadFiles();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        //Call a new HTTP Client for online requests.
        private HttpClient client = new HttpClient();

        private void LoadFiles()
        {
            //Initialise all appropriate 'global' variables.
            Current_Data.all_users = new List<User>();
            Current_Data.all_quizzes = new List<RootQuiz>();
            Current_Data.all_results = new List<Results>();

            LoadUserFile(); //First, try loading the 'users' file item.
            LoadQuizFile(); //Next, try loading the 'quizzes' file item.
            LoadResultsFile(); //Finaly, try loading the 'results' file item.
        }

        private async void LoadUserFile()
        {

            /* The following lines of code is about how to perform HTTP requests.
             * The code is referenced from the URL below.
             * URL: {https://developer.xamarin.com/guides/xamarin-forms/cloud-services/consuming/rest/}
             */

            /* Also the following URL is a reference of proper  HTTP encoding format for special characters (e.g. @#$%^&*, etc.)
             * URL: {http://www.degraeve.com/reference/urlencoding.php}
             */

            //First, try loading from the 'users' file from the online database.
            try
            {

                //Firstly, define the url with the appropriate request.
                string user_url = @"http://introtoapps.com/datastore.php?action=load&appid=214328958&objectid=users"; //GET request to load the data from the 'users' item.
                var user_uri = new Uri(string.Format(user_url, string.Empty));
                var user_response = await client.GetAsync(user_uri); //Perform the action.

                //Next, check whether the request is valid.
                if (user_response.IsSuccessStatusCode)
                {
                    //If the request is valid, try to deserialize the .json string into the list of 'User' objects.
                    string content = await user_response.Content.ReadAsStringAsync();
                    Current_Data.all_users = JsonConvert.DeserializeObject<List<User>>(content);
                }
                else
                {
                    //If the first request is invalid, this means that the item could not be found.
                    //Display an appropriate error message.
                    Debug.WriteLine("The 'users' item could not be found. Trying to create a new 'users' item.");

                    //The item should have been created server-side which means an exception would be thrown here.
                    //However, in this case, for this assignment, try creating a new 'users' item in the database.
                    string create_user_url = @"http://introtoapps.com/datastore.php?action=save&appid=214328958&objectid=users&data=%5B%5D";
                    var create_user_uri = new Uri(string.Format(create_user_url, string.Empty));
                    var create_user_response = await client.GetAsync(create_user_uri);
                    //Finally, check whether the item is properly created.
                    if (!create_user_response.IsSuccessStatusCode)
                        throw new Exception("All HTTP requests for the 'users' file have failed."); //Throw a final exception to indicate that all requests have failed.
                }
            }
            catch (Exception e)
            {
                //If there are any errors (e.g. wrong format, HTTP requests failed, etc.), set the 'User' list status to 'false'
                //and display the error, which will be used to stop any logins/edits/registers until the problem is solved.
                Current_Data.user_list_status = false;
                Debug.WriteLine("'Users' file error: " + e.Message.ToString());
            }

        }

        private async void LoadQuizFile()
        {

            string content = null; //Temporarily store the .json string.
            bool server_access = true; //Check whether the file can be accessed from the server.

            //The names for the folder and file for the local storage.
            string folder_name = "quiz_files";
            string filename = "quizzes.txt";
            IFolder root_folder = FileSystem.Current.LocalStorage; //Set the base local storage folder.

            /* These would be the sample quizzes for this assignment.
             * These would be added server-side but for this assignment, in the case of the missing 'quizzes' item in the database,
             * these question will be saved into the database, then loaded from the database and saved to the local storage.
             * The URL below can be used to check the .json file in the server.
             * URL: {http://introtoapps.com/datastore.php?action=load&appid=214328958&objectid=quizzes}
             * 
             * The string is formatted to HTTP encodings. The URL below contains the reference.
             * URL: {http://www.degraeve.com/reference/urlencoding.php}
             */

            string sample_quizzes = @"%5B" +
                @"%7B" +
                    @"%22id%22%3A%22quiz01%22%2C" +
                    @"%22title%22%3A%22Multiple%20Choice%22%2C" +
                    @"%22questions%22%3A%5B" +
                    @"%7B" +
                        @"%22text%22%3A%22Pick%20the%20first%20answer%2E%22%2C" +
                        @"%22type%22%3A%22choice%22%2C" +
                        @"%22options%22%3A%5B%22First%22%2C%22Second%22%2C%22Third%22%2C%22Fourth%22%5D%2C" +
                        @"%22answer%22%3A%22First%22" +
                    @"%7D%2C" +
                    @"%7B" +
                        @"%22text%22%3A%22What%20comes%20after%20the%20letter%20D%3F%22%2C" +
                        @"%22type%22%3A%22choice%22%2C" +
                        @"%22options%22%3A%5B%22C%22%2C%22Z%22%2C%22E%22%5D%2C" +
                        @"%22answer%22%3A%22E%22" +
                    @"%7D%2C" +
                    @"%7B" +
                        @"%22text%22%3A%221%2B1%3F%22%2C" +
                        @"%22type%22%3A%22choice%22%2C" +
                        @"%22options%22%3A%5B%221%22%2C%222%22%2C%223%22%2C%224%22%5D%2C" +
                        @"%22answer%22%3A%222%22" +
                    @"%7D%2C" +
                    @"%7B" +
                        @"%22text%22%3A%22What%20is%20the%20opposite%20of%20left%3F%22%2C" +
                        @"%22type%22%3A%22choice%22%2C" +
                        @"%22options%22%3A%5B%22Left%22%2C%22Right%22%5D%2C" +
                        @"%22answer%22%3A%22Right%22" +
                    @"%7D%2C" +
                    @"%7B" +
                        @"%22text%22%3A%22There%20are%20five%20questions%20in%20this%20quiz%2E%22%2C" +
                        @"%22type%22%3A%22choice%22%2C" +
                        @"%22options%22%3A%5B%22True%22%2C%22False%22%2C%22Maybe%22%5D%2C" +
                        @"%22answer%22%3A%22True%22" +
                    @"%7D%5D" +
                @"%7D%2C" +
                @"%7B" +
                    @"%22id%22%3A%22quiz02%22%2C" +
                    @"%22title%22%3A%22Short%20Answer%22%2C" +
                    @"%22questions%22%3A%5B" +
                    @"%7B" +
                        @"%22text%22%3A%22Type%20the%20word%20first%2E%22%2C" +
                        @"%22type%22%3A%22textbox%22%2C" +
                        @"%22answer%22%3A%22First%22" +
                    @"%7D%2C" +
                    @"%7B" +
                        @"%22text%22%3A%221%2B1%3F%22%2C" +
                        @"%22type%22%3A%22textbox%22%2C" +
                        @"%22answer%22%3A%222%22" +
                    @"%7D%2C" +
                    @"%7B" +
                        @"%22text%22%3A%22What%20is%20the%20opposite%20of%20left%3F%22%2C" +
                        @"%22type%22%3A%22textbox%22%2C" +
                        @"%22answer%22%3A%22Right%22" +
                    @"%7D%2C" +
                    @"%7B" +
                        @"%22text%22%3A%22What%20comes%20after%20the%20letter%20D%3F%22%2C" +
                        @"%22type%22%3A%22textbox%22%2C" +
                        @"%22answer%22%3A%22E%22" +
                    @"%7D%2C" +
                    @"%7B" +
                        @"%22text%22%3A%22How%20many%20questions%20are%20in%20this%20quiz%3F%22%2C" +
                        @"%22type%22%3A%22textbox%22%2C" +
                        @"%22answer%22%3A%225%22" +
                    @"%7D%5D" +
                @"%7D%5D";

            //First, try loading from the 'quizzes' file from the online database.
            try
            {

                //Firstly, define the url with the appropriate request.
                string quiz_url = @"http://introtoapps.com/datastore.php?action=load&appid=214328958&objectid=quizzes"; //GET request to load the data from the 'quizzes' item.
                var quiz_uri = new Uri(string.Format(quiz_url, string.Empty));
                var quiz_response_1 = await client.GetAsync(quiz_uri); //Perform the action.

                //Next, check whether the request is valid.
                //For this case, create the file if the first request fails.
                if (!quiz_response_1.IsSuccessStatusCode)
                {

                    //Display an appropriate error message.
                    Debug.WriteLine("The 'quiz' item could not be found.");

                    //In this case, try creating a new 'quizzes' item in the database.
                    string create_quiz_url = string.Format("{0}{1}", "http://introtoapps.com/datastore.php?action=save&appid=214328958&objectid=quizzes&data=", sample_quizzes);
                    var create_quiz_uri = new Uri(string.Format(create_quiz_url, string.Empty));
                    var create_quiz_response = await client.GetAsync(create_quiz_uri);
                    //Finally, check whether the item is properly created.
                    if (!create_quiz_response.IsSuccessStatusCode)
                        throw new Exception("The 'quizzes' file creation has failed."); //Throw a final exception to indicate that all requests have failed.

                }

                var quiz_response_2 = await client.GetAsync(quiz_uri); //Perform the action again.

                //Request for the 'quizzes' file.
                if (quiz_response_2.IsSuccessStatusCode)
                {

                    //If the request is valid, try to deserialize the .json string into the list of 'Quiz' objects.
                    content = await quiz_response_2.Content.ReadAsStringAsync();
                    Current_Data.all_quizzes = JsonConvert.DeserializeObject<List<RootQuiz>>(content);

                    //Next try to update the .txt file (create or overwrite the file) in the local storage.

                    //First, create (or overwrite) the local storage folder.
                    IFolder create_folder = await root_folder.CreateFolderAsync(folder_name, CreationCollisionOption.ReplaceExisting);

                    //Then, create (or overwrite) the .txt file to store the data.
                    IFile create_file = await create_folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

                    //Finally, try updating the file.
                    await create_file.WriteAllTextAsync(content);

                }
                //Throw an exception if the file cannot be found.
                else
                    throw new Exception("There was an error while requesting the 'quizzes' object from the online database. Trying the local storage.");

            }
            catch (Exception e)
            {
                //If there are any errors (e.g. wrong format, HTTP requests failed, etc.), indicate that the there is no server access
                //and display the error. Next is to check the local storage for the quizzes.
                server_access = false;
                Debug.WriteLine(e.Message.ToString());
            }


            /* The following 'try and catch' statement below can be tested with the following steps.
             * 1. First, comment out the 'try and catch' statement above to test an 'empty' local storage.
             * 2. Next, run code normally to save the local file from the online storage.
             * 3. Finally, repeat Step 1, which will now allow the user to answer a 'quiz' now since the 'quizzes' are present,
             *    regardless of the state of the server.
             */


            //If the 'quizzes' file cannot be accessed online, check the local storage.
            try
            {

                if (!server_access)
                {

                    /* The following code on how to open and read from the local file is referenced from the URL below.
                     * URL: {https://stackoverflow.com/questions/32247264/read-text-file-with-pclstorage-in-xamarin-forms}
                     */

                    IFolder read_folder = await root_folder.GetFolderAsync(folder_name);

                    //Check whether the file exists.
                    ExistenceCheckResult exist = await read_folder.CheckExistsAsync(filename);

                    content = null;
                    //Read from the file if it exists.
                    if (exist == ExistenceCheckResult.FileExists)
                    {
                        IFile read_file = await read_folder.GetFileAsync(filename);
                        content = await read_file.ReadAllTextAsync();
                        Current_Data.all_quizzes = JsonConvert.DeserializeObject<List<RootQuiz>>(content);
                    }
                    //If not, throw an exception.
                    else
                        throw new Exception("The 'quizzes' item cannot be found");
                    
                }

            }
            catch (Exception e)
            {
                //If there are any errors (e.g. wrong format, HTTP requests failed, etc.), set the 'Quiz' list status to 'false'
                //and display the error, which will stop access to quizzes until the problem is solved.
                Current_Data.quiz_list_status = false;
                Debug.WriteLine("'Quizzes' file error: " + e.Message.ToString());
            }

        }

        private async void LoadResultsFile()
        {

            //First, try loading from the 'results' file from the online database.
            try
            {

                //Firstly, define the url with the appropriate request.
                string result_url = @"http://introtoapps.com/datastore.php?action=load&appid=214328958&objectid=results"; //GET request to load the data from the 'results' item.
                var result_uri = new Uri(string.Format(result_url, string.Empty));
                var result_response = await client.GetAsync(result_uri); //Perform the action.

                //Next, check whether the request is valid.
                if (result_response.IsSuccessStatusCode)
                {
                    //If the request is valid, try to deserialize the .json string into the list of 'Results' objects.
                    string content = await result_response.Content.ReadAsStringAsync();
                    Current_Data.all_results = JsonConvert.DeserializeObject<List<Results>>(content);
                }
                else
                {
                    //If the first request is invalid, this means that the item could not be found.
                    //Display an appropriate error message.
                    Debug.WriteLine("The 'results' item could not be found. Trying to create a new 'results' item.");

                    //The item should have been created server-side which means an exception would be thrown here.
                    //However, in this case, for this assignment, try creating a new 'results' item in the database.
                    string create_result_url = @"http://introtoapps.com/datastore.php?action=save&appid=214328958&objectid=results&data=%5B%5D";
                    var create_result_uri = new Uri(string.Format(create_result_url, string.Empty));
                    var create_result_response = await client.GetAsync(create_result_uri);
                    //Finally, check whether the item is properly created.
                    if (!create_result_response.IsSuccessStatusCode)
                        throw new Exception("All HTTP requests for the 'results' file have failed."); //Throw a final exception to indicate that all requests have failed.
                }
            }
            catch (Exception e)
            {
                //If there are any errors (e.g. wrong format, HTTP requests failed, etc.), set the 'Results' list status to 'false'
                //and display the error, which will be used to stop trying to save the current quiz to the online database until the problem is solved.
                Current_Data.result_list_status = false;
                Debug.WriteLine("'Results' file error: " + e.Message.ToString());
            }

        }

    }
}
