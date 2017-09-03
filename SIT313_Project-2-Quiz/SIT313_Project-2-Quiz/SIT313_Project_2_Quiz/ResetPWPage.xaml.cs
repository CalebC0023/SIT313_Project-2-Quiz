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
    public partial class ResetPWPage : ContentPage
    {

        //Variables used for dynamic layout changes.
        StackLayout new_PW_field_1 = new StackLayout();
        StackLayout new_PW_field_2 = new StackLayout();

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

            //The entry textfield.
            Entry entry = new Entry
            {
                Placeholder = label, //Set appropriate label.
                IsPassword = true, //Set bloack characters to hide passwords.
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            entry.Behaviors.Add(new Basic_Entry_Behaviors());

            //Return this Stacklayout after applying the changes
            return new StackLayout
            {
                Spacing = 1,
                Orientation = StackOrientation.Vertical,
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
                    entry
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
            //Display the alert first.
            await DisplayAlert("Password Changed", "The password has been reset.", "OK");
            //Then transition.
            await Navigation.PushAsync(new MainPage());
        }

    }
}