using Common.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Client.SomaxHTMLHelper
{
    public static class SomaxHTMLHelpers
    {
        public static MvcHtmlString SomaxAttachmentFileFor<TModel, TProperty>
            (this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            string propName = ExpressionHelper.GetExpressionText(expression);
            string propFullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(propName);
            string propValue = metadata.Model != null ? metadata.Model.ToString() : string.Empty;
            string propID = TagBuilder.CreateSanitizedId(propFullName);


            #region Html attributes creation
            var builder = new TagBuilder("input ");
            builder.Attributes.Add("name", propFullName); // for the data anotation validation to work.
            builder.Attributes.Add("id", propID);
            builder.Attributes.Add("type", "file");
            builder.Attributes.Add("value", propValue); // for model binding
            //builder.AddCssClass("form-control");       // optional 
            // 2020-Aug-20 - RKL - Allow jpg as attachment.
            string attachmentAllowed = System.Configuration.ConfigurationManager.AppSettings["AttachmentAllowedFileExtensions"];
            if (string.IsNullOrEmpty(attachmentAllowed))
            {
                attachmentAllowed = "xls,xlsx,doc,docx,jpg,jpeg,pdf";
            }
            string[] extlist = attachmentAllowed.ToLower().Split(',');
            string extattr;
            for (int idx = 0; idx < extlist.Length; idx++)
            {
                if (extlist[idx] == "pdf")
                {
                    extlist[idx] = "application/pdf";
                }
                else
                {
                    extlist[idx] = '.' + extlist[idx];
                }
            }
            extattr = string.Join(",", extlist);
            builder.Attributes.Add("accept", extattr);
            //builder.Attributes.Add("accept", ".xls,.xlsx,.doc,.docx,.jpg,.jpeg,application/pdf");            
            #endregion

            #region additional html attributes
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                builder.MergeAttributes(attributes);
            }
            #endregion 

            #region Enable Client Validation
            IDictionary<string, object> validationAttributes = htmlHelper.GetUnobtrusiveValidationAttributes(propFullName, metadata);
            foreach (string key in validationAttributes.Keys)
            {
                builder.Attributes.Add(key, validationAttributes[key].ToString());
            }
            #endregion

            MvcHtmlString retHtml = new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
            return retHtml;
        }

        public static IHtmlString UIConfigControl(this HtmlHelper htmlHelper, string Name, string Id, string DataType, int MaxLength, bool IsViewOnly = false, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromStringExpression(Name, htmlHelper.ViewData);
            string propValue = metadata.Model != null ? metadata.Model.ToString() : string.Empty;

            #region TagBuilder
            string buildertype;
            if (DataType == DataDictionaryColumnTypeConstant.VarCharMax.ToLower())
            {
                buildertype = "textarea ";
            }
                else
            {
                buildertype = "input ";
            }
            var builder = new TagBuilder(buildertype);
            if (DataType == DataDictionaryColumnTypeConstant.Bit.ToLower())
            {
                builder.Attributes.Add("type", "checkbox");
                builder.AddCssClass("uicheckboxdeisgn");
                if (propValue == "True")
                {
                    builder.Attributes.Add("checked", propValue);
                }
                propValue = "True";
            }
            else if (DataType == DataDictionaryColumnTypeConstant.Date.ToLower())
            {
                if (!string.IsNullOrEmpty(propValue) && metadata.Model.GetType().Name == "DateTime")
                {
                    propValue = ((DateTime)metadata.Model).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                builder.Attributes.Add("type", "text");
                builder.AddCssClass("form-control search m-input dtpicker");
            }
            else
            {
                builder.Attributes.Add("type", "text ");
                builder.AddCssClass("form-control search m-input");
                if (DataType == DataDictionaryColumnTypeConstant.Dec.ToLower())
                {
                    builder.AddCssClass("decimalinput");
                }
                if (DataType == DataDictionaryColumnTypeConstant.VarCharMax.ToLower())
                {
                    builder.AddCssClass("multilineedit");
                }
                if (MaxLength > 0)
                {
                    builder.Attributes.Add("maxlength", MaxLength.ToString());
                }
                if (MaxLength > 0 && !IsViewOnly)
                {
                    builder.AddCssClass("textcountmsg");
                }
            }
            builder.Attributes.Add("name", Name);
            builder.Attributes.Add("id", Id);
            builder.Attributes.Add("value", propValue);
            builder.Attributes.Add("autocomplete", "off");
            builder.Attributes.Add("style", "color:black;");

            if (IsViewOnly)
            {
                if (DataType == DataDictionaryColumnTypeConstant.Bit.ToLower())
                {
                    builder.Attributes.Add("onclick", "return false;");
                }
                else
                {
                    builder.Attributes.Add("readonly", "readonly");
                    builder.AddCssClass("readonly");
                }
            }
            #endregion

            #region additional html attributes
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                builder.MergeAttributes(attributes);
            }
            #endregion

            #region Enable Client Validation
            IDictionary<string, object> validationAttributes = htmlHelper.GetUnobtrusiveValidationAttributes(Name, metadata);
            foreach (string key in validationAttributes.Keys)
            {
                builder.Attributes.Add(key, validationAttributes[key].ToString());
            }
            #endregion

            return new MvcHtmlString(builder.ToString());
        }

        public static IHtmlString UIConfigControl_Mobiscroll(this HtmlHelper htmlHelper, string Name, string Id, string DataType, int MaxLength, bool IsViewOnly = false
            , object htmlAttributes = null, string Label = "")
        {
            var metadata = ModelMetadata.FromStringExpression(Name, htmlHelper.ViewData);
            string propValue = metadata.Model != null ? metadata.Model.ToString() : string.Empty;

            #region TagBuilder
            string buildertype;
            if (DataType == DataDictionaryColumnTypeConstant.VarCharMax.ToLower())
            {
                buildertype = "textarea ";
            }
            else
            {
                buildertype = "input ";
            }
            var builder = new TagBuilder(buildertype);
            if (DataType == DataDictionaryColumnTypeConstant.Bit.ToLower())
            {
                builder.Attributes.Add("type", "checkbox");
                if (propValue == "True")
                {
                    builder.Attributes.Add("checked", propValue);
                }
                propValue = "True";
                builder.Attributes.Add("mbsc-checkbox", "true");
            }
            else if (DataType == DataDictionaryColumnTypeConstant.Date.ToLower())
            {
                if (!string.IsNullOrEmpty(propValue) && metadata.Model.GetType().Name == "DateTime")
                {
                    propValue = ((DateTime)metadata.Model).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                builder.Attributes.Add("type", "text");
                builder.AddCssClass("form-control search m-input dtpicker");

                //mobiscroll begin for input text
                builder.Attributes.Add("mbsc-input", "true");
                builder.Attributes.Add("data-input-style", "box");
                builder.Attributes.Add("data-label-style", "floating");
                builder.Attributes.Add("data-label", Label);
                builder.Attributes.Add("placeholder", Label);
                //mobiscroll end
            }
            else
            {
                builder.Attributes.Add("type", "text ");
                builder.AddCssClass("form-control search m-input");
                if (DataType == DataDictionaryColumnTypeConstant.Dec.ToLower())
                {
                    builder.AddCssClass("decimalinput");
                }
                if (DataType == DataDictionaryColumnTypeConstant.VarCharMax.ToLower())
                {
                    builder.AddCssClass("multilineedit");
                }
                if (MaxLength > 0)
                {
                    builder.Attributes.Add("maxlength", MaxLength.ToString());
                }
                if (MaxLength > 0 && !IsViewOnly)
                {
                    builder.AddCssClass("textcountmsg_mobile");
                }

                //mobiscroll begin for input text
                if (DataType == DataDictionaryColumnTypeConstant.VarCharMax.ToLower())
                {
                    builder.Attributes.Add("mbsc-textarea", "true");
                }
                else
                {
                    builder.Attributes.Add("mbsc-input", "true");
                }
                builder.Attributes.Add("data-input-style", "box");
                builder.Attributes.Add("data-label-style", "floating");
                builder.Attributes.Add("data-label", Label);
                builder.Attributes.Add("placeholder", Label);
                //mobiscroll end
            }
            builder.Attributes.Add("name", Name);
            builder.Attributes.Add("id", Id);
            builder.Attributes.Add("value", propValue);
            builder.Attributes.Add("autocomplete", "off");
            builder.Attributes.Add("style", "color:black;");

            if (IsViewOnly)
            {
                if (DataType == DataDictionaryColumnTypeConstant.Bit.ToLower())
                {
                    builder.Attributes.Add("onclick", "return false;");
                }
                else
                {
                    builder.Attributes.Add("readonly", "readonly");
                    builder.AddCssClass("readonly");
                }
            }
            #endregion

            #region additional html attributes
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                builder.MergeAttributes(attributes);
            }
            #endregion

            #region Enable Client Validation
            IDictionary<string, object> validationAttributes = htmlHelper.GetUnobtrusiveValidationAttributes(Name, metadata);
            foreach (string key in validationAttributes.Keys)
            {
                builder.Attributes.Add(key, validationAttributes[key].ToString());
            }
            #endregion

            return new MvcHtmlString(builder.ToString());
        }
    }
}