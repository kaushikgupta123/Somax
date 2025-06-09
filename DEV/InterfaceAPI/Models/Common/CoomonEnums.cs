namespace InterfaceAPI.Models.Common
{
    public enum StatusEnum
    {
        success, failed, FileNotFound, ErrorConvertingJson, ColumnMismatch
    }
    public enum FileTypeEnum
    {
        Account, VendorMaster, PartMasterImport, PartMasterRequestExport, OraclePOReceiptImport, OraclePOImport, OraclePartMasterResponseImport, OracleVendorCatalogImport, OracleVendorMasterImport, OraclePurchaseRequestExport,
        PartCategoryMasterImport,SFTPIoTReadingImport, MonnitIoTReadingImport, EPMEDIPOExport, EPMInvoiceImport
    }
    public enum FileExtensionEnum
    {
        txt, csv, pgp
    }
}