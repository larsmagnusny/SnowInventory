using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common
{
    public class HardwareProfile
    {
        public Guid Id { get; set; }
        public string ComputerName { get; set; }
        public string OSVersion { get; set; }
        public string Motherboard { get; set; }
        public string BIOS { get; set; }
        public long Memory { get; set; }
        public long HDDSpace { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}\nComputerName: {ComputerName}\nOSVersion: {OSVersion}\nMotherboard: {Motherboard}\nBIOS: {BIOS}\nMemory: {Memory}\nHDDSpace: {HDDSpace}\n";
        }
    }
}
