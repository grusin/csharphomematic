/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */

using csharpmatic.Generic;
using csharpmatic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharpmatic.Interfaces.Devices
{
  public partial class HMIP_SWSD : Device, ILowBatteryInfo, ISmokeDetectorDevice, IHmIPDevice
  {
		public TypedDatapoint<System.Boolean> Config_Pending { get; private set; }

		public TypedDatapoint<System.Boolean> Duty_Cycle { get; private set; }

		public TypedDatapoint<System.Boolean> Low_Bat { get; private set; }

		public TypedDatapoint<System.Int32> Rssi_Device { get; private set; }

		public TypedDatapoint<System.Int32> Rssi_Peer { get; private set; }

		public TypedDatapoint<String> Time_Of_Operation { get; private set; }

		public TypedDatapoint<Int32> Time_Of_Operation_Status { get; private set; }

		public TypedDatapoint<System.Boolean> Unreach { get; private set; }

		public TypedDatapoint<System.Boolean> Update_Pending { get; private set; }

		public TypedDatapoint<System.Int32> Error_Code { get; private set; }

		public TypedDatapoint<csharpmatic.Interfaces.ISmokeDetectorDevice_Smoke_Detector_Alarm_Status_Enum> Smoke_Detector_Alarm_Status { get; private set; }

		public TypedDatapoint<csharpmatic.Interfaces.ISmokeDetectorDevice_Smoke_Detector_Command_Enum> Smoke_Detector_Command { get; private set; }

		public TypedDatapoint<csharpmatic.Interfaces.ISmokeDetectorDevice_Smoke_Detector_Test_Result_Enum> Smoke_Detector_Test_Result { get; private set; }


      public HMIP_SWSD(XMLAPI.DeviceList.Device d, XMLAPI.Client CGIClient, DeviceManager dm) : base(d, CGIClient, dm)
      {
			Config_Pending = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["CONFIG_PENDING"]);

			Duty_Cycle = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["DUTY_CYCLE"]);

			Low_Bat = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["LOW_BAT"]);

			Rssi_Device = new TypedDatapoint<System.Int32>(base.Channels[0].Datapoints["RSSI_DEVICE"]);

			Rssi_Peer = new TypedDatapoint<System.Int32>(base.Channels[0].Datapoints["RSSI_PEER"]);

			Time_Of_Operation = new TypedDatapoint<String>(base.Channels[0].Datapoints["TIME_OF_OPERATION"]);

			Time_Of_Operation_Status = new TypedDatapoint<Int32>(base.Channels[0].Datapoints["TIME_OF_OPERATION_STATUS"]);

			Unreach = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UNREACH"]);

			Update_Pending = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UPDATE_PENDING"]);

			Error_Code = new TypedDatapoint<System.Int32>(base.Channels[1].Datapoints["ERROR_CODE"]);

			Smoke_Detector_Alarm_Status = new TypedDatapoint<csharpmatic.Interfaces.ISmokeDetectorDevice_Smoke_Detector_Alarm_Status_Enum>(base.Channels[1].Datapoints["SMOKE_DETECTOR_ALARM_STATUS"]);

			Smoke_Detector_Command = new TypedDatapoint<csharpmatic.Interfaces.ISmokeDetectorDevice_Smoke_Detector_Command_Enum>(base.Channels[1].Datapoints["SMOKE_DETECTOR_COMMAND"]);

			Smoke_Detector_Test_Result = new TypedDatapoint<csharpmatic.Interfaces.ISmokeDetectorDevice_Smoke_Detector_Test_Result_Enum>(base.Channels[1].Datapoints["SMOKE_DETECTOR_TEST_RESULT"]);

      }
  }
}
