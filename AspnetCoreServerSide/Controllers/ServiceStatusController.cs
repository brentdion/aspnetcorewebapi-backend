using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspnetCoreServerSide.Controllers
{
    [Produces("application/json")] 
    public class ServiceStatusController : Controller
    {
        public ServiceStatusController()
        {
            
        }

        [HttpGet]
        [ActionName("Status")]
        public string Status()
        {
            return "The service is running...";
        }
    }
}