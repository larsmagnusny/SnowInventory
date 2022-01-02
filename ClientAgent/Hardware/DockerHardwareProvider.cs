using Inventory.Common;
using Inventory.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAgent.Hardware
{
    public class DockerHardwareProvider : IHardwareProvider
    {
        public HardwareProfile GetProfile()
        {
            var imageId = Environment.MachineName;

            var hardwareId = GuidUtility.Create(GuidUtility.UrlNamespace, imageId);

            

            return new HardwareProfile
            {
                Id = hardwareId,
                BIOS = "Docker Image",
                HDDSpace = 0,
                OSVersion = "Docker Image",
                ComputerName = imageId,
                Memory = 0,
                Motherboard = "Docker Image"
            };
        }
    }
}
