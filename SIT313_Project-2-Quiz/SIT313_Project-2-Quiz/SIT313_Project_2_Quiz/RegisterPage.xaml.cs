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
    public partial class RegisterPage : ContentPage
    {

        StackLayout layout_content;
        Button register_btn;

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
                        WidthRequest = 140, //Set appropriate width.
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center
                    },
                    //The entry textfield.
                    new Entry
                    {
                        Placeholder = label, //Set appropriate label.
                        IsPassword = isPassword, //Set block characters to hide ONLY passwords.
                        HorizontalOptions = LayoutOptions.FillAndExpand,
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
            //Display the alert first.
            await DisplayAlert("Account Registered", "The new account is active.", "OK");
            //Then transition.
            await Navigation.PushAsync(new MainPage());
        }

    }
}