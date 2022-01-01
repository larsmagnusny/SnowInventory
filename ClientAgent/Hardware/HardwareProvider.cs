using ClientAgent.Utility;
using Hardware.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAgent.Hardware
{
    public static class HardwareProvider
    {
        static readonly IHardwareInfo hardwareInfo = new HardwareInfo(); 
        public static HardwareProfile GetProfile()
        {
            hardwareInfo.RefreshMotherboardList();
            hardwareInfo.RefreshBIOSList();
            hardwareInfo.RefreshCPUList();
            hardwareInfo.RefreshMemoryList();
            hardwareInfo.RefreshMemoryStatus();
            hardwareInfo.RefreshVideoControllerList();
            hardwareInfo.RefreshDriveList();

            var driveSizes = hardwareInfo.DriveList.Select(o => o.Size);
            ulong sumDriveSizes = 0;

            foreach (var size in driveSizes)
            {
                sumDriveSizes += size;
            }

            var cpuIds = hardwareInfo.CpuList.Select(o => o.ProcessorId);

            StringBuilder builder = new StringBuilder();

            builder.Append("CPU_IDS: ");

            Guid hardwareId;

            int counter = 0;
            foreach (var cpuId in cpuIds)
            {
                builder.Append(counter);
                builder.Append(": ");
                builder.Append(cpuId);
            }
            builder.Append(" Motherboard: ");
            builder.Append(hardwareInfo.MotherboardList.FirstOrDefault()?.SerialNumber);

            hardwareId = GuidUtility.Create(GuidUtility.UrlNamespace, builder.ToString());

            var bios = hardwareInfo.BiosList.FirstOrDefault();
            var motherboard = hardwareInfo.MotherboardList.FirstOrDefault();

            return new HardwareProfile
            {
                Id = hardwareId,
                ComputerName = Environment.MachineName,
                OSVersion = Environment.OSVersion.ToString(),
                BIOS = bios != null ? $"{bios.Name} {bios.Manufacturer} {bios.Version}" : null,
                Motherboard = motherboard != null ? $"{motherboard.Product} {motherboard.Manufacturer} {motherboard.SerialNumber}" : null,
                HDDSpace = (long)sumDriveSizes,
                Memory = (long)hardwareInfo.MemoryStatus.TotalPhysical
            };
        }
    }
}
