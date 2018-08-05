/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */

using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharpmatic.XMLAPI.Interfaces.Devices
{
  public partial class HMIP_ETRV_2 : Device
  {
		public TypedDatapoint<Boolean> Config_Pending { get; private set; }

		public TypedDatapoint<Boolean> Duty_Cycle { get; private set; }

		public TypedDatapoint<Boolean> Low_Bat { get; private set; }

		public TypedDatapoint<Decimal> Operating_Voltage { get; private set; }

		public TypedDatapoint<Int32> Operating_Voltage_Status { get; private set; }

		public TypedDatapoint<String> Rssi_Device { get; private set; }

		public TypedDatapoint<String> Rssi_Peer { get; private set; }

		public TypedDatapoint<Boolean> Unreach { get; private set; }

		public TypedDatapoint<Boolean> Update_Pending { get; private set; }

		public TypedDatapoint<Int32> Active_Profile { get; private set; }

		public TypedDatapoint<Decimal> Actual_Temperature { get; private set; }

		public TypedDatapoint<Int32> Actual_Temperature_Status { get; private set; }

		public TypedDatapoint<Boolean> Boost_Mode { get; private set; }

		public TypedDatapoint<Int32> Boost_Time { get; private set; }

		public TypedDatapoint<Decimal> Control_Differential_Temperature { get; private set; }

		public TypedDatapoint<Int32> Control_Mode { get; private set; }

		public TypedDatapoint<Int32> Duration_Unit { get; private set; }

		public TypedDatapoint<Int32> Duration_Value { get; private set; }

		public TypedDatapoint<Boolean> Frost_Protection { get; private set; }

		public TypedDatapoint<Decimal> Level { get; private set; }

		public TypedDatapoint<Int32> Level_Status { get; private set; }

		public TypedDatapoint<Boolean> Party_Mode { get; private set; }

		public TypedDatapoint<Decimal> Party_Set_Point_Temperature { get; private set; }

		public TypedDatapoint<DateTime> Party_Time_End { get; private set; }

		public TypedDatapoint<DateTime> Party_Time_Start { get; private set; }

		public TypedDatapoint<Int32> Quick_Veto_Time { get; private set; }

		public TypedDatapoint<Int32> Set_Point_Mode { get; private set; }

		public TypedDatapoint<Decimal> Set_Point_Temperature { get; private set; }

		public TypedDatapoint<Boolean> Switch_Point_Occured { get; private set; }

		public TypedDatapoint<Boolean> Valve_Adaption { get; private set; }

		public TypedDatapoint<Int32> Valve_State { get; private set; }

		public TypedDatapoint<Int32> Window_State { get; private set; }


      public HMIP_ETRV_2()
      {
			Config_Pending = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["CONFIG_PENDING"]);

			Duty_Cycle = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["DUTY_CYCLE"]);

			Low_Bat = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["LOW_BAT"]);

			Operating_Voltage = new TypedDatapoint<Decimal>(base.Channels[0].Datapoints["OPERATING_VOLTAGE"]);

			Operating_Voltage_Status = new TypedDatapoint<Int32>(base.Channels[0].Datapoints["OPERATING_VOLTAGE_STATUS"]);

			Rssi_Device = new TypedDatapoint<String>(base.Channels[0].Datapoints["RSSI_DEVICE"]);

			Rssi_Peer = new TypedDatapoint<String>(base.Channels[0].Datapoints["RSSI_PEER"]);

			Unreach = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["UNREACH"]);

			Update_Pending = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["UPDATE_PENDING"]);

			Active_Profile = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["ACTIVE_PROFILE"]);

			Actual_Temperature = new TypedDatapoint<Decimal>(base.Channels[1].Datapoints["ACTUAL_TEMPERATURE"]);

			Actual_Temperature_Status = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["ACTUAL_TEMPERATURE_STATUS"]);

			Boost_Mode = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["BOOST_MODE"]);

			Boost_Time = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["BOOST_TIME"]);

			Control_Differential_Temperature = new TypedDatapoint<Decimal>(base.Channels[1].Datapoints["CONTROL_DIFFERENTIAL_TEMPERATURE"]);

			Control_Mode = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["CONTROL_MODE"]);

			Duration_Unit = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["DURATION_UNIT"]);

			Duration_Value = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["DURATION_VALUE"]);

			Frost_Protection = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["FROST_PROTECTION"]);

			Level = new TypedDatapoint<Decimal>(base.Channels[1].Datapoints["LEVEL"]);

			Level_Status = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["LEVEL_STATUS"]);

			Party_Mode = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["PARTY_MODE"]);

			Party_Set_Point_Temperature = new TypedDatapoint<Decimal>(base.Channels[1].Datapoints["PARTY_SET_POINT_TEMPERATURE"]);

			Party_Time_End = new TypedDatapoint<DateTime>(base.Channels[1].Datapoints["PARTY_TIME_END"]);

			Party_Time_Start = new TypedDatapoint<DateTime>(base.Channels[1].Datapoints["PARTY_TIME_START"]);

			Quick_Veto_Time = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["QUICK_VETO_TIME"]);

			Set_Point_Mode = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["SET_POINT_MODE"]);

			Set_Point_Temperature = new TypedDatapoint<Decimal>(base.Channels[1].Datapoints["SET_POINT_TEMPERATURE"]);

			Switch_Point_Occured = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["SWITCH_POINT_OCCURED"]);

			Valve_Adaption = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["VALVE_ADAPTION"]);

			Valve_State = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["VALVE_STATE"]);

			Window_State = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["WINDOW_STATE"]);

      }
  }
}
