using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components.Archived
{
    /*[RegisterTypeInIl2Cpp]
    internal class SpawnFoodForCubs : SRBehaviour
    {
        private TimeDirector timeDir;
        private SunBearCache sunBearCache;
        private int foodSpawnCount;
        private double nextFeedingTime;

        public IdentifiableType foodType;
        public float cubRadius = 10;
        public float hoursBetweenEachFeeding = 6;

        void Start()
        {
            timeDir = SceneContext.Instance.TimeDirector;
            sunBearCache = GetComponent<SunBearCache>();

            nextFeedingTime = timeDir.HoursFromNowOrStart(hoursBetweenEachFeeding);

            if (foodType is null)
                foodType = Get<IdentifiableType>("DeepBrineCraft");

            sunBearCache.RefreshCubCache();
            RefreshFoodSpawnCount();
        }

        void Update()
        {
            if (timeDir.HasReached(nextFeedingTime) && sunBearCache.GetCubCache().Count > 0 && nextFeedingTime != default)
            {
                if (GetComponent<SlimeEmotions>().GetCurr(SlimeEmotions.Emotion.HUNGER) > 0.9f)
                    return;
                sunBearCache.RefreshCubCache();
                RefreshFoodSpawnCount();
                Provide();
                nextFeedingTime = timeDir.HoursFromNowOrStart(hoursBetweenEachFeeding);
            }
        }

        void RefreshFoodSpawnCount()
        {
            if (sunBearCache.IsCubsNearby(cubRadius))
                foodSpawnCount = sunBearCache.GetNearbyCubs(cubRadius).Count;
        }

        void Provide()
        {
            foreach (GameObject obj in sunBearCache.GetCubCache())
            {
                if (Vector3.Distance(transform.position, obj.transform.position) <= cubRadius)
                {
                    if (!(obj.GetComponent<SlimeEmotions>().GetCurr(SlimeEmotions.Emotion.HUNGER) > 0.5f) || !(obj.GetComponent<SlimeEmotions>().GetCurr(SlimeEmotions.Emotion.AGITATION) > 0.5f))
                        continue;
                    for (int i = 0; i < foodSpawnCount; i++)
                    {
                        GameObject instantiatedFood = InstantiateActor(foodType.prefab, SceneContext.Instance.RegionRegistry.CurrentSceneGroup, obj.transform.position, Quaternion.identity);
                        SpawnAndPlayFX(Get<SlimeDefinition>("Pink").prefab.GetComponent<SlimeEat>().ProduceFX, instantiatedFood.transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }*/
}
