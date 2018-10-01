using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Shared.Modelo
{
    [DataContract]
    public class Software {
        [DataMember]
        public string Name;


        public Software(string nombre) {
            Name = nombre;
        }


        public static List<Software> getSoftwareList() {
            string registry_key = @"Installer\Products";
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(registry_key);
            List<Software> result = new List<Software>();
            foreach (string subkey_name in key.GetSubKeyNames()) {
                RegistryKey subkey = key.OpenSubKey(subkey_name);
                try {
                    if (!result.Exists(x => x.Name == subkey.GetValue("ProductName").ToString())) {
                        Software aux = new Software(subkey.GetValue("ProductName").ToString());
                        result.Add(aux);
                    }
                }
                catch (Exception) {
                }
            }

            registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            key = Registry.LocalMachine.OpenSubKey(registry_key);
            foreach (string subkey_name in key.GetSubKeyNames()) {
                RegistryKey subkey = key.OpenSubKey(subkey_name);
                try {
                    if (!result.Exists(x => x.Name == subkey.GetValue("DisplayName").ToString())) {
                        Software aux = new Software(subkey.GetValue("DisplayName").ToString());
                        result.Add(aux);
                    }
                }
                catch (Exception) {
                }
            }
            if (Environment.Is64BitOperatingSystem) {
                registry_key = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
                key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registry_key);
                foreach (string subkey_name in key.GetSubKeyNames()) {
                    RegistryKey subkey = key.OpenSubKey(subkey_name);
                    try {
                        if (!result.Exists(x => x.Name == subkey.GetValue("DisplayName").ToString())) {
                            Software aux = new Software(subkey.GetValue("DisplayName").ToString());
                            result.Add(aux);
                        }
                    }
                    catch (Exception) {
                    }
                }
            }

            result.OrderBy(x => x.Name);
            return result;
        }


        public static string getOfficeInstalled(List<Software> softlist) {
            string result = null;
            bool firstoffice = true;
            Software aux;

            aux = softlist.Find(x => x.Name == "Microsoft Office Prof");
            result = result + aux;

            firstoffice = (result.Length <= 1);


            aux = softlist.Find(x => x.Name == "Microsoft Office Hom");
            if (!firstoffice && aux != null) {
                result = result + " - ";
                result = result + aux;
            }
            else if (aux != null)
                result = result + aux;




            aux = softlist.Find(x => x.Name == "OpenOffice");
            if (!firstoffice && aux != null) {
                result = result + " - ";
                result = result + aux;
            }
            else if (aux != null)
                result = result + aux;


            aux = softlist.Find(x => x.Name == "LibreOffice");
            if (!firstoffice && aux != null) {
                result = result + " - ";
                result = result + aux;
            }
            else if (aux != null)
                result = result + aux;

            return result;
        }

    }
}
