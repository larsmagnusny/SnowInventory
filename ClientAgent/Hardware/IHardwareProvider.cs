using Inventory.Common;

namespace ClientAgent.Hardware
{
    public interface IHardwareProvider
    {
        public HardwareProfile GetProfile();
    }
}
