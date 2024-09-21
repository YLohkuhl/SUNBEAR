using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.Audio;
using Il2CppMonomiPark.SlimeRancher.Damage;
using Il2CppMonomiPark.SlimeRancher.Regions;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearAttack : CollidableActorBehaviour
    {
        private SunBearSavage sunBearSavage;
        private SunBearCache sunBearCache;
        private SunBearGoto sunBearGoto;
        private IdentifiableTypeGroup slimesGroup;
        private IdentifiableTypeGroup largoGroup;
        private IdentifiableType tarrIdentifiableType;
        private IdentifiableType sunBearIdentifiableType;

        private GotoPlayer gotoPlayer;
        private AttackPlayer attackPlayer;
        private SlimeDefinition slimeDefinition;
        private SlimeFaceAnimator slimeFaceAnimator;
        // private SlimeRandomMove slimeRandomMove;
        // private Vacuumable vacuumable;
        private SlimeAudio slimeAudio;
        // private SlimeEat slimeEat;
        // private Animator animator;
        private Chomper chomper;

        private GameObject target;
        private IdentifiableType targetIdent;
        // private System.Random random;
        private bool isCurrentlyAttacking;
        private bool isTargetBiggerOrEqual;
        private float nextLeapAvail;

        internal bool isInBearFriendlyArea;

        public float hoursBetweenEachRefresh = 1;

        new void Start()
        {
            sunBearSavage = GetComponent<SunBearSavage>();
            sunBearCache = GetComponent<SunBearCache>();
            sunBearGoto = GetComponent<SBGeneralizedBehaviour>().sunBearGoto;
            slimesGroup = Get<IdentifiableTypeGroup>("SlimesGroup");
            largoGroup = Get<IdentifiableTypeGroup>("LargoGroup");
            tarrIdentifiableType = Get<SlimeDefinition>("Tarr");
            sunBearIdentifiableType = Get<SlimeDefinition>("SunBear");

            gotoPlayer = GetComponent<GotoPlayer>();
            attackPlayer = GetComponent<AttackPlayer>();
            slimeDefinition = GetComponent<IdentifiableActor>().identType.Cast<SlimeDefinition>();
            slimeFaceAnimator = GetComponent<SlimeFaceAnimator>();
            // slimeRandomMove = GetComponent<SlimeRandomMove>();
            // vacuumable = GetComponent<Vacuumable>();
            slimeAudio = GetComponent<SlimeAudio>();
            // slimeEat = GetComponent<SlimeEat>();
            // animator = GetComponentInChildren<Animator>();
            chomper = GetComponent<Chomper>();
        }

        void Update()
        {
            if (target == null && IsAttacking())
                StopAttack();

            if (IsAttacking() && IsInFriendlyArea() && !sunBearSavage.IsSavage() && targetIdent != tarrIdentifiableType)
                StopAttack();
        }

        void FixedUpdate()
        {
            if (target != null && !SRSingleton<SceneContext>.Instance.TimeDirector.IsFastForwarding() && !chomper.IsChomping() && IsAttacking())
                sunBearGoto.MoveTowards(target.transform.position + Vector3.up, sunBearGoto.IsBlocked(target), ref nextLeapAvail, gotoPlayer.DriveToJumpiness(float.PositiveInfinity) * gotoPlayer.MaxJump);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (target == null)
                return;

            if (!IsAttacking())
                return;

            if (!IsSlimeTarget())
                return;

            if (collision.gameObject == target)
            {
                if (isTargetBiggerOrEqual)
                    AttemptSpinAndBite();
                else
                    AttemptSpinAndChomp();
            }
        }

        public bool IsAttacking() => isCurrentlyAttacking;

        public bool IsInFriendlyArea() => isInBearFriendlyArea;

        public bool IsTarrTarget() => targetIdent == tarrIdentifiableType;

        public bool IsSlimeTarget() => slimesGroup.IsMember(targetIdent) || largoGroup.IsMember(targetIdent);

        public bool IsPlayerTarget() { if (targetIdent) { return targetIdent.IsPlayer; } return false; }

        public bool IsSunBearTarget() => targetIdent == sunBearIdentifiableType;

        public bool IsTargetHealthLow(int currDamageAmount)
        {
            if (target == null)
                return false;

            if (target.GetComponent<SlimeHealth>()._currHealth <= currDamageAmount)
                return true;

            return false;
        }

        public void DisableFindConsumables()
        {
            foreach (FindConsumable findConsumable in GetComponents<FindConsumable>())
            {
                if (findConsumable != null)
                    findConsumable.enabled = false;
            }
        }

        public void EnableFindConsumables()
        {
            foreach (FindConsumable findConsumable in GetComponents<FindConsumable>())
            {
                if (findConsumable != null)
                    findConsumable.enabled = enabled;
            }
        }

        void DecideDamagePerAttack()
        {
            if (sunBearCache.IsCubsNearby() && !slimeDefinition.IsLargo)
            {
                if (SunBearPreferences.IsRealisticMode.Value)
                    attackPlayer.DamagePerAttack = 100;
                else
                    attackPlayer.DamagePerAttack = 80;
            }
            else
            {
                if (SunBearPreferences.IsRealisticMode.Value)
                    attackPlayer.DamagePerAttack = 80;
                else
                    attackPlayer.DamagePerAttack = 50;
            }
        }

        bool AttemptSpinAndBite()
        {
            if (chomper.CanChomp() && target != null && IsSlimeTarget())
            {
                Chomper.OnChompCompleteDelegate onChompComplete = new Action<GameObject, IdentifiableType, bool, bool>(
                    (chomped, chompedId, whileHeld, wasLaunched) => 
                    {
                        int currDamageAmount = 15;

                        if (IsTarrTarget())
                        {
                            currDamageAmount += 20;
                            if (sunBearCache.IsCubsNearby() && !slimeDefinition.IsLargo)
                                currDamageAmount += 20;
                        }

                        if (sunBearCache.IsCubsNearby() && !slimeDefinition.IsLargo && !IsTarrTarget())
                            currDamageAmount += 20;

                        if (IsSunBearTarget())
                            currDamageAmount += 20;

                        if (SunBearPreferences.IsRealisticMode.Value)
                            currDamageAmount += 5;

                        if (IsTargetHealthLow(currDamageAmount))
                            CompleteChomp();
                        else
                            CompleteBite(currDamageAmount);

                        /*MelonLogger.Msg(target.GetComponent<SlimeHealth>().currHealth.ToString() + " Target Health");
                        MelonLogger.Msg(GetComponent<SlimeHealth>().currHealth.ToString() + " Bear Health");*/
                    });
                transform.LookAt(target.transform);
                chomper.StartChomp(target, targetIdent, false, true, null, onChompComplete);
                return true;
            }
            return false;
        }

        void CompleteBite(int damageAmount)
        {
            if (target == null)
                return;

            if (!IsSlimeTarget())
                return;

            var targetBody = target.GetComponent<Rigidbody>();
            var targetAudio = target.GetComponent<SlimeAudio>();

            slimeAudio.Play(slimeAudio.SlimeSounds.AttackClipDefinition);
            targetAudio.Play(targetAudio.SlimeSounds.VoiceAlarmClipDefinition);

            if (!IsTarrTarget())
            {
                Vector3 vector = target.transform.position - transform.position;
                float calculation = Mathf.Min(1, Mathf.Sqrt(vector.sqrMagnitude) / 30);
                targetBody.AddForce((vector.normalized * calculation + Vector3.up).normalized * 12 * targetBody.mass, ForceMode.Impulse);
                targetAudio.Play(targetAudio.SlimeSounds.JumpClipDefinition);
                target.GetComponent<SlimeEmotions>().Adjust(SlimeEmotions.Emotion.FEAR, 0.2f);
            }

            target.GetComponent<SlimeHealth>().Damage(new Damage() { Amount = damageAmount, SourceObject = gameObject, DamageSource = LocalInstances.sunBearAttack });
            return;
        }

        bool AttemptSpinAndChomp()
        {
            if (chomper.CanChomp() && target != null && IsSlimeTarget())
            {
                Chomper.OnChompCompleteDelegate onChompComplete = new Action<GameObject, IdentifiableType, bool, bool>(
                    (chomped, chompedId, whileHeld, wasLaunched) => { CompleteChomp(); });
                transform.LookAt(target.transform);
                chomper.StartChomp(target, targetIdent, false, true, null, onChompComplete);
                return true;
            }
            return false;
        }

        void CompleteChomp()
        {
            if (target == null)
                return;

            if (!IsSlimeTarget())
                return;

            var animator = GetComponentInChildren<Animator>();
            var slimeEat = GetComponent<SlimeEat>();
            var targetAudio = target.GetComponent<SlimeAudio>();

            animator.SetBool(Animator.StringToHash("Digesting"), true);
            slimeAudio.Play(slimeAudio.SlimeSounds.ChompClipDefinition);
            targetAudio.Play(targetAudio.SlimeSounds.VoiceDamageClipDefinition);
            FXHelpers.SpawnAndPlayFX(slimeEat.EatFX, target.transform.position, target.transform.rotation);

            slimeAudio.Play(slimeAudio.SlimeSounds.GulpClipDefinition);
            Destroyer.DestroyActor(target, "SunBearAttack.CompleteChomp");
            slimeEat.PlayOnDeathAudio(target);

            GetComponent<SlimeEmotions>().Adjust(SlimeEmotions.Emotion.HUNGER, -0.3f);
            GetComponent<SlimeEmotions>().Adjust(SlimeEmotions.Emotion.AGITATION, -0.3f);

            if (UnityEngine.Random.Range(0f, 1f) <= 0.1f)
                StartCoroutine(slimeEat.ProduceAfterDelay(1, slimeDefinition.Diet.ProduceIdents[0].prefab, 2));
            else
                StartCoroutine(slimeEat.DigestOnlyAfterDelay(2));
            return;
        }

        public void StopAttack()
        {
            if (target != null && target.GetComponent<FleeThreats>() && !IsTarrTarget() && !IsSunBearTarget() && IsSlimeTarget())
                target.GetComponent<FleeThreats>().FearProfile = Get<FearProfile>("slimeStandardFearProfile");

            target = null;
            targetIdent = null;
            slimeFaceAnimator.ClearFeral();

            gotoPlayer.ShouldGotoPlayer = false;
            gotoPlayer.GiveUpTime = 10;
            gotoPlayer.AttemptTime = 10;
            attackPlayer.ShouldAttackPlayer = false;
            attackPlayer.DamagePerAttack = 30;

            GetComponent<Vacuumable>().enabled = true;
            GetComponent<SlimeRandomMove>().enabled = true;
            EnableFindConsumables();
            isCurrentlyAttacking = false;

            // MelonLogger.Msg("Stopped attack mode");
        }

        public void StartAttack(GameObject threat)
        {
            if (threat == null)
                return;

            // MelonLogger.Msg("Got past here 1");

            if (!threat.GetComponent<IdentifiableActor>() || !threat.GetComponent<IdentifiableActor>().identType)
                return;

            IdentifiableType threatIdent = threat.GetComponent<IdentifiableActor>().identType;
            // MelonLogger.Msg("Got past here 2");

            if (!(slimesGroup.IsMember(threatIdent) || largoGroup.IsMember(threatIdent)))
            {
                if (!threatIdent.IsPlayer)
                    return;
            }

            // MelonLogger.Msg("Got past here 3");

            target = threat;
            targetIdent = threatIdent;

            if (target.transform.localScale.magnitude >= transform.localScale.magnitude)
            { isTargetBiggerOrEqual = true; }
            else
            { isTargetBiggerOrEqual = false; }

            if (IsPlayerTarget())
            {
                slimeFaceAnimator.SetFeral();
                slimeFaceAnimator.SetState(slimeFaceAnimator._feral);

                /* gotoPlayer.shouldGotoPlayer = true;
                gotoPlayer.giveUpTime = giveUpTime;
                gotoPlayer.attemptTime = attemptTime;*/
                attackPlayer.ShouldAttackPlayer = true;
                DecideDamagePerAttack();

                GetComponent<Vacuumable>().enabled = false;
                GetComponent<SlimeRandomMove>().enabled = false;
                DisableFindConsumables();
                isCurrentlyAttacking = true;

                // MelonLogger.Msg("This ran 1");
            }
            else if (IsSlimeTarget())
            {
                slimeFaceAnimator.SetFeral();
                slimeFaceAnimator.SetState(slimeFaceAnimator._feral);

                gotoPlayer.ShouldGotoPlayer = false;
                attackPlayer.ShouldAttackPlayer = true;
                DecideDamagePerAttack();

                GetComponent<Vacuumable>().enabled = false;
                GetComponent<SlimeRandomMove>().enabled = false;
                DisableFindConsumables();
                isCurrentlyAttacking = true;

                if (!IsSunBearTarget() && !IsTarrTarget() && target.GetComponent<FleeThreats>())
                {
                    if (targetIdent == Get<SlimeDefinition>("Fire"))
                        target.GetComponent<FleeThreats>().FearProfile = LocalInstances.fireSlimeSBAFearProfile;
                    else
                        target.GetComponent<FleeThreats>().FearProfile = LocalInstances.slimeStandardSBAFearProfile;
                }

                // MelonLogger.Msg("This ran 2");
            }
        }
    }
}
