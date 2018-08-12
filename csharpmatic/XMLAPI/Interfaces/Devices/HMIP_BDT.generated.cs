/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */

using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharpmatic.XMLAPI.Interfaces.Devices
{
  public partial class HMIP_BDT : Device, IDimmerDevice, IHmIPDevice
  {
		public TypedDatapoint<Decimal> Actuator_Actual_Temperature { get; private set; }

		public TypedDatapoint<Int32> Actuator_Actual_Temperature_Status { get; private set; }

		public TypedDatapoint<System.Boolean> Config_Pending { get; private set; }

		public TypedDatapoint<System.Boolean> Duty_Cycle { get; private set; }

		public TypedDatapoint<String> Error_Code { get; private set; }

		public TypedDatapoint<Boolean> Error_Overheat { get; private set; }

		public TypedDatapoint<Boolean> Error_Overload { get; private set; }

		public TypedDatapoint<Boolean> Error_Update { get; private set; }

		public TypedDatapoint<System.Decimal> Operating_Voltage { get; private set; }

		public TypedDatapoint<csharpmatic.XMLAPI.Interfaces.IHmIP_Operating_Voltage_Status_Enum> Operating_Voltage_Status { get; private set; }

		public TypedDatapoint<System.String> Rssi_Device { get; private set; }

		public TypedDatapoint<System.String> Rssi_Peer { get; private set; }

		public TypedDatapoint<System.Boolean> Unreach { get; private set; }

		public TypedDatapoint<System.Boolean> Update_Pending { get; private set; }

		public TypedDatapoint<Int32> Activity_State { get; private set; }

		public TypedDatapoint<System.Decimal> Level { get; private set; }

		public TypedDatapoint<csharpmatic.XMLAPI.Interfaces.IDimmerDevice_Level_Status> Level_Status { get; private set; }

		public TypedDatapoint<Int32> Process { get; private set; }

		public TypedDatapoint<Int32> Section { get; private set; }

		public TypedDatapoint<Int32> Section_Status { get; private set; }

		public TypedDatapoint<System.Decimal> Ramp_Time { get; private set; }

		public TypedDatapoint<Int32> Week_Program_Channel_Locks { get; private set; }

		public TypedDatapoint<Int32> Week_Program_Target_Channel_Lock { get; private set; }

		public TypedDatapoint<Int32> Week_Program_Target_Channel_Locks { get; private set; }


      public HMIP_BDT(CGI.DeviceList.Device d, CGI.CGIClient CGIClient, DeviceManager dm) : base(d, CGIClient, dm)
      {
			Actuator_Actual_Temperature = new TypedDatapoint<Decimal>(base.Channels[0].Datapoints["ACTUATOR_ACTUAL_TEMPERATURE"]);

			Actuator_Actual_Temperature_Status = new TypedDatapoint<Int32>(base.Channels[0].Datapoints["ACTUATOR_ACTUAL_TEMPERATURE_STATUS"]);

			Config_Pending = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["CONFIG_PENDING"]);

			Duty_Cycle = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["DUTY_CYCLE"]);

			Error_Code = new TypedDatapoint<String>(base.Channels[0].Datapoints["ERROR_CODE"]);

			Error_Overheat = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["ERROR_OVERHEAT"]);

			Error_Overload = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["ERROR_OVERLOAD"]);

			Error_Update = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["ERROR_UPDATE"]);

			Operating_Voltage = new TypedDatapoint<System.Decimal>(base.Channels[0].Datapoints["OPERATING_VOLTAGE"]);

			Operating_Voltage_Status = new TypedDatapoint<csharpmatic.XMLAPI.Interfaces.IHmIP_Operating_Voltage_Status_Enum>(base.Channels[0].Datapoints["OPERATING_VOLTAGE_STATUS"]);

			Rssi_Device = new TypedDatapoint<System.String>(base.Channels[0].Datapoints["RSSI_DEVICE"]);

			Rssi_Peer = new TypedDatapoint<System.String>(base.Channels[0].Datapoints["RSSI_PEER"]);

			Unreach = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UNREACH"]);

			Update_Pending = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UPDATE_PENDING"]);

			Activity_State = new TypedDatapoint<Int32>(base.Channels[4].Datapoints["ACTIVITY_STATE"]);

			Level = new TypedDatapoint<System.Decimal>(base.Channels[4].Datapoints["LEVEL"]);

			Level_Status = new TypedDatapoint<csharpmatic.XMLAPI.Interfaces.IDimmerDevice_Level_Status>(base.Channels[4].Datapoints["LEVEL_STATUS"]);

			Process = new TypedDatapoint<Int32>(base.Channels[4].Datapoints["PROCESS"]);

			Section = new TypedDatapoint<Int32>(base.Channels[4].Datapoints["SECTION"]);

			Section_Status = new TypedDatapoint<Int32>(base.Channels[4].Datapoints["SECTION_STATUS"]);

			Ramp_Time = new TypedDatapoint<System.Decimal>(base.Channels[4].Datapoints["RAMP_TIME"]);

			Week_Program_Channel_Locks = new TypedDatapoint<Int32>(base.Channels[7].Datapoints["WEEK_PROGRAM_CHANNEL_LOCKS"]);

			Week_Program_Target_Channel_Lock = new TypedDatapoint<Int32>(base.Channels[7].Datapoints["WEEK_PROGRAM_TARGET_CHANNEL_LOCK"]);

			Week_Program_Target_Channel_Locks = new TypedDatapoint<Int32>(base.Channels[7].Datapoints["WEEK_PROGRAM_TARGET_CHANNEL_LOCKS"]);

      }
  }
}
