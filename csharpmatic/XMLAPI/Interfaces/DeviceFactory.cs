using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Interfaces
{
    public class DeviceFactory
    {
        public static Dictionary<Type, Dictionary<string, PropertyInfo>> AllSupportedIntefraces { get; private set; }
        public static Dictionary<Type, Dictionary<string, Type>> AllSupportedDevices { get; private set; }

        static DeviceFactory()
        {
            initAllSupportedIntefraces();
            initAllSupportedDevices();
        }

        static void initAllSupportedIntefraces()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(w => w.IsInterface && w.Namespace == "csharpmatic.XMLAPI.Interfaces");

            AllSupportedIntefraces = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

            foreach (var t in types)
            {
                var props = t.GetRuntimeProperties();

                if (props.Count() > 0)
                    AllSupportedIntefraces.Add(t, props.ToDictionary(dict => dict.Name));
            }
        }

        static void initAllSupportedDevices()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(w => w.Namespace == "csharpmatic.XMLAPI.Interfaces.Devices");

            AllSupportedDevices = new Dictionary<Type, Dictionary<string, Type>>();

            foreach (var t in types)
            {
                var hmInterfaces = t.GetInterfaces().Where(w => w.IsInterface && w.Namespace == "csharpmatic.XMLAPI.Interfaces");

                AllSupportedDevices.Add(t, hmInterfaces.ToDictionary(ks => ks.Name));
            }
        }

        public static Device CreateInstance(CGI.DeviceList.Device d, CGI.CGIClient CGIClient, DeviceManager dm)
        {
            Type t = GetInterfaceDeviceType(d);

            Device newDev = Activator.CreateInstance(t, d, CGIClient, dm) as Device;

            return newDev;
        }


        public static Type GetInterfaceDeviceType(CGI.DeviceList.Device d)
        {
            return GetInterfaceDeviceType(d.Device_type);
        }
        public static Type GetInterfaceDeviceType(Device d)
        {
            return GetInterfaceDeviceType(d.DeviceType);
        }

        public static Type GetInterfaceDeviceType(string device_type)
        {
            string className = device_type.Replace("-", "_").ToUpper();

            Type t = Type.GetType("csharpmatic.XMLAPI.Interfaces.Devices." + className + ", csharpmatic");

            //return Device in case no better match was found
            if (t == null)
                return typeof(Device);

            return t;
        }
    }
}
