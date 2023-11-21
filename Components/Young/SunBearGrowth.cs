using Il2CppDG.Tweening;
using Il2CppDG.Tweening.Core;
using Il2CppDG.Tweening.Plugins.Options;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearGrowth : SRBehaviour
    {
        private TimeDirector timeDir;
        private SlimeEmotions slimeEmotions;
        private double timeTillGrowth;

        public Vector3 growthScale = new Vector3(1.5f, 1.5f, 1.5f);
        public float growthDuration = 2;
        public float minGrowthHours = 24 * 3;
        public float maxGrowthHours = 24 * 5;

        void Start()
        {
            timeDir = SceneContext.Instance.TimeDirector;
            slimeEmotions = GetComponent<SlimeEmotions>();
            timeTillGrowth = timeDir.HoursFromNowOrStart(UnityEngine.Random.Range(minGrowthHours, maxGrowthHours));
        }

        void Update()
        {
            if (timeDir.HasReached(timeTillGrowth) && timeTillGrowth != default)
            {
                if (slimeEmotions.GetCurr(SlimeEmotions.Emotion.HUNGER) >= 0.99f || slimeEmotions.GetCurr(SlimeEmotions.Emotion.AGITATION) >= 0.9f || slimeEmotions.GetCurr(SlimeEmotions.Emotion.FEAR) >= 0.6f)
                    return;
                if (timeDir.IsFastForwarding())
                    return;
                ProcessGrowth();
                timeTillGrowth = default;
            }
        }

        void ProcessGrowth()
        {
            TweenCallback onTweenComplete = new Action(() =>
            {
                Destroyer.DestroyActor(gameObject, "SunBearGrowth.ProcessGrowth");
                InstantiateActor(Get<SlimeDefinition>("SunBear").prefab, SceneContext.Instance.RegionRegistry.CurrentSceneGroup, transform.position, transform.rotation);
                // MelonLogger.Msg("Finished Growth.");
            });
            ShortcutExtensions.DOScale(transform, growthScale, growthDuration).OnComplete(onTweenComplete);
        }
    }
}
