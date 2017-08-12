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
    public partial class PastResultPage : ContentPage
    {
        public PastResultPage()
        {
            InitializeComponent();

            //Build the base layout
            BuildPastResultsPage();
        }

        public void BuildPastResultsPage()
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
            StackLayout layout_content = BasePastResultLayout();

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
        public StackLayout BasePastResultLayout()
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

                    PastResultFields("Date", 1),
                    PastResultFields("Name", 2),
                    PastResultFields("Diary", 3),
                    PastResultFields("Gender", 4)
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
        public StackLayout PastResultFields(string title, int id)
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

    }
}