using System;
using System.Management;
using System.Runtime.Serialization;

namespace Shared.Modelo
{
    [DataContract]
    public class MoBo {
        [DataMember]
        public string Manufacturer;
        [DataMember]
        public string Prodcut;
        [DataMember]
        public string SerialNumber;
        /*
         * Los valores de ValueOverride corresponden a:
         * [0] - Manufacturer
         * [1] - Product
         */
        [DataMember]
        public bool[] ValueOverride = new bool[2];

        private const string defaultAnswer = "Sin Definir";

        public MoBo() {
            for (int i = 0; i < ValueOverride.Length; i++)
                ValueOverride[i] = false;
            var query = GetMoBoWMISearch();
            Manufacturer = GetMoBoManufacturer(query);
            Prodcut = GetMoBoProduct(query);
            SerialNumber = GetMoBoSerialNumber(query);
        }

        public static ManagementObjectCollection GetMoBoWMISearch() {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Manufacturer, Product, SerialNumber FROM Win32_BaseBoard");
            return searcher.Get();
        }

        public static string GetMoBoManufacturer(ManagementObjectCollection moboquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in moboquery) {
                    result = (queryObj["Manufacturer"]).ToString();
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static string GetMoBoProduct(ManagementObjectCollection moboquery) {
            try {
                string result = null;
                foreach (ManagementObject queryObj in moboquery) {
                    result = (queryObj["Product"]).ToString();
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static string GetMoBoSerialNumber(ManagementObjectCollection moboquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in moboquery) {
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
