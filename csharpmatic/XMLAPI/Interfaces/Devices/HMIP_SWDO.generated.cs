/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */

using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharpmatic.XMLAPI.Interfaces.Devices
{
  public partial class HMIP_SWDO : Device
  {
		public TypedDatapoint<Boolean> Config_Pending { get; private set; }

		public TypedDatapoint<Boolean> Duty_Cycle { get; private set; }

		public TypedDatapoint<String> Error_Code { get; private set; }

		public TypedDatapoint<Boolean> Low_Bat { get; private set; }

		public TypedDatapoint<Decimal> Operating_Voltage { get; private set; }

		public TypedDatapoint<Int32> Operating_Voltage_Status { get; private set; }

		public TypedDatapoint<String> Rssi_Device { get; private set; }

		public TypedDatapoint<String> Rssi_Peer { get; private set; }

		public TypedDatapoint<Boolean> Sabotage { get; private set; }

		public TypedDatapoint<Boolean> Unreach { get; private set; }

		public TypedDatapoint<Boolean> Update_Pending { get; private set; }

		public TypedDatapoint<Int32> State { get; private set; }


      public HMIP_SWDO()
      {
			Config_Pending = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["CONFIG_PENDING"]);

			Duty_Cycle = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["DUTY_CYCLE"]);

			Error_Code = new TypedDatapoint<String>(base.Channels[0].Datapoints["ERROR_CODE"]);

			Low_Bat = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["LOW_BAT"]);

			Operating_Voltage = new TypedDatapoint<Decimal>(base.Channels[0].Datapoints["OPERATING_VOLTAGE"]);

			Operating_Voltage_Status = new TypedDatapoint<Int32>(base.Channels[0].Datapoints["OPERATING_VOLTAGE_STATUS"]);

			Rssi_Device = new TypedDatapoint<String>(base.Channels[0].Datapoints["RSSI_DEVICE"]);

			Rssi_Peer = new TypedDatapoint<String>(base.Channels[0].Datapoints["RSSI_PEER"]);

			Sabotage = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["SABOTAGE"]);

			Unreach = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["UNREACH"]);

			Update_Pending = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["UPDATE_PENDING"]);

			State = new TypedDatapoint<Int32>(base.Channels[1].Datapoints["STATE"]);

      }
  }
}
