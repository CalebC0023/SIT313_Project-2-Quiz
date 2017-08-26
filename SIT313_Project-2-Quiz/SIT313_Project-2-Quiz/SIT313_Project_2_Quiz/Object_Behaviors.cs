using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SIT313_Project_2_Quiz
{

    //The basic behaviors for all entry fields.
    class Basic_Entry_Behaviors : Behavior<Entry>
    {

        bool isValid;

        //Check the field everytime a character is added.
        protected override void OnAttachedTo(Entry field)
        {
            field.TextChanged += TextChanged;
        }

        //Check the field everytime a character is removed.
        protected override void OnDetachingFrom(Entry field)
        {
            field.TextChanged -= TextChanged;
        }

        //Constantly check the entry value.
        void TextChanged(object sender, TextChangedEventArgs e)
        {

            if (e.NewTextValue == "" || e.NewTextValue == null)
                isValid = false;
            else
                isValid = true;

            ((Entry)sender).BackgroundColor = isValid ? Color.Default : Color.Red;

        }

    }

}
