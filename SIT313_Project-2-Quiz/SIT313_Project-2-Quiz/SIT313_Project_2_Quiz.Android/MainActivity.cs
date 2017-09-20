using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace SIT313_Project_2_Quiz.Droid
{

    /* Change the default icon and theme to a custom icon and splashscreen.
     * The following code is referenced from the following URL.
     * URL: {https://xamarinhelp.com/creating-splash-screen-xamarin-forms/}
     */
    [Activity(Label = "SIT313_Project_2_Quiz", Icon = "@drawable/SIT313_Quiz_Icon", Theme = "@style/splashscreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //Set the default theme after the splashscreen.
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }

}

