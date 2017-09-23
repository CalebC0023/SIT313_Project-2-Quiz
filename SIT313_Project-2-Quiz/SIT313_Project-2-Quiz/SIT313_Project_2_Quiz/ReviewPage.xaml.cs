using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PCLStorage;
using System.Net.Http;
using Newtonsoft.Json;

namespace SIT313_Project_2_Quiz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewPage : ContentPage
    {

        StackLayout bottom_btns, layout_content;

        List<Question> _questions; //Store the list of questions in that particular quiz.

        //Call a new HTTP Client for online requests.
        private HttpClient client = new HttpClient();

        public ReviewPage()
        {
            InitializeComponent();

            //Initialize any List.
            _questions = new List<Question>();

            //Get the necessary data.
            _questions = Current_Data.selected_Quiz.questions;

            //Build the base layout
            BuildReviewPage();
        }

        public void BuildReviewPage()
        {

            //The header label.
            Label header = new Label
            {
                Text = "Review Quiz",
                TextColor = Color.FromHex("FFFFFF"), //Set text colour.
                FontAttributes = FontAttributes.Bold, //Set text attributes.
                FontSize = 25, //Set text font.
                BackgroundColor = Color.FromHex("000020"), //Set background colour
                HorizontalOptions = LayoutOptions.CenterAndExpand //Control placement.
            };

            //Set the page's content using the base layout.
            layout_content = BaseReviewLayout();

            //The buttons at the bottom of the screen.
            bottom_btns = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                //Places the buttons at the bottom of the page (or side if landscape).
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,

                Children =
                {
                    ReviewButtons("Return"),
                    ReviewButtons("Submit")
                }
            };

            //Add the extra buttons to the layout.
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
        public StackLayout BaseReviewLayout()
        {

            //The base review form layout.
            StackLayout review_form = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            int index = 0;
            foreach (string s in Current_Data.all_user_answers)
            {
                //Decode the answers
                byte[] decode_bytes = Convert.FromBase64String(s);
                string decode_string = Encoding.UTF8.GetString(decode_bytes, 0, decode_bytes.Length);

                review_form.Children.Add(ReviewAnswerFields(_questions[index].text, decode_string));
                index++;
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
                        Content = review_form, //Set the form's layout into the ScollView.

                        //Fill any free space until the border (e.g. objects or screen).
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    }

                }

            };

            return base_layout; //Return the base layout.

        }

        //Base layout of textfields.
        public StackLayout ReviewAnswerFields(string question, string user_answer)
        {

            //For displaying a 'caution' icon
            Image _image = new Image
            {
                //Set appropriate size
                HeightRequest = 15,
                WidthRequest = 15,
                //Postion the image at the top-right corner.
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            };

            //Return this Stacklayout after applying the changes
            StackLayout pre_layout = new StackLayout
            {
                Spacing = 1,
                Padding = new Thickness(8, 8, 8, 8),
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    //The label for the answer field.
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label
                            {
                                Text = question + ":", //Set appropriate label.
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Center
                            },
                            _image
                        }
                    },
                    //The answer text.
                    new Label
                    {
                        Text = "Answer: " + user_answer, //Set dummy text.
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };

            //Used to 'mark' incomplete questions.
            if (user_answer == "" || user_answer == null)
            {
                //Set an appropriate background color to 'caution' the user
                pre_layout.BackgroundColor = Color.FromHex("fff997");
                //Display the 'caution' image.
                _image.Aspect = Aspect.AspectFit;
                _image.Source = ImageSource.FromFile("Caution.png");
            }

            //Return final layout.
            return pre_layout;

        }

        //Base layout of buttons
        public Button ReviewButtons(string label)
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
            if (label.Equals("Submit"))
            {
                btn.Command = new Command(ToResult);
            }
            else if (label.Equals("Return"))
            {
                btn.Command = new Command(ToQuiz);
            }

            return btn; //Return this button.

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

        //Transitions to the 'ResultPage'
        async void ToResult()
        {
            try
            {
                //Check the status of the 'Results' list.
                if (Current_Data.result_list_status)
                {
                    //Stores the results for the questions
                    List<int> _results = new List<int>();

                    int _index = 0;
                    //Check each answer.
                    foreach (string s in Current_Data.all_user_answers)
                    {
                        //Decode the answers
                        byte[] _decode_bytes = Convert.FromBase64String(s);
                        string _decode_string = Encoding.UTF8.GetString(_decode_bytes, 0, _decode_bytes.Length);

                        //If the answer is similar, add the results ('0' is 'Wrong', '1' is 'Correct')
                        if (_decode_string == _questions[_index].answer)
                            _results.Add(1);
                        else
                            _results.Add(0);

                        _index++;
                    };

                    //Store the user's results
                    Current_Data.all_user_results = _results;

                    //Get the total score.
                    int total_score = _questions.Count();
                    //Get the user score.
                    int user_score = 0;
                    foreach (int r in _results)
                    {
                        if (r == 1)
                            user_score++;
                    }

                    //Temporarily store the scores
                    Current_Data.review_total_score = total_score;
                    Current_Data.review_user_score = user_score;

                    //Only registered users may save their results
                    if (!Current_Data.isGuest)
                    {

                        //Finally, try to add it to the online database.

                        //First is to build the url.
                        StringBuilder url_sb = new StringBuilder();
                        url_sb.Append(@"http://introtoapps.com/datastore.php?action=append&appid=214328958&objectid=results&data=");

                        //Next, add each field.
                        url_sb.AppendFormat(@"%7B%22user%22%3a%22{0}%22%2C", Current_Data.Username);
                        url_sb.AppendFormat(@"%22quiz%5Fid%22%3a%22{0}%22%2C", Current_Data.selected_Quiz.id);

                        url_sb.AppendFormat(@"%22quiz%5Fanswers%22%3a%5B");
                        bool first_ans = true;
                        foreach (string ans in Current_Data.all_user_answers)
                        {
                            if (first_ans)
                            {
                                url_sb.AppendFormat(@"%22{0}%22", ans);
                                first_ans = false;
                            }
                            else
                                url_sb.AppendFormat(@"%2C%22{0}%22", ans);
                        }

                        url_sb.AppendFormat(@"%5D%2C%22quiz%5Fresults%22%3a%5B");
                        bool first_res = true;
                        foreach (int res in _results)
                        {
                            if (first_res)
                            {
                                url_sb.AppendFormat(@"{0}", res);
                                first_res = false;
                            }
                            else
                                url_sb.AppendFormat(@"%2C{0}", res);
                        }

                        url_sb.AppendFormat(@"%5D%2C%22quiz%5Ftotal%5Fscore%22%3a{0}", total_score);
                        url_sb.AppendFormat(@"%2C%22quiz%5Fuser%5Fscore%22%3a{0}", user_score);
                        url_sb.AppendFormat(@"%7D");

                        //Then, try loading the request.
                        string append_result_url = url_sb.ToString();
                        var append_result_uri = new Uri(string.Format(append_result_url, string.Empty));
                        var append_result_response = await client.GetAsync(append_result_uri); //Perform the action.

                        //Next, check whether the request is valid.
                        if (append_result_response.IsSuccessStatusCode)
                        {
                            //If it's valid, add the result to the temporary list.
                            reload_results_list();
                            //Display the alert.
                            await this.DisplayAlert("Quiz Submission", "The quiz has been submitted", "OK");

                        }
                        else
                        {
                            //Throw an exception.
                            throw new Exception("The HTTP request has failed.");
                        }

                        //Finally, try to reset the user's folder.
                        try
                        {
                            //Finally, delete the 'ongoing_file' for this user.
                            string folder_name = Current_Data.Username + "ongoing_files"; //The folder name for the specific user's saved ongoing quizzes.

                            //First, find the file
                            ExistenceCheckResult exist = await Current_Data.root_folder.CheckExistsAsync(folder_name);

                            //Then, reset it.
                            if (exist == ExistenceCheckResult.FolderExists)
                                await Current_Data.root_folder.CreateFolderAsync(folder_name, CreationCollisionOption.ReplaceExisting);

                            Current_Data.ongoingQuiz = false; //Set the current state of the quiz to 'none'.

                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Reset File Error:" + e.Message.ToString());
                        }

                    }
                    else
                        await this.DisplayAlert("Save Results", "Only registered user may save their results. Please register to use this function.", "OK");

                    await Navigation.PushAsync(new ResultPage());

                }
                else
                {
                    await this.DisplayAlert("Results List Error", "There was an error with retrieving the 'Results' list. Please try restarting the app.", "OK");
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("Saving Result Error:" + e.Message.ToString());
            }

        }

        //Reload the user list.
        public async void reload_results_list()
        {

            //First, try loading from the 'results' file from the online database.
            try
            {

                //Firstly, define the url with the appropriate request.
                string results_url = @"http://introtoapps.com/datastore.php?action=load&appid=214328958&objectid=results";
                var results_uri = new Uri(string.Format(results_url, string.Empty));
                var results_response = await client.GetAsync(results_url); //Perform the action.

                //Next, check whether the request is valid.
                if (results_response.IsSuccessStatusCode)
                {
                    //If the request is valid, try to deserialize the .json string into the list of 'Results' objects.
                    string content = await results_response.Content.ReadAsStringAsync();
                    Current_Data.all_results = JsonConvert.DeserializeObject<List<Results>>(content);
                }
                else
                    throw new Exception("The update of the 'results' file have failed.");
            }
            catch (Exception e)
            {
                //If there are any errors (e.g. wrong format, HTTP requests failed, etc.), set the 'Results' list status to 'false'
                //and display the error, which will be used to stop any logins/edits/registers until the problem is solved.
                Current_Data.result_list_status = false;
                Debug.WriteLine("'Results' file error: " + e.Message.ToString());
            }

        }

        //Go back to the 'QuizPage'
        async void ToQuiz()
        {
            await Navigation.PushAsync(new QuizPage());
        }

    }
}