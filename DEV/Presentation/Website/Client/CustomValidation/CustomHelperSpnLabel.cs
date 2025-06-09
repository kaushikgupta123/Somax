using Client.Common;

using Common.Constants;

using DataContracts;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Client.CustomValidation
{
    public static class CustomHelper
    {
        public static MvcHtmlString LabelFor(List<Localizations> Loc, string spnname)
        {
            string Returnstr = Loc.Where(x => x.ResourceId == spnname).Select(x => x.Value).FirstOrDefault();
            return new MvcHtmlString(Returnstr == null ? "|" : Returnstr);
        }
        public static MvcHtmlString EncodedActionLink(this HtmlHelper htmlHelper, string actionName, string controllerName, string Area, object routeValues = null)
        {
            string queryString = string.Empty;
            if (routeValues != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(routeValues);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    if (i > 0)
                    {
                        queryString += "?";
                    }
                    queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }

            StringBuilder link = new StringBuilder();

            if (Area != string.Empty)
            {
                link.Append("/" + Area);
            }

            if (controllerName != string.Empty)
            {
                link.Append("/" + controllerName);
            }

            if (actionName != string.Empty)
            {
                link.Append("/" + actionName);
            }
            if (queryString != string.Empty)
            {
                link.Append("?q=" + HttpContext.Current.Server.UrlEncode(queryString.Encrypt()));
                //ancor.Append("?q=" + HttpContext.Current.Server.UrlEncode(new HRMS_UtilityLayer.EncryptionUL().Encrypt(queryString)));
                //ancor.Append("?q=" + Encrypt(queryString));
            }
            return new MvcHtmlString(link.ToString());
        }

        public static IHtmlString UIConfigTextBox(this HtmlHelper htmlHelper, string DataType, int MaxLength, string Id)
        {
            var tb = new TagBuilder("input");
            if (DataType != DataDictionaryColumnTypeConstant.Bit)
            {
                tb.Attributes.Add("type", "text");
                tb.Attributes.Add("class", "form-control search m-input");
            }
            else
            {
                tb.Attributes.Add("type", "checkbox");
            }
            //outerDiv.AddCssClass("inputText");


            tb.Attributes.Add("id", Id);

            return new MvcHtmlString(tb.ToString());
        }
    }
}