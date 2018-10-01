using System;
using System.Management;
using System.Runtime.Serialization;

namespace Shared.Modelo
{
    [DataContract]
    public class CompSys {
        [DataMember]
        public string NotebookModel;
        [DataMember]
        public string ComputerManufacturer;
        /*
         * Los valores de ValueOverride corresponden a:
         * [0] - NotebookModel
         */
        [DataMember]
        public bool[] ValueOverride = new bool[1];

        private const string defaultAnswer = "SinDefinir";


        public CompSys() {
            for (int i = 0; i < ValueOverride.Length - 1; i++)
                ValueOverride[i] = false;
            var query = getCompSysWMISearch();
            NotebookModel = getNotebookModel(query);
            ComputerManufacturer = getComputerManufacturer(query);
        }


        static ManagementObjectCollection getCompSysWMISearch() {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Model, Manufacturer FROM Win32_ComputerSystem");
            return searcher.Get();
        }

        public static string getNotebookModel(ManagementObjectCollection compsysquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in compsysquery) {
                    result = (queryObj["Model"]).ToString();
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }
        public static string getComputerManufacturer(ManagementObjectCollection compsysquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in compsysquery) {
                    result = (queryObj["Manufacturer"]).ToString();
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }
    }
}
