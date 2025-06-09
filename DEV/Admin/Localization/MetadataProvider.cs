using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;


namespace Admin.Localization
{
    public class MetadataProvider : AssociatedMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes,
            Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metadata = new ModelMetadata(this, containerType, modelAccessor, modelType, propertyName);
            if (propertyName != null)
            {
                var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();
                if (displayAttribute != null)
                {
                    var localisationDetails = displayAttribute.Name.Split('|');

                    if (localisationDetails != null)
                    {
                        if(localisationDetails.Count() > 1)
                        {
                            metadata.DisplayName = Common.UtilityFunction.GetMessageFromResource(localisationDetails[0], localisationDetails[1]);
                        }
                        else
                        {
                            metadata.DisplayName = localisationDetails[0];
                        }
                    }
                }
            }
            return metadata;
        }
    }
}