using Admin.Models;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.BusinessWrapper.Knowledgebase
{
    public interface IKnowledgebaseWrapper
    {
        UserData userData { get; set; }
        List<KBTopics> GetKnowledgebaseGridData(int CustomQueryDisplayId, out Dictionary<string, Dictionary<string, string>> lookupLists, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0,
           string Title = "", string Category="", string Description = "", string Tags = "", string Folder = "", string searchText = "");
        KnowledgebaseModel GetKnowledgebaseDetails(long kBTopicsId);
        KBTopics AddOrEditKbTopics(KnowledgebaseCombined objKbcomb);
        List<List<KBTopics>> KbTopicsPersonnelList(string KBTopicsId = "");
        string RetrieveTagValue(long KBTopicsId = 0);
        List<string> KbTopicsTags();
    }
}
