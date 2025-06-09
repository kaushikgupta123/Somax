using System;
using System.Web.Mvc;

namespace Client.Models.CustomModelBinder
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            // Check if the DateTime property being parsed is not null or "" (for JSONO
            if (value != null && value.AttemptedValue != null && value.AttemptedValue != "")
            {
                // Parse the datetime then convert it back to universal time.
                var dt = DateTime.ParseExact(value.AttemptedValue, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                return dt;
            }
            else
            {
                return null;
            }
        }
    }
}