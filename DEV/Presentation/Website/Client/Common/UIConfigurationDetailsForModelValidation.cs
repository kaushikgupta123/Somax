namespace Client.Common
{
    public class UIConfigurationDetailsForModelValidation
    {
        public string TableName { get; set; }
        public string ColumnLabel { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public bool SystemRequired { get; set; }
        public string SystemRequiredErrorMessage { get; set; }
        public bool Required { get; set; }
        public string RequiredErrorMessage { get; set; }
        public int MaxLength { get; set; }
        public string MaxLengthErrorMessage { get; set; }
        public int MinLength { get; set; }
        public string MinLengthErrorMessage { get; set; }
        public string RegularExpression { get; set; }
        public string RegularExpressionErrorMessage { get; set; }
        public bool Range { get; set; }
        public decimal RangeMinValue { get; set; }
        public decimal RangeMaxValue { get; set; }
        public string RangeErrorMessage { get; set; }
        public bool Section { get; set; }
        public string SectionName { get; set; }
        public string LookupType { get; set; }
        public string LookupName { get; set; }
        public bool Display { get; set; }
        public bool ViewOnly { get; set; }
        public bool UDF { get; set; }
    }
}