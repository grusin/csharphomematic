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


            string[] interfaces = { "IHmIP", "IHumidityControl", "ILightControl", "ITempControl", "IValveControl" };

            Type t2 = typeof(csharpmatic.XMLAPI.Interfaces.IHmIP);


            foreach(var name in interfaces)
            {
                Type t = Type.GetType("csharpmatic.XMLAPI.Interfaces." + name + ", csharpmatic");
                var props = t.GetRuntimeProperties();


            }
            

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
                        foreach(var dp in c.Datapoints.Values)
                        {
                            tw.WriteLine("\t\t{0}", ToCSharpPropertyTemplate(dp));
                            tw.WriteLine();
                        }
                    }

                    tw.WriteLine();
                    tw.WriteLine("      public {0}()", deviceClass);
                    tw.WriteLine("      {");

                    foreach (var c in d.Channels)
                    {
                        foreach (var dp in c.Datapoints.Values)
                        {
                            tw.WriteLine("\t\t\t{0}", ToCSharpPropertyInitTemplate(dp));
                            tw.WriteLine();
                        }
                    }

                    tw.WriteLine("      }");
                    tw.WriteLine("  }");
                    tw.WriteLine("}");

                }
            }
        }

        public static string ToCSharpPropertyTemplate(Datapoint dp)
        {
            string propname = dp.GetInterfacePropertyName();

            string csharpdatatype = dp.ValueType.Name;
            
            return String.Format("public TypedDatapoint<{0}> {1} {{ get; private set; }}", csharpdatatype, propname);
        }

        public static string ToCSharpPropertyInitTemplate(Datapoint dp)
        {
            string propname = dp.GetInterfacePropertyName();

            string csharpdatatype = dp.ValueType.Name;

            return String.Format("{0} = new TypedDatapoint<{1}>(base.Channels[{2}].Datapoints[\"{3}\"]);", 
                propname, csharpdatatype, dp.Channel.ChannelIndex, dp.Type);
        }

    }
}
