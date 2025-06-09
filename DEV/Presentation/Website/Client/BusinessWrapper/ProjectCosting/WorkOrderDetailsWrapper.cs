using Client.Models.Project;
using Client.Models.ProjectCosting;

using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper.ProjectsCosting
{
    public class WorkOrderDetailsWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;

        public WorkOrderDetailsWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        
    }
}