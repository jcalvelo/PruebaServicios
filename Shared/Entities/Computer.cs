using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Shared.Modelo
{
    [KnownType(typeof(Bios))]
    [KnownType(typeof(CompSys))]
    [KnownType(typeof(DiskDrives))]
    [KnownType(typeof(MoBo))]
    [KnownType(typeof(NWAdapter))]
    [KnownType(typeof(OS))]
    [KnownType(typeof(Processor))]
    [KnownType(typeof(RAM))]
    [KnownType(typeof(Software))]
    [KnownType(typeof(VCard))]
    [KnownType(typeof(Partition))]
    [DataContract]
    public class Computer {

        [BsonId][DataMember]
        public int CompID;
        [DataMember]
        public Bios Bios;//
        [DataMember]
        public CompSys Compsys;//
        [DataMember]
        public List<DiskDrives> DDList;
        [DataMember]
        public MoBo Mobo; //
        [DataMember]
        public List<NWAdapter> NWAdapterList;
        [DataMember]
        public OS Os; //
        [DataMember]
        public List<Processor> ProcessorList; //
        [DataMember]
        public List<RAM> RAMList;
        [DataMember]
        public List<Software> Soft;
        [DataMember]
        public VCard VideoCard;
        [DataMember]
        public string ComputerName; //
        [DataMember]
        public string TotalRam; //
        [DataMember]
        public string ActiveUser;
        [DataMember]
        public DateTime ReportTime;

        public Computer() {
/*            Bios = new Bios();
            Compsys = new CompSys();

            var auxDD = DiskDrives.GetDiskList();
            DDList = new List<DiskDrives>();
            foreach (var dd in auxDD)
                DDList.Add(new DiskDrives(dd));

            Mobo = new MoBo();

            var auxNWA = NWAdapter.GetNWAdapterStack();
            NWAdapterList = new List<NWAdapter>();
            foreach (var nwa in auxNWA)
                NWAdapterList.Add(new NWAdapter(nwa));

            Os = new OS();

            var auxProc = Processor.GetCPUStack();
            ProcessorList = new List<Processor>();
            foreach (var proc in auxProc)
                ProcessorList.Add(new Processor(proc));

            var auxRAM = RAM.GetRamSlotList();
            RAMList = new List<RAM>();
            foreach (var ram in auxRAM)
                RAMList.Add(new RAM(ram));

            Soft = Software.getSoftwareList();

            VideoCard = new VCard();

            ComputerName = Environment.MachineName;

            TotalRam = RAM.GetTotalRam();*/
        }
    }
}
