/*
 * This app is derived from the quiz app completed in SIT313: Assignment 1.
 * The following URL references are from the Assignment 1.
 *   
 * - Create transition stack.
 *   URL: {https://www.youtube.com/watch?v=OT2pwGQgAqQ}
 *   
 * - Create different layouts.
 *   URL: {https://developer.xamarin.com/guides/xamarin-forms/user-interface/controls/layouts/}
 *   URL: {https://github.com/xamarin/xamarin-forms-samples/blob/master/FormsGallery/FormsGallery/FormsGallery/StackLayoutDemoPage.cs}
 *   
 * - Create click events for buttons.
 *   URL: {https://blog.xamarin.com/simplifying-events-with-commanding/}
 *   
 * - Create click events on labels.
 *   URL: {https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/gestures/tap/}
 *   URL: {https://stackoverflow.com/questions/35811060/how-to-create-click-event-on-label-in-xamarin-forms-dynamically}
 *   
 * - Hex color codes.
 *   URL: {http://htmlcolorcodes.com/}
 * 
 * - Dynamically change layout depending on orientation.
 *   URL: {https://www.youtube.com/watch?v=pYVjQco0e-Y}
 *   
 * - Transition to other pages.
 *   URL: {https://www.youtube.com/watch?v=OT2pwGQgAqQ}
 *   
 * - Handle serializing/deserializing of JSON files.
 *   URL: {https://www.youtube.com/watch?v=RCcWQCxVrus}
 *   URL: {https://www.youtube.com/watch?v=VF2UC86jjs0}
 *   URL: {https://stackoverflow.com/questions/38743280/deserialize-json-object-xamarin-android-c-sharp}
 *   
 * - Create ListView
 *   URL: {https://developer.xamarin.com/api/type/Xamarin.Forms.ListView/}
 *   URL: {https://www.youtube.com/watch?v=cMRg0P2f9N4}
 *
 * 
 * The references for Assignment 2 will be included within the code where they are first used.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using PCLCrypto;

namespace SIT313_Project_2_Quiz
{
    public partial class MainPage : ContentPage
    {

        //Private class variables which allows dynamic changes to its properties
        private StackLayout layout_content;
        private StackLayout layout_btn_group;

        //Temporarily store the username and password for verification.
        private string _username = null;
        private string _password = null;

        //Define the SHA256 hasher for encrytion.
        IHashAlgorithmProvider pwHash = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);

        public MainPage()
        {
            InitializeComponent();

            //Build initial layout for the 'login' page.
            BuildLoginPage();
        }


        public void BuildLoginPage()
        {

            //The header label.
            Label header = new Label
            {
                //Set the text and its colour.
                Text = "SIT313 Quiz!",
                TextColor = Color.FromHex("FFFFFF"),

                //Set the text's font size and current attribute (e.g. Bold, Italic, etc).
                FontAttributes = FontAttributes.Bold,
                FontSize = 50,

                //Set and appropriate background colour.
                BackgroundColor = Color.FromHex("000020"),

                //Set where this object will 'fit' in the page.
                //Horizontal placement: 'HorizontalOptions'. Vertical placement: 'VerticalOptions'.
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            //Grouped objects to make layout orientation changes easier.
            //Also defined to allow dynamic change of properties.
            layout_btn_group = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                        {
                            //Display the appropriate buttons.
                            LoginButtons("Login"), //The 'Login' Button.
                            LoginButtons("Register"), //The 'Register' Button.
                            LoginButtons("Guest"), //The 'Guest' Button (For guests; no persistent data).

                            //Display clickable label for recovering passwords.
                            LoginClickLabel() //The clickable 'Forgot Password?" Label.
                        }
            };

            //The StackLayout containing the body contents for the page.
            layout_content = new StackLayout
            {

                Spacing = 3,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("e6fcff"),
                Padding = new Thickness(7, 13, 7, 0),

                //The UI elements within this layout.
                Children =
                {

                    //Grouped objects to make layout orientation changes easier.
                    new StackLayout {
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Children =
                        {
                            //Display the appropriate labels and textfields for specific input.
                            LoginTextFields("Username", false), //The following is for the 'Username' field.
                            LoginTextFields("Password", true) //The following is for the 'Password' field.
                        }
                    },

                    //Second grouped objects.
                    layout_btn_group

                }

            };

            //Combine all components for the final layout.
            this.Content = new StackLayout
            {
                BackgroundColor = Color.FromHex("000020"),
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    header,
                    layout_content
                }
            };

        }

        //Base layout of textfields
        public StackLayout LoginTextFields(string label, bool isPassWord)
        {

            //Define the base Entry field.
            Entry MainEntry = new Entry
            {
                Placeholder = label, //Set appropriate placeholder text.
                IsPassword = isPassWord, //If true, set block characters to hide passwords.
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            /* The following reference below is to demonstrate how to handle TextChanged events (or similar events).
             * URL: {https://stackoverflow.com/questions/43595801/handle-event-when-label-text-change}
             */

            //Define the 'TextChanged' event for the entry for the Username and Password.
            if (label == "Username")
            {
                MainEntry.TextChanged += (s, e) =>
                {

                    _username = e.NewTextValue;

                };
            }
            else if (label == "Password")
            {
                MainEntry.TextChanged += (s, e) =>
                {

                    _password = e.NewTextValue;

                };
            }

            //Return this Stacklayout after applying the changes
            return new StackLayout
            {
                Spacing = 1,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    //The label for the textfield.
                    new Label
                    {
                        Text = label + ":", //Set appropriate label.
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center
                    },
                    //The entry textfield.
                    MainEntry
                }
            };

        }

        //Base layout of buttons
        public Button LoginButtons(string label)
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

            if (label.Equals("Login"))
            {
                btn.Command = new Command(ToProfile);
            }
            else if (label.Equals("Register"))
            {
                btn.Command = new Command(ToRegister);
            }
            else if (label.Equals("Guest"))
            {
                btn.Command = new Command(ToGuestProfile);
            }

            return btn; //Return this button.

        }

        //A custom clickable label.
        public Label LoginClickLabel()
        {
            //Create the base "Forgot Password?" label.
            Label get_pw_lbl = new Label
            {
                Text = "Forgot password?",
                FontSize = 11,
                HorizontalOptions = LayoutOptions.Center
            };

            //Add the 'onclick' event
            var lbl_tap = new TapGestureRecognizer();
            lbl_tap.Command = new Command(ToGetPassword);

            //Add the 'tap' gesture with its event.
            get_pw_lbl.GestureRecognizers.Add(lbl_tap);

            return get_pw_lbl; //Return the custom label.
        }

        /* End of building page layout */


        /* Dynamic layout changes depending on orientation.
         */

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //Controls the layout for each orientation
            if (width > height)
            {
                //Landscape layout
                layout_content.Orientation = StackOrientation.Horizontal; //Change the StackOrientation for landscape.
                layout_btn_group.HorizontalOptions = LayoutOptions.End; //Change the positioning for landscape.
            }
            else
            {
                //Portrait layout
                layout_content.Orientation = StackOrientation.Vertical; //Change the StackOrientation for portrait.
                layout_btn_group.HorizontalOptions = LayoutOptions.Center; //Change the positioning for portrait.
            }
        }


        //Transitions to the 'ProfilePage'.
        async void ToProfile()
        {
            //Only transition if the 'user' list is valid.
            if (Current_Data.user_list_status)
            {
                //Verify that the user has correctly entered their details. If not, display an alert.
                //This checks the original input for each entry. Must not be 'null'.
                if (_username == null || _username == "" || _password == null || _password == "")
                    await this.DisplayAlert("Login Error", "Please ensure that both fields have been filled.", "OK");
                else
                {

                    //Next, check whether the details match with any existing user.

                    //Convert from bytes to string (the current format in the database).
                    byte[] _user_bytes = Encoding.UTF8.GetBytes(_username);
                    string username = Convert.ToBase64String(_user_bytes);

                    /* Hash the UTF8 encoded password.
                     * The following code is referenced from the URL below.
                     * URL: {https://stackoverflow.com/questions/40557467/xamarin-pclcrypto-sha256-give-different-hash}
                     */

                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in pwHash.HashData(Encoding.UTF8.GetBytes(_password)))
                        sb.Append(b.ToString("X2"));

                    string password = sb.ToString();

                    bool found = false; //Check whether the user is correctly found.

                    //Go through each recorded user.
                    if (Current_Data.all_users != null)
                    {
                        foreach (User u in Current_Data.all_users)
                        {
                            if (u.username == username && u.password == password)
                                found = true;
                        }
                    }

                    //If the details match, transition.
                    if (found)
                    {

                        Current_Data.Username = username; //Store the current 'username'.
                        Current_Data.isGuest = false; //Set current user as 'User' (false in this case).
                        await Navigation.PushAsync(new ProfilePage());
                    }
                    //Else, display the appropriate error message.
                    else
                        await this.DisplayAlert("Login Error", "The details entered was invalid. Please try again.", "OK");

                }

            }
            else
                await this.DisplayAlert("User List Error", "There was an error with retrieving the 'User' list. Please try restarting the app.", "OK");
        }

        //Transitions to the 'ProfilePage' as a 'Guest'.
        async void ToGuestProfile()
        {
            Current_Data.isGuest = true; //Set current user as 'Guest' (true in this case).
            await Navigation.PushAsync(new ProfilePage());
        }

        //Transitions to the 'RegisterPage'.
        async void ToRegister()
        {
            //Only transition if the 'user' list is valid.
            if (Current_Data.user_list_status)
                await Navigation.PushAsync(new RegisterPage());
            else
                await this.DisplayAlert("User List Error", "There was an error with retrieving the 'User' list. Please try restarting the app.", "OK");
        }

        //Transitions to the 'GetPWPage'.
        async void ToGetPassword()
        {
            //Only transition if the 'user' list is valid.
            if (Current_Data.user_list_status)
                await Navigation.PushAsync(new GetPWPage());
            else
                await this.DisplayAlert("User List Error", "There was an error with retrieving the 'User' list. Please try restarting the app.", "OK");
        }

        /* Stop the 'back' event when on specific pages.
         * The following method is referenced from the URL below.
         * URL: {https://stackoverflow.com/questions/30657344/how-to-handle-cancel-back-navigation-in-xamarin-forms}
         */
        protected override bool OnBackButtonPressed()
        {
            //Returning true prevents the page from transitioning back to the last page.
            //(In this case, in the login screen, it'll prevent the app from closing/sleeping. There should already be specific button for it)
            return true;
        }


    }

}
