using KSP.UI.Screens;
using UnityEngine;

namespace YourModNamespace
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class WaypointAlarmModule : MonoBehaviour
    {
        private static ApplicationLauncherButton toolbarButton;
        private static Waypoint currentWaypoint;

        private void Start()
        {
            GameEvents.onGUIApplicationLauncherReady.Add(OnGUIApplicationLauncherReady);
            GameEvents.onGUIApplicationLauncherUnreadifying.Add(OnGUIApplicationLauncherUnreadifying);
        }

        private void OnGUIApplicationLauncherReady()
        {
            if (ApplicationLauncher.Ready)
            {
                Texture2D buttonTexture = GameDatabase.Instance.GetTexture("QAB/Textures/ToolbarIcon", false);
                toolbarButton = ApplicationLauncher.Instance.AddModApplication(
                    OnToolbarButtonToggleOn,
                    OnToolbarButtonToggleOff,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH,
                    buttonTexture);
            }
        }

        private void OnGUIApplicationLauncherUnreadifying(GameScenes scene)
        {
            if (toolbarButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(toolbarButton);
            }
        }

        private void OnToolbarButtonToggleOn()
        {
            currentWaypoint = FlightGlobals.ActiveVessel?.targetObject as Waypoint;
            if (currentWaypoint != null)
            {
                // Calculate the time until the waypoint and add an alarm notification
                double timeToWaypoint = currentWaypoint.UT - Planetarium.GetUniversalTime();
                AlarmClockScenario.AddAlarm(
                    new KACWrapper.KACAPI.AlarmObject(FlightGlobals.ActiveVessel.vesselName, timeToWaypoint, "Waypoint Alarm"));
            }
        }

        private void OnToolbarButtonToggleOff()
        {
            // Remove the notification for the current waypoint
            if (currentWaypoint != null)
            {
                AlarmClockScenario.DeleteAlarm(currentWaypoint.UT, FlightGlobals.ActiveVessel.vesselName);
            }
        }
    }
}