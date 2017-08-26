using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SIT313_Project_2_Quiz
{
    public partial class MainPage : ContentPage
    {

        //Public class variables which allows dynamic changes to its properties
        private StackLayout layout_content;
        private StackLayout layout_btn_group;

        public MainPage()
        {
            InitializeComponent();

            //Build initial layout for the 'login' page.
            BuildLoginPage();
        }

        public void BuildLoginPage()
        {

            /*
             *  The follwing codes are referenced from these URLs.
             *  Base URL: {https://developer.xamarin.com/guides/xamarin-forms/user-interface/controls/layouts/}
             *  Specific Layout URL: {https://developer.xamarin.com/api/type/Xamarin.Forms.StackLayout/}
             *  
             *  It shows some examples of how to dynamically build a layout from the .cs file.
             */

            //The header label.
            Label header = new Label
            {
                //Set the text and its colour.
                Text = "SIT313 Quiz!",
                TextColor = Color.FromHex("FFFFFF"), //Color codes are taken from this URL: {http://htmlcolorcodes.com/}

                //Set the text's font size and current attribute (e.g. Bold, Italic, etc).
                FontAttributes = FontAttributes.Bold,
                FontSize = 50,

                //Set and appropriate background colour.
                BackgroundColor = Color.FromHex("000020"),

                //Set where this object will 'fit' in the page.
                HorizontalOptions = LayoutOptions.CenterAndExpand //Controls horizontal placement. Vertical placement: 'VerticalOptions'.
            };

            //Grouped objects to make layout orientation chnages easier.
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

                Spacing = 3, //Assign appropriate spacing.
                Orientation = StackOrientation.Vertical, //Set orientation to vertical (Display from top to bottom).
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("e6fcff"), //Set background colour.
                Padding = new Thickness(7, 13, 7, 0), //Set the padding for a better style.

                //The UI elements within this layout.
                Children =
                {

                    //Grouped objects to make layout orientation chnages easier.
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
            //URL: {https://github.com/xamarin/xamarin-forms-samples/blob/master/FormsGallery/FormsGallery/FormsGallery/StackLayoutDemoPage.cs}
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

        //Base layout of textfields
        public StackLayout LoginTextFields(string label, bool isPassWord)
        {

            Entry entry = new Entry
            {
                Placeholder = label, //Set appropriate label.
                IsPassword = isPassWord, //If true, set block characters to hide passwords.
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            entry.Behaviors.Add(new Basic_Entry_Behaviors());

            //Return this Stacklayout after applying the changes
            return new StackLayout
            {
                Spacing = 1,
                Orientation = StackOrientation.Horizontal, //Set orientation to horizontal (Display from left to right).
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
                    entry
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

            /* Verify which button click event is applied.
             * Use of the 'Command' object is found in the link below.
             * URL: {https://blog.xamarin.com/simplifying-events-with-commanding/}
             */
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
                btn.Command = new Command(ToProfile);
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

            /* The following codes on command and gestures derived from the
             * following links.
             * URL: {https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/gestures/tap/}
             * URL: {https://stackoverflow.com/questions/35811060/how-to-create-click-event-on-label-in-xamarin-forms-dynamically}
             */

            //Add the 'onclick' event
            var lbl_tap = new TapGestureRecognizer();
            lbl_tap.Command = new Command(ToGetPassword);

            //Add the 'tap' gesture with its event.
            get_pw_lbl.GestureRecognizers.Add(lbl_tap);

            return get_pw_lbl; //Return the custom label.
        }

        /* Gets the current orientation.
         * With the help from the tutorial below, the following code allows
         * the layout to dynamically change depending on the orientation.
         * URL: {https://www.youtube.com/watch?v=pYVjQco0e-Y}
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

        /* Transitions to the 'GetPWPage'.
         * The following functions on transitions are referenced from the link below.
         * URL: {https://www.youtube.com/watch?v=OT2pwGQgAqQ}
         */
        async void ToProfile()
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        //Transitions to the 'RegisterPage'.
        async void ToRegister()
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        //Transitions to the 'GetPWPage'.
        async void ToGetPassword()
        {
            await Navigation.PushAsync(new GetPWPage());
        }

    }

}
