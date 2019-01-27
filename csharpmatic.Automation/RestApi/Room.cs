using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csharpmatic.Generic;
using csharpmatic.Interfaces;

namespace csharpmatic.Automation.RestApi
{
    public class Room
    {
        public string Name { get; set; }
        public string ISEID { get; set; }
        public decimal? ActualTempAvg { get; set; }
        public decimal? ActualTempMin { get; set; }
        public decimal? ActualTempMax { get; set; }
        public decimal? SetTemp { get; set; }
        public decimal? HumidityAvg { get; set; }
        public decimal? HumidityMin { get; set; }
        public decimal? HumidityMax { get; set; }
        public decimal? ValveOpenAvg { get; set; }
        public decimal? ValveOpenMin { get; set; }
        public decimal? ValveOpenMax { get; set; }
        
        public bool DehumidifierActive { get; set; }
        public bool HeatingActive { get; set; }
        public bool WindowOpen { get; set; }
        public bool BoostActive { get; set; }
        public int BoostSecondsLeft { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public List<string> Warnings { get; set; } = new List<string>();

        public Room()
        {

        }

        public Room(csharpmatic.Generic.Room dr)
        {
            Name = dr.Name;
            ISEID = dr.ISEID;

            if (dr.SingleSwitchControlDevices != null)
            {
                //get heating actuators
                if (dr.SingleSwitchControlDevices.Where(w => w.Functions.Contains(Function.Heating) && w.State.Value).FirstOrDefault() != null)
                    HeatingActive = true;

                //get dehumidyfier actuators
                if (dr.SingleSwitchControlDevices.Where(w => w.Functions.Contains(Function.Humidity) && w.State.Value).FirstOrDefault() != null)
                    DehumidifierActive = true;
            }

            //get temps
            if (dr.TempControlDevices != null && dr.TempControlDevices.GroupLeader != null)
            {
                var pts = dr.TempControlDevices.Select(s => s.Actual_Temperature.Value);

                ActualTempAvg = RoundToHalfPoint(pts.Average());
                ActualTempMax = RoundToHalfPoint(pts.Max());
                ActualTempMin = RoundToHalfPoint(pts.Min());
                SetTemp = dr.TempControlDevices.GroupLeader.Set_Point_Temperature.Value;
                WindowOpen = dr.TempControlDevices.GroupLeader.Window_State.Value == ITempControl_Windows_State_Enum.OPEN;
                BoostActive = dr.TempControlDevices.GroupLeader.Boost_Mode.Value;
                BoostSecondsLeft = dr.TempControlDevices.GroupLeader.Boost_Time.Value;
            }

            //get humidity
            if (dr.HumidityControlDevices != null && dr.HumidityControlDevices.GroupLeader != null)
            {
                var pts = dr.HumidityControlDevices.Select(s => Convert.ToDecimal(s.Humidity.Value));

                HumidityAvg = Math.Round(pts.Average());
                HumidityMin = Math.Round(pts.Min());
                HumidityMax = Math.Round(pts.Max());
            }

            //get valve opens
            if (dr.ValveControlDevices != null && dr.ValveControlDevices.GroupLeader != null)
            {
                var pts = dr.ValveControlDevices.Select(s => Convert.ToDecimal(s.Level.Value));
                ValveOpenAvg = Math.Round(pts.Average()*100);
                ValveOpenMin = Math.Round(pts.Min()*100);
                ValveOpenMax = Math.Round(pts.Max()*100);
            }

            //get warnings
            Warnings.AddRange(
                dr.HmIPDevices.Where(w => w is ILowBatteryInfo).Cast<ILowBatteryInfo>().Where(w => (w.Low_Bat.Value))
                    .Select(s => String.Format($"{s.Name}: low voltage"))
                    .ToList()
                    );

            Warnings.AddRange(
                dr.HmDevices.Where(w => w.PendingConfig)
                    .Select(s => String.Format($"{s.Name}: pending configuration"))
                    .ToList()
                    );

            Warnings.AddRange(
                dr.HmDevices.Where(w=> !w.Reachable)
                    .Select(s => String.Format($"{s.Name}: unreachable"))
                    .ToList()
                );
        }

        private decimal RoundToHalfPoint(decimal input)
        {
            return Math.Round(input * 2) / 2.0M;
        }
    }
}
