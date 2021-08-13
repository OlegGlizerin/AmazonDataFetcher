using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Services
{
    public class WGribService : IWGribService
    {
        public string CalcKelvins(string fileName)
        {
            var res = LaunchWGribCli(fileName);
            return res;
        }

        private string LaunchWGribCli(string fileName)
        {
            string result = "";
            var kelvins = "";

            var originalDirectory = Directory.GetCurrentDirectory();
            var fullDirectory = $"{originalDirectory}\\AmazonWeatherApplication\\wgrib2";
            Console.WriteLine($"Start LaunchWGribCli -> originalDirectory: {fullDirectory}");
            Directory.SetCurrentDirectory(fullDirectory);
            Process myProcess = new Process();

            myProcess.StartInfo.FileName = $"wgrib2.exe";
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.RedirectStandardInput = true;
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.StartInfo.Arguments = "../downloads/" + fileName + " -match ':(TMP:2 m above ground):' -lon 34.855 32.109";
            myProcess.Start();

            StreamReader sOut = myProcess.StandardOutput;
            result = sOut.ReadToEnd();

            if (!myProcess.HasExited)
            {
                myProcess.Kill();
            }

            sOut.Close();
            myProcess.Close();

            Directory.SetCurrentDirectory(originalDirectory);

            if(!string.IsNullOrEmpty(result))
            {
                kelvins = result.Split(",")[2].Split("=")[1].Replace("\n", "");
            }

            Console.WriteLine($"Finish LaunchWGribCli -> kelvins: {kelvins}");
            return kelvins;
        }
    }
}
