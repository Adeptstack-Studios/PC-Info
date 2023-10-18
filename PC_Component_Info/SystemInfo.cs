using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using System;
using System.Management;

namespace PC_Component_Info
{
    public class SystemInfo
    {

        public (string userName, string machineName) GetNames()
        {
            return (Environment.UserName, Environment.MachineName);
        }

        public string GetOSInfo()
        {
            string osa = "";
            string osv = "";

            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject managementObject in mos.Get())
            {
                if (managementObject["Caption"] != null)
                {
                    osv = managementObject["Caption"].ToString();
                }
                if (managementObject["OSArchitecture"] != null)
                {
                    osa = managementObject["OSArchitecture"].ToString();
                }
            }

            return $"{osv} ({osa})";
        }

        public string GetOSPlatform()
        {
            ComputerInfo ci = new ComputerInfo();
            return ci.OSPlatform.ToString();
        }

        public (string name, string architecture) GetProcessorName()
        {
            string cpu = "";
            string architecture = "";
            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    try
                    {
                        cpu = processor_name.GetValue("ProcessorNameString").ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "CPU");
                    }
                }

                foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
                {
                    architecture = GetProcessorArchitecture(int.Parse(item["Architecture"].ToString()));
                }
            }
            return (cpu, architecture);
        }

        string GetProcessorArchitecture(int i)
        {
            switch (i)
            {
                case 0:
                    return "x86";
                case 1:
                    return "MIPS";
                case 2:
                    return "Alpha";
                case 3:
                    return "PowerPC";
                case 5:
                    return "ARM";
                case 6:
                    return "ia64";
                case 9:
                    return "x64";
                case 12:
                    return "ARM64";
                default: return "unknown";
            }
        }

        public (int threads, int cores) GetCoresAndThreads()
        {
            int threads = Environment.ProcessorCount;
            int cores = 0;

            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                cores = int.Parse(item["NumberOfCores"].ToString());
            }

            return (threads, cores);
        }

        public (int l2cache, int l3cahce, int maxCache) GetCacheSize()
        {
            int l2 = 0;
            int l3 = 0;
            int max = 0;

            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                l3 = int.Parse(item["L3CacheSize"].ToString());
                l2 = int.Parse(item["L2CacheSize"].ToString());
            }

            ManagementClass mgmtc = new ManagementClass("Win32_CacheMemory");
            foreach (ManagementObject o in mgmtc.GetInstances())
            {
                max = int.Parse(o["MaxCacheSize"].ToString());
            }

            return (l2, l3, max);
        }

        public (int maxSpeed, int currentSpeed) GetCPUClockSpeed()
        {
            int max = 0;
            int current = 0;

            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                current = int.Parse(item["CurrentClockSpeed"].ToString()) / 1000;
                max = int.Parse(item["MaxClockSpeed"].ToString()) / 1000;
            }

            return (max, current);
        }

        public long GetInstalledRam()
        {
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            ManagementObjectCollection oCollection = oSearcher.Get();
            long MemSize = 0;

            foreach (ManagementObject obj in oCollection)
            {
                long mCap = Convert.ToInt64(obj["Capacity"]);
                MemSize += mCap;
            }
            return MemSize / 1073741824;
        }

        public double GetTotalUsableRam()
        {
            ComputerInfo ci = new ComputerInfo();
            return Math.Round(ci.TotalPhysicalMemory / 1073741824f, 3);
        }

        public double GetAvailableRam()
        {
            ComputerInfo ci = new ComputerInfo();
            return Math.Round(ci.AvailablePhysicalMemory / 1073741824f, 3);
        }

        public double GetUsedRam()
        {
            return Math.Round(GetTotalUsableRam() - GetAvailableRam(), 3);
        }

        public double GetHardwareReservedRam()
        {
            return Math.Round(GetInstalledRam() - GetTotalUsableRam(), 3);
        }

        public (string manufacturer, string frequency, string voltage) GetRamInfo()
        {
            string manufacturer = "";
            string freq = "";
            string voltage = "";

            ConnectionOptions connection = new ConnectionOptions();
            connection.Impersonation = ImpersonationLevel.Impersonate;
            ManagementScope scope = new ManagementScope("\\\\.\\root\\CIMV2", connection);
            scope.Connect();
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_PhysicalMemory"); ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query); foreach (ManagementObject queryObj in searcher.Get())
            {
                manufacturer = queryObj["Manufacturer"].ToString();
                freq = queryObj["ConfiguredClockSpeed"] + " MHz";
                voltage = queryObj["ConfiguredVoltage"] + "V";
            }

            return (manufacturer, freq, voltage);
        }

        public (string manufacturer, string model) Getmainboard()
        {
            string manufacturer = "";
            string model = "";

            ManagementClass mgc = new ManagementClass("Win32_BaseBoard");
            foreach (ManagementObject o in mgc.GetInstances())
            {
                manufacturer = o["Manufacturer"].ToString();
                model = o["Product"].ToString();
            }

            return (manufacturer, model);
        }

        public (string manufacturer, string versionName, string version) GetBIOSInfo()
        {
            string manufacturer = "";
            string versionName = "";
            string version = "";

            ManagementClass mc = new ManagementClass("Win32_BIOS");
            foreach (ManagementObject o in mc.GetInstances())
            {
                versionName = o["Name"].ToString();
                version = o["SystemBiosMajorVersion"].ToString() + "." + o["SystemBiosMinorVersion"].ToString();
                manufacturer = o["Manufacturer"].ToString();
            }

            return (manufacturer, versionName, version);
        }

        public (long vram, string name, string DriverVersion) GetGraphicscardInfo()
        {
            string name = "";
            string driver = "";
            long vram = 0;

            try
            {
                ManagementClass c = new ManagementClass("Win32_VideoController");
                foreach (ManagementObject o in c.GetInstances())
                {
                    string gpuTotalMem = String.Format("{0} ", o["AdapterRam"]);
                    vram = Convert.ToInt64(gpuTotalMem);
                    vram = vram / 1073741824;
                    name = o["Name"].ToString();
                    driver = o["DriverVersion"].ToString();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
            }

            return (vram, name, driver);
        }

    }
}