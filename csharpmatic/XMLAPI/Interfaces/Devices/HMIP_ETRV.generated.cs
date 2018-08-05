/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */

using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharpmatic.XMLAPI.Interfaces.Devices
{
  public partial class HMIP_ETRV : Device
  {
		public TypedDatapoint<Boolean> Config_Pending_C0 { get; private set; }

		public TypedDatapoint<Boolean> Duty_Cycle_C0 { get; private set; }

		public TypedDatapoint<Boolean> Low_Bat_C0 { get; private set; }

		public TypedDatapoint<Decimal> Operating_Voltage_C0 { get; private set; }

		public TypedDatapoint<Int32> Operating_Voltage_Status_C0 { get; private set; }

		public TypedDatapoint<String> Rssi_Device_C0 { get; private set; }

		public TypedDatapoint<String> Rssi_Peer_C0 { get; private set; }

		public TypedDatapoint<Boolean> Unreach_C0 { get; private set; }

		public TypedDatapoint<Boolean> Update_Pending_C0 { get; private set; }

		public TypedDatapoint<Int32> Active_Profile_C1 { get; private set; }

		public TypedDatapoint<Decimal> Actual_Temperature_C1 { get; private set; }

		public TypedDatapoint<Int32> Actual_Temperature_Status_C1 { get; private set; }

		public TypedDatapoint<Boolean> Boost_Mode_C1 { get; private set; }

		public TypedDatapoint<Int32> Boost_Time_C1 { get; private set; }

		public TypedDatapoint<Decimal> Control_Differential_Temperature_C1 { get; private set; }

		public TypedDatapoint<Int32> Control_Mode_C1 { get; private set; }

		public TypedDatapoint<Int32> Duration_Unit_C1 { get; private set; }

		public TypedDatapoint<Int32> Duration_Value_C1 { get; private set; }

		public TypedDatapoint<Boolean> Frost_Protection_C1 { get; private set; }

		public TypedDatapoint<Decimal> Level_C1 { get; private set; }

		public TypedDatapoint<Int32> Level_Status_C1 { get; private set; }

		public TypedDatapoint<Boolean> Party_Mode_C1 { get; private set; }

		public TypedDatapoint<Decimal> Party_Set_Point_Temperature_C1 { get; private set; }

		public TypedDatapoint<DateTime> Party_Time_End_C1 { get; private set; }

		public TypedDatapoint<DateTime> Party_Time_Start_C1 { get; private set; }

		public TypedDatapoint<Int32> Set_Point_Mode_C1 { get; private set; }

		public TypedDatapoint<Decimal> Set_Point_Temperature_C1 { get; private set; }

		public TypedDatapoint<Boolean> Switch_Point_Occured_C1 { get; private set; }

		public TypedDatapoint<Boolean> Valve_Adaption_C1 { get; private set; }

		public TypedDatapoint<Int32> Valve_State_C1 { get; private set; }

		public TypedDatapoint<Int32> Window_State_C1 { get; private set; }


      public HMIP_ETRV()
      {
			Config_Pending_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["CONFIG_PENDING"]);

			Duty_Cycle_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["DUTY_CYCLE"]);

			Low_Bat_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["LOW_BAT"]);

			Operating_Voltage_C0 = new TypedDatapoint<Decimal>(base.Channels[0].Datapoints["OPERATING_VOLTAGE"]);

			Operating_Voltage_Status_C0 = new TypedDatapoint<Int32>(base.Channels[0].Datapoints["OPERATING_VOLTAGE_STATUS"]);

			Rssi_Device_C0 = new TypedDatapoint<String>(base.Channels[0].Datapoints["RSSI_DEVICE"]);

			Rssi_Peer_C0 = new TypedDatapoint<String>(base.Channels[0].Datapoints["RSSI_PEER"]);

			Unreach_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["UNREACH"]);

			Update_Pending_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["UPDATE_PENDING"]);

			Active_Profile_C1 = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["ACTIVE_PROFILE"]);

			Actual_Temperature_C1 = new TypedDatapoint<Decimal>(base.Channels[1].Datapoints["ACTUAL_TEMPERATURE"]);

			Actual_Temperature_Status_C1 = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["ACTUAL_TEMPERATURE_STATUS"]);

			Boost_Mode_C1 = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["BOOST_MODE"]);

			Boost_Time_C1 = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["BOOST_TIME"]);

			Control_Differential_Temperature_C1 = new TypedDatapoint<Decimal>(base.Channels[1].Datapoints["CONTROL_DIFFERENTIAL_TEMPERATURE"]);

			Control_Mode_C1 = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["CONTROL_MODE"]);

			Duration_Unit_C1 = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["DURATION_UNIT"]);

			Duration_Value_C1 = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["DURATION_VALUE"]);

			Frost_Protection_C1 = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["FROST_PROTECTION"]);

			Level_C1 = new TypedDatapoint<Decimal>(base.Channels[1].Datapoints["LEVEL"]);

			Level_Status_C1 = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["LEVEL_STATUS"]);

			Party_Mode_C1 = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["PARTY_MODE"]);

			Party_Set_Point_Temperature_C1 = new TypedDatapoint<Decimal>(base.Channels[1].Datapoints["PARTY_SET_POINT_TEMPERATURE"]);

			Party_Time_End_C1 = new TypedDatapoint<DateTime>(base.Channels[1].Datapoints["PARTY_TIME_END"]);

			Party_Time_Start_C1 = new TypedDatapoint<DateTime>(base.Channels[1].Datapoints["PARTY_TIME_START"]);

			Set_Point_Mode_C1 = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["SET_POINT_MODE"]);

			Set_Point_Temperature_C1 = new TypedDatapoint<Decimal>(base.Channels[1].Datapoints["SET_POINT_TEMPERATURE"]);

			Switch_Point_Occured_C1 = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["SWITCH_POINT_OCCURED"]);

			Valve_Adaption_C1 = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["VALVE_ADAPTION"]);

			Valve_State_C1 = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["VALVE_STATE"]);

			Window_State_C1 = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["WINDOW_STATE"]);

      }
  }
}
