using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.Slime;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearFollow : SRBehaviour
    {
        private TimeDirector timeDir;
        private SunBearCache sunBearCache;
        private SunBearPlayful sunBearPlayful;
        private SunBearGoto sunBearGoto;
        private Chomper chomper;

        internal GameObject targetBear;
        private bool isCurrentlyFollowing;
        private double nextSearchTime;
        private float nextLeapAvail;

        public float hoursBetweenEachSearch = 2;
        public float followRadius = 10;

        void Start()
        {
            timeDir = SceneContext.Instance.TimeDirector;
            sunBearCache = GetComponent<SunBearCache>();
            sunBearPlayful = GetComponent<SunBearPlayful>();
            sunBearGoto = GetComponent<SBGeneralizedBehaviour>().sunBearGoto;
            chomper = GetComponent<Chomper>();

            if (targetBear == null)
            {
                if (sunBearCache.GetBearCount() > 0)
                {
                    if (sunBearCache.IsBearsNearby())
                    {
                        GameObject randomBear = sunBearCache.GetNearbyBears()[new System.Random().Next(0, sunBearCache.GetNearbyBears().Count)];
                        AttemptSearch(randomBear);
                    }
                }
            }

            nextSearchTime = timeDir.HoursFromNowOrStart(hoursBetweenEachSearch);
        }

        void Update()
        {
            if (targetBear == null && IsFollowing())
                isCurrentlyFollowing = false;

            if (timeDir.HasReached(nextSearchTime) && targetBear == null)
            {
                if (sunBearCache.GetBearCount() > 0)
                {
                    if (sunBearCache.IsBearsNearby())
                    {
                        GameObject randomBear = sunBearCache.GetNearbyBears()[new System.Random().Next(0, sunBearCache.GetNearbyBears().Count)];
                        AttemptSearch(randomBear);
                    }
                }
                nextSearchTime = timeDir.HoursFromNowOrStart(hoursBetweenEachSearch);
            }
        }

        void FixedUpdate()
        {
            if (targetBear != null && !SRSingleton<SceneContext>.Instance.TimeDirector.IsFastForwarding() && !chomper.IsChomping() && IsFollowing())
            {
                var distance = Vector3.Distance(transform.position, targetBear.transform.position);
                if (distance > followRadius)
                {
                    if (sunBearPlayful.enabled == true)
                        sunBearPlayful.enabled = false;
                    sunBearGoto.MoveTowards(targetBear.transform.position + Vector3.up, sunBearGoto.IsBlocked(targetBear), ref nextLeapAvail, DriveToJumpiness(float.PositiveInfinity) * 6);
                    // MelonLogger.Msg("following");
                }
                else
                {
                    if (sunBearPlayful.enabled == false)
                        sunBearPlayful.enabled = true;
                }
            }
        }

        public bool IsFollowing() => isCurrentlyFollowing;

        float DriveToJumpiness(float drive)
        {
            float num = Mathf.Max(0f, drive - 0.666f) / 0.334f;
            return Mathf.Lerp(0.4f, 1f, num * num);
        }

        public void AttemptSearch(GameObject bear)
        {
            if (bear == null)
                return;

            if (bear.transform == null)
                return;

            if (sunBearCache.GetBearCache().Contains(bear))
            {
                targetBear = bear;
                isCurrentlyFollowing = true;
            }
        }
    }
}
