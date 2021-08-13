using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IceTestTask
{
    public static class Constants
    {
        //Etc
        public static string GetDestinationFolder(string fileName) => $"{Directory.GetCurrentDirectory()}\\downloads\\{fileName}";

        //Amazon
        public static string GetBucketName(string date) => $"noaa-gfs-bdp-pds/gfs.{date}/00/atmos";
        //public static string BucketName = "noaa-gfs-bdp-pds/enkfgdas.20210810/00/atmos/mem001";
        public static string AccessKey = "AKIA2PHROIFXAMABFET7";
        public static string SecretKey = "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";

        //Date
        public static string DateFormat = "yyyy/MM/dd HH:mm";
        public static string DatePadding = "000.##";
    }
}
