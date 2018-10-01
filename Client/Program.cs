using Shared.Components;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.ServiceModel.Configuration;
using System.Text;
using Newtonsoft.Json;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Client
{
    public class Program {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Program.cs");

        [STAThread]
        public static void Main(string[] args) {
            try {

                StuffAsync();

                Console.ReadKey();

            }
            catch (Exception e) {
                log.Error(e);                
            }
            
        }

        public static async System.Threading.Tasks.Task<bool> StuffAsync()
        {
            Shared.Modelo.Computer c = DataGathering.GatherData();
            HttpClient client = new HttpClient();

            var json = JsonConvert.SerializeObject(c);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:55050/api/computer/computerpost", stringContent);

            return true;
        }


        public static string GetEndpointAddress() {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            ServiceModelSectionGroup serviceModelSectionGroup = ServiceModelSectionGroup.GetSectionGroup(configuration);
            ClientSection clientSection = serviceModelSectionGroup.Client;
            var el = clientSection.Endpoints[0];
            return el.Address.Host.ToString();
        }

        static string GetIDFromAppConfig() {
            string id = ConfigManager.ReadSetting("ComputerID");
            if (id == "Not Found") {
                CreateKeyAppConfig();
                return "";
            }
            else
                return id;
        }

        static void SetCompIDAppConfig(int id) {
            ConfigManager.AddUpdateAppSettings("ComputerID", id.ToString());
        }

        static string CreateKeyAppConfig() {
            ConfigManager.AddUpdateAppSettings("ComputerID", "");
            return "";
        }

    }
}
