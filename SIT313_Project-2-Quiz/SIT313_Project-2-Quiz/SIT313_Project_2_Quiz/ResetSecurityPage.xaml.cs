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
    public partial class ResetSecurityPage : ContentPage
    {
        public ResetSecurityPage()
        {
            InitializeComponent();

            //Build the base layout
            BuildResetSecurityPage();
        }

        //Build the base layout for the 'ResetSecurityPage'.
        public void BuildResetSecurityPage()
        {

            //The header label.
            Label header = new Label
            {
                Text = "Security Question",
                TextColor = Color.FromHex("FFFFFF"), //Set text colour.
                FontAttributes = FontAttributes.Bold, //Set text attributes.
                FontSize = 25, //Set text font.
                BackgroundColor = Color.FromHex("000020"), //Set background colour
                HorizontalOptions = LayoutOptions.CenterAndExpand //Control placement.
            };

            //The entry textfield.
            Entry entry = new Entry
            {
                Placeholder = "Enter Answer",
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            //The content StackLayout.
            StackLayout layout_content = new StackLayout
            {

                Spacing = 3, //Assign appropriate spacing.
                Orientation = StackOrientation.Vertical, //Set orientation to vertical (Display from top to bottom).
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("e6fcff"), //Set background colour.
                Padding = new Thickness(7, 13, 7, 0), //Set the padding for a better style.

                Children =
                {

                    //Layout for the security question's label.
                    new Label
                    {
                        Text = "Question:\nWhat is the Deakin Code for this subject?",
                        HorizontalTextAlignment = TextAlignment.Center, //Set the text's allignment
                        FontAttributes = FontAttributes.Bold, //Set text attributes.
                        FontSize = 20, //Set text font.
                        HorizontalOptions = LayoutOptions.CenterAndExpand //Control placement.
                    },

                    //Layout for the text entry field.
                    new StackLayout
                    {
                        Spacing = 1,
                        Orientation = StackOrientation.Horizontal,

                        Children = {
                            //The label for the textfield.
                            new Label
                            {
                                Text = "Answer:",
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Center
                            },
                            entry
                        }
                    },

                    //Layout for the button.
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,

                        Children =
                        {
                            new Button
                            {
                                Text = "Confirm",
                                TextColor = Color.FromHex("FFFFFF"),
                                //Set the prefered size for the button
                                HeightRequest = 40,
                                WidthRequest = 150,
                                Command =  new Command(ToResetPWPage), //Set the click event.
                                BackgroundColor = Color.FromHex("000f3c"),
                            }
                        }
                    }

                }

            };

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

        //Transitions to the 'ResetPWPage'.
        async void ToResetPWPage()
        {
            await Navigation.PushAsync(new ResetPWPage());
        }

    }
}