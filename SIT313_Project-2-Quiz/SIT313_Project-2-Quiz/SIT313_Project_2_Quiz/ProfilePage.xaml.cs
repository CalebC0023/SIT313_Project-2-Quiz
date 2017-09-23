using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PCLStorage;
using Newtonsoft.Json;

namespace SIT313_Project_2_Quiz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {

        StackLayout layout_content, bottom_btns;

        //Temporarily store the results for the user.
        private List<Results> _user_results;

        //Decoded strings.
        private string _profile_username = null;

        public ProfilePage()
        {
            InitializeComponent();
            _user_results = new List<Results>();

            //Decode any necessary data only if there is a registered user.
            if (!Current_Data.isGuest)
            {
                byte[] user_bytes = Convert.FromBase64String(Current_Data.Username);
                _profile_username = Encoding.UTF8.GetString(user_bytes, 0, user_bytes.Length);
            }
            else
                _profile_username = "Guest";

            //Get user's results.
            GetUserResults();

            //Get user's details (including any ongoing quiz).
            GetCurrentUserDetails();

            //Build the base layout.
            BuildQuizPage();
        }

        public void BuildQuizPage()
        {

            //The header label.
            Label header = new Label
            {
                Text = "Welcome, '" + _profile_username + "'!", //Set the Profile Label to the current Username.
                TextColor = Color.FromHex("FFFFFF"), //Set text colour.
                FontAttributes = FontAttributes.Bold, //Set text attributes.
                FontSize = 25, //Set text font.
                BackgroundColor = Color.FromHex("000020"), //Set background colour
                HorizontalOptions = LayoutOptions.CenterAndExpand //Control placement.
            };

            //Create the base content layout.
            layout_content = BaseProfileLayout();

            //The buttons at the bottom of the screen.
            bottom_btns = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                //Places the buttons at the bottom of the page (or side if landscape).
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,

                Children =
                {
                    ProfileButtons("New Quiz"),
                    ProfileButtons("Resume Quiz")
                }
            };

            //Add the last objects to the layout
            layout_content.Children.Add(bottom_btns);

            //Combine and build base layout.
            this.Content = new StackLayout
            {
                BackgroundColor = Color.FromHex("000020"), //Set base background colour.
                Orientation = StackOrientation.Vertical, //Set base orientation.
                Children =
                {
                    header,
                    layout_content
                }
            };

        }

        //The base layout for the content.
        public StackLayout BaseProfileLayout()
        {

            //Get the final list if there are any results for that user.
            ListView result_listview = new ListView(); //Define the new ListView

            //The base register form layout.
            StackLayout profile_form = new StackLayout
            {
                Orientation = StackOrientation.Vertical,

                Children =
                {                
                    //Set the final list.
                    //result_listview
                    ProfileResults() //For testing
                }
            };

            //Combine all components
            StackLayout base_layout = new StackLayout
            {

                Spacing = 3, //Assign appropriate spacing.
                Orientation = StackOrientation.Vertical, //Set orientation to vertical (Display from top to bottom).
                BackgroundColor = Color.FromHex("e6fcff"), //Set background colour.
                Padding = new Thickness(7, 13, 7, 0), //Set the padding for a better style.
                VerticalOptions = LayoutOptions.FillAndExpand,

                Children =
                {

                    // A ScrollView used to help display the entire form within the screen constraints.
                    new ScrollView
                    {
                        Orientation = ScrollOrientation.Vertical, //Controls the scoll direction.
                        IsClippedToBounds = true, //Ensures the ScrollView is 'clipped' to the borders regardless of the amount of content.
                        Content = profile_form, //Set the form's layout into the ScollView.

                        //Fill any free space until the border (e.g. objects or screen).
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    }

                }

            };

            return base_layout; //Return the base layout.

        }

        //Base layout of buttons
        public Button ProfileButtons(string label)
        {

            //Return this button layout after applying the changes
            Button btn = new Button
            {
                Text = label, //Set appropriate label.
                TextColor = Color.FromHex("FFFFFF"),
                //Set the prefered size for the button
                HeightRequest = 40,
                WidthRequest = 150,
                BackgroundColor = Color.FromHex("000f3c"),
                HorizontalOptions = LayoutOptions.Center
            };

            // Verify which button click event is applied.
            if (label.Equals("New Quiz"))
            {
                btn.Command = new Command(ToQuiz);
            }
            else if (label.Equals("Resume Quiz"))
            {
                btn.Command = new Command(ToResumeQuiz);
            }

            return btn; //Return this button.

        }

        //Base layout of ListView
        public ListView ProfileResults()
        {

            //For getting each item from the list.
            int count = 0;

            //The ListView example items.
            List<string> records = new List<string>();

            //Add all available records
            foreach (Results user_result in _user_results)
                records.Add(string.Format("Quiz ID: {0} ({1} out of {2} points)", user_result.quiz_id, user_result.quiz_user_score, user_result.quiz_total_score));

            // Create a ListView filled with the user's records.
            ListView list_layout = new ListView
            {
                ItemsSource = records,

                //Go through each 'record' in the ItemsSource.
                ItemTemplate = new DataTemplate(() =>
                {

                    Label record = new Label();
                    //Assign the record per list cell.
                    record.Text = records[count];
                    count++;

                    BoxView boxView = new BoxView
                    {
                        BackgroundColor = Color.FromHex("e6fcff")
                    };

                    // Return an assembled ViewCell.
                    return new ViewCell
                    {

                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                boxView,
                                new StackLayout
                                {
                                    VerticalOptions = LayoutOptions.Center,
                                    Spacing = 0,
                                    Children =
                                    {
                                        record
                                    }
                                }
                            }
                        }
                    };

                })

            };

            //Check which item is currently selected.
            list_layout.ItemSelected += (s, e) =>
            {
                int index = 0; //The current index in the list.
                int item_index = 0; //The index of the selected item.
                //Go through
                foreach (string str in records)
                {
                    if (e.SelectedItem.ToString() == str)
                        item_index = index;

                    index++;
                }
                ToPastResult(item_index);
            };


            //Retrun final list.
            return list_layout;

        }

        // Gets the current orientation.
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //Controls the layout for each orientation
            if (width > height)
            {
                //Landscape layout

                layout_content.Orientation = StackOrientation.Horizontal; //Change the StackOrientation for landscape.

                //Change the button's postion for landscape.
                bottom_btns.Orientation = StackOrientation.Vertical;
                bottom_btns.VerticalOptions = LayoutOptions.Start;
                bottom_btns.HorizontalOptions = LayoutOptions.End;
            }
            else
            {
                //Portrait layout

                layout_content.Orientation = StackOrientation.Vertical; //Change the StackOrientation for portrait.

                //Change the button's postion for portrait.
                bottom_btns.Orientation = StackOrientation.Horizontal;
                bottom_btns.VerticalOptions = LayoutOptions.End;
                bottom_btns.HorizontalOptions = LayoutOptions.Center;
            }
        }

        //Try to open and find the user's current quiz.
        async void GetCurrentUserDetails()
        {

            //Only registered users can resume unfinished quizzes.
            if (!Current_Data.isGuest)
            {

                try
                {

                    //Get the folder and file name
                    string folder_name = Current_Data.Username + "ongoing_files"; //The folder name for the specific user's saved ongoing quizzes.
                    string filename = Current_Data.Username +"_ongoingQuiz.txt"; //This will create/overwrite the specific user's local file.

                    //First, get the folder.
                    IFolder read_folder = await Current_Data.root_folder.GetFolderAsync(folder_name);

                    //Check whether the file exists.
                    ExistenceCheckResult file_exist = await read_folder.CheckExistsAsync(filename);

                    string content = null;
                    //Read from the file if it exists.
                    if (file_exist == ExistenceCheckResult.FileExists)
                    {
                        //Once the file is found, get the .json string from it.
                        IFile read_file = await read_folder.GetFileAsync(filename);
                        content = await read_file.ReadAllTextAsync();

                        //Then, deserialize it.
                        List<Results> base_ongoing_quiz = new List<Results>();
                        base_ongoing_quiz = JsonConvert.DeserializeObject<List<Results>>(content);

                        //Lastly, extract the data. (Should only be 1 'object')
                        foreach (Results q in base_ongoing_quiz)
                        {
                            Current_Data.ongoing_Quiz = q;

                            //Next, get the current quiz.
                            foreach (RootQuiz quiz in Current_Data.all_quizzes)
                            {
                                if (Current_Data.ongoing_Quiz.quiz_id == quiz.id)
                                    Current_Data.selected_Quiz = quiz;
                            }
                        }

                        Current_Data.ongoingQuiz = true;

                    }
                    //If not, throw an exception.
                    else
                        throw new Exception("The user's file in the 'ongoing_files' folder cannot be found");

                }
                catch (Exception e)
                {
                    Current_Data.ongoingQuiz = false;
                    Debug.WriteLine("Extract File Error:" + e.Message.ToString());
                }

            }

        }

        async void GetUserResults()
        {
            //Only registered users have recorded results.
            if (!Current_Data.isGuest)
            {

                //The user may only view the past results if all the file list are valid.
                //(The 'user' list status was already checked beforehand)
                if (Current_Data.quiz_list_status && Current_Data.result_list_status)
                {
                    //Get all the results for this user.
                    foreach (Results r in Current_Data.all_results)
                    {
                        if (r.user == Current_Data.Username)
                            _user_results.Add(r);
                    }
                }
                else
                    await this.DisplayAlert("File Alert", "There was an error with retrieving some of the lists. Please try restarting the app.", "OK");

            }
        }

        // Transtion to 'QuizPage' after selecting the type of quiz.
        async void ToQuiz()
        {

            //The user may only start a quiz if the 'quizzes' list is valid.
            if (Current_Data.quiz_list_status)
            {

                bool confirm_new_quiz = true;

                if (Current_Data.ongoingQuiz)
                {
                    confirm_new_quiz = false;
                    string new_quiz = await DisplayActionSheet("There is an ongoing quiz. Start a new quiz anyway?", "Cancel", null, "Yes");
                    if (new_quiz.Contains("Yes"))
                        confirm_new_quiz = true;
                }

                //If the user chooses to start a new quiz regardless, continue.
                if (confirm_new_quiz)
                {
                    //Display the 'Dialog Action Sheet' for displaying the different types of quizzes.
                    string quiz_selection = await DisplayActionSheet("Select type:", "Cancel", null, "Quiz 1", "Quiz 2");
                    //Depending on which was selected, load the 'QuizPage'. For Project 2, there will be two 'set' quizzes available.
                    if (quiz_selection.Contains("Quiz 1"))
                        Current_Data.selected_Quiz = Current_Data.all_quizzes[0]; //Get the first quiz in the list
                    else if (quiz_selection.Contains("Quiz 2"))
                        Current_Data.selected_Quiz = Current_Data.all_quizzes[1]; //Get the second quiz in the list

                    Current_Data.ongoingQuiz = false;

                    await Navigation.PushAsync(new QuizPage());
                }

            }
            else
                await this.DisplayAlert("Quiz Alert", "There was an error with retrieving the 'Quiz' list. Please try restarting the app.", "OK");

        }

        //Transition to the 'QuizPage'.
        async void ToResumeQuiz()
        {

            //The user may only start a quiz if the 'quizzes' list is valid.
            if (Current_Data.quiz_list_status)
            {
                //Transition only if there is an ongoing quiz.
                if (Current_Data.ongoingQuiz)
                    await Navigation.PushAsync(new QuizPage());
            }
            else
                await this.DisplayAlert("Quiz Alert", "There was an error with retrieving the 'Quiz' list. Please try restarting the app.", "OK");

        }

        //Transition to the 'PastResultPage'.
        async void ToPastResult(int _index)
        {
            //The user may only view the past results if all the file list are valid.
            //(The 'user' list status was already checked beforehand)
            if (Current_Data.quiz_list_status && Current_Data.result_list_status)
            {

                //Store the selected result for viewing.
                Current_Data.selected_Results = _user_results[_index];
                foreach (RootQuiz qz in Current_Data.all_quizzes)
                {
                    if (qz.id == Current_Data.selected_Results.quiz_id)
                        Current_Data.reference_Quiz = qz;
                }

                await Navigation.PushAsync(new PastResultPage());
            }
            else
                await this.DisplayAlert("File Alert", "There was an error with retrieving some of the lists. Please try restarting the app.", "OK");

        }

        //Override the 'back' button event.
        protected override bool OnBackButtonPressed()
        {
            CustomBackBtnEvent();
            return true;
        }

        //The custom event for the back button.
        async void CustomBackBtnEvent()
        {
            await Navigation.PushAsync(new MainPage());
        }

    }
}