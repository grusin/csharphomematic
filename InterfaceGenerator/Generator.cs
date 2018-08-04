using csharpmatic.XMLAPI.Generic;
using InterfaceGenerator.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGenerator
{
    class Generator
    {
        static void Main(string[] args)
        {
            string generatedDir = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", @"csharpmatic\XMLAPI\Interfaces\Generated");
            
            DeviceManager dm = new DeviceManager(Settings.Default.ServerAddress);

            dm.Refresh();

            HashSet<string> generatedTypes = new HashSet<string>();

            foreach(var d in dm.Devices)
            {
                string deviceClass = d.DeviceType.Replace("-", "_").ToUpper();

                if (generatedTypes.Contains(deviceClass))
                    continue;

                generatedTypes.Add(deviceClass);

                string deviceFile = Path.Combine(generatedDir, deviceClass + ".generated.cs");

                using (TextWriter tw = new StreamWriter(deviceFile))
                {

                }
            }
        }
    }
}
