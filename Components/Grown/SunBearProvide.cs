using Il2CppMonomiPark.SlimeRancher.Regions;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearProvide : SRBehaviour
    {
        private TimeDirector timeDir;
        private SunBearCache sunBearCache;
        private SlimeEmotions slimeEmotions;
        private double nextFeedingTime;

        public float hoursBetweenEachFeeding = 3;

        void Start()
        {
            timeDir = SceneContext.Instance.TimeDirector;
            sunBearCache = GetComponent<SunBearCache>();
            slimeEmotions = GetComponent<SlimeEmotions>();
            nextFeedingTime = timeDir.HoursFromNowOrStart(hoursBetweenEachFeeding);
        }

        void Update()
        {
            if (timeDir.HasReached(nextFeedingTime) && sunBearCache.GetCubCount() > 0)
            {
                if (slimeEmotions.GetCurr(SlimeEmotions.Emotion.HUNGER) >= 0.99f)
                    return;
                Provide();
                nextFeedingTime = timeDir.HoursFromNowOrStart(hoursBetweenEachFeeding);
            }
        }

        void Provide()
        {
            if (sunBearCache.IsCubsNearby())
            {
                foreach (GameObject obj in sunBearCache.GetNearbyCubs())
                {
                    if (sunBearCache.GetCubCache().Contains(obj))
                    {
                        SlimeEmotions cubEmotions = obj.GetComponent<SlimeEmotions>();

                        if (!(cubEmotions.GetCurr(SlimeEmotions.Emotion.HUNGER) >= 0.666f))
                            continue;

                        cubEmotions.Adjust(SlimeEmotions.Emotion.HUNGER, -0.666f);

                        if (cubEmotions.GetCurr(SlimeEmotions.Emotion.AGITATION) > 0)
                            cubEmotions.Adjust(SlimeEmotions.Emotion.AGITATION, -cubEmotions.GetCurr(SlimeEmotions.Emotion.AGITATION));
                    }
                }
            }
        }
    }
}
