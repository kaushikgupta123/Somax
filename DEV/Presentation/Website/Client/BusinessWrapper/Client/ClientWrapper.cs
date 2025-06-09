using Business.Authentication;

using Client.Models.ClientMessages;

using Common.Constants;
using Common.Extensions;

using Database.Business;

using DataContracts;

using DevExpress.Internal;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;


namespace Client.BusinessWrapper
{
    public class ClientWrapper
    {
        private DatabaseKey _dbKey;
        public readonly DataContracts.UserData _userData;

        List<string> errorMessage = new List<string>();
        public ClientWrapper(DataContracts.UserData userData)
        {
            this._userData = userData;
            _dbKey = _userData.DatabaseKey;
        }

        #region GetClientMessage
        public ClientMessage GetClientMessage(long ClientId)
        {
            string timeZone = _userData.Site.TimeZone;
            DataContracts.ClientMessage clientMessage = new DataContracts.ClientMessage();
            clientMessage.ClientId = _userData.DatabaseKey.Client.ClientId;
            clientMessage.TimeZone = timeZone;
            clientMessage = clientMessage.RetrieveClientMessageSch(_userData.DatabaseKey, timeZone, ClientId);
            return clientMessage;
        }
        #endregion
    }
}