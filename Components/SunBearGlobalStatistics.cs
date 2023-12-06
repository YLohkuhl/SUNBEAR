using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearGlobalStatistics : MonoBehaviour
    {
        private const float PLORTONOMICS_WAIT_TIME = 12;

        private static TimeDirector timeDir;
        private static double nextPlortonomicsUsableTime;

        public static bool IsPlortonomicsUsable { get; private set; }

        static SunBearGlobalStatistics()
        {
            IsPlortonomicsUsable = true;
        }

        void Awake() => timeDir = SceneContext.Instance.TimeDirector;

        void Update()
        {
            if (!IsPlortonomicsUsable && nextPlortonomicsUsableTime == default)
                RestartNextPlortonomicsTime();

            if (!timeDir.HasReached(nextPlortonomicsUsableTime) && IsPlortonomicsUsable)
                SetPlortonomicsUsable(false);

            if (timeDir.HasReached(nextPlortonomicsUsableTime) && !IsPlortonomicsUsable)
            {
                SetPlortonomicsUsable(true);
                nextPlortonomicsUsableTime = default;
            }
        }

        public static bool GetPlortonomicsUsable() => IsPlortonomicsUsable;

        public static bool SetPlortonomicsUsable(bool value) => IsPlortonomicsUsable = value;

        public static double RestartNextPlortonomicsTime() => nextPlortonomicsUsableTime = timeDir.HoursFromNowOrStart(PLORTONOMICS_WAIT_TIME);
    }
}
