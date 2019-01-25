using csharpmatic.Generic;
using csharpmatic.Interfaces;
using csharpmatic.Interfaces.Devices;
using csharpmatic.JsonAPI;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmaticAutomations.Alarm
{
    public class AlarmAutomation
    {
        public bool AlarmArmed { get { return _alarmArmed; } }
        public bool AlarmTriggered { get { return _alarmTriggered; } }
        public DeviceManager DeviceManager { get; private set; }

        public Dictionary<string, DateTime> ISEIDModify;

        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool _alarmArmed;
        private bool _alarmTriggered;
        //private bool lightFlickerState;
        //private DateTime nextLightFlickerStateChange;
        private Client rpcClient;

        public AlarmAutomation(DeviceManager dm)
        {
            DeviceManager = dm;
            ISEIDModify = new Dictionary<string, DateTime>();

            rpcClient = new Client(dm.HttpServerUri.Host);

            _alarmArmed = rpcClient.GetOrCreateSystemBoolVariable("Alarm Armed", false);
            _alarmTriggered = rpcClient.GetOrCreateSystemBoolVariable("Alarm Triggered", false);
            
            LOGGER.Info($"Initalized alarm: {ToString()}");
        }

        public void Work()
        {
            if (AlarmArmed)
                Work_Armed();

            if (AlarmTriggered)
                Work_AlarmTriggered();

        }

        public override string ToString()
        {
            if (_alarmTriggered)
                return "ALARM TRIGGERED";

            if (_alarmArmed)
                return "Alarm ARMED";

            return "Alarm DISARMED";
        }

        public List<AlarmSensorScanOutput> ScanSensors(bool onlyTriggered)
        {
            List<AlarmSensorScanOutput> ret = new List<AlarmSensorScanOutput>();

            var devices = DeviceManager.Devices.Where(w => w.Functions.Contains(Function.Alarm)).ToList();

            var sensors = devices.Where(w => w is HMIP_SWDO && w.Reachable && !w.PendingConfig && !w.Functions.Contains(Function.Not_Sensor)).Cast<HMIP_SWDO>();

            foreach (var s in sensors)
            {
                //alwasy check both, as it initalizes the iseid array
                var t1 = IsDatapointTriggered(s.Sabotage, false, false);
                var t2 = IsDatapointTriggered(s.State, 0, false);

                if (t1 != null)
                    ret.Add(t1);
                else if (t2 != null)
                    ret.Add(t2);
                else if (onlyTriggered == false)
                    ret.Add(new AlarmSensorScanOutput(s, null));
            }

            return ret;
        }

        private AlarmSensorScanOutput IsDatapointTriggered<T>(TypedDatapoint<T> dp, T notTrippedValue, bool checkTimestamp)
        {
            if (dp.Value == null || !dp.Value.Equals(notTrippedValue))
                return new AlarmSensorScanOutput(dp.Channel.Device, dp.UnderlyingDatapoint);

            if (checkTimestamp)
            {

                if (ISEIDModify.ContainsKey(dp.ISEID))
                {
                    var lastTimestamp = ISEIDModify[dp.ISEID];
                    if (lastTimestamp != dp.Timestamp)
                    {
                        ISEIDModify[dp.ISEID] = dp.Timestamp;
                        return new AlarmSensorScanOutput(dp.Channel.Device, dp.UnderlyingDatapoint);
                    }
                }
                else
                    ISEIDModify.Add(dp.ISEID, dp.Timestamp);
            }

            return null;
        }


        private void Work_Armed()
        {
            if (!AlarmArmed)
                return;

            var triggered = ScanSensors(true);

            if (triggered.Count > 0)
            {
                foreach (var t in triggered)
                    LOGGER.Warn($"ALARM {t.GetTriggeredMessage()}");

                _setAlarmTriggered(true);
            }
        }

        public void Work_AlarmTriggered()
        {
            //turn on all switches, dimmers in alarm group, make sure they are constantly triggered
            SetAlarmOutputDevicesSate(true);
        }

        private void SetAlarmOutputDevicesSate(bool turnOn)
        {
            List<Task> taskList = new List<Task>();

            var alarmDevices = DeviceManager.Devices.Where(w => w.Functions.Contains(Function.Alarm)).ToList();

            foreach (var d in alarmDevices)
            {
                //switch devices
                ISingleSwitchControlDevice switchDevice = d as ISingleSwitchControlDevice;
                if (switchDevice != null)
                {
                    bool newState = turnOn;

                    if (switchDevice.State.Value != newState)
                        taskList.Add(switchDevice.State.SetValueAsync(newState));
                }

                //dimmer devices
                HMIP_BDT dimmerDevice = d as HMIP_BDT;
                if (dimmerDevice != null)
                {
                    decimal newState = turnOn ? 1.0M : 0.0M;

                    if (dimmerDevice.Level.Value != newState)
                        taskList.Add(dimmerDevice.Level.SetValueAsync(newState));
                }


                /*
                 
                //smoke detectors
                HMIP_SWSD smokeDetector = d as HMIP_SWSD;
                if(smokeDetector != null)
                {
                    ISmokeDetectorDevice_Smoke_Detector_Command_Enum newState = turnOn ? ISmokeDetectorDevice_Smoke_Detector_Command_Enum.INTRUSION_ALARM : ISmokeDetectorDevice_Smoke_Detector_Command_Enum.INTRUSION_ALARM_OFF;

                    if (smokeDetector.Smoke_Detector_Command.Value != newState)
                        taskList.Add(smokeDetector.Smoke_Detector_Command.SetValueAsync(newState));
                }
                */
            }

            if (taskList.Count > 0)
                Task.WaitAll(taskList.ToArray());
        }

        public List<AlarmSensorScanOutput> Arm()
        {
            LOGGER.Info("Alarm arm requested...");

            if (AlarmArmed)
                throw new InvalidOperationException("Alarm is already ARMED");

            ISEIDModify.Clear();

            var res = ScanSensors(true);

            if (res.Count == 0)
            {
                _setAlarmArmed(true);
                _setAlarmTriggered(false);                                          
            }
            else
            {
                LOGGER.Info($"Alarm arming aborted. {res.Count} device(s) are triggered:");
                foreach (var d in res)
                    LOGGER.Warn($"  Arm aborted: {d.GetTriggeredMessage()}");
            }

            return res;
        }

        private void _setAlarmArmed(bool value)
        {
            _alarmArmed = value;
            JToken ret = null;

            if (value)
                LOGGER.Info("Alarm has been ARMED");
            else
                LOGGER.Info("Alarm has been DISARMED");

            try
            {
                ret = rpcClient.SetSystemVariable("Alarm Armed", value ? "1" : "0");               
            }
            catch(Exception e)
            {
                LOGGER.Error($"Could not set 'Alarm Armed' system variable to '{value}'", e);
            }
        }

        private void _setAlarmTriggered(bool value)
        {
            _alarmTriggered = value;
            JToken ret = null;

            if(value)
                LOGGER.Warn($"ALARM Has been TRIGGERED");              

            try
            {
                ret = rpcClient.SetSystemVariable("Alarm Triggered", value ? "1" : "0");
            }
            catch (Exception e)
            {
                LOGGER.Error($"Could not set 'Alarm Triggered' system variable to '{value}'", e);
            }            
        }

        public void Disarm()
        {
            LOGGER.Info("Alarm disarm requested");

            if (!AlarmArmed)
                LOGGER.Info("Alarm is already DISARMED, disarming all devices one more time anyway");

            _setAlarmArmed(false);
            _setAlarmTriggered(false);

            SetAlarmOutputDevicesSate(false);

            LOGGER.Info("Alarm DISARMED");
        }
    }
}
