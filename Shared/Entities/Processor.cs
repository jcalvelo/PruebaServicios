using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management;
using System.Runtime.Serialization;

namespace Shared.Modelo
{
    [DataContract]
    public class Processor {
        [DataMember]
        public string Manufacturer;
        [DataMember]
        public string Model;
        [DataMember]
        public string Arch;
        [DataMember]
        public string ClockSpeed;
        [DataMember]
        public string CoreCount;
        [DataMember]
        public string ThreadCount;
        /*
         * Los valores de ValueOverride corresponden a:
         * [0] - Manufacturer
         * [1] - Model
         * [2] - Arch
         * [3] - ClockSpeed
         * [4] - CoreCount
         * [5] - ThreadCount
         */
        [DataMember]
        public bool[] ValueOverride = new bool[6];


        private const string defaultAnswer = "Sin Definir";


        public Processor(string index, ManagementObjectCollection wmiquery) {
            for (int i = 0; i < ValueOverride.Length; i++)
                ValueOverride[i] = false;
            Manufacturer = GetCPUManufacturer(index, wmiquery);
            Model = GetCPUModel(index, wmiquery);
            Arch = GetCPUArch(index, wmiquery);
            ClockSpeed = GetCPUClockSpeed(index, wmiquery);
            CoreCount = GetCPUNumCores(index, wmiquery);
            ThreadCount = GetCPUThreadCount(index, wmiquery);
        }

        public static ManagementObjectCollection GetWMIProcSearch() {
            ManagementObjectSearcher result = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID, Name, AddressWidth, MaxClockSpeed, NumberOfCores, ThreadCount, NumberOfLogicalProcessors, Manufacturer FROM Win32_Processor");
            result = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID, Name, AddressWidth, MaxClockSpeed, NumberOfCores, NumberOfLogicalProcessors, Manufacturer FROM Win32_Processor");

            return result.Get();
        }

        public static ManagementObjectCollection GetWMIProcSearchAlt() {
            ManagementObjectSearcher result = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID, Name, AddressWidth, MaxClockSpeed, NumberOfCores, NumberOfLogicalProcessors, Manufacturer FROM Win32_Processor");
            result = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID, Name, AddressWidth, MaxClockSpeed, NumberOfCores, NumberOfLogicalProcessors, Manufacturer FROM Win32_Processor");

            return result.Get();
        }

        public static List<Processor> GetCPUList() {
            try {
                var wmi = GetWMIProcSearch();
                List<string> cpuidlist = new List<string>();
                foreach (ManagementObject queryObj in wmi) { 
                    cpuidlist.Add((queryObj["DeviceID"]).ToString());
                }

                List<Processor> result = new List<Processor>();
                foreach (var i in cpuidlist) {
                    result.Add(new Processor(i, wmi));
                }

                return result;
            }
            catch (Exception) {
                var wmi = GetWMIProcSearchAlt();
                List<string> cpuidlist = new List<string>();
                foreach (ManagementObject queryObj in wmi) { //S-MAIN da error aca, posible tema con multiples CPU?
                    System.IO.File.WriteAllText(@"./log.log", "pasa");
                    cpuidlist.Add((queryObj["DeviceID"]).ToString());
                }

                List<Processor> result = new List<Processor>();
                foreach (var i in cpuidlist) {
                    result.Add(new Processor(i, wmi));
                }

                return result;

            }

        }
        public static string GetCPUModel(string cpuid, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if (queryObj["DeviceID"].ToString() == cpuid) {
                        result = (queryObj["Name"]).ToString(); 
                    }
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }
        public static string GetCPUArch(string cpuid, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if (queryObj["DeviceID"].ToString() == cpuid) {
                        result = (queryObj["AddressWidth"]).ToString(); 
                    }
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }
        public static string GetCPUClockSpeed(string cpuid, ManagementObjectCollection wmiquery) {
            NumberFormatInfo nfi = new NumberFormatInfo() {
                NumberDecimalSeparator = "."
            };
            string result = null;
            decimal auxresult = 0;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if (queryObj["DeviceID"].ToString() == cpuid) {
                        auxresult = Convert.ToDecimal(queryObj["MaxClockSpeed"]);

                    }                }
                if (auxresult >= 1000)
                    result = Math.Round((auxresult / 1000), 2).ToString(nfi) + "Ghz";
                else
                    result = auxresult.ToString(nfi) + "Mhz";
                return result;
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }
        public static string GetCPUNumCores(string cpuid, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if (queryObj["DeviceID"].ToString() == cpuid) {
                        result = (queryObj["NumberOfCores"]).ToString();

                    }                }
                return result;
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }
        public static string GetCPUThreadCount(string cpuid, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if (queryObj["DeviceID"].ToString() == cpuid) {
                        result = (queryObj["ThreadCount"]).ToString();

                    }                }
                return result;
            }
            catch (Exception) {
                try {
                    foreach (ManagementObject queryObj in wmiquery) {
                        if (queryObj["DeviceID"].ToString() == cpuid) {
                            result = (queryObj["NumberOfLogicalProcessors"]).ToString();

                        }                    }
                    return result;
                }
                catch (Exception) {
                    return defaultAnswer;
                }
            }
        }
        public static string GetCPUManufacturer(string cpuid, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if (queryObj["DeviceID"].ToString() == cpuid) {
                        result = (queryObj["Manufacturer"]).ToString();

                    }                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }
    }
}
