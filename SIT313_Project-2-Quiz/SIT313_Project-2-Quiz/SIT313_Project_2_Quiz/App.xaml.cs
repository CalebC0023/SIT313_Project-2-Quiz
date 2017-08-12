using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SIT313_Project_2_Quiz
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            /* Root page of the app
             * Calls a navigation stack for transitions. This was provided in the tutorial below.
             * URL: {https://www.youtube.com/watch?v=OT2pwGQgAqQ}
             */
            MainPage = new NavigationPage(new SIT313_Project_2_Quiz.MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
