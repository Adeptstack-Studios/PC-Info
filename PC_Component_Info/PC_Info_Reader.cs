using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Management;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic;
using System.Threading;

namespace PC_Component_Info
{
    class PC_Info_Reader
    {
        void startreading()
        {
            SystemInfo si = new SystemInfo();       //Create an object of SystemInfo class.
            si.getGeneralSystemInfo();            //Call get operating system info method which will display operating system information.
            si.getProcessorInfo();                  //Call get  processor info method which will display processor info.
            Console.ReadLine();                     //Wait for user to accept input key.
        }
    }

    public class SystemInfo
    {

        public void getGeneralSystemInfo()
        {
            //Create an object of ManagementObjectSearcher class and pass query as parameter.
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject managementObject in mos.Get())
            {
                if (managementObject["Caption"] != null)
                {
                    Vars.OSV = managementObject["Caption"].ToString(); //Display operating system caption
                }
                if (managementObject["OSArchitecture"] != null)
                {
                    Vars.OSA = managementObject["OSArchitecture"].ToString(); //Display operating system architecture.
                }
            }

            ComputerInfo ci = new ComputerInfo();

            //Console.WriteLine(ci.InstalledUICulture.DisplayName);
            // Console.WriteLine(ci.InstalledUICulture.Name);
            //Console.WriteLine(ci.OSVersion.ToString());
            Vars.osp =  ci.OSPlatform.ToString(); //Wird benutzt
            //Console.WriteLine(ci.OSFullName.ToString());

            Vars.pc_name = "Benutzername: " + Environment.UserName; // User name of PC
            Vars.user_name = "PC Name: " + Environment.MachineName;// Machine name
        }

        //CPU Variablen
        public void getProcessorInfo()
        {

            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    try
                    {
                        Vars.CPU_Name = processor_name.GetValue("ProcessorNameString").ToString();   //Display processor ingo.


                        int prozessoranzahl = Environment.ProcessorCount;
                        Vars.CPU_Threads = prozessoranzahl;

                        foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
                        {
                            Vars.cpu_cores = item["NumberOfCores"].ToString();
                        }

                        //PerformanceCounter pc_Cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");

                        //Console.WriteLine(pc_Cpu.NextValue());

                        ManagementObject Mo = new ManagementObject("Win32_Processor.DeviceID='CPU0'");
                        uint sp = (uint)(Mo["CurrentClockSpeed"]);
                        Mo.Dispose();

                        Vars.cpu_speed = sp / 1000d;

                        /*ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher("root\\WMI",
                        "SELECT * FROM MSAcpi_ThermalZoneTemperature");

                        foreach (ManagementObject queryObj in searcher.Get())
                        {
                            Console.WriteLine("-----------------------------------");
                            Console.WriteLine("MSAcpi_ThermalZoneTemperature instance");
                            Console.WriteLine("-----------------------------------");
                            Console.WriteLine("CurrentTemperature: {0}", queryObj["CurrentTemperature"]);
                        }*/
                        Console.WriteLine("GetCPUINFO aufgerufen!");
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "CPU");
                    }
                }
            }
        }

        //Ram Variablen
        public void getRamInfo()
        {

            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            ManagementObjectCollection oCollection = oSearcher.Get();

            long MemSize = 0;
            long mCap = 0;

            // In case more than one Memory sticks are installed
            foreach (ManagementObject obj in oCollection)
            {
                mCap = Convert.ToInt64(obj["Capacity"]);
                MemSize += mCap;
            }
            Vars.installed_ram = MemSize / 1073741824;

            Computer c = new Computer();

            //if (c.Keyboard.ShiftKeyDown) { }

            ComputerInfo ci = new ComputerInfo();

            Vars.free_ram = ci.AvailablePhysicalMemory / 1073741824f;
            
            Vars.total_ram = ci.TotalPhysicalMemory / 1073741824f;

            Vars.ram_in_use = Vars.total_ram - Vars.free_ram;

            Vars.hardware_ram = Vars.installed_ram - Vars.total_ram;

        }

        //Others
        public void getOthersInfo()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_DisplayControllerConfiguration");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("-----------------------------------    ");
                    Console.WriteLine("Win32_DisplayControllerConfiguration instance");
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Name: {0}", queryObj["Name"]);
                    Vars.GPU_Name = "Name: " + queryObj["Name"].ToString();
                }

                ManagementClass c = new ManagementClass("Win32_VideoController");
                foreach (ManagementObject o in c.GetInstances())
                {
                    string gpuTotalMem = String.Format("{0} ", o["AdapterRam"]);
                    long vram = Convert.ToInt64(gpuTotalMem);
                    Console.WriteLine(vram / 1073741824 + "Gb");
                    Vars.GPU_VRAM = vram / 1073741824;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
            }
        }

    }
}
