using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.ClientMessages
{
    public class ClientMessages : LocalisationBaseVM
    {
        public ClientMessageModel ClientMessageModel { get; set; }

    }
    [Serializable]
    public class ClientMessageModel
    {
        public long ClientId { get; set; }
        public string Message { get; set; }
    }
}