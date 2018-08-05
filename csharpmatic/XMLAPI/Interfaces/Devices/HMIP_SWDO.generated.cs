/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */

using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharpmatic.XMLAPI.Interfaces.Devices
{
  public partial class HMIP_SWDO : Device
  {
		public TypedDatapoint<Boolean> Config_Pending_C0 { get; private set; }

		public TypedDatapoint<Boolean> Duty_Cycle_C0 { get; private set; }

		public TypedDatapoint<String> Error_Code_C0 { get; private set; }

		public TypedDatapoint<Boolean> Low_Bat_C0 { get; private set; }

		public TypedDatapoint<Decimal> Operating_Voltage_C0 { get; private set; }

		public TypedDatapoint<Int32> Operating_Voltage_Status_C0 { get; private set; }

		public TypedDatapoint<String> Rssi_Device_C0 { get; private set; }

		public TypedDatapoint<String> Rssi_Peer_C0 { get; private set; }

		public TypedDatapoint<Boolean> Sabotage_C0 { get; private set; }

		public TypedDatapoint<Boolean> Unreach_C0 { get; private set; }

		public TypedDatapoint<Boolean> Update_Pending_C0 { get; private set; }

		public TypedDatapoint<Int32> State_C1 { get; private set; }


      public HMIP_SWDO()
      {
			Config_Pending_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["CONFIG_PENDING"]);

			Duty_Cycle_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["DUTY_CYCLE"]);

			Error_Code_C0 = new TypedDatapoint<String>(base.Channels[0].Datapoints["ERROR_CODE"]);

			Low_Bat_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["LOW_BAT"]);

			Operating_Voltage_C0 = new TypedDatapoint<Decimal>(base.Channels[0].Datapoints["OPERATING_VOLTAGE"]);

			Operating_Voltage_Status_C0 = new TypedDatapoint<Int32>(base.Channels[0].Datapoints["OPERATING_VOLTAGE_STATUS"]);

			Rssi_Device_C0 = new TypedDatapoint<String>(base.Channels[0].Datapoints["RSSI_DEVICE"]);

			Rssi_Peer_C0 = new TypedDatapoint<String>(base.Channels[0].Datapoints["RSSI_PEER"]);

			Sabotage_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["SABOTAGE"]);

			Unreach_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["UNREACH"]);

			Update_Pending_C0 = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["UPDATE_PENDING"]);

			State_C1 = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["STATE"]);

      }
  }
}
