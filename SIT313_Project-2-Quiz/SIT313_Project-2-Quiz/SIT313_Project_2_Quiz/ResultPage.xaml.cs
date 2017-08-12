using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SIT313_Project_2_Quiz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {

        StackLayout bottom_btns, layout_content;

        public ResultPage()
        {
            InitializeComponent();

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
                        Text = "Final Score: 2/4",
                        TextColor = Color.FromHex("ffffff"),
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 16,
                        BackgroundColor = Color.FromHex("0045b0"),
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    },

                    ResultFields("Date", 1),
                    ResultFields("Name", 2),
                    ResultFields("Diary", 3),
                    ResultFields("Gender", 4)
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
        public StackLayout ResultFields(string title, int id)
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
                                Text = title + ":", //Set appropriate label.
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Center
                            },
                            _image
                        }
                    },
                    //The answer text.
                    new Label
                    {
                        Text = "[ ANSWER ]", //Set dummy text.
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };

            //Used to 'mark' wrong questions.
            if (id == 2 || id == 3)
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

        //Transition to the 'ReviewPage'.
        async void ToQuiz()
        {
            //Display the 'Dialog Action Sheet' for displaying the different types of quizzes.
            string action = await DisplayActionSheet("Select type:", "Cancel", null, "10 Questions", "20 Questions", "30 Questions");
            //Depending on which was selected, load the 'QuizPage'. For Project 1, it will all load the same type.
            if (action.Contains("10 Questions") || action.Contains("20 Questions") || action.Contains("30 Questions"))
            {
                await Navigation.PushAsync(new QuizPage());
            }
        }

    }
}