using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Client.Models.PunchoutImport
{
    [XmlRoot(ElementName = "Credential")]
    public partial class Credential
    {
        [XmlElement(ElementName = "Identity")]
        public string Identity { get; set; }
        [XmlElement(ElementName = "SharedSecret")]
        public string SharedSecret { get; set; }
        [XmlAttribute(AttributeName = "domain")]
        public string Domain { get; set; }
    }

    [XmlRoot(ElementName = "From")]
    public class From
    {
        [XmlElement(ElementName = "Credential")]
        public Credential Credential { get; set; }
    }

    [XmlRoot(ElementName = "To")]
    public class To
    {
        [XmlElement(ElementName = "Credential")]
        public Credential Credential { get; set; }
    }

    [XmlRoot(ElementName = "Sender")]
    public class Sender
    {
        [XmlElement(ElementName = "Credential")]
        public Credential Credential { get; set; }
        [XmlElement(ElementName = "UserAgent")]
        public string UserAgent { get; set; }
    }

    [XmlRoot(ElementName = "Header")]
    public class Header
    {
        [XmlElement(ElementName = "From")]
        public From From { get; set; }
        [XmlElement(ElementName = "To")]
        public To To { get; set; }
        [XmlElement(ElementName = "Sender")]
        public Sender Sender { get; set; }
    }

    [XmlRoot(ElementName = "Money")]
    public class Money
    {
        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Total")]
    public class Total
    {
        [XmlElement(ElementName = "Money")]
        public Money Money { get; set; }
    }

    [XmlRoot(ElementName = "Description")]
    public class Description
    {
        [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string Lang { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Shipping")]
    public class Shipping
    {
        [XmlElement(ElementName = "Money")]
        public Money Money { get; set; }
        [XmlElement(ElementName = "Description")]
        public Description Description { get; set; }
    }

    [XmlRoot(ElementName = "Tax")]
    public class Tax
    {
        [XmlElement(ElementName = "Money")]
        public Money Money { get; set; }
        [XmlElement(ElementName = "Description")]
        public Description Description { get; set; }
    }

    [XmlRoot(ElementName = "PunchOutOrderMessageHeader")]
    public class PunchOutOrderMessageHeader
    {
        [XmlElement(ElementName = "Total")]
        public Total Total { get; set; }
        [XmlElement(ElementName = "Shipping")]
        public Shipping Shipping { get; set; }
        [XmlElement(ElementName = "Tax")]
        public Tax Tax { get; set; }
        [XmlAttribute(AttributeName = "operationAllowed")]
        public string OperationAllowed { get; set; }
    }

    [XmlRoot(ElementName = "ItemID")]
    public class ItemID
    {
        [XmlElement(ElementName = "SupplierPartID")]
        public string SupplierPartID { get; set; }
        [XmlElement(ElementName = "SupplierPartAuxiliaryID")]
        public string SupplierPartAuxiliaryID { get; set; }
    }

    [XmlRoot(ElementName = "UnitPrice")]
    public class UnitPrice
    {
        [XmlElement(ElementName = "Money")]
        public Money Money { get; set; }
    }

    [XmlRoot(ElementName = "Classification")]
    public class Classification
    {
        [XmlAttribute(AttributeName = "domain")]
        public string Domain { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ItemDetail")]
    public class ItemDetail
    {
        [XmlElement(ElementName = "UnitPrice")]
        public UnitPrice UnitPrice { get; set; }
        [XmlElement(ElementName = "Description")]
        public Description Description { get; set; }
        [XmlElement(ElementName = "UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }
        [XmlElement(ElementName = "Classification")]
        public Classification Classification { get; set; }
        [XmlElement(ElementName = "ManufacturerPartID")]
        public string ManufacturerPartID { get; set; }
        [XmlElement(ElementName = "ManufacturerName")]
        public string ManufacturerName { get; set; }
    }

    [XmlRoot(ElementName = "ItemIn")]
    public class ItemIn
    {
        [XmlElement(ElementName = "ItemID")]
        public ItemID ItemID { get; set; }
        [XmlElement(ElementName = "ItemDetail")]
        public ItemDetail ItemDetail { get; set; }
        [XmlElement(ElementName = "Shipping")]
        public Shipping Shipping { get; set; }
        [XmlElement(ElementName = "Tax")]
        public Tax Tax { get; set; }
        [XmlAttribute(AttributeName = "lineNumber")]
        public string LineNumber { get; set; }
        [XmlAttribute(AttributeName = "quantity")]
        public string Quantity { get; set; }
    }

    [XmlRoot(ElementName = "PunchOutOrderMessage")]
    public class PunchOutOrderMessage
    {
        [XmlElement(ElementName = "BuyerCookie")]
        public string BuyerCookie { get; set; }
        [XmlElement(ElementName = "PunchOutOrderMessageHeader")]
        public PunchOutOrderMessageHeader PunchOutOrderMessageHeader { get; set; }
        [XmlElement(ElementName = "ItemIn")]
        public List<ItemIn> ItemIn { get; set; }
    }

    [XmlRoot(ElementName = "Message")]
    public class Message
    {
        [XmlElement(ElementName = "PunchOutOrderMessage")]
        public PunchOutOrderMessage PunchOutOrderMessage { get; set; }
    }

    [XmlRoot(ElementName = "cXML")]
    public class PunchOutOrderMessageResponse 
    {
        [XmlElement(ElementName = "Header")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "Message")]
        public Message Message { get; set; }
        [XmlAttribute(AttributeName = "payloadID")]
        public string PayloadID { get; set; }
        [XmlAttribute(AttributeName = "timestamp")]
        public string Timestamp { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
    }

    [XmlRoot(ElementName = "Payload")]
    public class Payload
    {
        [XmlElement(ElementName = "cXML")]
        public PunchOutOrderMessageResponse CXML { get; set; }
    }

    [XmlRoot(ElementName = "SIReturnCartResponse")]
    public class SIReturnCartResponse
    {
        [XmlElement(ElementName = "Payload")]
        public Payload Payload { get; set; }
    }

}