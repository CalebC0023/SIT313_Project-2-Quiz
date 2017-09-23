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

        //Stores the entered value.
        private string _username = null;

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

            //The entry textfield.
            Entry entry = new Entry
            {
                Placeholder = "Enter Username",
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            entry.TextChanged += (s, e) =>
            {
                _username = e.NewTextValue;
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

            if (_username == null || _username == "")
                await this.DisplayAlert("Entry Error", "Please ensure that the Username has been filled.", "OK");
            else
            {

                //Next, check whether the details match with any existing user.

                //Convert from bytes to string (the current format in the database).
                byte[] _user_bytes = Encoding.UTF8.GetBytes(_username);
                string username = Convert.ToBase64String(_user_bytes);

                //Store user details for editting.
                string secure_question = null;
                string secure_answer = null;

                bool found = false; //Check whether the user is correctly found.

                //Go through each recorded user.
                if (Current_Data.all_users != null)
                {
                    foreach (User u in Current_Data.all_users)
                    {
                        if (u.username == username)
                        {
                            found = true;
                            secure_question = u.security_question;
                            secure_answer = u.security_answer;
                        }
                    }
                }

                //If the details match, transition.
                if (found)
                {

                    //Get the confirmation to continue.
                    bool answer = await DisplayAlert("Confirmation", "Are you sure you want to reset Password?", "Yes", "No");
                    //If the user 'accepts', store data and transition.
                    if (answer)
                    {
                        Current_Data.edit_username = username;
                        Current_Data.edit_secure_question = secure_question;
                        Current_Data.edit_secure_answer = secure_answer;
                        ToResetSecurityPage();
                    }

                }
                //Else, display the appropriate error message.
                else
                    await this.DisplayAlert("User Error", "The details entered was invalid. Please try again.", "OK");

            }

        }

        //Transitions to the 'ResetSecurityPage'.
        async void ToResetSecurityPage()
        {
            await Navigation.PushAsync(new ResetSecurityPage());
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