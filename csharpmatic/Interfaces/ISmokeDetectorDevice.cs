using csharpmatic.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Interfaces
{
    public interface ISmokeDetectorDevice
    {
        TypedDatapoint<String> Error_Code { get; }

        TypedDatapoint<ISmokeDetectorDevice_Smoke_Detector_Alarm_Status_Enum> Smoke_Detector_Alarm_Status { get; }

        TypedDatapoint<ISmokeDetectorDevice_Smoke_Detector_Command_Enum> Smoke_Detector_Command { get; }

        TypedDatapoint<ISmokeDetectorDevice_Smoke_Detector_Test_Result_Enum> Smoke_Detector_Test_Result { get; }
    }

    public enum ISmokeDetectorDevice_Smoke_Detector_Alarm_Status_Enum
    {
        IDLE_OFF,
        PRIMARY_ALARM,
        INTRUSION_ALARM,
        SECONDARY_ALARM
    }

    public enum ISmokeDetectorDevice_Smoke_Detector_Command_Enum
    {
        RESERVED_ALARM_OFF,
        INTRUSION_ALARM_OFF,
        INTRUSION_ALARM,
        SMOKE_TEST,
        COMMUNICATION_TEST,
        COMMUNICATION_TEST_REPEATED,
    }

    public enum ISmokeDetectorDevice_Smoke_Detector_Test_Result_Enum
    {
        NONE,
        SMOKE_TEST_OK,
        SMOKE_TEST_FAILED,
        COMMUNICATION_TEST_SENT,
        COMMUNICATION_TEST_OK
    }
}
