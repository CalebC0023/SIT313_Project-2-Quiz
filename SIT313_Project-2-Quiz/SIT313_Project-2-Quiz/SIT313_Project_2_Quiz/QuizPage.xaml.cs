using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PCLStorage;

namespace SIT313_Project_2_Quiz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuizPage : ContentPage
    {

        //For dynamic layouts
        StackLayout bottom_btns, layout_content;

        List<Question> _questions; //Store the list of questions in that particular quiz.
        string[] saved_answers; //Stores the saved answers.
        string[] user_answers; //Store the current unsaved answers.

        //The quiz's properties
        string quiz_id;
        string quiz_name;

        //For displaying 'save' icons
        List<Button> save_btns; //All the 'save' buttons for each question.
        List<Image> question_imgs; //All the 'image' section for each question.
        int count = 0; //For getting each item from the CarouselView list as well.

        public QuizPage()
        {
            InitializeComponent();

            //Initialize any List.
            _questions = new List<Question>();

            //Set the default values
            quiz_id = Current_Data.selected_Quiz.id;
            quiz_name = Current_Data.selected_Quiz.title;
            _questions = Current_Data.selected_Quiz.questions;

            //Initialize these list with a set size (equal to the number of questions in this quiz).
            saved_answers = new string[_questions.Count];
            user_answers = new string[_questions.Count];

            ExtractOngoingQuiz(); //Try extracting the previous quiz, if any.
            BuildQuizPage(); //Build the base layout.
        }

        //Build the base layout.
        public void BuildQuizPage()
        {

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
                ItemsSource = _questions,

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
                        Text = _questions[count].text,
                        HorizontalOptions = LayoutOptions.Center,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 20
                    };

                    //Get the type of question.
                    type = _questions[count].type;

                    //Get the options for 'choice' type questions.
                    if (type == "choice")
                        options = _questions[count].options;

                    //Add the answer field for that question.
                    StackLayout _answer = QuizAnswerField(options, type, count);

                    //Add the 'Save' button for saving answers
                    Button _save = QuizButtons("Save");
                    _save.VerticalOptions = LayoutOptions.End;

                    //Save the buttons and imageboxes
                    question_imgs.Add(_image);
                    save_btns.Add(_save);

                    //Set all the questions in 'saved state' (Set the 'save' image if resuming from an unfinished quiz).
                    if (Current_Data.ongoingQuiz)
                    {
                        //Set the 'save' image.
                        question_imgs[count].Aspect = Aspect.AspectFit; //Ensure the image 'fits' without resizing itself
                        question_imgs[count].Source = ImageSource.FromFile("save.png"); //Load the image from the local sources.
                    }

                    count++; //Add the counter

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
        public StackLayout QuizAnswerField(List<string> _option, string _type, int _index)
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

                //The base Entry.
                Entry textbox = new Entry
                {
                    //Expand the answer field as far as possible
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                textbox.TextChanged += (s, e) =>
                {
                    user_answers[_index] = e.NewTextValue;
                };

                //Reenter the previously saved answer.
                if (Current_Data.ongoingQuiz)
                {
                    //Set the given answer.
                    textbox.Text = saved_answers[_index];
                };

                _field.Children.Add(textbox);
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

                //Add the placeholder text
                _choice.Items.Add("- Select an answer -");

                //Add each option to the picker
                foreach (string option in _option)
                {
                    _choice.Items.Add(option);
                };

                _choice.SelectedIndex = 0; //Set the default index.
                user_answers[_index] = ""; //Set the default value.

                _choice.SelectedIndexChanged += (s, e) =>
                {
                    if (_choice.SelectedIndex == 0)
                        user_answers[_index] = ""; //The first option is always null (placeholder).
                    else
                        user_answers[_index] = _option[_choice.SelectedIndex - 1]; //Set the answer ('-1' is to 'remove' the first placeholder option from the list)
                };

                //Reenter the previously saved answer.
                if (Current_Data.ongoingQuiz)
                {

                    int answer_index = 1; //The default value of the answer.
                    //Next, check where that option is in the Picker. If it's 'null' then the answer hasn't been picked.
                    if (saved_answers[_index] == "" || saved_answers[_index] == null)
                        _choice.SelectedIndex = 0;
                    else
                    {
                        //Check through each answer.
                        foreach (string qtn in _option)
                        {
                            if (saved_answers[_index] == qtn)
                                _choice.SelectedIndex = answer_index;

                            answer_index++;
                        }
                    }

                };

                //Finally, add the picker
                _field.Children.Add(_choice);
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
                btn.Command = new Command(() => { SaveAnswer(btn.StyleId); });
            }

            return btn; //Return this button.

        }

        //Try to extract the ongoing quiz.
        public void ExtractOngoingQuiz()
        {

            //Check whether it's an ongoing quiz.
            if (Current_Data.ongoingQuiz)
            {
                
                int current_index = 0;
                //Go through each question.
                foreach (string s in Current_Data.ongoing_Quiz.quiz_answers)
                {
                    
                    string current_line = s;
                    //If the recorded answer was 'null', set it as so.
                    if (s == "" || s == null)
                    {
                        user_answers[current_index] = "";
                        saved_answers[current_index] = "";
                    }
                    else
                    {
                        //Decode the answers.
                        byte[] encode_ans_bytes = Convert.FromBase64String(s);
                        string decode_ans = Encoding.UTF8.GetString(encode_ans_bytes, 0, encode_ans_bytes.Length);

                        //Then, record it.
                        user_answers[current_index] = decode_ans;
                        saved_answers[current_index] = decode_ans;
                    }
                    
                    current_index++;
                }
            }

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
            SaveQuiz(); //Save the current state of the Quiz.
            await this.DisplayAlert("Saved Quiz", "The quiz has been saved, you may resume whenever.", "OK");
            await Navigation.PushAsync(new ProfilePage());
        }

        //Transition to the 'ReviewPage'.
        async void ToReview()
        {
            SaveQuiz(); //Save the current state of the Quiz.
            await this.DisplayAlert("Saved Quiz", "The quiz has been saved, you may resume whenever.", "OK");
            await Navigation.PushAsync(new ReviewPage());
        }

        async void SaveQuiz()
        {

            try
            {

                //Only registered users can resume the quizzes.
                if (!Current_Data.isGuest)
                {

                    //First, save all the answers.
                    int _index = 0;
                    foreach (string s in user_answers)
                    {
                        if (s == null || s == "")
                            saved_answers[_index] = "";
                        else
                            saved_answers[_index] = s;

                        _index++;
                    }

                    //Next, build the correct .json string using the recorded data for the file.
                    string content = "";

                    //Store the current user.
                    content += "[{\"user\":\"";
                    content += Current_Data.Username;

                    //Store the current quiz id.
                    content += "\",\"quiz_id\":\"";
                    content += Current_Data.selected_Quiz.id;

                    //Store the current saved answers.
                    content += "\",\"quiz_answers\":[";
                    bool first_answer = true; //Used for correctly building the .json string.
                    foreach (string answer in saved_answers)
                    {

                        string input1 = "";
                        //Properly define the input for the file
                        if (answer == null || answer == "")
                            input1 = "";
                        else
                        {
                            //Encode the answers.
                            byte[] answer_bytes = Encoding.UTF8.GetBytes(answer);
                            string _answer = Convert.ToBase64String(answer_bytes);
                            input1 = _answer;
                        }

                        //Change the format for the last answer.
                        if (first_answer)
                        {
                            first_answer = false;
                            content += "\"";
                            content += input1;
                            content += "\"";
                        }
                        else
                        {
                            content += ",\"";
                            content += input1;
                            content += "\"";
                        }

                    }

                    //Store the results of each answer (for properly saving to the file).
                    content += "],\"quiz_results\":[";
                    bool first_result = true; //Used for correctly building the .json string.
                    for (int x = 0; x < saved_answers.Count(); x++)
                    {

                        //This field is mainly for correct formatting of the file. Therefore, it can all be false
                        string input2 = "0";

                        //Change the format for the last answer.
                        if (first_result)
                        {
                            first_result = false;
                            content += input2;
                        }
                        else
                        {
                            content += ",";
                            content += input2;
                        }

                    }

                    //Store the total score.
                    //(For properly saving to the file, therefore the proper score is not necesarry).
                    content += "],\"quiz_total_score\":0";

                    //Store the user score.
                    //(For properly saving to the file, therefore the proper score is not necesarry).
                    content += ",\"quiz_user_score\":0}]";

                    //Get the folder and file name
                    string folder_name = string.Format("{0}{1}", Current_Data.Username, "ongoing_files"); //The folder name for the specific user's saved ongoing quizzes.
                    string filename = string.Format("{0}{1}", Current_Data.Username, "_ongoingQuiz.txt"); //This will create/overwrite the specific user's local file.

                    //First, create/overwrite the folder
                    IFolder create_folder = await Current_Data.root_folder.CreateFolderAsync(folder_name, CreationCollisionOption.ReplaceExisting);

                    //Then, create (or overwrite) the .txt file to store the data.
                    IFile create_file = await create_folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

                    //Finally, try updating the file.
                    await create_file.WriteAllTextAsync(content);
                    Current_Data.ongoingQuiz = true; //After saving, the current state of the quiz is 'ongoing'.

                }
                else
                    await this.DisplayAlert("Save Quiz", "Only registered users may resume an unfinished quiz. Please register to use this function.", "OK");

            }
            catch (Exception e)
            {
                Current_Data.ongoingQuiz = false;
                Debug.WriteLine("Saving File Error:" + e.Message.ToString());
            }

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

                    //Save the answer for that question.
                    if (user_answers[x] == null || user_answers[x] == "")
                        saved_answers[x] = "";
                    else
                        saved_answers[x] = user_answers[x];


                    //Set the 'save' image.
                    question_imgs[x].Aspect = Aspect.AspectFit; //Ensure the image 'fits' without resizing itself
                    question_imgs[x].Source = ImageSource.FromFile("save.png"); //Load the image from the local sources.
                };
            };
        }

        //Override the 'back' button event.
        protected override bool OnBackButtonPressed()
        {
            ToProfile();
            return true;
        }

    }
}