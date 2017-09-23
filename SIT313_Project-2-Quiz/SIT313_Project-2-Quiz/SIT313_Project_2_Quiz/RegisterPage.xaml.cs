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
    public partial class RegisterPage : ContentPage
    {

        StackLayout layout_content;
        Button register_btn;

        //Temporarily store the new 'User' details for verification.
        private string _username = null;
        private string _password = null;
        private string _confirm_password = null;
        private string _secure_question = null;
        private string _secure_answer = null;

        //Check whether each field's value is valid.
        private bool username_valid = false;
        private bool password_valid = false;
        private bool confirm_pw_valid = false;
        private bool secure_ans_valid = false;

        //Check whether the form is correctly filled out.
        private bool _form_valid = false;

        //Define the SHA256 hasher for encrytion.
        IHashAlgorithmProvider pwHash = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);

        //Call a new HTTP Client for online requests.
        private HttpClient client = new HttpClient();

        public RegisterPage()
        {
            InitializeComponent();

            //Build the base layout
            BuildRegisterPage();
        }

        public void BuildRegisterPage()
        {

            //The header label.
            Label header = new Label
            {
                Text = "Register",
                TextColor = Color.FromHex("FFFFFF"), //Set text colour.
                FontAttributes = FontAttributes.Bold, //Set text attributes.
                FontSize = 25, //Set text font.
                BackgroundColor = Color.FromHex("000020"), //Set background colour
                HorizontalOptions = LayoutOptions.CenterAndExpand //Control placement.
            };

            //Set the page's content using the base layout.
            layout_content = BaseRegisterLayout();

            //The 'Register' button, with properties that will be changed depending on the screen orientation.
            register_btn = new Button
            {
                Text = "Register",
                TextColor = Color.FromHex("FFFFFF"),
                //Set the prefered size for the button
                HeightRequest = 40,
                WidthRequest = 150,
                Command = new Command(ToMainPage), //Set the click event.
                BackgroundColor = Color.FromHex("000f3c"),
                //Set the position within the page.
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Center,
            };

            //Add the previous button to the main content.
            layout_content.Children.Add(register_btn);

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
        public StackLayout BaseRegisterLayout()
        {

            //List of all picker options
            List<string> picker_options = new List<string>
            {
                "What is the Deakin code for this Unit?",
                "Which Deakin Campus are you attending?",
                "What course are you doing?"
            };

            /* A base picker for certain fields in the form.
             * The following code is referenced from the link below.
             * URL: {https://developer.xamarin.com/api/type/Xamarin.Forms.Picker/}
             */
            Picker security_question = new Picker
            {
                HorizontalOptions = LayoutOptions.FillAndExpand //Ensures the picker fully fills any free space.
            };

            //Add the options to the picker
            foreach (string option in picker_options)
            {
                security_question.Items.Add(option);
            };

            //Set the default value of the picker.
            security_question.SelectedIndex = 0;
            _secure_question = security_question.Items[security_question.SelectedIndex]; //Store the default value.

            //Added the picker event for when a new item is selected.
            security_question.SelectedIndexChanged += (s, e) =>
            {
                _secure_question = security_question.Items[security_question.SelectedIndex]; //Store the new value.
            };

            //The base register form layout.
            StackLayout register_form = new StackLayout
            {
                Orientation = StackOrientation.Vertical,

                Children =
                {
                    RegisterTextFields("Username", false),
                    RegisterTextFields("Password", true),
                    RegisterTextFields("Confirm Password", true),

                    //Define a new StackLayout pickerfield.
                    new StackLayout
                    {

                        Padding = new Thickness(0, 20, 0, 0), //Set some extra padding for a better style.
                        Spacing = 1,
                        Orientation = StackOrientation.Vertical,

                        Children =
                        {
                            //The label for the picker.
                            new Label
                            {
                                Text = "Security Question:", //Set appropriate label.
                                WidthRequest = 140, //Set appropriate width.
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Center
                            },
                            //The picker
                            security_question

                        }

                    },

                    RegisterTextFields("Answer", false)
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

                    /* A ScrollView used to help display the entire form within the screen constraints
                     * The following code is referenced from the link below.
                     * URL: {https://developer.xamarin.com/api/type/Xamarin.Forms.ScrollView/}
                     */
                    new ScrollView
                    {
                        Orientation = ScrollOrientation.Vertical, //Controls the scoll direction.
                        IsClippedToBounds = true, //Ensures the ScrollView is 'clipped' to the borders regardless of the amount of content.
                        Content = register_form, //Set the form's layout into the ScollView.

                        //Fill any free space until the border (e.g. objects or screen).
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    }

                }

            };

            return base_layout; //Return the base layout.

        }

        //Base layout of textfields.
        public StackLayout RegisterTextFields(string label, bool isPassword)
        {

            //Define the base Entry field.
            Entry MainEntry = new Entry
            {
                Placeholder = label, //Set appropriate placeholder text.
                IsPassword = isPassword, //If true, set block characters to hide passwords.
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

            //Define the 'TextChanged' event for the entry for the 'Register' fields.
            if (label == "Username")
            {
                MainEntry.TextChanged += (s, e) =>
                {

                    //First check whether something has been entered.
                    if (e.NewTextValue == null || e.NewTextValue == "")
                    {
                        //Display the error.
                        ErrorLbl.Text = "The Username must be filled.";
                        ErrorLbl.FontSize = 12;
                        username_valid = false; //Set the value to invalid.
                    }
                    else
                    {
                        //'Remove' the error.
                        ErrorLbl.Text = "";
                        ErrorLbl.FontSize = 1;

                        //Next, check whether the value's length is within the limit.
                        if (e.NewTextValue.Length > 16)
                        {
                            //Display the error.
                            ErrorLbl.Text = "The Username cannot be over 16 characters long.";
                            ErrorLbl.FontSize = 12;
                            username_valid = false;
                        }
                        else
                        {
                            //'Remove' the error.
                            ErrorLbl.Text = "";
                            ErrorLbl.FontSize = 1;

                            /* The following Regex is referenced from the URL below.
                             * URL: {http://www.regular-expressions.info/unicode.html#prop}
                             */

                            //Check the Regex. The username can only contain 'letters' from any language and 'numbers'.
                            if (!Regex.IsMatch(e.NewTextValue, @"^[\p{L}\p{N}]+$"))
                            {
                                //Display the error.
                                ErrorLbl.Text = "The Username must only contain 'letter' and 'numbers'.";
                                ErrorLbl.FontSize = 12;
                                username_valid = false;
                            }
                            else
                            {
                                //'Remove' the error.
                                ErrorLbl.Text = "";
                                ErrorLbl.FontSize = 1;

                                //If it clears all verification, the value is valid.
                                _username = e.NewTextValue;
                                username_valid = true;
                            }
                        }
                    }

                };
            }
            else if (label == "Password")
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
            else if (label == "Answer")
            {
                MainEntry.TextChanged += (s, e) =>
                {

                    //First check whether something has been entered.
                    if (e.NewTextValue == null || e.NewTextValue == "")
                    {
                        //Display the error.
                        ErrorLbl.Text = "The Security Answer must be filled for confirmation.";
                        ErrorLbl.FontSize = 12;
                        secure_ans_valid = false; //Set the value to invalid.
                    }
                    else
                    {
                        //'Remove' the error.
                        ErrorLbl.Text = "";
                        ErrorLbl.FontSize = 1;

                        //Next, check whether the value's length is within the limit.
                        if (e.NewTextValue.Length > 25)
                        {
                            //Display the error.
                            ErrorLbl.Text = "The Security Answer cannot be over 25 characters long.";
                            ErrorLbl.FontSize = 12;
                            secure_ans_valid = false;
                        }
                        else
                        {
                            //'Remove' the error.
                            ErrorLbl.Text = "";
                            ErrorLbl.FontSize = 1;

                            //This answer field would have more freedom, therefore any character can be used.
                            //If it clears all verification, the value is valid.
                            _secure_answer = e.NewTextValue;
                            secure_ans_valid = true;
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
                register_btn.VerticalOptions = LayoutOptions.Start;
                register_btn.HorizontalOptions = LayoutOptions.End;
            }
            else
            {
                //Portrait layout

                layout_content.Orientation = StackOrientation.Vertical; //Change the StackOrientation for portrait.

                //Change the button's postion for portrait.
                register_btn.VerticalOptions = LayoutOptions.End;
                register_btn.HorizontalOptions = LayoutOptions.Center;
            }
        }

        //Popup a confirmation alert then transition to the 'MainPage'.
        async void ToMainPage()
        {
            try
            {
                //Check whether all the fields are valid.
                if (username_valid && password_valid && confirm_pw_valid && secure_ans_valid)
                    _form_valid = true;
                else
                    _form_valid = false;

                if (!_form_valid)
                    await DisplayAlert("Invalid Form", "There were invalid entries in the form. Please resolve any error.", "OK");
                else
                {

                    //Encode the entries for storage.
                    byte[] _user_bytes = Encoding.UTF8.GetBytes(_username);
                    string username = Convert.ToBase64String(_user_bytes);

                    byte[] _sec_question_bytes = Encoding.UTF8.GetBytes(_secure_question);
                    string secure_question = Convert.ToBase64String(_sec_question_bytes);

                    byte[] _sec_answer_bytes = Encoding.UTF8.GetBytes(_secure_answer);
                    string secure_answer = Convert.ToBase64String(_sec_answer_bytes);

                    //Hash the password
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in pwHash.HashData(Encoding.UTF8.GetBytes(_password)))
                        sb.Append(b.ToString("X2"));

                    string password = sb.ToString();

                    bool found = false; //Check whether a similar username is found

                    //Check whether the list is empty.
                    if (Current_Data.all_users != null)
                    {

                        //Perform the final check to see if the User is 'unique'
                        foreach (User u in Current_Data.all_users)
                        {
                            if (u.username == username)
                                found = true;    
                        }

                        //If the user is not 'unique', display error.
                        if (found)
                            await DisplayAlert("User Error", "There was already a user with that username. Please change it.", "OK");
                        else
                        {
                            //Finally, try to add it to the online database.

                            //First is to build the url.
                            StringBuilder url_sb = new StringBuilder();
                            url_sb.Append(@"http://introtoapps.com/datastore.php?action=append&appid=214328958&objectid=users&data=");

                            //Next, add each field.
                            url_sb.AppendFormat(@"%7B%22username%22%3a%22{0}%22%2C", username);
                            url_sb.AppendFormat(@"%22password%22%3a%22{0}%22%2C", password);
                            url_sb.AppendFormat(@"%22security%5Fquestion%22%3a%22{0}%22%2C", secure_question);
                            url_sb.AppendFormat(@"%22security%5Fanswer%22%3a%22{0}%22%7D", secure_answer);

                            //Then, try loading the request.
                            string append_user_url = url_sb.ToString();
                            var append_user_uri = new Uri(string.Format(append_user_url, string.Empty));
                            var append_user_response = await client.GetAsync(append_user_uri); //Perform the action.

                            //Next, check whether the request is valid.
                            if (append_user_response.IsSuccessStatusCode)
                            {
                                //If it's valid, add the user to the temporary list.
                                reload_user_list();
                                //Display the alert.
                                await DisplayAlert("Account Registered", "The account has been registered", "OK");
                                //Then transition.
                                await Navigation.PushAsync(new MainPage());
                            }
                            else
                            {
                                //Throw an exception.
                                throw new Exception("The HTTP request has failed.");
                            }
                        }
                    }
                    //If there are no current registered users yet, perform initial setup
                    else
                    {

                        //Since there are no users yet, bypass the user 'unique' check.

                        //First is to build the url.
                        StringBuilder url_sb = new StringBuilder();
                        url_sb.Append(@"http://introtoapps.com/datastore.php?action=append&appid=214328958&objectid=users&data=");

                        //Next, add each field.
                        url_sb.AppendFormat(@"%7B%22username%22%3a%22{0}%22%2C", username);
                        url_sb.AppendFormat(@"%22password%22%3a%22{0}%22%2C", password);
                        url_sb.AppendFormat(@"%22security%5Fquestion%22%3a%22{0}%22%2C", secure_question);
                        url_sb.AppendFormat(@"%22security%5Fanswer%22%3a%22{0}%22%7D", secure_answer);

                        //Then, try loading the request.
                        string append_user_url = url_sb.ToString();
                        var append_user_uri = new Uri(string.Format(append_user_url, string.Empty));
                        var append_user_response = await client.GetAsync(append_user_uri); //Perform the action.

                        //Next, check whether the request is valid.
                        if (append_user_response.IsSuccessStatusCode)
                        {
                            //If it's valid, reload the user list.
                            reload_user_list();
                            //Display the alert.
                            await DisplayAlert("Account Registered", "The account has been registered", "OK");
                            //Then transition.
                            await Navigation.PushAsync(new MainPage());
                        }
                        else
                        {
                            //Throw an exception.
                            throw new Exception("The HTTP request has failed.");
                        }

                    }

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