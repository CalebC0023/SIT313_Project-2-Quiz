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
    public partial class ReviewPage : ContentPage
    {

        StackLayout bottom_btns, layout_content;

        public ReviewPage()
        {
            InitializeComponent();

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

                Children =
                {
                    ReviewAnswerFields("Date", 1),
                    ReviewAnswerFields("Name", 2),
                    ReviewAnswerFields("Diary", 3),
                    ReviewAnswerFields("Gender", 4)
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
        public StackLayout ReviewAnswerFields(string title, int id)
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

            //Used to 'mark' incomplete questions.
            if (id == 2 || id == 3)
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
            await Navigation.PushAsync(new ResultPage());
        }

        //Go back to the 'QuizPage'
        async void ToQuiz()
        {
            await Navigation.PushAsync(new QuizPage());
        }

    }
}