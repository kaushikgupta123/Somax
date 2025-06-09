using System;
using System.Collections.Generic;
using System.Linq;
using Common.Constants;
using Newtonsoft.Json;
using System.Web.Mvc;
using Client.Common;
using Client.Models.Work_Order;
using Client.BusinessWrapper.PurchaseApproval;
using Client.Models.PurchaseApproval;
using Client.Controllers.Common;
using Client.ActionFilters;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using DataContracts;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Client.Controllers.PurchaseApproval
{
    public class PurchaseApprovalController : SomaxBaseController
    {
        //[CheckUserSecurity(securityType = SecurityConstants.Purchasing_Approve)]
        [CheckUserSecurity(securityType = SecurityConstants.PurchaseRequest_ApprovalPage)]
        public ActionResult Index()
        {
            PurchaseApprovalVM objPAM = new PurchaseApprovalVM();
            PurchaseApprovalModel PAModel = new PurchaseApprovalModel();
            //***V2-820
            objPAM.IncludePRReview = this.userData.Site.IncludePRReview;
            objPAM.ShoppingCartIncludeBuyer = this.userData.Site.ShoppingCartIncludeBuyer;
            //***
            var StatusList = UtilityFunction.WorkBenchStatusList();
            if (StatusList != null)
            {
                PAModel.scheduleStatusList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var CreateDateList = UtilityFunction.WorkBenchCreateDatesList();
            if (CreateDateList != null)
            {
                PAModel.scheduleCreateDateList = CreateDateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            // V2-851
            //var VendorsLookUplist = GetLookupList_Vendor();
            //if (VendorsLookUplist != null)
            //{
            //    PAModel.VendorsList = VendorsLookUplist.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.Vendor.ToString() });
            //}
            objPAM.purchaseApprovalModel = PAModel;
            LocalizeControls(objPAM, LocalizeResourceSetConstants.PurchaseRequest);
            return View(objPAM);
        }
        [HttpPost]
        public string GetPurchaseApprovalData(int? draw, int? start, int? length, int StatusTypeId = 0, int CreatedDatesId = 0, string PRClientLookupId = "", string Reason = "", string CreatedBy = "", DateTime? CreatedDate = null, string VendorClientLookupId = "", string VendorName = "", decimal? TotalCost = null)
        {
            ActualWBdropDownsModel actualWBdropDownsModel = new ActualWBdropDownsModel();
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PurchaseApprovalWrapper PAWrapper = new PurchaseApprovalWrapper(userData);
            //var PAMasterList = PAWrapper.RetrievePurchaseApprovalData(StatusTypeId, CreatedDatesId);
            var PAMasterList = PAWrapper.RetrievePurchaseApprovalData_V2(StatusTypeId, CreatedDatesId);/*V2-820*/
            
            if (PAMasterList != null)
            {
                PAMasterList = this.GetAllPurchaseApprovalByColumnWithOrder(order, orderDir, PAMasterList);
                if (!string.IsNullOrEmpty(PRClientLookupId))
                {
                    PRClientLookupId = PRClientLookupId.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.PRClientLookupId) && x.PRClientLookupId.ToUpper().Contains(PRClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Reason))
                {
                    Reason = Reason.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Reason) && x.Reason.ToUpper().Contains(Reason))).ToList();
                }
                if (!string.IsNullOrEmpty(CreatedBy))
                {
                    CreatedBy = CreatedBy.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.CreatedBy) && x.CreatedBy.ToUpper().Contains(CreatedBy))).ToList();
                }
                if (CreatedDate != null)
                {
                    PAMasterList = PAMasterList.Where(x => (x.CreateDate != null && x.CreateDate.Value.Date.Equals(CreatedDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorClientLookupId))
                {
                    // V2-851
                    //PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorClientLookupId) && x.VendorClientLookupId.Equals(VendorClientLookupId))).ToList();
                    VendorClientLookupId = VendorClientLookupId.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorClientLookupId) && x.VendorClientLookupId.ToUpper().Contains(VendorClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorName))
                {
                    VendorName = VendorName.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorName) && x.VendorName.ToUpper().Contains(VendorName))).ToList();
                }
                if (TotalCost != null)
                {
                    PAMasterList = PAMasterList.Where(x => (x.TotalCost.Value.Equals(TotalCost))).ToList();
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PAMasterList.Count();
            totalRecords = PAMasterList.Count();

            int initialPage = start.Value;

            var filteredResult = PAMasterList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, options = actualWBdropDownsModel }, JsonSerializer12HoursDateAndTimeSettings);
        }
        [HttpGet]
        public JsonResult GetPurchaseApprovalAllData(int? draw, int? start, int? length, int StatusTypeId = 0, int CreatedDatesId = 0, string PRClientLookupId = "", string Reason = "", string CreatedBy = "", DateTime? CreatedDate = null, string VendorClientLookupId = "", string VendorName = "", decimal? TotalCost = null)
        {
            ActualWBdropDownsModel actualWBdropDownsModel = new ActualWBdropDownsModel();
            PurchaseApprovalWrapper PAWrapper = new PurchaseApprovalWrapper(userData);
            var PAMasterList = PAWrapper.RetrievePurchaseApprovalData(StatusTypeId, CreatedDatesId);
            if (PAMasterList != null)
            {
                if (!string.IsNullOrEmpty(PRClientLookupId))
                {
                    PRClientLookupId = PRClientLookupId.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.PRClientLookupId) && x.PRClientLookupId.ToUpper().Contains(PRClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Reason))
                {
                    Reason = Reason.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Reason) && x.Reason.ToUpper().Contains(Reason))).ToList();
                }
                if (CreatedDate != null)
                {
                    PAMasterList = PAMasterList.Where(x => (x.CreateDate != null && x.CreateDate.Value.Date.Equals(CreatedDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(CreatedBy))
                {
                    CreatedBy = CreatedBy.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.CreatedBy) && x.CreatedBy.ToUpper().Contains(CreatedBy))).ToList();
                }

                if (!string.IsNullOrEmpty(VendorClientLookupId))
                {
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorClientLookupId) && x.VendorClientLookupId.Equals(VendorClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorName))
                {
                    VendorName = VendorName.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorName) && x.VendorName.ToUpper().Contains(VendorName))).ToList();
                }
                if (TotalCost != null)
                {
                    PAMasterList = PAMasterList.Where(x => (x.TotalCost.Value.Equals(TotalCost))).ToList();
                }
            }

            return Json(PAMasterList, JsonRequestBehavior.AllowGet);
        }
        private List<PurchaseApprovalModel> GetAllPurchaseApprovalByColumnWithOrder(string order, string orderDir, List<PurchaseApprovalModel> data)
        {
            List<PurchaseApprovalModel> lst = new List<PurchaseApprovalModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PRClientLookupId).ToList() : data.OrderBy(p => p.PRClientLookupId).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Reason).ToList() : data.OrderBy(p => p.Reason).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreatedBy).ToList() : data.OrderBy(p => p.CreatedBy).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.BuyerReview).ToList() : data.OrderBy(p => p.BuyerReview).ToList();
                        break;
                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorClientLookupId).ToList() : data.OrderBy(p => p.VendorClientLookupId).ToList();
                        break;
                    case "8":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorName).ToList() : data.OrderBy(p => p.VendorName).ToList();
                        break;
                    case "9":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                        break;
                }
            }
            return lst;
        }
        [HttpGet]
        public ActionResult PRLineItem(long Siteid)
        {
            PurchaseApprovalWrapper PAWrapper = new PurchaseApprovalWrapper(userData);
            var LineItemMasterList = PAWrapper.GetLineItems(Siteid);
            PurchaseApprovalVM objPAM = new PurchaseApprovalVM();
            objPAM.LineItemList = LineItemMasterList;
            LocalizeControls(objPAM, LocalizeResourceSetConstants.PurchaseRequest);
            return View(objPAM);
        }
        [HttpPost]
        public async Task<JsonResult> SaveApprovalList(List<long> list)
        {
            PurchaseApprovalWrapper SVWrapper = new PurchaseApprovalWrapper(userData);
            List<PRExportModel_Coupa> PrCoupa = new List<PRExportModel_Coupa>();
            string errMsg = string.Empty;
            var result = SVWrapper.UpdateApprovalList(list);

            #region PR Export
            //var result = false;  //---to be removed after completion of coupa
            if (SVWrapper.CheckIsActiveInterface())
            {
                PrCoupa = SVWrapper.GetApprovalList(list);
                if (PrCoupa.Count > 0)
                {
                    // V2-852 - Change to using token
                    InterfaceProp iprop = SVWrapper.RetrieveInterfaceProperties();
                    string int_url = iprop.APIKey1;   // Coupa - dean foods - "https://deanfoods-test.coupahost.com/api/requisitions/submit_for_approval"
                    string coupa_token = string.Empty;
                    if (SVWrapper.RetrieveCoupaToken(iprop, ref coupa_token))
                    {
                      foreach (var vprc in PrCoupa)
                      {
                          JsonSerializerSettings settings = new JsonSerializerSettings()
                          {
                            NullValueHandling = NullValueHandling.Ignore,
                          };
                          string strJson = JsonConvert.SerializeObject(vprc,settings);
                          if(strJson.Length > 0)
                          { 
                              HttpResponseMessage httpResponse = new HttpResponseMessage();
                              using (var httpClient = new HttpClient())
                              {
                                // V2-852 - Use Token
                                using (var request = new HttpRequestMessage(new HttpMethod("POST"), int_url))
                                {
                                  httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", coupa_token);
                                  request.Headers.TryAddWithoutValidation("Accept", "application/json");
                                  request.Content = new StringContent(strJson, System.Text.Encoding.UTF8, "application/json");
                                  try
                                  {    
                                    httpResponse = await httpClient.SendAsync(request);
                                  }
                                    catch (Exception ex)
                                  { }
                                }
                                /* - V2-852 - Change to using token 
                                //using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://deanfoods-test.coupahost.com/api/requisitions/submit_for_approval"))
                                using (var request = new HttpRequestMessage(new HttpMethod("POST"), int_url))
                                {
                                  request.Headers.TryAddWithoutValidation("Accept", "application/json");
                                  request.Headers.TryAddWithoutValidation("X-COUPA-API-KEY", int_key);
                                  //request.Headers.TryAddWithoutValidation("X-COUPA-API-KEY", "8b2c306ada338ccc50e8afb8249ed344168f7ee7");
                                  request.Content = new StringContent(strJson,Encoding.UTF8,"application/json");
                                  //request.Content = new StringContent(Regex.Replace(strJson, "(?:\\r\\n|\\n|\\r)", string.Empty), Encoding.UTF8, "application/json");
                                  try
                                  {
                                    httpResponse = await httpClient.SendAsync(request);
                                  }
                                  catch (Exception ex)
                                  { }
                                }
                                */
                              }
                              //----------------------------------------
                              if (httpResponse.IsSuccessStatusCode)
                              {
                                //if success 1. notify user, 2. status="Extracted"
                                string resultJSON = httpResponse.Content.ReadAsStringAsync().Result;
                                //var errList = SVWrapper.UpdatePrStatus(vprc.PurchaseRequestId, "Extracted");    
                              }
                              else
                              {
                                //else 1. notify user, 2.status="Awaiting Approval"
                                //errMsg = errMsg + "Request:" + vprc.somax_pr + ", Error:" + httpResponse.StatusCode + ";";
                                string resultJSON = httpResponse.Content.ReadAsStringAsync().Result;
                              }
                          }
                      } // end of foreach loop
                    }   // If Retrieve Token 
                    else 
                    {
                      //string resultJSON = "Error retrieving coupa token";
                      //errMsgList.Add(pritem.ClientLookupId + " failed :" + resultJSON);
                      return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);

                    }
                } // Count > 0
        #region - unused code
        //        ////----using curl ----- step 1
        //        //var client = new HttpClient();

        //        //// --Create the HttpContent for the form to be posted.
        //        //var requestContent = new FormUrlEncodedContent(new[] {
        //        //        new KeyValuePair<string, string>("text", strJson),});

        //        ////var requestContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("something", "here"), });

        //        //// --Get the response.
        //        //HttpResponseMessage response = await client.PostAsync(
        //        //    "https://deanfoods-test.coupahost.com/API",
        //        //    requestContent);

        //        ////HttpResponseMessage response = await client.PostAsync("https://api.lob.com/v1/postcards", requestContent);

        //        //// Get the response content.
        //        //HttpContent responseContent = response.Content;

        //        //// Get the stream of the content.
        //        //using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
        //        //{
        //        //    // Write the output.
        //        //    // Console.WriteLine(await reader.ReadToEndAsync());
        //        //}

        //        ////------------------------

        //        ////---using curl  step 2
        //        ////------------------------------------------
        //        //string url = "https://deanfoods-test.coupahost.com/API";
        //        //string data = strJson;
        //        //WebRequest myReq = WebRequest.Create(url);
        //        //myReq.Method = "POST";
        //        //myReq.ContentLength = data.Length;
        //        //myReq.ContentType = "application/json; charset=UTF-8";
        //        //UTF8Encoding enc = new UTF8Encoding();
        //        //myReq.Headers.Remove("auth-token");

        //        ////string credentials = "8b2c306ada338ccc50e8afb8249ed344168f7ee7";
        //        ////CredentialCache mycache = new CredentialCache();
        //        ////myReq.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
        //        //using (Stream ds = myReq.GetRequestStream())
        //        //{
        //        //    ds.Write(enc.GetBytes(data), 0, data.Length);
        //        //}
        //        //try
        //        //{
        //        //    WebResponse wr = await myReq.GetResponseAsync();//.GetResponse();
        //        //    Stream receiveStream = wr.GetResponseStream();
        //        //    StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
        //        //    string content = reader.ReadToEnd();
        //        //}
        //        //catch (Exception ex)
        //        //{

        //        //}
        //        ////------------------------------------------

        //        ////---using curl  step 3
        //        ////------------------------------------------
        //        //using (var httpClient = new HttpClient())
        //        //{
        //        //    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://reqres.in/api/users"))
        //        //    {
        //        //        request.Headers.TryAddWithoutValidation("Accept", "application/json");
        //        //        request.Headers.TryAddWithoutValidation("User-Agent", "curl/7.60.0");

        //        //        //var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("username:8b2c306ada338ccc50e8afb8249ed344168f7ee7"));
        //        //        //request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

        //        //        var response = await httpClient.SendAsync(request);
        //        //        //----
        //        //        string data = strJson;
        //        //        using (Stream ds = request.GetRequestStream())
        //        //        {
        //        //            ds.Write(enc.GetBytes(data), 0, data.Length);
        //        //        }
        //        //        try
        //        //        {
        //        //            WebResponse wr = await myReq.GetResponseAsync();//.GetResponse();
        //        //            Stream receiveStream = wr.GetResponseStream();
        //        //            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
        //        //            string content = reader.ReadToEnd();
        //        //        }
        //        //        catch (Exception ex)
        //        //        {

        //        //        }
        //        //    }

        //        //}


        //        //------------------------------------------

        //        //-------using curl genereted code    step 4--------

        //        string filepath = "D:\\CURL64\\curl-7.65.3-win64-mingw\\bin\\Test_JAB.json";
        //    using (StreamReader r = new StreamReader(filepath))
        //    {
        //        var json = r.ReadToEnd();
        //        try
        //        {
        //            var jobj = JObject.Parse(json);              //--for single json
        //           // var jobj = JArray.Parse(json.ToString());  //--for json array
        //            strJson = jobj.ToString();
        //        }
        //        catch (Exception ex1)
        //        {

        //        }
        //        //foreach (var item in jobj.Properties())
        //        //{
        //        //    item.Value = item.Value.ToString().Replace("v1", "v2");
        //        //}
        //    }

        //        HttpResponseMessage httpResponse = new HttpResponseMessage();
        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://deanfoods-test.coupahost.com/api/requisitions/submit_for_approval"))
        //        {
        //            request.Headers.TryAddWithoutValidation("Accept", "application/json");
        //            request.Headers.TryAddWithoutValidation("X-COUPA-API-KEY", "8b2c306ada338ccc50e8afb8249ed344168f7ee7");

        //            //request.Content = new StringContent(Regex.Replace(File.ReadAllText("sample.json"), "(?:\\r\\n|\\n|\\r)", string.Empty), Encoding.UTF8, "application/json");

        //            request.Content = new StringContent(Regex.Replace(strJson, "(?:\\r\\n|\\n|\\r)", string.Empty), Encoding.UTF8, "application/json");

        //            try
        //            {
        //                    httpResponse = await httpClient.SendAsync(request);
        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //    }
        //        //----------------------------------------

        //        if (httpResponse.IsSuccessStatusCode)
        //        {
        //            //if success 1. notify user, 2. status="Extracted"

        //            var errList = SVWrapper.UpdatePrStatus(vprc.PurchaseRequestId,"Extracted");
        //        }
        //        else
        //        {
        //            //else 1. notify user, 2.status="Awaiting Approval"
        //            errMsg = errMsg + "Request:" + vprc.somax_pr + ", Error:" + httpResponse.StatusCode + ";";
        //        }
        //    }// end of foreach loop
        //}
        //public async Task<Uri> CreateProductAsync(List<PRExportModel_Coupa> PrCoupa)
        //{
        //    HttpClient client = new HttpClient();
        //    //------------------------
        //    HttpResponseMessage response = await client.PostAsJsonAsync(
        //        $"https://deanfoods-test.coupahost.com/API", PrCoupa);
        //    response.EnsureSuccessStatusCode();

        //    // return URI of the created resource.
        //    return response.Headers.Location;

        //    //------------------------
        //    //client.BaseAddress = new Uri("https://deanfoods-test.coupahost.com/API");
        //    //client.DefaultRequestHeaders.Accept.Clear();
        //    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //}

        #endregion - unused code
            } // Is Interface Active

            #endregion

            if (result)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveDenyList(List<long> list)
        {
            PurchaseApprovalWrapper SVWrapper = new PurchaseApprovalWrapper(userData);
            var result = SVWrapper.UpdateDenyList(list);
            if (result)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateReturnToRequester(string[] wOIds, string Comments)
        {
            PurchaseApprovalWrapper SVWrapper = new PurchaseApprovalWrapper(userData);
            var result = SVWrapper.UpdateReturnToRequesterList(wOIds, Comments);
            if (result)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}