using csharpmatic.Generic;
using csharpmatic.Interfaces;
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
        Dictionary<Type, Dictionary<string, PropertyInfo>> supportedIntefraces;

        static void Main(string[] args)
        {
            Generator g = new Generator();
            g.Generate();
        }

        public void Generate()
        {
            DeviceFactory x = new DeviceFactory();

            string generatedDir = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", @"csharpmatic\Interfaces\Devices");
            
            DeviceManager dm = new DeviceManager(Settings.Default.ServerAddress);

            dm.Refresh();                                
            
            HashSet<string> generatedTypes = new HashSet<string>();

            foreach (var d in dm.Devices)
            {
                string deviceClass = d.DeviceType.Replace("-", "_").ToUpper();

                if (generatedTypes.Contains(deviceClass))
                    continue;

                generatedTypes.Add(deviceClass);

                string deviceFile = Path.Combine(generatedDir, deviceClass + ".cs");

                if (File.Exists(deviceFile))
                    continue;
                
                string generatedDeviceFile = Path.Combine(generatedDir, deviceClass + ".generated.cs");
               
                supportedIntefraces = GetSupportedInterefaces(d);

                List<string> inhericanceList = supportedIntefraces.Keys.Select(s => s.Name).ToList();
                inhericanceList.Insert(0, "Device");

                using (TextWriter tw = new StreamWriter(generatedDeviceFile))
                {
                    tw.WriteLine("/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */");
                    tw.WriteLine();
                    tw.WriteLine("using csharpmatic.Generic;");
                    tw.WriteLine("using csharpmatic.Interfaces;");
                    tw.WriteLine("using System;");
                    tw.WriteLine("using System.Collections.Generic;");
                    tw.WriteLine("using System.IO;");
                    tw.WriteLine();

                    tw.WriteLine("namespace csharpmatic.Interfaces.Devices");
                    tw.WriteLine("{");
                    tw.WriteLine("  public partial class {0} : {1}", deviceClass, String.Join(", ", inhericanceList));
                    tw.WriteLine("  {");


                    foreach (var dp in d.GetDatapoints())
                    {
                        tw.WriteLine("\t\t{0}", ToCSharpPropertyTemplate(dp));
                        tw.WriteLine();
                    }                    

                    tw.WriteLine();
                    tw.WriteLine("      public {0}(XMLAPI.DeviceList.Device d, XMLAPI.Client CGIClient, DeviceManager dm) : base(d, CGIClient, dm)", deviceClass);
                    tw.WriteLine("      {");
                    
                    foreach (var dp in d.GetDatapoints())
                    {
                        tw.WriteLine("\t\t\t{0}", ToCSharpPropertyInitTemplate(dp));
                        tw.WriteLine();
                    }

                    tw.WriteLine("      }");
                    tw.WriteLine("  }");
                    tw.WriteLine("}");

                    //break;
                }
            }
        }

        public Dictionary<Type, Dictionary<string, PropertyInfo>> GetSupportedInterefaces(Device d)
        {
            string[] allInterfaces = DeviceFactory.AllSupportedIntefraces.Keys.Select(s => s.AssemblyQualifiedName).ToArray();

            Type t2 = typeof(csharpmatic.Interfaces.IHmIPDevice);

            var datapoints = d.Channels.SelectMany(s => s.Datapoints.Values).Select(dp => dp.GetInterfacePropertyName()).ToList();

            var supported = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

            foreach (var name in allInterfaces)
            {
                Type t = Type.GetType(name);
                var props = t.GetRuntimeProperties();
                
                if (props.Count() > 0 && props.All(p => datapoints.Contains(p.Name)))
                    supported.Add(t, props.ToDictionary(dict => dict.Name));                
            }

            return supported;
        }

        public string ToCSharpPropertyTemplate(Datapoint dp)
        {
            string propname = dp.GetInterfacePropertyName();
            string csharpdatatype = GetCsharpDataType(dp);
            
            return String.Format("public TypedDatapoint<{0}> {1} {{ get; private set; }}", csharpdatatype, propname);
        }

        public string GetCsharpDataType(Datapoint dp)
        {
            string propname = dp.GetInterfacePropertyName();
            string csharpdatatype = dp.ValueType.Name;

            foreach (var tkvp in supportedIntefraces)
            {
                if (tkvp.Value.ContainsKey(propname))
                {
                    string newType = tkvp.Value[propname].PropertyType.GenericTypeArguments[0].ToString();
                    csharpdatatype = newType;
                }
            }

            return csharpdatatype;
        }

        public string ToCSharpPropertyInitTemplate(Datapoint dp)
        {
            string propname = dp.GetInterfacePropertyName();
            string csharpdatatype = GetCsharpDataType(dp);

            return String.Format("{0} = new TypedDatapoint<{1}>(base.Channels[{2}].Datapoints[\"{3}\"]);", 
                propname, csharpdatatype, dp.GetChannel().ChannelIndex, dp.Type);
        }

    }
}
