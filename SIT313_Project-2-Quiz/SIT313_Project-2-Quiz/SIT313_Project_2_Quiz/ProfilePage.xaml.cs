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
    public partial class ProfilePage : ContentPage
    {

        StackLayout layout_content, bottom_btns;

        public ProfilePage()
        {
            InitializeComponent();

            //Build the base layout.
            BuildQuizPage();
        }

        public void BuildQuizPage()
        {

            //The header label.
            Label header = new Label
            {
                Text = "Welcome, (USERNAME)!",
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

            //The base register form layout.
            StackLayout profile_form = new StackLayout
            {
                Orientation = StackOrientation.Vertical,

                Children =
                {                
                    //Get the final list
                    ProfileResults(),
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
            List<string> records = new List<string>
            {
                "21/07/2017 - 10 Questions (7/10 points)",
                "01/03/2017 - 30 Questions (4/30 points)",
                "17/02/2016 - 20 Questions (15/20 points)",
                "30/08/2015 - 30 Questions (16/30 points)"
            };

            /* Create a ListView filled with dummy records.
             * The following code is referenced from the link below.
             * URL: {https://developer.xamarin.com/api/type/Xamarin.Forms.ListView/}
             */
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

            list_layout.ItemSelected += OnItemTapped;


            //Retrun final list.
            return list_layout;

        }

        /* This will handle the cell click events.
         * The following code is referenced from teh link below.
         * URL: {https://www.youtube.com/watch?v=cMRg0P2f9N4}
         */
        private void OnItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            ToPastResult();
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

        /* Transtion to 'QuizPage' after selecting the type of quiz.
         * The following function is referenced from the link below.
         * URL: {https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/navigation/pop-ups/}
         */
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

        //Transition to the 'QuizPage'.
        async void ToResumeQuiz()
        {
            await Navigation.PushAsync(new QuizPage());
        }

        //Transition to the 'PastResultPage'.
        async void ToPastResult()
        {
            await Navigation.PushAsync(new PastResultPage());
        }

    }
}