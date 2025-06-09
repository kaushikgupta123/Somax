using Client.BusinessWrapper.Common;
using Client.Models.Configuration.ShipToAddress;

using Data.DataContracts;

using DataContracts;

using System;
using System.Collections.Generic;

namespace Client.BusinessWrapper.Configuration
{
    public class ShipToWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public ShipToWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<ShipToModel> GetShipToAddressList(string orderbycol = "", int length = 0, string orderDir = "", int skip = 0, string _clientLookupId = "", string _address1 = null, string _addresscity = null, string _addressstate = null)
        {
            List<ShipToModel> ModelList = new List<ShipToModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            ShipToModel Model;
            ShipTo shipTo = new ShipTo();
            shipTo.ClientId = userData.DatabaseKey.Client.ClientId;
            shipTo.orderbyColumn = orderbycol;
            shipTo.orderBy = orderDir;
            shipTo.offset1 = skip;
            shipTo.nextrow = length;
            shipTo.ClientLookupId = _clientLookupId;
            shipTo.Address1 = _address1;
            shipTo.AddressCity = _addresscity;
            shipTo.AddressState = _addressstate;

            var shipToList = shipTo.RetrieveChunkSearch(userData.DatabaseKey);

            foreach (var item in shipToList)
            {
                Model = new ShipToModel();
                Model.ShipToId = item.ShipToId;
                Model.ClientLookupId = item.ClientLookupId;
                Model.AttnName = item.AttnName;
                Model.Address1 = item.Address1;
                Model.AddressCity = item.AddressCity;
                Model.AddressState = item.AddressState;
                Model.TotalCount = item.totalCount;
                ModelList.Add(Model);
            }

            return ModelList;
        }

        #endregion

        #region Add


        public List<String> AddOrEditShipToRecord(ShipToModel model)
        {
            if (model.ShipToId == 0)
            {
                ShipTo shipTo = new ShipTo();
                shipTo.ClientId = userData.DatabaseKey.Personnel.ClientId;
                shipTo.SiteId = this.userData.Site.SiteId;
                shipTo.ClientLookupId = model.ClientLookupId;
                shipTo.AttnName = model.AttnName;
                shipTo.EmailAddress = model.EmailAddress;
                shipTo.PhoneNumber = model.PhoneNumber;
                shipTo.FaxNumber = model.FaxNumber;
                shipTo.Address1 = model.Address1;
                shipTo.Address2 = model.Address2;
                shipTo.Address3 = model.Address3;
                shipTo.AddressCity = model.AddressCity;
                shipTo.AddressState = model.AddressState;
                shipTo.AddressPostCode = model.AddressPostCode;
                shipTo.AddressCountry = model.AddressCountry;
                shipTo.Add(userData.DatabaseKey);
                return shipTo.ErrorMessages;
            }
            else
            {
                ShipTo shipToModel = new ShipTo()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    ShipToId = model.ShipToId,
                };
                shipToModel.Retrieve(userData.DatabaseKey);
                shipToModel.ClientId = userData.DatabaseKey.Personnel.ClientId;               
                shipToModel.AttnName = model.AttnName ?? string.Empty;
                shipToModel.EmailAddress = model.EmailAddress ?? string.Empty;
                shipToModel.PhoneNumber = model.PhoneNumber ?? string.Empty;
                shipToModel.FaxNumber = model.FaxNumber ?? string.Empty;
                shipToModel.Address1 = model.Address1 ?? string.Empty;
                shipToModel.Address2 = model.Address2 ?? string.Empty;
                shipToModel.Address3 = model.Address3 ?? string.Empty;
                shipToModel.AddressCity = model.AddressCity ?? string.Empty;
                shipToModel.AddressState = model.AddressState ?? string.Empty;
                shipToModel.AddressPostCode = model.AddressPostCode ?? string.Empty;
                shipToModel.AddressCountry = model.AddressCountry ?? string.Empty;
                if (shipToModel.ClientLookupId == model.ClientLookupId)
                {
                    shipToModel.Update(userData.DatabaseKey);
                }
                else 
                {
                    shipToModel.ClientLookupId = model.ClientLookupId;
                    shipToModel.UpdateCustom(userData.DatabaseKey);
                }

                return shipToModel.ErrorMessages;
            }
        }
        #endregion

        #region Delete
        public List<String> DeleteShipToAddress(long ShipToId)
        {
            ShipTo shipTo = new ShipTo()
            {
                ShipToId = ShipToId
            };
            shipTo.Delete(userData.DatabaseKey);
            return shipTo.ErrorMessages;
        }
        #endregion



        public ShipToModel RetrieveShipToDetailsById(long ShipToId)
        {
            ShipTo shipTo = new ShipTo()
            {
                ShipToId = ShipToId,
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            shipTo.Retrieve(userData.DatabaseKey);

            ShipToModel model = new ShipToModel();
            model.ShipToId = shipTo.ShipToId;
            model.ClientLookupId = shipTo.ClientLookupId;
            model.AttnName = shipTo.AttnName;
            model.EmailAddress = shipTo.EmailAddress;
            model.PhoneNumber = shipTo.PhoneNumber;
            model.FaxNumber = shipTo.FaxNumber;
            model.Address1 = shipTo.Address1;
            model.Address2 = shipTo.Address2;
            model.Address3 = shipTo.Address3;
            model.AddressCity = shipTo.AddressCity;
            model.AddressState = shipTo.AddressState;
            model.AddressPostCode = shipTo.AddressPostCode;
            model.AddressCountry = shipTo.AddressCountry;

            return model;
        }
    }
}