using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PCLCrypto;
using System.Net.Http;
using Newtonsoft.Json;

namespace SIT313_Project_2_Quiz
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPWPage : ContentPage
    {

        //Variables used for dynamic layout changes.
        StackLayout new_PW_field_1 = new StackLayout();
        StackLayout new_PW_field_2 = new StackLayout();

        //Temporarily store the new 'User' details for verification.
        private string _password = null;
        private string _confirm_password = null;

        //Check whether each field's value is valid.
        private bool password_valid = false;
        private bool confirm_pw_valid = false;

        //Check whether the form is correctly filled out.
        private bool _form_valid = false;

        //Define the SHA256 hasher for encrytion.
        IHashAlgorithmProvider pwHash = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);

        //Call a new HTTP Client for online requests.
        private HttpClient client = new HttpClient();

        public ResetPWPage()
        {
            InitializeComponent();

            //Build the base layout
            BuildResetPWPage();
        }

        //Build the base layout for the 'ResetPWPage'.
        public void BuildResetPWPage()
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

            //Assign the dynamic variables.
            new_PW_field_1 = ResetPWTextFields("New Password");
            new_PW_field_2 = ResetPWTextFields("Confirm Password");

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

                    //Layout for the text entry fields.
                    new_PW_field_1,
                    new_PW_field_2,

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
                                Command =  new Command(ResetPassword), //Set the click event.
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

        //Base layout of textfields.
        public StackLayout ResetPWTextFields(string label)
        {

            //Define the base Entry field.
            Entry MainEntry = new Entry
            {
                Placeholder = label, //Set appropriate placeholder text.
                IsPassword = true, //If true, set block characters to hide passwords.
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            //Define the base Error Label field.
            Label ErrorLbl = new Label
            {
                Text = "",
                TextColor = Color.FromHex("#ff0000"), //Set the default colour to 'Red'.
                FontSize = 1, //This essentially 'hides' the label.
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            if (label == "New Password")
            {
                MainEntry.TextChanged += (s, e) =>
                {

                    //First check whether something has been entered.
                    if (e.NewTextValue == null || e.NewTextValue == "")
                    {
                        //Display the error.
                        ErrorLbl.Text = "The Password must be filled.";
                        ErrorLbl.TextColor = Color.FromHex("#ff0000");
                        ErrorLbl.FontSize = 12;
                        password_valid = false; //Set the value to invalid.
                    }
                    else
                    {
                        //'Remove' the error.
                        ErrorLbl.Text = "";
                        ErrorLbl.FontSize = 1;

                        //Next, check whether the value's length is within the limit.
                        if (e.NewTextValue.Length > 20)
                        {
                            //Display the error.
                            ErrorLbl.Text = "The Password cannot be over 20 characters long.";
                            ErrorLbl.TextColor = Color.FromHex("#ff0000");
                            ErrorLbl.FontSize = 12;
                            password_valid = false;
                        }
                        else
                        {
                            //'Remove' the error.
                            ErrorLbl.Text = "";
                            ErrorLbl.FontSize = 1;

                            //Check the Regex. The username can only contain 'letters' from any language, 'numbers' and 'punctuation symbols'.
                            if (!Regex.IsMatch(e.NewTextValue, @"^[\p{L}\p{N}\p{P}]+$"))
                            {
                                //Display the error.
                                ErrorLbl.Text = "The Password must only contain 'letter', 'numbers' and 'punctuation symbols' (e.g. Test0123!).";
                                ErrorLbl.TextColor = Color.FromHex("#ff0000");
                                ErrorLbl.FontSize = 12;
                                password_valid = false;
                            }
                            else
                            {
                                //Display the 'Warning'. In this case, the user must input the passwrod into this field first,
                                //then reconfirm it again in the next.
                                ErrorLbl.Text = "- Warning: Remember to retype the password in the 'Confirm Password' field -";
                                ErrorLbl.TextColor = Color.FromHex("#c2a200"); //The 'Warning' text colour.
                                ErrorLbl.FontSize = 12;

                                //If it clears all verification, the value is valid.
                                _password = e.NewTextValue;
                                password_valid = true;
                                confirm_pw_valid = false; //This would ensure that the user double-checks both passwords.
                            }
                        }
                    }

                };
            }
            else if (label == "Confirm Password")
            {
                MainEntry.TextChanged += (s, e) =>
                {

                    //First check whether something has been entered.
                    if (e.NewTextValue == null || e.NewTextValue == "")
                    {
                        //Display the error.
                        ErrorLbl.Text = "The Password must be filled for confirmation.";
                        ErrorLbl.FontSize = 12;
                        confirm_pw_valid = false; //Set the value to invalid.
                    }
                    else
                    {
                        //'Remove' the error.
                        ErrorLbl.Text = "";
                        ErrorLbl.FontSize = 1;

                        //Next, check whether the value's length is within the limit.
                        if (e.NewTextValue.Length > 20)
                        {
                            //Display the error.
                            ErrorLbl.Text = "The Password cannot be over 20 characters long.";
                            ErrorLbl.FontSize = 12;
                            confirm_pw_valid = false;
                        }
                        else
                        {
                            //'Remove' the error.
                            ErrorLbl.Text = "";
                            ErrorLbl.FontSize = 1;

                            //Check whether the passwords match.
                            if (e.NewTextValue != _password)
                            {
                                //Display the error.
                                ErrorLbl.Text = "The Passwords do not match.";
                                ErrorLbl.FontSize = 12;
                                confirm_pw_valid = false;
                            }
                            else
                            {
                                //'Remove' the error.
                                ErrorLbl.Text = "";
                                ErrorLbl.FontSize = 1;

                                //If it clears all verification, the value is valid.
                                _confirm_password = e.NewTextValue;
                                confirm_pw_valid = true;
                            }
                        }
                    }

                };
            }

            //Return this Stacklayout after applying the changes
            return new StackLayout //The base layout containing the answer fields and error label.
            {
                Spacing = 0,
                Orientation = StackOrientation.Vertical,
                Children = {
                    ErrorLbl,
                    new StackLayout //The layout containing the answer field label and entry.
                    {
                        Spacing = 1,
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            //The label for the textfield.
                            new Label
                            {
                                Text = label + ":", //Set appropriate label.
                                WidthRequest = 140, //Set appropriate width.
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Center
                            },
                            MainEntry
                        }
                    }
                }
            };

        }

        //Gets the current orientation.
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //Controls the layout for each orientation
            if (width > height)
            {
                //Landscape layout
                new_PW_field_1.Orientation = StackOrientation.Horizontal; //Change the StackOrientation for landscape.
                new_PW_field_2.Orientation = StackOrientation.Horizontal;
            }
            else
            {
                //Portrait layout
                new_PW_field_1.Orientation = StackOrientation.Vertical; //Change the StackOrientation for portrait.
                new_PW_field_2.Orientation = StackOrientation.Vertical;
            }
        }

        //Popup a confirmation alert then transition to the 'MainPage'.
        async void ResetPassword()
        {

            try
            {
                //Check whether all the fields are valid.
                if (password_valid && confirm_pw_valid)
                    _form_valid = true;
                else
                    _form_valid = false;

                if (!_form_valid)
                    await DisplayAlert("Invalid Form", "There were invalid entries in the form. Please resolve any error.", "OK");
                else
                {

                    //Hash the password
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in pwHash.HashData(Encoding.UTF8.GetBytes(_password)))
                        sb.Append(b.ToString("X2"));

                    string password = sb.ToString();

                    //Update the user
                    foreach (User u in Current_Data.all_users)
                    {
                        if (u.username == Current_Data.edit_username)
                            u.password = password;
                    }

                    //Reset the database.
                    string reset_user_url = @"http://introtoapps.com/datastore.php?action=save&appid=214328958&objectid=users&data=%5B%5D";
                    var reset_user_uri = new Uri(string.Format(reset_user_url, string.Empty));
                    var reset_user_response = await client.GetAsync(reset_user_uri); //Perform the action.

                    //Next, check whether the request is valid.
                    if (!reset_user_response.IsSuccessStatusCode)
                    {
                        //Throw an exception.
                        throw new Exception("The HTTP request to reset has failed.");
                    }

                    //Finally update the database.
                    foreach (User u in Current_Data.all_users)
                    {

                        //First is to build the url.
                        StringBuilder url_sb = new StringBuilder();
                        url_sb.Append(@"http://introtoapps.com/datastore.php?action=append&appid=214328958&objectid=users&data=");

                        //Next, add each field.
                        url_sb.AppendFormat(@"%7B%22username%22%3a%22{0}%22%2C", u.username);
                        url_sb.AppendFormat(@"%22password%22%3a%22{0}%22%2C", u.password);
                        url_sb.AppendFormat(@"%22security%5Fquestion%22%3a%22{0}%22%2C", u.security_question);
                        url_sb.AppendFormat(@"%22security%5Fanswer%22%3a%22{0}%22%7D", u.security_answer);

                        //Then, try loading the request.
                        string update_user_url = url_sb.ToString();
                        var update_user_uri = new Uri(string.Format(update_user_url, string.Empty));
                        var update_user_response = await client.GetAsync(update_user_uri); //Perform the action.

                        //Next, check whether the request is valid.
                        if (!update_user_response.IsSuccessStatusCode)
                        {
                            //Throw an exception.
                            throw new Exception("The HTTP request to update has failed.");
                        }
                    }

                    //Then, reload the user list
                    reload_user_list();

                    //Finally, display the confirmation.
                    await DisplayAlert("Password Changed", "The password has been reset.", "OK");
                    //Then transition.
                    await Navigation.PushAsync(new MainPage());

                }
            }
            catch (Exception e)
            {
                //Display any error.
                Debug.WriteLine("Registration Error: " + e.Message.ToString());
            }

        }

        //Reload the user list.
        public async void reload_user_list()
        {

            //First, try loading from the 'users' file from the online database.
            try
            {

                //Firstly, define the url with the appropriate request.
                string user_url = @"http://introtoapps.com/datastore.php?action=load&appid=214328958&objectid=users";
                var user_uri = new Uri(string.Format(user_url, string.Empty));
                var user_response = await client.GetAsync(user_uri); //Perform the action.

                //Next, check whether the request is valid.
                if (user_response.IsSuccessStatusCode)
                {
                    //If the request is valid, try to deserialize the .json string into the list of 'User' objects.
                    string content = await user_response.Content.ReadAsStringAsync();
                    Current_Data.all_users = JsonConvert.DeserializeObject<List<User>>(content);
                }
                else
                    throw new Exception("The update of the 'users' file have failed.");
            }
            catch (Exception e)
            {
                //If there are any errors (e.g. wrong format, HTTP requests failed, etc.), set the 'User' list status to 'false'
                //and display the error, which will be used to stop any logins/edits/registers until the problem is solved.
                Current_Data.user_list_status = false;
                Debug.WriteLine("'Users' file error: " + e.Message.ToString());
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