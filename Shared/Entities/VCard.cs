using System;
using System.Management;
using System.Runtime.Serialization;

namespace Shared.Modelo
{
    [DataContract]
    public class VCard {
        [DataMember]
        public string Name;
        [DataMember]
        public string Memory;
        [DataMember]
        public string DriverVersion;
        /*
         * Los valores de ValueOverride corresponden a:
         * [0] - Name
         * [1] - Memory
         * [2] - DriverVersion
         */
        [DataMember]
        public bool[] ValueOverride = new bool[3];

        private const string defaultAnswer = "Sin Definir";
        private const string defaultAnswerNum = "-1";


        public VCard() {
            for (int i = 0; i < ValueOverride.Length; i++)
                ValueOverride[i] = false;
            var query = GetVCardWMISearch();
            Name = GetVCardName(query);
            Memory = GetVCardMemory(query);
            DriverVersion = GetVCardDriverVersion(query);
        }


        /*        public static ManagementObjectSearcher GetVCardWMISearch() {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, AdapterRAM, DriverVersion FROM Win32_VideoController");
                    return searcher;
                } */

        public static ManagementObjectCollection GetVCardWMISearch() {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, AdapterRAM, DriverVersion FROM Win32_VideoController");
            return searcher.Get();
        }

        public static string GetVCardName(ManagementObjectCollection vcardquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in vcardquery) {
                    result = (queryObj["Name"]).ToString();
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static string GetVCardMemory(ManagementObjectCollection vcardquery) {
            long result = 0;
            try {
                foreach (ManagementObject queryObj in vcardquery) {
                    result = Convert.ToInt64(queryObj["AdapterRAM"]);
                }
                if (result >= 1073741824)
                    return (result / (1024 * 1024 * 1024)).ToString() + "Gb";
                else
                    return (result / (1024 * 1024)).ToString() + "Mb";
            }
            catch (Exception) {
                return defaultAnswerNum;
            }
        }

        public static string GetVCardDriverVersion(ManagementObjectCollection vcardquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in vcardquery) {
                    result = (queryObj["DriverVersion"]).ToString();
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }



    }
}
