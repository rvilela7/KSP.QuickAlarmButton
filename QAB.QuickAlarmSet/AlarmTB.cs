using KSP.UI;
using KSP.UI.Screens;

namespace QAB.QuickAlarmSet
{
    public class AlarmTB : AlarmTypeBase
    {
        [AppUI_InputDateTime(guiName = "Alarm Date", datetimeMode = AppUIMemberDateTime.DateTimeModes.date)]
        public double alarmDate;
        public Alarm()
        {
            // iconURL = "TriggerTech/TestMod/AlarmIcons/RvTest";
        }

        public override bool CanSetAlarm(AlarmUIDisplayMode displayMode)
        {
            return true;
        }

        public override string GetDefaultTitle() => "Test Alarm";

        public override bool RequiresVessel() => false;

        public override void OnUIEndInitialization(AlarmUIDisplayMode displayMode)
        {
            if (displayMode == AlarmUIDisplayMode.Add)
            {
                alarmDate = Planetarium.GetUniversalTime() + 600;
            }
            AppUIMemberDateTime m = (AppUIMemberDateTime)uiPanel.GetControl("alarmDate");
            m.DatetimeMode = AppUIMemberDateTime.DateTimeModes.date;
        }

        public override void OnInputPanelUpdate(AlarmUIDisplayMode displayMode)
        {
            ut = alarmDate;
        }
    }
}