/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */

using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharpmatic.XMLAPI.Interfaces.Devices
{
  public partial class HMIP_SWDO : Device, IHmIPDevice
  {
		public ManagedDatapoint<System.Boolean> Config_Pending { get; private set; }

		public ManagedDatapoint<System.Boolean> Duty_Cycle { get; private set; }

		public ManagedDatapoint<String> Error_Code { get; private set; }

		public ManagedDatapoint<Boolean> Low_Bat { get; private set; }

		public ManagedDatapoint<System.Decimal> Operating_Voltage { get; private set; }

		public ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.IHmIP_Operating_Voltage_Status_Enum> Operating_Voltage_Status { get; private set; }

		public ManagedDatapoint<System.String> Rssi_Device { get; private set; }

		public ManagedDatapoint<System.String> Rssi_Peer { get; private set; }

		public ManagedDatapoint<Boolean> Sabotage { get; private set; }

		public ManagedDatapoint<System.Boolean> Unreach { get; private set; }

		public ManagedDatapoint<System.Boolean> Update_Pending { get; private set; }

		public ManagedDatapoint<Int32> State { get; private set; }


      public HMIP_SWDO(CGI.DeviceList.Device d, CGI.CGIClient CGIClient) : base(d, CGIClient)
      {
			Config_Pending = new ManagedDatapoint<System.Boolean>(base.Channels[0].Datapoints["CONFIG_PENDING"]);

			Duty_Cycle = new ManagedDatapoint<System.Boolean>(base.Channels[0].Datapoints["DUTY_CYCLE"]);

			Error_Code = new ManagedDatapoint<String>(base.Channels[0].Datapoints["ERROR_CODE"]);

			Low_Bat = new ManagedDatapoint<Boolean>(base.Channels[0].Datapoints["LOW_BAT"]);

			Operating_Voltage = new ManagedDatapoint<System.Decimal>(base.Channels[0].Datapoints["OPERATING_VOLTAGE"]);

			Operating_Voltage_Status = new ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.IHmIP_Operating_Voltage_Status_Enum>(base.Channels[0].Datapoints["OPERATING_VOLTAGE_STATUS"]);

			Rssi_Device = new ManagedDatapoint<System.String>(base.Channels[0].Datapoints["RSSI_DEVICE"]);

			Rssi_Peer = new ManagedDatapoint<System.String>(base.Channels[0].Datapoints["RSSI_PEER"]);

			Sabotage = new ManagedDatapoint<Boolean>(base.Channels[0].Datapoints["SABOTAGE"]);

			Unreach = new ManagedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UNREACH"]);

			Update_Pending = new ManagedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UPDATE_PENDING"]);

			State = new ManagedDatapoint<Int32>(base.Channels[1].Datapoints["STATE"]);

      }
  }
}
