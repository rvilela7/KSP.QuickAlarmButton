using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QAB.QuickAlarmSet
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class QuickAlarmSet : MonoBehaviour
    {
        AlarmTB alarm = new AlarmTB();

    }
}