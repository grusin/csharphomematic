/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */

using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharpmatic.XMLAPI.Interfaces.Devices
{
  public partial class HMIP_PSM : Device
  {
		public TypedDatapoint<Decimal> Actual_Temperature_C0 { get; private set; }

		public TypedDatapoint<Int32> Actual_Temperature_Status_C0 { get; private set; }

		public TypedDatapoint<Boolean> Config_Pending_C0 { get; private set; }

		public TypedDatapoint<Boolean> Duty_Cycle_C0 { get; private set; }

		public TypedDatapoint<String> Error_Code_C0 { get; private set; }

		public TypedDatapoint<Boolean> Error_Overheat_C0 { get; private set; }

		public TypedDatapoint<Decimal> Operating_Voltage_C0 { get; private set; }

		public TypedDatapoint<Int32> Operating_Voltage_Status_C0 { get; private set; }

		public TypedDatapoint<String> Rssi_Device_C0 { get; private set; }

		public TypedDatapoint<String> Rssi_Peer_C0 { get; private set; }

		public TypedDatapoint<Boolean> Unreach_C0 { get; private set; }

		public TypedDatapoint<Boolean> Update_Pending_C0 { get; private set; }

		public TypedDatapoint<Boolean> Press_Long_C1 { get; private set; }

		public TypedDatapoint<Boolean> Press_Short_C1 { get; private set; }

		public TypedDatapoint<Int32> Process_C2 { get; private set; }

		public TypedDatapoint<Int32> Section_C2 { get; private set; }

		public TypedDatapoint<Int32> Section_Status_C2 { get; private set; }

		public TypedDatapoint<Boolean> State_C2 { get; private set; }

		public TypedDatapoint<Int32> Process_C3 { get; private set; }

		public TypedDatapoint<Int32> Section_C3 { get; private set; }

		public TypedDatapoint<Int32> Section_Status_C3 { get; private set; }

		public TypedDatapoint<Boolean> State_C3 { get; private set; }

		public TypedDatapoint<Int32> Process_C4 { get; private set; }

		public TypedDatapoint<Int32> Section_C4 { get; private set; }

		public TypedDatapoint<Int32> Section_Status_C4 { get; private set; }

		public TypedDatapoint<Boolean> State_C4 { get; private set; }

		public TypedDatapoint<Int32> Process_C5 { get; private set; }

		public TypedDatapoint<Int32> Section_C5 { get; private set; }

		public TypedDatapoint<Int32> Section_Status_C5 { get; private set; }

		public TypedDatapoint<Boolean> State_C5 { get; private set; }

		public TypedDatapoint<Decimal> Current_C6 { get; private set; }

		public TypedDatapoint<Int32> Current_Status_C6 { get; private set; }

		public TypedDatapoint<Decimal> Energy_Counter_C6 { get; private set; }

		public TypedDatapoint<Boolean> Energy_Counter_Overflow_C6 { get; private set; }

		public TypedDatapoint<Decimal> Frequency_C6 { get; private set; }

		public TypedDatapoint<Int32> Frequency_Status_C6 { get; private set; }

		public TypedDatapoint<Decimal> Power_C6 { get; private set; }

		public TypedDatapoint<Int32> Power_Status_C6 { get; private set; }

		public TypedDatapoint<Decimal> Voltage_C6 { get; private set; }

		public TypedDatapoint<Int32> Voltage_Status_C6 { get; private set; }


      public HMIP_PSM()
      {
			Actual_Temperature_C0 = new TypedDatapoint<Decimal>(base.Channels[0].Datapoints["ACTUAL_TEMPERATURE"]);

			Actual_Temperature_Status_C0 = new TypedDatapoint<Int32>(base.Channels[0].Datapoints["ACTUAL_TEMPERATURE_STATUS"]);

			Config_Pending_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["CONFIG_PENDING"]);

			Duty_Cycle_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["DUTY_CYCLE"]);

			Error_Code_C0 = new TypedDatapoint<String>(base.Channels[0].Datapoints["ERROR_CODE"]);

			Error_Overheat_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["ERROR_OVERHEAT"]);

			Operating_Voltage_C0 = new TypedDatapoint<Decimal>(base.Channels[0].Datapoints["OPERATING_VOLTAGE"]);

			Operating_Voltage_Status_C0 = new TypedDatapoint<Int32>(base.Channels[0].Datapoints["OPERATING_VOLTAGE_STATUS"]);

			Rssi_Device_C0 = new TypedDatapoint<String>(base.Channels[0].Datapoints["RSSI_DEVICE"]);

			Rssi_Peer_C0 = new TypedDatapoint<String>(base.Channels[0].Datapoints["RSSI_PEER"]);

			Unreach_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["UNREACH"]);

			Update_Pending_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["UPDATE_PENDING"]);

			Press_Long_C1 = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["PRESS_LONG"]);

			Press_Short_C1 = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["PRESS_SHORT"]);

			Process_C2 = new TypedDatapoint<Int32>(base.Channels[2].Datapoints["PROCESS"]);

			Section_C2 = new TypedDatapoint<Int32>(base.Channels[2].Datapoints["SECTION"]);

			Section_Status_C2 = new TypedDatapoint<Int32>(base.Channels[2].Datapoints["SECTION_STATUS"]);

			State_C2 = new TypedDatapoint<Boolean>(base.Channels[2].Datapoints["STATE"]);

			Process_C3 = new TypedDatapoint<Int32>(base.Channels[3].Datapoints["PROCESS"]);

			Section_C3 = new TypedDatapoint<Int32>(base.Channels[3].Datapoints["SECTION"]);

			Section_Status_C3 = new TypedDatapoint<Int32>(base.Channels[3].Datapoints["SECTION_STATUS"]);

			State_C3 = new TypedDatapoint<Boolean>(base.Channels[3].Datapoints["STATE"]);

			Process_C4 = new TypedDatapoint<Int32>(base.Channels[4].Datapoints["PROCESS"]);

			Section_C4 = new TypedDatapoint<Int32>(base.Channels[4].Datapoints["SECTION"]);

			Section_Status_C4 = new TypedDatapoint<Int32>(base.Channels[4].Datapoints["SECTION_STATUS"]);

			State_C4 = new TypedDatapoint<Boolean>(base.Channels[4].Datapoints["STATE"]);

			Process_C5 = new TypedDatapoint<Int32>(base.Channels[5].Datapoints["PROCESS"]);

			Section_C5 = new TypedDatapoint<Int32>(base.Channels[5].Datapoints["SECTION"]);

			Section_Status_C5 = new TypedDatapoint<Int32>(base.Channels[5].Datapoints["SECTION_STATUS"]);

			State_C5 = new TypedDatapoint<Boolean>(base.Channels[5].Datapoints["STATE"]);

			Current_C6 = new TypedDatapoint<Decimal>(base.Channels[6].Datapoints["CURRENT"]);

			Current_Status_C6 = new TypedDatapoint<Int32>(base.Channels[6].Datapoints["CURRENT_STATUS"]);

			Energy_Counter_C6 = new TypedDatapoint<Decimal>(base.Channels[6].Datapoints["ENERGY_COUNTER"]);

			Energy_Counter_Overflow_C6 = new TypedDatapoint<Boolean>(base.Channels[6].Datapoints["ENERGY_COUNTER_OVERFLOW"]);

			Frequency_C6 = new TypedDatapoint<Decimal>(base.Channels[6].Datapoints["FREQUENCY"]);

			Frequency_Status_C6 = new TypedDatapoint<Int32>(base.Channels[6].Datapoints["FREQUENCY_STATUS"]);

			Power_C6 = new TypedDatapoint<Decimal>(base.Channels[6].Datapoints["POWER"]);

			Power_Status_C6 = new TypedDatapoint<Int32>(base.Channels[6].Datapoints["POWER_STATUS"]);

			Voltage_C6 = new TypedDatapoint<Decimal>(base.Channels[6].Datapoints["VOLTAGE"]);

			Voltage_Status_C6 = new TypedDatapoint<Int32>(base.Channels[6].Datapoints["VOLTAGE_STATUS"]);

      }
  }
}
