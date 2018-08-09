/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */

using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharpmatic.XMLAPI.Interfaces.Devices
{
  public partial class HMIP_ETRV : Device, ITempControlDevice, IHmIPDevice, IValveControlDevice
  {
		public ManagedDatapoint<System.Boolean> Config_Pending { get; private set; }

		public ManagedDatapoint<System.Boolean> Duty_Cycle { get; private set; }

		public ManagedDatapoint<Boolean> Low_Bat { get; private set; }

		public ManagedDatapoint<System.Decimal> Operating_Voltage { get; private set; }

		public ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.IHmIP_Operating_Voltage_Status_Enum> Operating_Voltage_Status { get; private set; }

		public ManagedDatapoint<System.String> Rssi_Device { get; private set; }

		public ManagedDatapoint<System.String> Rssi_Peer { get; private set; }

		public ManagedDatapoint<System.Boolean> Unreach { get; private set; }

		public ManagedDatapoint<System.Boolean> Update_Pending { get; private set; }

		public ManagedDatapoint<System.Int32> Active_Profile { get; private set; }

		public ManagedDatapoint<System.Decimal> Actual_Temperature { get; private set; }

		public ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.ITempControl_Actual_Temperature_Status> Actual_Temperature_Status { get; private set; }

		public ManagedDatapoint<System.Boolean> Boost_Mode { get; private set; }

		public ManagedDatapoint<System.Int32> Boost_Time { get; private set; }

		public ManagedDatapoint<Decimal> Control_Differential_Temperature { get; private set; }

		public ManagedDatapoint<Int32> Control_Mode { get; private set; }

		public ManagedDatapoint<Int32> Duration_Unit { get; private set; }

		public ManagedDatapoint<Int32> Duration_Value { get; private set; }

		public ManagedDatapoint<System.Boolean> Frost_Protection { get; private set; }

		public ManagedDatapoint<System.Decimal> Level { get; private set; }

		public ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.IValveControlDevice_Level_Status> Level_Status { get; private set; }

		public ManagedDatapoint<Boolean> Party_Mode { get; private set; }

		public ManagedDatapoint<Decimal> Party_Set_Point_Temperature { get; private set; }

		public ManagedDatapoint<DateTime> Party_Time_End { get; private set; }

		public ManagedDatapoint<DateTime> Party_Time_Start { get; private set; }

		public ManagedDatapoint<System.Int32> Set_Point_Mode { get; private set; }

		public ManagedDatapoint<System.Decimal> Set_Point_Temperature { get; private set; }

		public ManagedDatapoint<System.Boolean> Switch_Point_Occured { get; private set; }

		public ManagedDatapoint<System.Boolean> Valve_Adaption { get; private set; }

		public ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.IValveControl_Valve_State_Enum> Valve_State { get; private set; }

		public ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.ITempControl_Windows_State_Enum> Window_State { get; private set; }


      public HMIP_ETRV(CGI.DeviceList.Device d, CGI.CGIClient CGIClient) : base(d, CGIClient)
      {
			Config_Pending = new ManagedDatapoint<System.Boolean>(base.Channels[0].Datapoints["CONFIG_PENDING"]);

			Duty_Cycle = new ManagedDatapoint<System.Boolean>(base.Channels[0].Datapoints["DUTY_CYCLE"]);

			Low_Bat = new ManagedDatapoint<Boolean>(base.Channels[0].Datapoints["LOW_BAT"]);

			Operating_Voltage = new ManagedDatapoint<System.Decimal>(base.Channels[0].Datapoints["OPERATING_VOLTAGE"]);

			Operating_Voltage_Status = new ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.IHmIP_Operating_Voltage_Status_Enum>(base.Channels[0].Datapoints["OPERATING_VOLTAGE_STATUS"]);

			Rssi_Device = new ManagedDatapoint<System.String>(base.Channels[0].Datapoints["RSSI_DEVICE"]);

			Rssi_Peer = new ManagedDatapoint<System.String>(base.Channels[0].Datapoints["RSSI_PEER"]);

			Unreach = new ManagedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UNREACH"]);

			Update_Pending = new ManagedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UPDATE_PENDING"]);

			Active_Profile = new ManagedDatapoint<System.Int32>(base.Channels[1].Datapoints["ACTIVE_PROFILE"]);

			Actual_Temperature = new ManagedDatapoint<System.Decimal>(base.Channels[1].Datapoints["ACTUAL_TEMPERATURE"]);

			Actual_Temperature_Status = new ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.ITempControl_Actual_Temperature_Status>(base.Channels[1].Datapoints["ACTUAL_TEMPERATURE_STATUS"]);

			Boost_Mode = new ManagedDatapoint<System.Boolean>(base.Channels[1].Datapoints["BOOST_MODE"]);

			Boost_Time = new ManagedDatapoint<System.Int32>(base.Channels[1].Datapoints["BOOST_TIME"]);

			Control_Differential_Temperature = new ManagedDatapoint<Decimal>(base.Channels[1].Datapoints["CONTROL_DIFFERENTIAL_TEMPERATURE"]);

			Control_Mode = new ManagedDatapoint<Int32>(base.Channels[1].Datapoints["CONTROL_MODE"]);

			Duration_Unit = new ManagedDatapoint<Int32>(base.Channels[1].Datapoints["DURATION_UNIT"]);

			Duration_Value = new ManagedDatapoint<Int32>(base.Channels[1].Datapoints["DURATION_VALUE"]);

			Frost_Protection = new ManagedDatapoint<System.Boolean>(base.Channels[1].Datapoints["FROST_PROTECTION"]);

			Level = new ManagedDatapoint<System.Decimal>(base.Channels[1].Datapoints["LEVEL"]);

			Level_Status = new ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.IValveControlDevice_Level_Status>(base.Channels[1].Datapoints["LEVEL_STATUS"]);

			Party_Mode = new ManagedDatapoint<Boolean>(base.Channels[1].Datapoints["PARTY_MODE"]);

			Party_Set_Point_Temperature = new ManagedDatapoint<Decimal>(base.Channels[1].Datapoints["PARTY_SET_POINT_TEMPERATURE"]);

			Party_Time_End = new ManagedDatapoint<DateTime>(base.Channels[1].Datapoints["PARTY_TIME_END"]);

			Party_Time_Start = new ManagedDatapoint<DateTime>(base.Channels[1].Datapoints["PARTY_TIME_START"]);

			Set_Point_Mode = new ManagedDatapoint<System.Int32>(base.Channels[1].Datapoints["SET_POINT_MODE"]);

			Set_Point_Temperature = new ManagedDatapoint<System.Decimal>(base.Channels[1].Datapoints["SET_POINT_TEMPERATURE"]);

			Switch_Point_Occured = new ManagedDatapoint<System.Boolean>(base.Channels[1].Datapoints["SWITCH_POINT_OCCURED"]);

			Valve_Adaption = new ManagedDatapoint<System.Boolean>(base.Channels[1].Datapoints["VALVE_ADAPTION"]);

			Valve_State = new ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.IValveControl_Valve_State_Enum>(base.Channels[1].Datapoints["VALVE_STATE"]);

			Window_State = new ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.ITempControl_Windows_State_Enum>(base.Channels[1].Datapoints["WINDOW_STATE"]);

      }
  }
}
