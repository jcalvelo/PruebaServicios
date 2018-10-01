using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace TestService.Controllers
{
    public class FileController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Clientexe()
        {
            var path = @"C:\Temp\Client.exe";
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            Byte[] bytes = File.ReadAllBytes(path);
            String file = Convert.ToBase64String(bytes);
            result.Content = new StringContent(file);
            return result;

        }

        [HttpGet]
        public HttpResponseMessage Shareddll()
        {
            var path = @"C:\Temp\Shared.dll";
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            Byte[] bytes = File.ReadAllBytes(path);
            String file = Convert.ToBase64String(bytes);
            result.Content = new StringContent(file);
            return result;
        }

        [HttpGet]
        public HttpResponseMessage Config()
        {
            var path = @"C:\Temp\Client.exe.config";
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            Byte[] bytes = File.ReadAllBytes(path);
            String file = Convert.ToBase64String(bytes);
            result.Content = new StringContent(file);
            return result;
        }

        [HttpGet]
        public HttpResponseMessage log4net()
        {
            var path = @"C:\Temp\log4net.dll";
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            Byte[] bytes = File.ReadAllBytes(path);
            String file = Convert.ToBase64String(bytes);
            result.Content = new StringContent(file);
            return result;
        }


        [HttpGet]
        public HttpResponseMessage MongoBson()
        {
            var path = @"C:\Temp\MongoDB.Bson.dll";
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            Byte[] bytes = File.ReadAllBytes(path);
            String file = Convert.ToBase64String(bytes);
            result.Content = new StringContent(file);
            return result;
        }


        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}