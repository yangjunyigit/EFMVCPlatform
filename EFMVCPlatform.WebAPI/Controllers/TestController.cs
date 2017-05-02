using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EFMVCPlatform.WebAPI.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public int TestMethod(string txt1)
        {
            return 1;
        }
        [HttpGet]
        public int TestMethod2(string txt1)
        {
            return 2;
        }
        [HttpGet]
        public int TestMethod3(string txt3)
        {
            return 3;
        }
        public string Index()
        {
            return "123";
        }
    }
}
