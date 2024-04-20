using System.IO;

namespace PC_Component_Info.Utilities
{
    class DriveReader
    {

        public string name_vl;
        public string dt;
        public string df;
        public float ts;
        public float tfs;


        public void DriveRead(string drive)
        {
            DriveInfo di_1 = new DriveInfo(drive);

            if (di_1.IsReady)
            {
                string icon = "";

                switch (di_1.DriveType)
                {
                    case DriveType.Unknown:
                        icon = "../Images/unknown.png";
                        break;
                    case DriveType.NoRootDirectory:
                        icon = "../Images/prohibited.png";
                        break;
                    case DriveType.Removable:
                        icon = "../Images/usb.png";
                        break;
                    case DriveType.Fixed:
                        icon = "../Images/hard-disk.png";
                        break;
                    case DriveType.Network:
                        icon = "../Images/cloud.png";
                        break;
                    case DriveType.CDRom:
                        icon = "../Images/disk.png";
                        break;
                    case DriveType.Ram:
                        icon = "../Images/ram-memory.png";
                        break;
                    default:
                        break;
                }

                Vars.ready_devices.Add(new ListContexts.Drives
                {
                    Name = $"{di_1.VolumeLabel} ({di_1.Name})",
                    VolumeLabel = di_1.VolumeLabel,
                    DriveLetter = di_1.Name,
                    Icon = icon
                });
            }
        }

        public void DriveInfo(string drive)
        {
            DriveInfo di = new DriveInfo(drive);

            name_vl = di.VolumeLabel + " (" + di.Name + ")";
            df = di.DriveFormat;
            dt = di.DriveType.ToString();
            ts = di.TotalSize / 1073741824;
            tfs = di.TotalFreeSpace / 1073741824;
        }

    }
}
