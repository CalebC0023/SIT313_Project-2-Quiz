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

        //Stores the retrieved security details.
        private string _user_answer = null;

        //Decoded strings
        private string _decode_secure_question = null;

        public ResetSecurityPage()
        {
            InitializeComponent();

            //Decode any necessary data
            byte[] question_bytes = Convert.FromBase64String(Current_Data.edit_secure_question);
            _decode_secure_question = Encoding.UTF8.GetString(question_bytes, 0, question_bytes.Length);

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

            entry.TextChanged += (s, e) =>
            {
                _user_answer = e.NewTextValue;
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
                        Text = "Question:\n" + _decode_secure_question, //Display the user's selected question.
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
            if (_user_answer == null || _user_answer == "")
                await this.DisplayAlert("Entry Error", "Please ensure that the answer has been filled.", "OK");
            else
            {

                //Next, check whether the details match with any existing user.

                //Convert from bytes to string (the current format in the database).
                byte[] _answer_bytes = Encoding.UTF8.GetBytes(_user_answer);
                string user_answer = Convert.ToBase64String(_answer_bytes);

                bool match = false; //Check whether the user is correctly found.

                //Go through each recorded user.
                if (Current_Data.edit_secure_answer == user_answer)
                    match = true;

                //If the details match, transition.
                if (match)
                    await Navigation.PushAsync(new ResetPWPage());
                //Else, display the appropriate error message.
                else
                    await this.DisplayAlert("User Error", "The details entered was invalid. Please try again.", "OK");

            }

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