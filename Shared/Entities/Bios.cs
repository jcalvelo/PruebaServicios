using System;
using System.Management;
using System.Runtime.Serialization;

namespace Shared.Modelo
{
    [DataContract]
    public class Bios {
        [DataMember]
        public string Manufacturer;
        [DataMember]
        public string Version;
        [DataMember]
        public string SerialNumber;
        /*
         * Los valores de ValueOverride corresponden a:
         * [0] - Manufacturer
         * [1] - Version
         * [2] - SerialNumber
         */
        [DataMember]
        public bool[] ValueOverride = new bool[3];

        private const string defaultAnswer = "Sin Definir";

        public Bios() {
            for (int i = 0; i < ValueOverride.Length; i++)
                ValueOverride[i] = false;
            var query = GetBIOSWMISearch();
            Manufacturer = GetBIOSManufacturer(query);
            Version = GetBIOSVersion(query);
            SerialNumber = GetSerialNumber(query);
        }

        public static ManagementObjectCollection GetBIOSWMISearch() {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Manufacturer, SMBIOSBIOSVersion, SerialNumber FROM Win32_BIOS");
            return searcher.Get();
        }

        public static string GetBIOSManufacturer(ManagementObjectCollection biosquery) {
            try {
                string result = null;
                foreach (ManagementObject queryObj in biosquery) {
                    result = (queryObj["Manufacturer"]).ToString();
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static string GetBIOSVersion(ManagementObjectCollection biosquery) {
            try {
                string result = null;
                foreach (ManagementObject queryObj in biosquery) {
                    result = (queryObj["SMBIOSBIOSVersion"]).ToString();
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static string GetSerialNumber(ManagementObjectCollection biosquery) {
            try {
                string result = null;
                foreach (ManagementObject queryObj in biosquery) {
                    result = (queryObj["SerialNumber"]).ToString();
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

    }
}
