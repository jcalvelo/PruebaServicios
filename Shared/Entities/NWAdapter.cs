using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Serialization;

namespace Shared.Modelo
{
    [DataContract]
    public class NWAdapter {
        [DataMember]
        public string Name;
        [DataMember]
        public string Speed;
        [DataMember]
        public string DHCPEnabled;
        [DataMember]
        public string DHCPServerIP;
        [DataMember]
        public string DNSDomain;
        [DataMember]
        public List<string> DNSServers;
        [DataMember]
        public List<string> IPAddress;
        [DataMember]
        public List<string> Subnet;
        [DataMember]
        public string MACAddress;

        private const string defaultAnswer = "Sin Definir";


        public NWAdapter(string mac, ManagementObjectCollection wmiquery, ManagementObjectCollection wmiqueryconfig) {
            MACAddress = mac;
            Name = GetNWAdapterName(mac, wmiquery);
            Speed = GetNWAdapterSpeed(mac, wmiquery);
            DHCPEnabled = GetNWAdapterDHCPEnabled(mac, wmiqueryconfig);
            DHCPServerIP = GetDHCPServerIP(mac, wmiqueryconfig);
            DNSDomain = GetDNSDomain(mac, wmiqueryconfig);
            DNSServers = GetDNSServers(mac, wmiqueryconfig).ToList();
            IPAddress = GetIPAddress(mac, wmiqueryconfig).Reverse().ToList();
            Subnet = GetIPSubnet(mac, wmiqueryconfig).Reverse().ToList();
        }


        public static ManagementObjectCollection GetWMINWAdapterSearch() {
            ManagementObjectSearcher result = new ManagementObjectSearcher("root\\CIMV2", "SELECT MACAddress, ProductName, Speed FROM Win32_NetworkAdapter WHERE PhysicalAdapter = True");
            return result.Get();
        }

        public static ManagementObjectCollection GetWMINWAdapterConfigSearch() {
            ManagementObjectSearcher result = new ManagementObjectSearcher("root\\CIMV2", "SELECT MACAddress, DHCPEnabled, DHCPServer, DNSDomain, DNSServerSearchOrder, IPSubnet, IPAddress FROM Win32_NetworkAdapterConfiguration");
            return result.Get();
        }

        public static List<NWAdapter> GetNWAdapterList() {
            List<string> NWAMacList = new List<string>();
            var wmiquery = GetWMINWAdapterSearch();
            var wmiqueryconfig = GetWMINWAdapterConfigSearch();
            foreach (ManagementObject queryObj in wmiquery) {
                try {
                    NWAMacList.Add((queryObj["MACAddress"]).ToString());
                }
                catch (Exception) {
                }
            }

            List<NWAdapter> result = new List<NWAdapter>();
            foreach(var i in NWAMacList) {
                result.Add(new NWAdapter(i, wmiquery, wmiqueryconfig));
            }

            return result;

        }

        public static string GetNWAdapterName(string adapter, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    try {
                        if (queryObj["MACAddress"].ToString() == adapter) {
                            result = (queryObj["ProductName"]).ToString();
                        }
                    }
                    catch (Exception) {
                    }
                }
                return result;
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static string GetNWAdapterSpeed(string adapter, ManagementObjectCollection wmiquery) {
            int result = 0;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    try {
                        if (queryObj["MACAddress"].ToString() == adapter) {
                            var asd = ((queryObj["Speed"])).ToString();
                            result = Convert.ToInt32((queryObj["Speed"]));
                        }
                    }
                    catch (Exception) {
                    }
                }
                if (result > 0)
                    result = result / (1000 * 1000);
                if (result < 1000)
                    return (result.ToString()) + "Mbps";
                else
                    return (result / 1000).ToString() + "Gbps";

            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static string GetNWAdapterDHCPEnabled(string adapter, ManagementObjectCollection wmiquery) {
            bool result = false;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    try {
                        if (queryObj["MACAddress"].ToString() == adapter) {
                            result = Convert.ToBoolean(queryObj["DHCPEnabled"]);
                        }
                    }
                    catch (Exception) {
                    }
                }
                if (result)
                    return "SI";
                else
                    return "NO";
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static string GetDHCPServerIP(string adapter, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    try {
                        if (queryObj["MACAddress"].ToString() == adapter) {
                            result = (queryObj["DHCPServer"]).ToString();
                        }
                    }
                    catch (Exception) {
                    }
                }
                return result;
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static string GetDNSDomain(string adapter, ManagementObjectCollection wmiquery) {
            string result = null;
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    try {
                        if (queryObj["MACAddress"].ToString() == adapter) {
                            result = (queryObj["DNSDomain"]).ToString();
                        }
                    }
                    catch (Exception) {
                    }
                }
                return result;
            }
            catch (Exception) {
                return defaultAnswer;
            }
        }

        public static Stack<string> GetDNSServers(string adapter, ManagementObjectCollection wmiquery) {
            Stack<string> result = new Stack<string>();
            string[] auxarray = new string[2];
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    try {
                        if (queryObj["MACAddress"].ToString() == adapter) {
                            auxarray = (string[])(queryObj["DNSServerSearchOrder"]);
                        }
                    }
                    catch (Exception) {
                    }
                }
                foreach (String arrValue in auxarray) {
                    result.Push(arrValue);
                }
                return result;
            }
            catch (Exception) {
                result.Push(defaultAnswer);
                return result;
            }
        }

        public static Stack<string> GetIPAddress(string adapter, ManagementObjectCollection wmiquery) {
            Stack<string> result = new Stack<string>();
            string[] auxarray = new string[2];
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    try {
                        if (queryObj["MACAddress"].ToString() == adapter) {
                            auxarray = (String[])(queryObj["IPAddress"]);
                        }
                    }
                    catch (Exception) {
                    }
                }
                foreach (String arrValue in auxarray) {
                    result.Push(arrValue);
                }
                return result;

            }
            catch (Exception) {
                result.Push(defaultAnswer);
                return result;
            }
        }

        public static Stack<string> GetIPSubnet(string adapter, ManagementObjectCollection wmiquery) {
            Stack<string> result = new Stack<string>();
            string[] auxarray = new string[2];
            try {
                foreach (ManagementObject queryObj in wmiquery) {
                    try {
                        if (queryObj["MACAddress"].ToString() == adapter) {
                            auxarray = (string[])(queryObj["IPSubnet"]);
                        }
                    }
                    catch (Exception) {
                    }
                }
                foreach (String arrValue in auxarray) {
                    result.Push(arrValue);
                }
                return result;

            }
            catch (Exception) {
                result.Push(defaultAnswer);
                return result;
            }
        }

    }
}
