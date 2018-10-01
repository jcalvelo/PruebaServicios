using System;
using System.Management;
using System.Runtime.Serialization;

namespace Shared.Modelo
{
    [DataContract]
    public class OS {
        [DataMember]
        public string Name;
        [DataMember]
        public string Arch;
        /*
         * Los valores de ValueOverride corresponden a:
         * [0] - Name
         * [1] - Arch
         */
        [DataMember]
        public bool[] ValueOverride = new bool[2];

        private const string defaultAnswer = "Sin Definir";


        public OS() {
            for (int i = 0; i < ValueOverride.Length; i++)
                ValueOverride[i] = false;
            var query = GetOSWMISearch();
            Name = GetOS(query);
            Arch = GetOSArch(query);
        }


        public static ManagementObjectCollection GetOSWMISearch() {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Caption, OSArchitecture FROM Win32_OperatingSystem");
            return searcher.Get();
        }

        public static string GetOS(ManagementObjectCollection osquery) {
            try {
                string result = null;
                foreach (ManagementObject queryObj in osquery) {
                    result = (queryObj["Caption"]).ToString();
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static string GetOSArch(ManagementObjectCollection osquery) {
            try {
                string result = null;
                foreach (ManagementObject queryObj in osquery) {
                    result = (queryObj["OSArchitecture"]).ToString();
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

    }
}
