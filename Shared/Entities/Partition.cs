using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management;
using System.Runtime.Serialization;

namespace Shared.Modelo
{
    [DataContract]
    public class Partition {
        [DataMember]
        public string UnitTag;
        [DataMember]
        public string Name;
        [DataMember]
        public string Capacity;
        [DataMember]
        public string FreeSpace;
        [DataMember]
        public string PartitionType;
        /*
         * Los valores de ValueOverride corresponden a:
         * [0] - UnitTag
         * [1] - Name
         * [2] - Capacity
         * [3] - PartitionType
         */
        [DataMember]
        public bool[] ValueOverride = new bool[4];


        private const string defaultAnswer = "Sin Definir";
        private const string defaultAnswerNum = "-1";


        public Partition(string index, ManagementObjectCollection wmiquery) {
            for (int i = 0; i < ValueOverride.Length; i++)
                ValueOverride[i] = false;
            UnitTag = index;
            Name = GetPartitionVolumeName(index, wmiquery);
            Capacity = GetPartitionSize(index, wmiquery);
            FreeSpace = GetPartitionFreeSpace(index, wmiquery);
            PartitionType = GetPartitionFS(index, wmiquery);
        }


        public static ManagementObjectCollection GetWMIPartSearch(string index) {
            ManagementObjectSearcher result = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID, VolumeName, FileSystem, FreeSpace, Size FROM Win32_LogicalDisk WHERE DeviceID ='" + index + "'");
            return result.Get();
        }


        public static List<Partition> GetPartitionList(string disk) {
            string physicaldrive = @"\\\\.\\PHYSICALDRIVE" + disk;
            Stack<string> diskidlist = new Stack<string>();
            List<string> disktopart = new List<string>();
            try {
                ManagementObjectSearcher searcher2 =
                     new ManagementObjectSearcher("root\\CIMV2",
                     "SELECT * FROM Win32_DiskDriveToDiskPartition");

                foreach (ManagementObject queryObj in searcher2.Get()) {

                    string auxAntecedent = queryObj["Antecedent"].ToString();

                    if (auxAntecedent.Contains(physicaldrive)) {
                        string auxDependent = queryObj["Dependent"].ToString();
                        var aux = auxDependent.LastIndexOf('=');
                        auxDependent = auxDependent.Substring(aux + 2);
                        auxDependent = auxDependent.Substring(0, auxDependent.Length - 1);
                        disktopart.Add(auxDependent);
                    }
                }
            }
            catch (Exception) { }

            try {
                ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher("root\\CIMV2",
                        "SELECT Antecedent, Dependent FROM Win32_LogicalDiskToPartition");

                foreach (var coso in disktopart) {

                    foreach (ManagementObject queryObj in searcher.Get()) {

                        string auxAntecedent = queryObj["Antecedent"].ToString();

                        if (auxAntecedent.Contains(coso)) {
                            string auxDependent = queryObj["Dependent"].ToString();
                            var aux1 = auxDependent.LastIndexOf('=');
                            auxDependent = auxDependent.Substring(aux1 + 2);
                            auxDependent = auxDependent.Substring(0, auxDependent.Length - 1);

                            diskidlist.Push(auxDependent);
                        }
                    }
                }

            }
            catch (ManagementException) { }

            

            List<Partition> result = new List<Partition>();

            foreach (var i in diskidlist) {
                var wmi = GetWMIPartSearch(i);
                result.Add(new Partition(i, wmi));
            }

            return result;

            /*Stack<string> result = new Stack<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID FROM Win32_LogicalDisk WHERE DriveType = 3");
            foreach (ManagementObject queryObj in searcher.Get()) {
                result.Push((queryObj["DeviceID"]).ToString());
            }
            return result;*/
        }

        public static string GetPartitionSize(string part, ManagementObjectCollection wmiquery) {
            NumberFormatInfo nfi = new NumberFormatInfo() {
                NumberDecimalSeparator = "."
            };
            decimal result = 0;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if ((queryObj["DeviceID"]).ToString() == part) {
                        result = Convert.ToInt64(queryObj["Size"]); 
                    }
                }
                if (result >= 1099511627776) {
                    result = result / (1024 * 1024 * 1024);
                    return Math.Round((result / 1024), 2).ToString(nfi) + "TB";
                }
                else if (result >= 1073741824)
                    return Math.Round((result / (1024 * 1024 * 1024)), 2).ToString(nfi) + "GB";
                else
                    return Math.Round((result / (1024 * 1024)), 2).ToString(nfi) + "MB";
            }
            catch (Exception) {
                return defaultAnswerNum;
            }
        }

        public static string GetPartitionFreeSpace(string part, ManagementObjectCollection wmiquery) {
            NumberFormatInfo nfi = new NumberFormatInfo() {
                NumberDecimalSeparator = "."
            };
            decimal result = 0;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if ((queryObj["DeviceID"]).ToString() == part) {
                        result = Convert.ToInt64(queryObj["FreeSpace"]); 
                    }
                }
                if (result >= 1099511627776) {
                    result = result / (1024 * 1024 * 1024);
                    return Math.Round((result / 1024), 2).ToString(nfi) + "TB";
                }
                else if (result >= 1073741824)
                    return Math.Round((result / (1024 * 1024 * 1024)), 2).ToString(nfi) + "GB";
                else
                    return Math.Round((result / (1024 * 1024)), 2).ToString(nfi) + "MB";
            }
            catch (Exception) {
                return defaultAnswerNum;
            }
        }

        public static string GetPartitionFS(string part, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if ((queryObj["DeviceID"]).ToString() == part) {
                        result = (queryObj["FileSystem"]).ToString(); 
                    }
                }
                return result.Trim();
            }
            catch (Exception) {
            }
            return defaultAnswer;
        }

        public static string GetPartitionVolumeName(string part, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if ((queryObj["DeviceID"]).ToString() == part) {
                        result = (queryObj["VolumeName"]).ToString();

                    }                }
                return result;
            }
            catch (Exception) {
            }
            return defaultAnswer;
        }
    }
}
