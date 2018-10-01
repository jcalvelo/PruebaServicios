using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TestUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            download("https://inventario.ssi.com.uy/api/file/clientexe", "Client.exe");
            download("https://inventario.ssi.com.uy/api/file/shareddll", "Shared.dll");
            download("https://inventario.ssi.com.uy/api/file/config", "Client.exe.config");
            download("https://inventario.ssi.com.uy/api/file/log4net", "log4net.dll");
            download("https://inventario.ssi.com.uy/api/file/mongobson", "MongoDB.Bson.dll");


            Process.Start(@"C:\Users\JCalvelo\Desktop\Inventario\Client.exe");
        }

        public static void download(String url, String filename)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            X509Certificate2Collection certificates = new X509Certificate2Collection();
            //certificates.Import(@"C:\Users\jcalvelo\Desktop\SSICerts\SSIInventario.cer", "ReservaSSI2018", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            //certificates.Import(@"C:\Users\JCalvelo\Desktop\SSICerts\SSIInventario.crt");
            //webrequest.ClientCertificates = certificates;
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            responseStream.Close();
            webresponse.Close();
            Byte[] bytes = Convert.FromBase64String(result);
            File.WriteAllBytes(@"C:\users\jcalvelo\Desktop\Inventario\" + filename, bytes);
        }
    }
}
