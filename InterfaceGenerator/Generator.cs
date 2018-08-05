using csharpmatic.XMLAPI.Generic;
using csharpmatic.XMLAPI.Interfaces;
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
            InterfaceFactory x = new InterfaceFactory();

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
                supportedIntefraces = GetSupportedInterefaces(d);

                List<string> inhericanceList = supportedIntefraces.Keys.Select(s => s.Name).ToList();
                inhericanceList.Insert(0, "Device");

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
                    tw.WriteLine("  public partial class {0} : {1}", deviceClass, String.Join(", ", inhericanceList));
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
                    tw.WriteLine("      public {0}(CGI.DeviceList.Device d, CGI.CGIClient CGIClient) : base(d, CGIClient)", deviceClass);
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

                    //break;
                }
            }
        }

        public Dictionary<Type, Dictionary<string, PropertyInfo>> GetSupportedInterefaces(Device d)
        {
            string[] allInterfaces = { "IHmIP", "IHumidityControl", "ILightControl", "ITempControl", "IValveControl" };

            Type t2 = typeof(csharpmatic.XMLAPI.Interfaces.IHmIP);

            var datapoints = d.Channels.SelectMany(s => s.Datapoints.Values).Select(dp => dp.GetInterfacePropertyName()).ToList();

            var supported = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

            foreach (var name in allInterfaces)
            {
                Type t = Type.GetType("csharpmatic.XMLAPI.Interfaces." + name + ", csharpmatic");
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
                propname, csharpdatatype, dp.Channel.ChannelIndex, dp.Type);
        }

    }
}
