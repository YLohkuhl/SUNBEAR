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
    internal class SunBearProtection : SRBehaviour
    {
        private SunBearCache sunBearCache;
        private SunBearAttack sunBearAttack;
        private SlimeEmotions slimeEmotions;
        private SphereCollider sphereCollider;

        private IdentifiableTypeGroup slimesGroup;
        private IdentifiableTypeGroup largoGroup;
        private IdentifiableType targetIdent;
        private GameObject slimeObject;
        private GameObject target;
        private bool isCurrentlyProtecting;
        private float defaultRadius;

        void Start()
        {
            slimeObject = transform.GetComponentInParent<IdentifiableActor>().gameObject;
            sunBearCache = GetComponentInParent<SunBearCache>();
            sunBearAttack = GetComponentInParent<SunBearAttack>();
            slimeEmotions = GetComponentInParent<SlimeEmotions>();
            sphereCollider = GetComponent<SphereCollider>();
            slimesGroup = Get<IdentifiableTypeGroup>("SlimesGroup");
            largoGroup = Get<IdentifiableTypeGroup>("LargoGroup");

            defaultRadius = sphereCollider.radius;
        }

        void Update()
        {
            if (target == null && IsProtecting())
                FinishProtection();

            if (!sunBearAttack.IsAttacking() && IsProtecting())
                FinishProtection();

            /*if (transform.FindChild("tempVisual").gameObject)
            {
                GameObject tempVisual = transform.FindChild("tempVisual").gameObject;
                float radius = sphereCollider.radius;
                tempVisual.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
            }*/
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject == null)
                return;

            if (sunBearAttack.IsAttacking() && IsProtecting())
                return;

            DecideProtection(collider.gameObject);
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject == null)
                return;

            if (!sunBearAttack.IsAttacking() && !IsProtecting())
                return;

            if (collider.gameObject == target)
            {
                /*if (targetIdent.IsPlayer)
                    FinishProtection();
                else if (UnityEngine.Random.Range(0f, 1f) <= 0.3f)
                    FinishProtection();*/
                FinishProtection();
            }
        }

        bool IsProtecting() => isCurrentlyProtecting;

        bool IsAgitatedOrHungry()
        {
            if (slimeEmotions.GetCurr(SlimeEmotions.Emotion.HUNGER) >= 0.99f || slimeEmotions.GetCurr(SlimeEmotions.Emotion.AGITATION) >= 0.9f)
                return true;
            return false;
        }

        void FinishProtection()
        {
            if (target != null)
            {
                if (targetIdent == Get<SlimeDefinition>("SunBear"))
                    target.GetComponent<SunBearAttack>().StopAttack();
            }

            target = null;
            targetIdent = null;
            sphereCollider.radius = defaultRadius;
            sunBearAttack.StopAttack();
            isCurrentlyProtecting = false;
        }

        void InitiateProtection(GameObject potentialThreat, IdentifiableType potentialThreatIdent)
        {
            target = potentialThreat;
            targetIdent = potentialThreatIdent;
            sphereCollider.radius = defaultRadius + 10;

            sunBearAttack.StartAttack(potentialThreat);
            if (targetIdent == Get<SlimeDefinition>("SunBear"))
                target.gameObject.GetComponent<SunBearAttack>().StartAttack(slimeObject);

            isCurrentlyProtecting = true;
            // MelonLogger.Msg("Decided to attack");
        }

        void DecideProtection(GameObject potentialThreat)
        {
            if (potentialThreat == null)
                return;

            if (!potentialThreat.GetComponent<IdentifiableActor>() || !potentialThreat.GetComponent<IdentifiableActor>().identType)
                return;

            IdentifiableType potentialThreatIdent = potentialThreat.GetComponent<IdentifiableActor>().identType;

            if (!(slimesGroup.IsMember(potentialThreatIdent) || largoGroup.IsMember(potentialThreatIdent)))
            {
                if (!potentialThreatIdent.IsPlayer)
                    return;
            }

            if (potentialThreatIdent == Get<SlimeDefinition>("CubSunBear"))
            {
                if (potentialThreat.GetComponent<SunBearFollow>().targetBear == slimeObject)
                    return;

                if (!(UnityEngine.Random.Range(0f, 1f) <= 0.05f))
                    return;
            }

            float rand = UnityEngine.Random.Range(0f, 1f);
            float probability = 0.1f;

            if (IsAgitatedOrHungry())
            {
                probability += 0.2f;
                if (sunBearCache.IsCubsNearby())
                    probability += 0.5f;
            }

            if (sunBearCache.IsCubsNearby() && !IsAgitatedOrHungry())
                probability += 0.4f;

            if (potentialThreatIdent == Get<SlimeDefinition>("SunBear"))
                probability += 0.3f;

            if (potentialThreatIdent == Get<SlimeDefinition>("Tarr"))
                probability += 1;

            // MelonLogger.Msg(rand.ToString());
            if (rand <= probability)
                InitiateProtection(potentialThreat, potentialThreatIdent);

            // MelonLogger.Msg("Decided not to attack");
        }
    }
}
