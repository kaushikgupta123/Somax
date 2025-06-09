using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;

namespace AzureUtil
{
    public class AzureBlob
    {

        #region Properties
        public string BlobEndpoint { get; set; }
        public byte[] ImageByte { get; set; }
        public long ImageLength { get; set; }
        public string ImageURI { get; set; }
        public int BlobCount { get; set; }
        public string QueueEndpoint { get; set; }
        public string TableEndpoint { get; set; }
        public string FileEndpoint { get; set; }
        public string SharedAccessSignature { get; set; }
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string DefaultEndpointsProtocol { get; set; }
        public string EndpointSuffix { get; set; }

        #endregion

        #region public methods
        public string GetFileNumberbyClientSite(Int64 ClientId, Int64 SiteId)
        {
            //--- Client-Site folder format : 0004-00025 //
            string ReturnFileNo = string.Empty;
            string ClientMask = string.Empty;
            string SiteMask = string.Empty;
            string mask4 = "0000";
            string mask3 = "000";
            string mask2 = "00";
            string mask1 = "0";

            //---Adding mask to ClientId--//
            if (ClientId <= 9)
            {
                ClientMask = mask3 + ClientId.ToString();
            }
            else if (ClientId > 9 && ClientId <= 99)
            {
                ClientMask = mask2 + ClientId.ToString();
            }
            else if (ClientId > 99 && ClientId <= 999)
            {
                ClientMask = mask1 + ClientId.ToString();
            }
            else
            {
                ClientMask = ClientId.ToString();
            }

            //---Adding mask to SiteId--//
            if (SiteId <= 9)
            {
                SiteMask = mask4 + SiteId.ToString();
            }
            else if (SiteId > 9 && SiteId <= 99)
            {
                SiteMask = mask3 + SiteId.ToString();
            }
            else if (SiteId > 99 && SiteId <= 999)
            {
                SiteMask = mask2 + SiteId.ToString();
            }
            else if (SiteId > 999 && SiteId <= 9999)
            {
                SiteMask = mask1 + SiteId.ToString();
            }
            else
            {
                SiteMask = SiteId.ToString();
            }

            return ReturnFileNo = ClientMask + "-" + SiteMask;
        }
        public string GetFileNumberbyClientSite(Int64 ClientId)
        {
            //--- Client Based folder format : 0004//
            string ReturnFileNo = string.Empty;
            string ClientMask = string.Empty;
            string mask4 = "0000";
            string mask3 = "000";
            string mask2 = "00";
            string mask1 = "0";

            //---Adding mask to ClientId--//
            if (ClientId <= 9)
            {
                ClientMask = mask3 + ClientId.ToString();
            }
            else if (ClientId > 9 && ClientId <= 99)
            {
                ClientMask = mask2 + ClientId.ToString();
            }
            else if (ClientId > 99 && ClientId <= 999)
            {
                ClientMask = mask1 + ClientId.ToString();
            }
            else
            {
                ClientMask = ClientId.ToString();
            }



            return ReturnFileNo = ClientMask;
        }

    

        #endregion


    }
}
 