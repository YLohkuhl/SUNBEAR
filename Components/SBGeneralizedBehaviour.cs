using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SBGeneralizedBehaviour : SRBehaviour
    {
        // -- Default
        public bool isCub { get; set; }
        public bool IsCub { get { return isCub; } }

        private TimeDirector timeDir;
        private SlimeEmotions slimeEmotions;
        internal SlimeRandomMove slimeRandomMove;
        internal SunBearGoto sunBearGoto;

        // -- Auto Set Movement Fields
        private float maxJump;
        private float scootSpeedFactor;
        private float pursuitSpeedFactor;

        // -- Sun Bear Death
        private double timeTillDeath;

        public float hoursTillTimeOfDeath = 24 * 4;

        void Awake()
        {
            // -- Default
            isCub = SunBearCache.IsCub(gameObject);

            timeDir = SceneContext.Instance.TimeDirector;
            slimeEmotions = GetComponent<SlimeEmotions>();
            slimeRandomMove = GetComponent<SlimeRandomMove>();
            sunBearGoto = GetComponent<SunBearGoto>();

            // -- Auto Set Movement Fields

            maxJump = isCub ? 3.3f : 4;

            if (SunBearPreferences.IsRealisticMode.Value)
            {
                scootSpeedFactor = isCub ? 1 : 2.5f;
                pursuitSpeedFactor = isCub ? 1 : 2.5f;
            }
            else
            {
                scootSpeedFactor = isCub ? 0.8f : 2;
                pursuitSpeedFactor = isCub ? 0.8f : 2;
            }
        }

        void Update()
        {
            AutoSetMovementFields();
            if (SunBearPreferences.IsRealisticMode.Value)
                SunBearDeath();
        }

        /*FindConsumable TryGetFindConsumable()
        {
            // Try
            Component findConsumableComponent;
            foreach (Il2CppSystem.Type findConsumable in findConsumables)
            {
                if (TryGetComponent(findConsumable, out findConsumableComponent))
                    return findConsumableComponent.Cast<FindConsumable>();
            }
            // If Fails
            foreach (FindConsumable findConsumable in GetComponents<FindConsumable>())
            {
                if (findConsumable != null)
                    return findConsumable;
            }
            // If Fails Again
            return null;
        }*/

        void AutoSetMovementFields()
        {
            if (slimeRandomMove)
            {
                if (slimeRandomMove._maxJump != maxJump)
                {
                    slimeRandomMove._maxJump = maxJump;
                    MelonLogger.Msg("[SBGeneralizedBehaviour] MaxJump has been set back.");
                }
                if (slimeRandomMove.ScootSpeedFactor != scootSpeedFactor)
                {
                    slimeRandomMove.ScootSpeedFactor = scootSpeedFactor;
                    MelonLogger.Msg("[SBGeneralizedBehaviour] ScootSpeedFactor has been set back.");
                }
            }

            if (sunBearGoto)
            {
                if (sunBearGoto.pursuitSpeedFactor != pursuitSpeedFactor)
                {
                    sunBearGoto.pursuitSpeedFactor = pursuitSpeedFactor;
                    MelonLogger.Msg("[SBGeneralizedBehaviour] PursuitSpeedFactor has been set back.");
                }
            }
        }

        void SunBearDeath()
        {
            if (SunBearPreferences.IsRealisticMode.Value)
            {
                if (slimeEmotions)
                {
                    if (slimeEmotions.GetCurr(SlimeEmotions.Emotion.HUNGER) < 0.9f && timeTillDeath != default)
                        timeTillDeath = default;

                    if (slimeEmotions.GetCurr(SlimeEmotions.Emotion.HUNGER) >= 0.9f && timeTillDeath == default)
                        timeTillDeath = timeDir.HoursFromNowOrStart(hoursTillTimeOfDeath);

                    if (timeDir.HasReached(timeTillDeath) && timeTillDeath != default)
                    {
                        Destroyer.DestroyActor(gameObject, "SBGeneralizedBehaviour.Update");
                        FXHelpers.SpawnAndPlayFX(Get<GameObject>("slimePhosphor").GetComponent<DestroyOutsideHoursOfDay>().DestroyFX);
                        // MelonLogger.Msg("Death " + gameObject.name);
                        timeTillDeath = default;
                    }
                }
            }
        }
    }
}
