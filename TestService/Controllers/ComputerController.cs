using Shared.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TestService.Controllers
{
    public class ComputerController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage ComputerPost(Computer c)
        {
            Console.WriteLine("Allooooo!!!");
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
