using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearSavage : SRBehaviour
    {
        private SlimeDefinition slimeDefinition;
        private SlimeEmotions slimeEmotions;
        private SunBearAttack sunBearAttack;
        private SunBearCache sunBearCache;
        private GameObject target;
        private bool isCurrentlySavage;

        void Start()
        {
            slimeDefinition = GetComponent<IdentifiableActor>().identType.Cast<SlimeDefinition>();
            slimeEmotions = GetComponent<SlimeEmotions>();
            sunBearAttack = GetComponent<SunBearAttack>();
            sunBearCache = GetComponent<SunBearCache>();
        }

        void Update()
        {
            if (target == null && IsSavage())
                StopSavage();

            if (target != null && IsSavage())
            {
                if (slimeEmotions.GetCurr(SlimeEmotions.Emotion.AGITATION) < 0.9f)
                    StopSavage();
            }

            if (target == null && !IsSavage() && !sunBearAttack.IsAttacking())
            {
                if (slimeEmotions.GetCurr(SlimeEmotions.Emotion.AGITATION) >= 0.9f)
                {
                    if (!slimeDefinition.IsLargo)
                    {
                        if (sunBearCache.GetSlimeLargoCount() > 0)
                        {
                            if (sunBearCache.IsSlimesOrLargosNearby())
                            {
                                // MelonLogger.Msg("Going savage");
                                GameObject randomPrey = sunBearCache.GetNearbySlimesOrLargos()[new System.Random().Next(0, sunBearCache.GetNearbySlimesOrLargos().Count)];
                                StartSavage(randomPrey);
                            }
                        }
                    }
                    else
                        StartSavage(SceneContext.Instance.Player);
                }
            }
        }

        public bool IsSavage() => isCurrentlySavage;

        public void StopSavage()
        {
            target = null;
            if (sunBearAttack.IsAttacking())
                sunBearAttack.StopAttack();
            isCurrentlySavage = false;

            // MelonLogger.Msg("Stopped savage");
        }

        public void StartSavage(GameObject prey)
        {
            if (prey == null)
                return;

            if (sunBearAttack.IsAttacking())
                return;

            if (!prey.GetComponent<IdentifiableActor>())
                return;

            if (prey.GetComponent<IdentifiableActor>().identType == Get<SlimeDefinition>("CubSunBear") && !slimeDefinition.IsLargo)
            {
                if (!(UnityEngine.Random.Range(0f, 1f) <= 0.08f))
                    return;
            }

            target = prey;
            sunBearAttack.StartAttack(target);
            if (target.GetComponent<IdentifiableActor>().identType == Get<SlimeDefinition>("SunBear") && !slimeDefinition.IsLargo)
                target.GetComponent<SunBearAttack>().StartAttack(gameObject);
            sunBearAttack.EnableFindConsumables();
            isCurrentlySavage = true;

            // MelonLogger.Msg("Went savage");
        }
    }
}
