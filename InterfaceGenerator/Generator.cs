using csharpmatic.XMLAPI.Generic;
using InterfaceGenerator.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            string generatedDir = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", @"csharpmatic\XMLAPI\Interfaces\Devices");
            
            DeviceManager dm = new DeviceManager(Settings.Default.ServerAddress);

            dm.Refresh();

            HashSet<string> generatedTypes = new HashSet<string>();

            foreach (var d in dm.Devices)
            {
                string deviceClass = d.DeviceType.Replace("-", "_").ToUpper();

                if (generatedTypes.Contains(deviceClass))
                    continue;

                generatedTypes.Add(deviceClass);

                string deviceFile = Path.Combine(generatedDir, deviceClass + ".generated.cs");

                using (TextWriter tw = new StreamWriter(deviceFile))
                {
                    tw.WriteLine("/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */");
                    tw.WriteLine();
                    tw.WriteLine("using csharpmatic.XMLAPI.Generic;");
                    tw.WriteLine("using System;");
                    tw.WriteLine("using System.Collections.Generic;");
                    tw.WriteLine("using System.IO;");
                    tw.WriteLine();

                    tw.WriteLine("namespace csharpmatic.XMLAPI.Interfaces.Devices");
                    tw.WriteLine("{");
                    tw.WriteLine("  public partial class {0} : Device", deviceClass);
                    tw.WriteLine("  {");

                    foreach(var c in d.Channels)
                    {
                        foreach(var dp in c.Datapoints)
                        {
                            tw.WriteLine("\t\t{0}", ToCSharpPropertyTemplate(dp.Value, c.ChannelIndex));
                            tw.WriteLine();
                        }
                    }

                    tw.WriteLine();
                    tw.WriteLine("      public {0}()", deviceClass);
                    tw.WriteLine("      {");

                    foreach (var c in d.Channels)
                    {
                        foreach (var dp in c.Datapoints)
                        {
                            tw.WriteLine("\t\t\t{0}", ToCSharpPropertyInitTemplate(dp.Value, c.ChannelIndex));
                            tw.WriteLine();
                        }
                    }

                    tw.WriteLine("      }");
                    tw.WriteLine("  }");
                    tw.WriteLine("}");

                }
            }
        }

        public static string ToCSharpPropertyTemplate(Datapoint dp, int channel_idx)
        {
            string propname = dp.Type.Replace("_", " ").ToLower();
            propname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(propname);
            propname = propname.Replace(" ", "_");
            propname = String.Format("{0}_C{1}", propname, channel_idx);

            string csharpdatatype = dp.ValueType.Name;
            
            return String.Format("public TypedDatapoint<{0}> {1} {{ get; private set; }}", csharpdatatype, propname);
        }

        public static string ToCSharpPropertyInitTemplate(Datapoint dp, int channel_idx)
        {
            string propname = dp.Type.Replace("_", " ").ToLower();
            propname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(propname);
            propname = propname.Replace(" ", "_");
            propname = String.Format("{0}_C{1}", propname, channel_idx);

            string csharpdatatype = dp.ValueType.Name;

            return String.Format("{0} = new TypedDatapoint<{1}>(base.Channels[{2}].Datapoints[\"{3}\"]);", 
                propname, csharpdatatype, channel_idx, dp.Type);
        }
    }
}
