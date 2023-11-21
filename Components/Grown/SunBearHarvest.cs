using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.Damage;
using Il2CppMonomiPark.SlimeRancher.DataModel;
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
    internal class SunBearHarvest : CollidableActorBehaviour
    {
        private SunBearAttack sunBearAttack;
        private SunBearCache sunBearCache;
        private SunBearGoto sunBearGoto;
        private SlimeRandomMove slimeRandomMove;
        private SlimeEmotions slimeEmotions;
        private RegionMember regionMember;
        // private Vacuumable vacuumable;
        private GotoPlayer gotoPlayer;
        private SlimeAudio slimeAudio;
        private Chomper chomper;

        private GameObject targetNode;
        private ResourceNode targetResourceNode;
        private ResourceNodeDefinition wildHoneyNodeDefinition;
        private ResourceNodeSpawnerModel targetNodeModel;
        private bool isCurrentlyHarvesting;
        private float nextLeapAvail;

        new void Start()
        {
            sunBearAttack = GetComponent<SunBearAttack>();
            sunBearCache = GetComponent<SunBearCache>();
            slimeRandomMove = GetComponent<SlimeRandomMove>();
            sunBearGoto = GetComponent<SBGeneralizedBehaviour>().sunBearGoto;
            slimeEmotions = GetComponent<SlimeEmotions>();
            regionMember = GetComponent<RegionMember>();
            // vacuumable = GetComponent<Vacuumable>();
            gotoPlayer = GetComponent<GotoPlayer>();
            slimeAudio = GetComponent<SlimeAudio>();
            chomper = GetComponent<Chomper>();

            wildHoneyNodeDefinition = Get<ResourceNodeDefinition>("WildHoneyNode");
        }

        void Update()
        {
            if (CellDirector.IsOnRanch(regionMember))
                return;

            if (targetNode == null && IsHarvesting())
                StopHarvest();

            if (targetNode != null && !IsHarvesting())
                StopHarvest();

            if (targetNode != null && !IsHiveReady())
                StopHarvest();

            if (sunBearAttack)
            {
                if (sunBearAttack.IsAttacking() && IsHarvesting())
                    StopHarvest();
            }

            if (targetNode != null && IsHarvesting())
            {
                if (!sunBearCache.GetHiveCache().Contains(targetNode))
                    StopHarvest();

                if (slimeEmotions.GetCurr(SlimeEmotions.Emotion.HUNGER) < 0.666f)
                    StopHarvest();
            }

            if (targetNode == null && !IsHarvesting())
            {
                if (slimeEmotions.GetCurr(SlimeEmotions.Emotion.HUNGER) >= 0.666f)
                {
                    if (sunBearCache.GetHiveCount() > 0)
                    {
                        if (sunBearCache.IsHivesNearby())
                        {
                            GameObject randomHive = sunBearCache.GetNearbyHives()[new System.Random().Next(0, sunBearCache.GetNearbyHives().Count)];
                            StartHarvest(randomHive);
                        }
                    }
                }
            }
        }

        void FixedUpdate()
        {
            if (targetNode != null && !SRSingleton<SceneContext>.Instance.TimeDirector.IsFastForwarding() && !chomper.IsChomping() && IsHarvesting())
                sunBearGoto.MoveTowards(targetNode.transform.position + Vector3.up, sunBearGoto.IsBlocked(targetNode), ref nextLeapAvail, gotoPlayer.DriveToJumpiness(float.PositiveInfinity) * gotoPlayer.MaxJump + 4);
        }

        void OnCollisionEnter(Collision collision) 
        {
            if (targetNode == null)
                return;

            if (!IsHarvesting())
                return;

            if (!IsHiveReady())
                return;

            if (collision.gameObject == targetNode)
                AttemptSpinAndBite();
        }

        public bool IsHarvesting() => isCurrentlyHarvesting;

        public bool IsWildHive()
        {
            if (targetNode)
            {
                if (targetNodeModel.resourceNodeDefinition == wildHoneyNodeDefinition)
                    return true;
                return false;
            }
            return false;
        }

        public bool IsHiveReady()
        {
            if (IsWildHive())
            {
                if (targetNodeModel.nodeState == ResourceNode.NodeState.READY)
                    return true;
                return false;
            }
            return false;
        }

        public void StartHiveChomp(bool quick, bool invokeCompleteBite = true)
        {
            chomper._faceAnim.SetTrigger(quick ? "triggerChompOpenQuick" : "triggerChompOpen");
            chomper._bodyAnim.SetBool(quick ? chomper._animQuickBiteId : chomper._animBiteId, true);

            if (invokeCompleteBite)
                CompleteBite(quick);
        }

        bool AttemptSpinAndBite()
        {
            if (chomper.CanChomp() && targetNode != null && IsHiveReady())
            {
                transform.LookAt(targetNode.transform);
                StartHiveChomp(true);
                return true;
            }
            return false;
        }

        void CompleteBite(bool quick)
        {
            if (targetNode == null)
                return;

            if (!IsHiveReady())
                return;

            slimeAudio.Play(slimeAudio.SlimeSounds.AttackClipDefinition);
            targetResourceNode.SpawnResources();
            chomper._bodyAnim.SetBool(quick ? chomper._animQuickBiteId : chomper._animBiteId, false);
            StopHarvest();
        }

        public void StopHarvest()
        {
            targetNode = null;
            targetNodeModel = null;
            targetResourceNode = null;

            // vacuumable.enabled = true;
            slimeRandomMove.enabled = true;
            isCurrentlyHarvesting = false;

            // MelonLogger.Msg("Harvest over");
        }

        public void StartHarvest(GameObject hive)
        {
            if (hive == null)
                return;

            if (sunBearAttack)
            {
                if (sunBearAttack.IsAttacking())
                    return;
            }

            if (hive.GetComponent<ResourceNode>())
            {
                if (!hive.GetComponent<ResourceNode>()._model.resourceNodeDefinition == wildHoneyNodeDefinition)
                    return;

                if (!(hive.GetComponent<ResourceNode>()._model.nodeState == ResourceNode.NodeState.READY))
                    return;
            }
            else
                return;

            targetNode = hive;
            targetNodeModel = hive.GetComponent<ResourceNode>()._model;
            targetResourceNode = hive.GetComponent<ResourceNode>();

            // vacuumable.enabled = false;
            slimeRandomMove.enabled = false;
            isCurrentlyHarvesting = true;

            // MelonLogger.Msg("Harvesting");
        }
    }
}
