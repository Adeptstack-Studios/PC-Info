using System.IO;

namespace PC_Component_Info
{
    class DriveReader
    {

        public string name_vl;
        public string dt;
        public string df;
        public float ts;
        public float tfs;


        public void Drive_Read(string drive)
        {
            DriveInfo di_1 = new DriveInfo(drive);

            if (di_1.IsReady)
            {
                Vars.ready_devices.Add(drive);
            }
        }

        public void drive_info(string drive)
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
