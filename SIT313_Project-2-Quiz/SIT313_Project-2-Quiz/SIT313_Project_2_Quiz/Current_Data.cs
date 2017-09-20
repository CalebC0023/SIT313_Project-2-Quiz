using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT313_Project_2_Quiz
{

    /* A static class used to store temporary 'global' data.
     * The following code is referenced from this URL.
     * URL: {https://stackoverflow.com/questions/2445436/global-variables-in-c-net}
     */
    public static class Current_Data
    {

        //Temporarily store the user's details.
        public static string Username;

        //Check whether the user has an account or is a 'Guest'.
        public static bool isGuest;


    }

}
