using System;
using System.Collections.Generic;
using System.Linq;

using SOMAX.G4.Data.Common.Constants;
using System.Text;
using System.Text.RegularExpressions;

namespace SOMAX.G4.Presentation.Common
{
    public class PageState
    {
        public static void SaveCurrentState(Dictionary<string,string> states, string pageName)
        {
            StringBuilder appenedState = new StringBuilder();

            // The state is stored in a string.  The string is in the format of A=1;#B=2;#C=3;#
            foreach (KeyValuePair<string, string> state in states)
            {
                appenedState.Append(state.Key);
                appenedState.Append("=");
                appenedState.Append(state.Value);
                appenedState.Append(";#");
            }

            // The name of the page is used for the name of the cookie.  
            // For instance, EquipmentSearch_aspx
            string key = Cookie.GeneratePageCookieName(pageName);

            // Store the state in a cookie
            Cookie.Set(key, appenedState.ToString());
        }

        public static Dictionary<string, string> RetrievePreviousState(string pageName)
        {
            // The name of the page is used for the name of the cookie.  
            // For instance, EquipmentSearch_aspx
            string key = Cookie.GeneratePageCookieName(pageName);

            Dictionary<string, string> states = null;

            // Retrieve the state from a cookie
            string state = Cookie.Get(key);

            // The state of the page is returned as a dictionary
            if (!string.IsNullOrEmpty(state))
            {
                string[] tmpStates = state.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);

                states = tmpStates.Select(item => item.Split('=')).ToDictionary(s => s[0], s => s[1]);
            }

            return states;
        }
    }
}
