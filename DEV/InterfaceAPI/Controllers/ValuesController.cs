using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web.Http;
using System.Web.Mvc;

using Newtonsoft.Json;

using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace InterfaceAPI.Controllers
{
  //[Authorize]
  public class ValuesController : Controller
  {
    // GET api/values
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }

    // GET api/values/5
    public string Get(int id)
    {
      return "value";
    }

        // POST api/values
    [HttpPost]
    public ActionResult GetIdDetails(long id1)
    {
            var obj1 = (object)id1;
            return Json(obj1, JsonRequestBehavior.AllowGet);
    }

    // PUT api/values/5
    //public void Put(int id, [FromBody]string value)
    //{
    //}

    // DELETE api/values/5
    public void Delete(int id)
    {
    }
  }
}
