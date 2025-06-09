using DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using AzureUtil;

namespace Client.BusinessWrapper
{
    public class AzureUtilWrapper
    {
        #region Property
        const string UploadDirectory = "~/Images/DisplayImg/";
        const string ThumbnailFileName = "NoImage.jpg";
        bool IsAzureHost = false;
        public UserData userData { get; set; }
        #endregion

        #region Methods
        public string ProcessImage(Object obj, Int64 ObjectId, string TblName)
        {
            string StrImageUrl = string.Empty;
            AzureBlob az = CheckAzureOrLocal(obj, ObjectId, TblName);
            return StrImageUrl = az.ImageURI == null ? @"../Scripts/ImageZoom/images/NoImage.jpg" : az.ImageURI;
        }

        private AzureBlob CheckAzureOrLocal(Object obj, Int64 ObjectId, string TblName)
        {
            IsAzureHost = System.Configuration.ConfigurationManager.AppSettings["ImageStorageHost"].ToString().ToLower() == "azure";

            var data = System.Web.HttpContext.Current.Session["userData"];
            userData = (UserData)data;
            MemoryStream ms = null;
            AzureBlob az = null;

            if (IsAzureHost)
            {
                az = RetrieveImageFromAzure(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, ObjectId, TblName);
            }
            if (IsAzureHost && az.BlobCount > 0)
            {
                ms = new MemoryStream(az.ImageByte, 0, az.ImageByte.Length);
                ms.Write(az.ImageByte, 0, az.ImageByte.Length);
            }
            else
            {
                DataContracts.Equipment eq = new DataContracts.Equipment();
                DataContracts.Part pt = new DataContracts.Part();

               
                switch (TblName.ToLower().Trim())
                {
                    case "equipment":
                        eq = (DataContracts.Equipment)obj;
                        eq.EquipmentId = ObjectId;
                        //if (IsAzureHost == false && eq.EquipmentImage.Length > 0)
                        //{
                        //    ms = new MemoryStream(eq.EquipmentImage, 0, eq.EquipmentImage.Length);
                        //    ms.Write(eq.EquipmentImage, 0, eq.EquipmentImage.Length);
                        //}
                        break;
                    case "workorder":
                        //wo = (DataContracts.WorkOrder)obj;
                        //wo.WorkOrderId = ObjectId;
                        //if (IsAzureHost == false && wo.WorkOrderImage.Length > 0)
                        //{
                        //    ms = new MemoryStream(wo.WorkOrderImage, 0, wo.WorkOrderImage.Length);
                        //    ms.Write(wo.WorkOrderImage, 0, wo.WorkOrderImage.Length);
                        //}
                        break;
                    case "part":
                        pt = (DataContracts.Part)obj;
                        pt.PartId = ObjectId;
                        //if (IsAzureHost == false && pt.PartImage.Length > 0)
                        //{
                        //    ms = new MemoryStream(pt.PartImage, 0, pt.PartImage.Length);
                        //    ms.Write(pt.PartImage, 0, pt.PartImage.Length);
                        //}
                        break;

                   
                }
            }
            return az;
        }

        private AzureBlob RetrieveImageFromAzure(long ClientId, long SiteId, long ObjId, string TblName)
        {
            List<byte[]> imgList = new List<byte[]>();
            AzureSetup aset = new AzureSetup();
            Int64 Clientid = ClientId;
            Int64 Siteid = SiteId;
            string imgName = "";
            string fileName = aset.CreateFileNamebyObject(TblName, ObjId.ToString(), imgName);
            AzureBlob obj = aset.RetrieveAzureBlob(Clientid, Siteid, fileName);
            return obj;
        }

        #endregion

    }
}