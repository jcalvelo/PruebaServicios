//using Client.ServiceGatherData;
using Shared.Modelo;
using System;
using Shared.Components;

namespace Client
{
    class DataGathering {

        public static Computer GatherData() {
            Computer comp = new Computer();
            comp.Bios = new Bios();
            comp.Compsys = new CompSys();
            comp.DDList = DiskDrives.GetDiskList();
            comp.Mobo = new MoBo();
            comp.NWAdapterList = NWAdapter.GetNWAdapterList();
            comp.Os = new OS();
            comp.ProcessorList = Processor.GetCPUList();
            comp.RAMList = RAM.GetRAMList();
            comp.Soft = Software.getSoftwareList();
            comp.VideoCard = new VCard();
            comp.ComputerName = Environment.MachineName;
            comp.TotalRam = RAM.GetTotalRam();
            comp.ActiveUser = LoggedUser.GetLoggedUser();

            return comp;
        }

        
    }
}




