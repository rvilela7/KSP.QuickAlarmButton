using FinePrint;
using KSP.UI.Screens;
using System;
using UnityEngine;

namespace WaypointAlarmMod
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class WaypointAlarmModule : MonoBehaviour
    {
        private static ApplicationLauncherButton toolbarButton;
        private static Waypoint currentWaypoint;

        private const string ButtonImage1 = "QAB/Textures/AddAlarmBt.png";

        private void Start()
        {
            GameEvents.onGUIApplicationLauncherReady.Add(OnGUIApplicationLauncherReady);
            GameEvents.onGUIApplicationLauncherUnreadifying.Add(OnGUIApplicationLauncherUnreadifying);
        }

        private void OnGUIApplicationLauncherReady()
        {
            if (ApplicationLauncher.Ready)
            {
                Texture2D buttonTexture = GameDatabase.Instance.GetTexture(ButtonImage1, false);
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
            if (FlightGlobals.ActiveVessel != null)
            {
                CelestialBody currentBody = FlightGlobals.ActiveVessel.mainBody;
                Vector3d vesselPosition = FlightGlobals.ActiveVessel.GetWorldPos3D();
                Vector3d waypointPosition = currentWaypoint.celestialBody.GetWorldSurfacePosition(
                    currentWaypoint.latitude, currentWaypoint.longitude, currentWaypoint.altitude);

                // Calculate the time until the vessel reaches the waypoint
                double timeToWaypoint = CalculateTimeToWaypoint(currentBody, vesselPosition, waypointPosition);
                StartCoroutine(SetWaypointAlarm(timeToWaypoint));
            }
        }

        private void OnToolbarButtonToggleOff()
        {
            // Remove the notification for the current waypoint
            if (currentWaypoint != null)
            {
                // Implement alarm removal logic here
            }
        }

        private System.Collections.IEnumerator SetWaypointAlarm(double timeToWaypoint)
        {
            yield return new WaitForSeconds((float)timeToWaypoint);

            // Implement alarm notification logic here
            Debug.Log("Alarm triggered for waypoint!");
        }

        private double CalculateTimeToWaypoint(CelestialBody body, Vector3d vesselPosition, Vector3d waypointPosition)
        {
            // Get the semimajor axis of the vessel's orbit (assuming a circular orbit)
            double semiMajorAxis = vesselPosition.magnitude;

            // Calculate the velocity of the vessel in the circular orbit
            double orbitalVelocity = Math.Sqrt(body.gravParameter / semiMajorAxis);

            // Calculate the distance between the vessel and the waypoint
            double distanceToWaypoint = Vector3d.Distance(vesselPosition, waypointPosition);

            // Calculate the time it takes for the vessel to cover the distance to the waypoint
            double timeToWaypoint = distanceToWaypoint / orbitalVelocity;

            return timeToWaypoint;
        }
    }
}
