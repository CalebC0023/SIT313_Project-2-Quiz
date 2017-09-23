using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PCLStorage;

namespace SIT313_Project_2_Quiz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {

        StackLayout bottom_btns, layout_content;

        List<Question> _questions; //Store the list of questions in that particular quiz.

        public ResultPage()
        {
            InitializeComponent();

            //Initialize any List.
            _questions = new List<Question>();

            //Get the necessary data.
            _questions = Current_Data.selected_Quiz.questions;

            //Build the base layout
            BuildResultPage();
        }

        public void BuildResultPage()
        {

            //The header label.
            Label header = new Label
            {
                Text = "Results",
                TextColor = Color.FromHex("FFFFFF"), //Set text colour.
                FontAttributes = FontAttributes.Bold, //Set text attributes.
                FontSize = 25, //Set text font.
                BackgroundColor = Color.FromHex("000020"), //Set background colour
                HorizontalOptions = LayoutOptions.CenterAndExpand //Control placement.
            };

            //Set the page's content using the base layout.
            layout_content = BaseResultLayout();

            //The buttons at the bottom of the screen.
            bottom_btns = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                //Places the buttons at the bottom of the page (or side if landscape).
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,

                Children =
                {
                    ResultButtons("New Quiz"),
                    ResultButtons("Profile")
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
        public StackLayout BaseResultLayout()
        {

            //The base result form layout.
            StackLayout result_form = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,

                Children =
                {

                    //Display the final score
                    new Label
                    {
                        Text = string.Format("Final score: {0} out of {1}", Current_Data.review_user_score, Current_Data.review_total_score),
                        TextColor = Color.FromHex("ffffff"),
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 16,
                        BackgroundColor = Color.FromHex("0045b0"),
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    },

                }
            };

            int index = 0;
            foreach (string s in Current_Data.all_user_answers)
            {
                //Decode the answers
                byte[] decode_bytes = Convert.FromBase64String(s);
                string decode_string = Encoding.UTF8.GetString(decode_bytes, 0, decode_bytes.Length);

                result_form.Children.Add(ResultFields(_questions[index].text, decode_string, Current_Data.all_user_results[index]));
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
                HorizontalOptions = LayoutOptions.FillAndExpand,

                Children =
                {

                    // A ScrollView used to help display the entire form within the screen constraints.
                    new ScrollView
                    {
                        Orientation = ScrollOrientation.Vertical, //Controls the scoll direction.
                        IsClippedToBounds = true, //Ensures the ScrollView is 'clipped' to the borders regardless of the amount of content.
                        Content = result_form, //Set the form's layout into the ScollView.

                        //Fill any free space until the border (e.g. objects or screen).
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    }

                }

            };

            return base_layout; //Return the base layout.

        }

        //Base layout of textfields.
        public StackLayout ResultFields(string question, string user_answer, int result)
        {

            //For displaying an icon
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

            //Used to 'mark' wrong questions.
            if (result == 0)
            {
                //Set an appropriate background color for wrong answers
                pre_layout.BackgroundColor = Color.FromHex("ffa897");
                //Display the 'wrong' image.
                _image.Aspect = Aspect.AspectFit;
                _image.Source = ImageSource.FromFile("wrong.png");
            }
            //Used to 'mark' correct questions.
            else
            {
                //Set an appropriate background color for correct answers
                pre_layout.BackgroundColor = Color.FromHex("c8ff97");
                //Display the 'correct' image.
                _image.Aspect = Aspect.AspectFit;
                _image.Source = ImageSource.FromFile("correct.png");
            }

            //Return final layout.
            return pre_layout;

        }

        //Base layout of buttons
        public Button ResultButtons(string label)
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
            else if (label.Equals("Profile"))
            {
                btn.Command = new Command(ToProfile);
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

        //Transition to the 'ProfilePage'.
        async void ToProfile()
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        //Start a new quiz.
        async void ToQuiz()
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

        //Override the 'back' button event.
        protected override bool OnBackButtonPressed()
        {
            ToProfile();
            return true;
        }

    }
}