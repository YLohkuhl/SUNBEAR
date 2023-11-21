using Il2CppMonomiPark.SlimeRancher.Regions;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components.Archived
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearIsolation : SRBehaviour
    {
        private TimeDirector timeDir;
        private SunBearCache sunBearCache;
        private System.Random random = new System.Random();
        private double nextDecisionTime;

        public int minRange = 1;
        public int maxRange = 10;
        public int bearRadius = 23;
        public int decisionNumber = 2;
        public float hoursBetweenEachDecision = 0.8f;

        void Start()
        {
            timeDir = SceneContext.Instance.TimeDirector;
            sunBearCache = GetComponent<SunBearCache>();
            nextDecisionTime = timeDir.HoursFromNowOrStart(hoursBetweenEachDecision);
        }

        void Update()
        {
            if (timeDir.HasReached(nextDecisionTime) && !SRSingleton<SceneContext>.Instance.TimeDirector.IsFastForwarding())
            {
                if (Decide())
                    Attack();
                nextDecisionTime = timeDir.HoursFromNowOrStart(hoursBetweenEachDecision);
            }
        }

        bool Decide()
        {
            int rand = UnityEngine.Random.Range(minRange, maxRange);
            if (rand == decisionNumber)
                return true;
            return false;
        }

        void Attack()
        {
            if (sunBearCache.IsBearsNearby(bearRadius))
            {
                int randomIndex = random.Next(0, sunBearCache.GetNearbyBears(bearRadius).Count);
                GameObject randomBear = sunBearCache.GetNearbyBears(bearRadius)[randomIndex];

                if (randomBear == gameObject)
                    return;

                GetComponent<SunBearAttack>().StartAttack(randomBear);
                randomBear.GetComponent<SunBearAttack>().StartAttack(gameObject);
            }
        }
    }
}
