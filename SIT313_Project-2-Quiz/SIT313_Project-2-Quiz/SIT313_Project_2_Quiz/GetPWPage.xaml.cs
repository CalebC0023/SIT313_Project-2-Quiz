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
    public partial class GetPWPage : ContentPage
    {
        public GetPWPage()
        {
            InitializeComponent();

            //Build the base layout
            BuildGetPWPage();
        }

        //Build the base layout for the 'GetPWPage'.
        public void BuildGetPWPage()
        {

            //The header label.
            Label header = new Label
            {
                Text = "Reset Password",
                TextColor = Color.FromHex("FFFFFF"), //Set text colour.
                FontAttributes = FontAttributes.Bold, //Set text attributes.
                FontSize = 25, //Set text font.
                BackgroundColor = Color.FromHex("000020"), //Set background colour
                HorizontalOptions = LayoutOptions.CenterAndExpand //Control placement.
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

                    //Layout for the text entry field.
                    new StackLayout
                    {
                        Spacing = 1,
                        Orientation = StackOrientation.Horizontal,

                        Children = {
                            //The label for the textfield.
                            new Label
                            {
                                Text = "Username:",
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Center
                            },
                            //The entry textfield.
                            new Entry
                            {
                                Placeholder = "Enter Username",
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                            }
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
                                Text = "Reset",
                                TextColor = Color.FromHex("FFFFFF"),
                                //Set the prefered size for the button
                                HeightRequest = 40,
                                WidthRequest = 150,
                                Command =  new Command(ConfirmReset), //Set the click event.
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

        /* Popup a confirmation alert.
         * The following code was referenced from the tutorial below.
         * URL: {https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/navigation/pop-ups/}
         */
        async void ConfirmReset()
        {
            //Get the answer for the alert.
            bool answer = await DisplayAlert("Confirmation", "Are you sure you want to reset Password?", "Yes", "No");
            //If the user 'accepts', run another action (e.g. transition to new page).
            if (answer)
            {
                ToResetSecurityPage();
            }
        }

        //Transitions to the 'ResetSecurityPage'.
        async void ToResetSecurityPage()
        {
            await Navigation.PushAsync(new ResetSecurityPage());
        }

    }
}