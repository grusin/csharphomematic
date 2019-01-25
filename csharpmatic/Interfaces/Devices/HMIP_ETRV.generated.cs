/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */

using csharpmatic.Generic;
using csharpmatic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharpmatic.Interfaces.Devices
{
  public partial class HMIP_ETRV : Device, ILowBatteryInfo, IOperatingVoltageInfo, ITempControlDevice, IHmIPDevice, IValveControlDevice
  {
		public TypedDatapoint<System.Boolean> Config_Pending { get; private set; }

		public TypedDatapoint<System.Boolean> Duty_Cycle { get; private set; }

		public TypedDatapoint<System.Boolean> Low_Bat { get; private set; }

		public TypedDatapoint<System.Decimal> Operating_Voltage { get; private set; }

		public TypedDatapoint<csharpmatic.Interfaces.IOperating_Voltage_Status_Enum> Operating_Voltage_Status { get; private set; }

		public TypedDatapoint<System.Int32> Rssi_Device { get; private set; }

		public TypedDatapoint<System.Int32> Rssi_Peer { get; private set; }

		public TypedDatapoint<System.Boolean> Unreach { get; private set; }

		public TypedDatapoint<System.Boolean> Update_Pending { get; private set; }

		public TypedDatapoint<System.Int32> Active_Profile { get; private set; }

		public TypedDatapoint<System.Decimal> Actual_Temperature { get; private set; }

		public TypedDatapoint<csharpmatic.Interfaces.ITempControl_Actual_Temperature_Status> Actual_Temperature_Status { get; private set; }

		public TypedDatapoint<System.Boolean> Boost_Mode { get; private set; }

		public TypedDatapoint<System.Int32> Boost_Time { get; private set; }

		public TypedDatapoint<Decimal> Control_Differential_Temperature { get; private set; }

		public TypedDatapoint<Int32> Control_Mode { get; private set; }

		public TypedDatapoint<Int32> Duration_Unit { get; private set; }

		public TypedDatapoint<Int32> Duration_Value { get; private set; }

		public TypedDatapoint<System.Boolean> Frost_Protection { get; private set; }

		public TypedDatapoint<System.Decimal> Level { get; private set; }

		public TypedDatapoint<csharpmatic.Interfaces.IValveControlDevice_Level_Status> Level_Status { get; private set; }

		public TypedDatapoint<Boolean> Party_Mode { get; private set; }

		public TypedDatapoint<Decimal> Party_Set_Point_Temperature { get; private set; }

		public TypedDatapoint<DateTime> Party_Time_End { get; private set; }

		public TypedDatapoint<DateTime> Party_Time_Start { get; private set; }

		public TypedDatapoint<System.Int32> Set_Point_Mode { get; private set; }

		public TypedDatapoint<System.Decimal> Set_Point_Temperature { get; private set; }

		public TypedDatapoint<System.Boolean> Switch_Point_Occured { get; private set; }

		public TypedDatapoint<System.Boolean> Valve_Adaption { get; private set; }

		public TypedDatapoint<csharpmatic.Interfaces.IValveControl_Valve_State_Enum> Valve_State { get; private set; }

		public TypedDatapoint<csharpmatic.Interfaces.ITempControl_Windows_State_Enum> Window_State { get; private set; }


      public HMIP_ETRV(XMLAPI.DeviceList.Device d, XMLAPI.Client CGIClient, DeviceManager dm) : base(d, CGIClient, dm)
      {
			Config_Pending = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["CONFIG_PENDING"]);

			Duty_Cycle = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["DUTY_CYCLE"]);

			Low_Bat = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["LOW_BAT"]);

			Operating_Voltage = new TypedDatapoint<System.Decimal>(base.Channels[0].Datapoints["OPERATING_VOLTAGE"]);

			Operating_Voltage_Status = new TypedDatapoint<csharpmatic.Interfaces.IOperating_Voltage_Status_Enum>(base.Channels[0].Datapoints["OPERATING_VOLTAGE_STATUS"]);

			Rssi_Device = new TypedDatapoint<System.Int32>(base.Channels[0].Datapoints["RSSI_DEVICE"]);

			Rssi_Peer = new TypedDatapoint<System.Int32>(base.Channels[0].Datapoints["RSSI_PEER"]);

			Unreach = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UNREACH"]);

			Update_Pending = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UPDATE_PENDING"]);

			Active_Profile = new TypedDatapoint<System.Int32>(base.Channels[1].Datapoints["ACTIVE_PROFILE"]);

			Actual_Temperature = new TypedDatapoint<System.Decimal>(base.Channels[1].Datapoints["ACTUAL_TEMPERATURE"]);

			Actual_Temperature_Status = new TypedDatapoint<csharpmatic.Interfaces.ITempControl_Actual_Temperature_Status>(base.Channels[1].Datapoints["ACTUAL_TEMPERATURE_STATUS"]);

			Boost_Mode = new TypedDatapoint<System.Boolean>(base.Channels[1].Datapoints["BOOST_MODE"]);

			Boost_Time = new TypedDatapoint<System.Int32>(base.Channels[1].Datapoints["BOOST_TIME"]);

			Control_Differential_Temperature = new TypedDatapoint<Decimal>(base.Channels[1].Datapoints["CONTROL_DIFFERENTIAL_TEMPERATURE"]);

			Control_Mode = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["CONTROL_MODE"]);

			Duration_Unit = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["DURATION_UNIT"]);

			Duration_Value = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["DURATION_VALUE"]);

			Frost_Protection = new TypedDatapoint<System.Boolean>(base.Channels[1].Datapoints["FROST_PROTECTION"]);

			Level = new TypedDatapoint<System.Decimal>(base.Channels[1].Datapoints["LEVEL"]);

			Level_Status = new TypedDatapoint<csharpmatic.Interfaces.IValveControlDevice_Level_Status>(base.Channels[1].Datapoints["LEVEL_STATUS"]);

			Party_Mode = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["PARTY_MODE"]);

			Party_Set_Point_Temperature = new TypedDatapoint<Decimal>(base.Channels[1].Datapoints["PARTY_SET_POINT_TEMPERATURE"]);

			Party_Time_End = new TypedDatapoint<DateTime>(base.Channels[1].Datapoints["PARTY_TIME_END"]);

			Party_Time_Start = new TypedDatapoint<DateTime>(base.Channels[1].Datapoints["PARTY_TIME_START"]);

			Set_Point_Mode = new TypedDatapoint<System.Int32>(base.Channels[1].Datapoints["SET_POINT_MODE"]);

			Set_Point_Temperature = new TypedDatapoint<System.Decimal>(base.Channels[1].Datapoints["SET_POINT_TEMPERATURE"]);

			Switch_Point_Occured = new TypedDatapoint<System.Boolean>(base.Channels[1].Datapoints["SWITCH_POINT_OCCURED"]);

			Valve_Adaption = new TypedDatapoint<System.Boolean>(base.Channels[1].Datapoints["VALVE_ADAPTION"]);

			Valve_State = new TypedDatapoint<csharpmatic.Interfaces.IValveControl_Valve_State_Enum>(base.Channels[1].Datapoints["VALVE_STATE"]);

			Window_State = new TypedDatapoint<csharpmatic.Interfaces.ITempControl_Windows_State_Enum>(base.Channels[1].Datapoints["WINDOW_STATE"]);

      }
  }
}
