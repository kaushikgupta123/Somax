using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Common
{
    public class SecurityQuestConstant
    {
        public const string SecurityQuestConstant1 = "What was the street name you lived on as a child?";
        public const string SecurityQuestConstant2 = "In what town or city was your first full time job?";
        public const string SecurityQuestConstant3 = "In what town or city did your mother and father meet?";
        public const string SecurityQuestConstant4 = "What is the name of your first pet?";
        public const string SecurityQuestConstant5 = "What is your mother's middle name?";
        public const string SecurityQuestConstant6 = "What was your first car?";
        public const string SecurityQuestConstant7 = "What elementary school did you attend?";
        public const string SecurityQuestConstant8 = "What is the name of your favorite childhood friend?";

        static internal List<DropDownModel> SecurityQuestValues()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= "What was the street name you lived on as a child?",value= ""},
                new DropDownModel{ text="In what town or city was your first full time job?",value=""},
                new DropDownModel{ text="In what town or city did your mother and father meet?",value=""},
                new DropDownModel{ text="What is the name of your first pet?",value=""},
                new DropDownModel{ text="What is your mother's middle name?",value=""},
                new DropDownModel{ text="What was your first car?",value=""},
                new DropDownModel{ text="What elementary school did you attend?",value=""},
                new DropDownModel{ text="What is the name of your favorite childhood friend?",value=""},
             };
            return ddList;
        }
    }
}