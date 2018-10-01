using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management;
using System.Runtime.Serialization;


namespace Shared.Modelo
{
    [KnownType(typeof(Partition))]
    [DataContract]
    public class DiskDrives {
        [DataMember]
        public string Modelo;
        [DataMember]
        public string SerialNumber;
        [DataMember]
        public string Size;
        [DataMember]
        public List<Partition> PartList;
        /*
         * Los valores de ValueOverride corresponden a:
         * [0] - Modelo
         * [1] - SerialNumber
         * [2] - Size
         */
        [DataMember]
        public bool[] ValueOverride = new bool[3];

        private const string defaultAnswer = "Sin Definir";
        private const string defaultAnswerNum = "-1";


        public DiskDrives(string index, ManagementObjectCollection wmiquery) {
            for (int i = 0; i < ValueOverride.Length; i++)
                ValueOverride[i] = false;
            
            Modelo = GetDiskModel(index, wmiquery);
            SerialNumber = GetDiskSN(index, wmiquery);
            Size = GetDiskSize(index, wmiquery);
            var auxPart = Partition.GetPartitionList(index);
            PartList = Partition.GetPartitionList(index);
        }
        
        
        public static ManagementObjectCollection GetWMIDiskSearch() {
            ManagementObjectSearcher result = new ManagementObjectSearcher("root\\CIMV2", "SELECT Index, Model, SerialNumber, Size FROM Win32_DiskDrive WHERE InterfaceType = 'IDE' OR InterfaceType = 'SCSI'");
            return result.Get();
        }

        public static List<DiskDrives> GetDiskList() {
            var wmi = GetWMIDiskSearch();

            List<DiskDrives> result = new List<DiskDrives>();

            Stack<string> indesxlist = new Stack<string>();
            foreach (ManagementObject queryObj in wmi) {
                indesxlist.Push((queryObj["Index"]).ToString());
            }

            foreach (var i in indesxlist) {
                result.Add(new DiskDrives(i, wmi));
            }
            return result;
        }

        public static string GetDiskModel(string disk, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if ((queryObj["Index"]).ToString() == disk) {
                        result = (queryObj["Model"]).ToString(); 
                    }
                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static string GetDiskSN(string disk, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if ((queryObj["Index"]).ToString() == disk) {
                        result = (queryObj["SerialNumber"]).ToString();

                    }                }
                return result.Trim();
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static string GetDiskSize(string disk, ManagementObjectCollection wmiquery) {
            NumberFormatInfo nfi = new NumberFormatInfo() {
                NumberDecimalSeparator = "."
            };
            decimal result = 0;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    if ((queryObj["Index"]).ToString() == disk) {
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

    }
}
