using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// Added library for serialising/deserialising JSON files.
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SIT313_Project_2_Quiz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuizPage : ContentPage
    {

        List<RootQuiz> quizzes; //Holds the deserialised json file.
        List<Question> questions; //Holds the questions for the selected quiz.

        //For dynamic layouts
        StackLayout bottom_btns, layout_content;

        //For displaying 'save' icons
        List<Button> save_btns; //All the 'save' buttons for each question.
        List<Image> question_imgs; //All the 'image' section for each question.
        int count = 0; //For getting each item from the CarouselView list as well.

        public QuizPage()
        {
            InitializeComponent();

            BuildJSONFile(); //Load the Quiz Object from the JSON file.
            BuildQuizPage(); //Build the base layout.
        }

        /* Get the Objects in the JSON file.
         * The following code is referenced from the links below.
         * URL: {https://www.youtube.com/watch?v=RCcWQCxVrus}
         * URL: {https://www.youtube.com/watch?v=VF2UC86jjs0}
         */
        public void BuildJSONFile()
        {

            /* The following lines of code are of the JSON file containing some sample question types.
             * The code has been minorly tweaked due to some issues with Xamarin which had cause it to crash.
             * Changes:
             * - The 'answer' property had two different variable types (e.g. List<string>, string).
             *   Therefore, it has been changed to only one type (List<string>).
             */
            string json =
                @"[{

                    ""id"": ""quiz01"",
                    ""title"": ""All Types"",
                    ""questions"": [
                    {
                        ""id"": 1,
                        ""text"": ""Date"",
                        ""type"": ""date"",
                        ""help"": ""The date you started this quiz.""
                    },
                    {
                        ""id"": 2,
                        ""text"": ""Name"",
                        ""type"": ""textbox"",
                        ""help"": ""Your full name""
                    },
                    {
                        ""id"": 3,
                        ""text"": ""Diary"",
                        ""type"": ""textarea"",
                        ""help"": ""Write 4 paragraphs""
                    },
                    {
                        ""id"": 4,
                        ""text"": ""Gender"",
                        ""type"": ""choice"",
                        ""options"": [ ""Male"", ""Female"", ""Depends what day it is"" ]
                    }
                    ]

                },
                {

                    ""id"": ""quiz02"",
                    ""title"": ""Exam Grade"",
                    ""questionsPerPage"": [ 2, 4 ],
                    ""score"": 20,
                    ""questions"": [
                    {
                        ""id"": 1,
                        ""text"": ""Sid"",
                        ""type"": ""textbox"",
                        ""validate"": "" /[0 - 9] +/ ""
                    },
                    {
                        ""id"": 2,
                        ""text"": ""Name"",
                        ""type"": ""textbox"",
                        ""help"": ""Your full name""
                    },
                    {
                        ""id"": 3,
                        ""text"": ""What is the capital of Australia?"",
                        ""type"": ""textbox"",
                        ""answer"": [ ""Canberra"" ],
                        ""weighting"": 5
                    },
                    {
                        ""id"": 4,
                        ""text"": ""What is the largest state in Australia?"",
                        ""type"": ""textbox"",
                        ""answer"": [ ""Western Australia"", ""WA"" ],
                        ""weighting"": 5
                    },
                    {
                        ""id"": 5,
                        ""text"": ""What is the capital of Victoria?"",
                        ""type"": ""choice"",
                        ""options"": [ ""Sydney"", ""Brisbane"", ""Melbourne"" ],
                        ""answer"": [ ""Melbourne"" ],
                        ""weighting"": 5
                    }
                    ]

                }]";

            quizzes = JsonConvert.DeserializeObject<List<RootQuiz>>(json);

        }

        //Build the base layout.
        public void BuildQuizPage()
        {

            //The quiz's properties
            string quiz_id = quizzes[0].id;
            string quiz_name = quizzes[0].title;
            questions = quizzes[0].questions;

            //The header label.
            Label header = new Label
            {
                Text = "ID: " + quiz_id + "\nName: " + quiz_name,
                TextColor = Color.FromHex("FFFFFF"), //Set text colour.
                FontAttributes = FontAttributes.Bold, //Set text attributes.
                FontSize = 22, //Set text font.
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.FromHex("000020"), //Set background colour
                HorizontalOptions = LayoutOptions.CenterAndExpand //Control placement.
            };

            //Define new list of buttons and imageboxes
            save_btns = new List<Button>();
            question_imgs = new List<Image>();

            //Build the CarouselView
            CarouselView body_content = BuildCarousel();

            //The buttons at the bottom of the screen.
            bottom_btns = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                //Places the buttons at the bottom of the page (or side if landscape).
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,

                Children =
                {
                    QuizButtons("Pause"),
                    QuizButtons("Submit")
                }
            };

            //Combine all components
            layout_content = new StackLayout
            {

                Spacing = 3, //Assign appropriate spacing.
                Orientation = StackOrientation.Vertical, //Set orientation to vertical (Display from top to bottom).
                BackgroundColor = Color.FromHex("e6fcff"), //Set background colour.
                Padding = new Thickness(7, 13, 7, 0), //Set the padding for a better style.
                VerticalOptions = LayoutOptions.FillAndExpand,

                Children =
                {
                    body_content,
                    bottom_btns
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

        //Base layout for the CarouselView.
        public CarouselView BuildCarousel()
        {

            CarouselView body_content = new CarouselView
            {
                ItemsSource = questions,

                //Go through each 'Question' object in the ItemsSource.
                ItemTemplate = new DataTemplate(() =>
                {

                    string type = ""; //Holds what type of question it is.
                    List<string> options = new List<string>();

                    //Set new imagebox
                    Image _image = new Image
                    {
                        //Set the proper image size
                        HeightRequest = 15,
                        WidthRequest = 15,
                        //Positions the image in the top-right corner
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.End
                    };

                    //Set the question
                    Label _question = new Label()
                    {
                        Text = questions[count].text,
                        HorizontalOptions = LayoutOptions.Center,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 20
                    };

                    //Checks through each question for their type.
                    foreach (Question q in questions)
                    {
                        if (q.text.Equals(_question.Text))
                        {
                            type = q.type;
                            if (type.Equals("choice"))
                            {
                                options = q.options;
                            }
                        }
                    };

                    //Add the answer field for that question.
                    StackLayout _answer = QuizAnswerField(options, type);

                    //Add the 'Save' button for saving answers
                    Button _save = QuizButtons("Save");
                    _save.VerticalOptions = LayoutOptions.End;

                    count++; //Add the counter
                    //Save the buttons and imageboxes
                    question_imgs.Add(_image);
                    save_btns.Add(_save);

                    //Combine the other objects and set this layout as a 'page' in the CarouselView.
                    return new StackLayout
                    {
                        Orientation = StackOrientation.Vertical, //Set the orientation to vertical (up/down).
                        Padding = new Thickness(0, 20, 0, 0), //Set the padding at the top of the border.

                        Children =
                        {
                            _image,
                            _question,
                            _answer,
                            _save
                        }
                    };
                })

            };

            return body_content;
        }

        //Base layout of answer fields.
        public StackLayout QuizAnswerField(List<string> _option, string _type)
        {

            //Empty StackLaout used for holding the correct field type.
            StackLayout _field = new StackLayout
            {
                //Expand the answer field as far as possible
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            //Set an Entry for 'text' answers.
            if (_type.Equals("textbox"))
            {
                _field.Children.Add(
                        new Entry
                        {
                            //Expand the answer field as far as possible
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        }
                    );
            }
            //Set and Editor for 'textarea' answers.
            //URL: {https://developer.xamarin.com/api/type/Xamarin.Forms.Editor/}
            else if (_type.Equals("textarea"))
            {
                _field.Children.Add(
                        new Editor
                        {
                            HeightRequest = 110, //Set an appropriate height.
                            //Expand the answer field as far as possible
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        }
                    );
            }
            //Set a Picker for single 'choice' answers.
            else if (_type.Equals("choice"))
            {
                //Initially define the picker.
                Picker _choice = new Picker
                {
                    //Expand the answer field as far as possible
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                //Add each option to the picker
                foreach (string option in _option)
                {
                    _choice.Items.Add(option);
                };

                //Finally, add the picker
                _field.Children.Add(_choice);
            }
            //Set a DatePicker for 'date' answers.
            //URL: {https://developer.xamarin.com/api/type/Xamarin.Forms.DatePicker/}
            else if (_type.Equals("date"))
            {
                _field.Children.Add(
                        new DatePicker
                        {
                            Format = "D", //The format which the date is displayed.
                            //Expand the answer field as far as possible
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        }
                    );
            }

            //Return this Stacklayout after applying the changes
            return new StackLayout
            {
                Spacing = 1,
                Orientation = StackOrientation.Horizontal,
                //Expand the answer field as far as possible
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,

                Children =
                {
                    //The label for the textfield.
                    new Label
                    {
                        Text = "Answer: ", //Set appropriate label.
                        WidthRequest = 50, //Set appropriate width.
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center
                    },
                    //The answer field depending on the type.
                    _field
                }
            };

        }

        //Base layout of buttons
        public Button QuizButtons(string label)
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

            // Verify which button click event is applied.
            if (label.Equals("Pause"))
            {
                btn.Command = new Command(ToProfile);
            }
            else if (label.Equals("Submit"))
            {
                btn.Command = new Command(ToReview);
            }
            else if (label.Equals("Save"))
            {
                btn.StyleId = count.ToString(); //Set correct ID for the button.
                //The following code is referenced from the link below.
                //URL: {https://forums.xamarin.com/discussion/64248/why-im-getting-the-error-cannot-convert-from-method-group-to-action}
                btn.Command = new Command(() => { SaveAnswer(btn.StyleId); });
            }

            return btn; //Return this button.

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
                bottom_btns.Orientation = StackOrientation.Vertical;
                bottom_btns.VerticalOptions = LayoutOptions.Start;
                bottom_btns.HorizontalOptions = LayoutOptions.End;
            }
            else
            {
                //Portrait layout

                layout_content.Orientation = StackOrientation.Vertical; //Change the StackOrientation for portrait.

                //Change the button's postion for portrait.
                bottom_btns.Orientation = StackOrientation.Horizontal;
                bottom_btns.VerticalOptions = LayoutOptions.End;
                bottom_btns.HorizontalOptions = LayoutOptions.Center;
            }
        }

        //Transition to the 'ProfilePage'.
        async void ToProfile()
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        //Transition to the 'ReviewPage'.
        async void ToReview()
        {
            await Navigation.PushAsync(new ReviewPage());
        }

        //For Project 1, just display a notification and 'save' icon.
        async void SaveAnswer(string id)
        {
            await DisplayAlert("Saved", "The answer has been saved.", "OK");

            //Look through each button id for the one which was clicked.
            for (int x = 0; x < save_btns.Count; x++)
            {
                //One the button is found, display 'save' icon for that question.
                if (save_btns[x].StyleId.Equals(id))
                {
                    //The following code is referenced from the link below.
                    //URL: {https://developer.xamarin.com/guides/xamarin-forms/user-interface/images/#Local_Images}
                    question_imgs[x].Aspect = Aspect.AspectFit; //Ensure the image 'fits' without resizing itself
                    question_imgs[x].Source = ImageSource.FromFile("save.png"); //Load the image from the local sources.
                };
            };
        }

    }
}