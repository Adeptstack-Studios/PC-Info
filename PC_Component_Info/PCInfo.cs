namespace PC_Component_Info
{
    public class PCInfo
    {
        public static string GetOSPlatform()
        {
            //ComputerInfo ci = new ComputerInfo();
            return "unknown"; // ci.OSPlatform.ToString();
        }

        public static double GetTotalUsableRam()
        {
            //ComputerInfo ci = new ComputerInfo();
            return 1; // Math.Round(ci.TotalPhysicalMemory / 1073741824f, 3);
        }

        public static double GetHardwareReservedRam()
        {
            return 1; // Math.Round(RamInfo.GetInstalledRAMSize() - GetTotalUsableRam(), 3);
        }

        public static double GetAvailableRam()
        {
            //ComputerInfo ci = new ComputerInfo();
            return 1; // Math.Round(ci.AvailablePhysicalMemory / 1073741824f, 3);
        }

        public static double GetUsedRam()
        {
            return 1; // Math.Round(GetTotalUsableRam() - GetAvailableRam(), 3);
        }
    }
}