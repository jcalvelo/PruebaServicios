using System;
using System.Collections.Generic;
using System.Management;
using System.Runtime.Serialization;

namespace Shared.Modelo
{
    [DataContract]
    public class RAM {
        [DataMember]
        public string Manufacturer;
        [DataMember]
        public string Capacity;
        [DataMember]
        public string Freq;
        [DataMember]
        public string PartNumber;
        [DataMember]
        public string SerialNumber;
        /*
         * Los valores de ValueOverride corresponden a:
         * [0] - Manufacturer
         * [1] - Capacity
         * [2] - Freq
         * [3] - PartNumber
         * [4] - SerialNumber
         */
        [DataMember]
        public bool[] ValueOverride = new bool[5];

        private const string defaultAnswer = "Sin Definir";
        private const string defaultAnswerNum = "-1";


        public RAM(string index, ManagementObjectCollection wmiquery) {
            for (int i = 0; i < ValueOverride.Length; i++)
                ValueOverride[i] = false;
            Manufacturer = GetRamSlotManufacturer(index, wmiquery);
            Capacity = GetRamSlotCapacity(index, wmiquery);
            Freq = GetRamSlotFreq(index, wmiquery);
            PartNumber = GetRamSlotPN(index, wmiquery);
            SerialNumber = GetRamSlotSN(index, wmiquery);
        }

/*        public static ManagementObjectSearcher GetWMIRAMSearch() {
            ManagementObjectSearcher result = new ManagementObjectSearcher("root\\CIMV2", "SELECT BankLabel, Manufacturer, Capacity, ConfiguredClockSpeed, Speed, PartNumber, SerialNumber FROM Win32_PhysicalMemory");
            return result;
        }*/

        public static ManagementObjectCollection GetWMIRAMSearch() {
            ManagementObjectSearcher result = new ManagementObjectSearcher("root\\CIMV2", "SELECT BankLabel, Manufacturer, Capacity, ConfiguredClockSpeed, Speed, PartNumber, SerialNumber FROM Win32_PhysicalMemory");
            return result.Get();
        }

        public static ManagementObjectCollection GetWMIRAMSearchAlt() {
            ManagementObjectSearcher result = new ManagementObjectSearcher("root\\CIMV2", "SELECT BankLabel, Manufacturer, Capacity, Speed, PartNumber, SerialNumber FROM Win32_PhysicalMemory");
            return result.Get();
        }

        public static List<RAM> GetRAMList() {
            try {
                List<string> banklabelIDs = new List<string>();
                var wmi = GetWMIRAMSearch();
                foreach (ManagementObject queryObj in wmi) {
                    banklabelIDs.Add((queryObj["BankLabel"]).ToString());
                }

                List<RAM> result = new List<RAM>();
                foreach (var i in banklabelIDs) {
                    result.Add(new RAM(i, wmi));
                }

                return result;
            }
            catch (Exception) {
                List<string> banklabelIDs = new List<string>();
                var wmi = GetWMIRAMSearchAlt();
                foreach (ManagementObject queryObj in wmi) {
                    banklabelIDs.Add((queryObj["BankLabel"]).ToString());
                }

                List<RAM> result = new List<RAM>();
                foreach (var i in banklabelIDs) {
                    result.Add(new RAM(i, wmi));
                }

                return result;
            }
        }

        public static string GetTotalRam() {
            long result = 0;
            ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("root\\CIMV2", "SELECT Capacity FROM Win32_PhysicalMemory");
            foreach (ManagementObject queryObj in searcher2.Get()) {
                result = result + Convert.ToInt64(queryObj["Capacity"]);
            }
            if (result >= 1073741824)
                return (result / (1024 * 1024 * 1024)).ToString() + "Gb";
            else
                return (result / (1024 * 1024)).ToString() + "Mb";
        }
        public static string GetRamSlotManufacturer(string bankslot, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if (queryObj["BankLabel"].ToString() == bankslot) {
                        result = (queryObj["Manufacturer"]).ToString();
                    }
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }
        public static string GetRamSlotCapacity(string bankslot, ManagementObjectCollection wmiquery) {
            long result = 0;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if (queryObj["BankLabel"].ToString() == bankslot) {
                        result = Convert.ToInt64(queryObj["Capacity"]);

                    }                }
                if (result >= 1073741824)
                    return (result / (1024 * 1024 * 1024)).ToString() + "Gb";
                else
                    return (result / (1024 * 1024)).ToString() + "Mb";
            }
            catch (Exception) {
                return defaultAnswerNum;
            }
        }
        public static string GetRamSlotFreq(string bankslot, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if (queryObj["BankLabel"].ToString() == bankslot) {
                        result = (queryObj["ConfiguredClockSpeed"]).ToString();
                    }
                }
                return result.Trim() + "Mhz";
            }
            catch (Exception) {
                try {
                    foreach (ManagementObject queryObj in wmiquery) {
                        if (queryObj["BankLabel"].ToString() == bankslot) {
                            result = (queryObj["Speed"]).ToString(); 
                        }
                    }
                    return result.Trim() + "Mhz";
                }
                catch (Exception) {
                    return defaultAnswer;
                }
            }
        }
        public static string GetRamSlotPN(string bankslot, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if (queryObj["BankLabel"].ToString() == bankslot) {
                        result = (queryObj["PartNumber"]).ToString(); 
                    }
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }
        public static string GetRamSlotSN(string bankslot, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if (queryObj["BankLabel"].ToString() == bankslot) {
                        result = (queryObj["SerialNumber"]).ToString();

                    }                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }
        public static string GetRamType() {
            string result = null;
            int ramtype = 0;
            ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("root\\CIMV2", "SELECT MemoryType FROM Win32_PhysicalMemory");
            try {
                foreach (ManagementObject queryObj in searcher2.Get()) {
                    ramtype = Convert.ToInt32(queryObj["MemoryType"]);
                }
                switch (ramtype) {
                    case 1:
                        result = "Other";
                        break;
                    case 2:
                        result = "DRAM";
                        break;
                    case 3:
                        result = "Synchronous DRAM";
                        break;
                    case 4:
                        return "Cache DRAM";
                    case 5:
                        result = "EDO";
                        break;
                    case 6:
                        result = "EDRAM";
                        break;
                    case 7:
                        result = "VRAM";
                        break;
                    case 8:
                        result = "SRAM";
                        break;
                    case 9:
                        result = "RAM";
                        break;
                    case 10:
                        result = "ROM";
                        break;
                    case 11:
                        result = "Flash";
                        break;
                    case 12:
                        result = "EEPROM";
                        break;
                    case 13:
                        result = "FEPROM";
                        break;
                    case 14:
                        result = "EPROM";
                        break;
                    case 15:
                        result = "CDRAM";
                        break;
                    case 16:
                        result = "3DRAM";
                        break;
                    case 17:
                        result = "SDRAM";
                        break;
                    case 18:
                        result = "SGRAM";
                        break;
                    case 19:
                        result = "RDRAM";
                        break;
                    case 20:
                        result = "DDR";
                        break;
                    case 21:
                        result = "DDR2";
                        break;
                    case 22:
                        result = "DDR2 FB-DIMM";
                        break;
                    case 24:
                        result = "DDR3";
                        break;
                    case 25:
                        result = "FBD2";
                        break;
                    default:
                        result = "Sin Definir";
                        break;
                }
                return result;
            }
            catch (Exception) {
                return defaultAnswer;
            }

        }

    }
}
