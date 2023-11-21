using Il2CppMonomiPark.SlimeRancher.Regions;
using MelonLoader;
using SUNBEAR.Data.Slimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearCache : SRBehaviour
    {
        private readonly Il2CppSystem.Collections.Generic.List<GameObject> cubCache = new Il2CppSystem.Collections.Generic.List<GameObject>();
        private readonly Il2CppSystem.Collections.Generic.List<GameObject> bearCache = new Il2CppSystem.Collections.Generic.List<GameObject>();
        private Il2CppSystem.Collections.Generic.List<GameObject> hiveCache = new Il2CppSystem.Collections.Generic.List<GameObject>();
        private Il2CppSystem.Collections.Generic.List<GameObject> slimeLargoCache = new Il2CppSystem.Collections.Generic.List<GameObject>();

        private static IdentifiableTypeGroup slimesGroup;
        private static IdentifiableTypeGroup largoGroup;
        private static IdentifiableType cubSunBearIdentifiableType;
        private static IdentifiableType sunBearIdentifiableType;
        private static ResourceNodeDefinition wildHoneyNodeDefinition;

        void Start()
        {
            slimesGroup = Get<IdentifiableTypeGroup>("SlimesGroup");
            largoGroup = Get<IdentifiableTypeGroup>("LargoGroup");
            cubSunBearIdentifiableType = Get<IdentifiableType>("CubSunBear");
            sunBearIdentifiableType = Get<IdentifiableType>("SunBear");
            wildHoneyNodeDefinition = Get<ResourceNodeDefinition>("WildHoneyNode");
        }

        public int GetCubCount() => cubCache.Count;

        public int GetBearCount() => bearCache.Count;

        public int GetHiveCount() => hiveCache.Count;

        public int GetSlimeLargoCount() => slimeLargoCache.Count;

        public Il2CppSystem.Collections.Generic.List<GameObject> GetCubCache() => cubCache;

        public Il2CppSystem.Collections.Generic.List<GameObject> GetBearCache() => bearCache;

        public Il2CppSystem.Collections.Generic.List<GameObject> GetHiveCache() => hiveCache;

        public Il2CppSystem.Collections.Generic.List<GameObject> GetSlimeLargoCache() => slimeLargoCache;

        public bool IsCubsNearby()
        {
            List<GameObject> inRadius = GetNearbyCubs();
            if (inRadius.Count > 0)
                return true;
            return false;
        }

        public bool IsBearsNearby()
        {
            List<GameObject> inRadius = GetNearbyBears();
            if (inRadius.Count > 0)
                return true;
            return false;
        }

        public bool IsHivesNearby()
        {
            List<GameObject> inRadius = GetNearbyHives();
            if (inRadius.Count > 0)
                return true;
            return false;
        }

        public bool IsSlimesOrLargosNearby()
        {
            List<GameObject> inRadius = GetNearbySlimesOrLargos();
            if (inRadius.Count > 0)
                return true;
            return false;
        }

        public static bool IsCub(GameObject gameObject)
        {
            if (gameObject == null)
                return false;

            if (!gameObject.GetComponent<IdentifiableActor>() || !gameObject.GetComponent<IdentifiableActor>().identType)
                return false;

            return gameObject.GetComponent<IdentifiableActor>().identType == cubSunBearIdentifiableType;
        }

        public static bool IsBear(GameObject gameObject)
        {
            if (gameObject == null)
                return false;

            if (!gameObject.GetComponent<IdentifiableActor>() || !gameObject.GetComponent<IdentifiableActor>().identType)
                return false;

            return gameObject.GetComponent<IdentifiableActor>().identType == sunBearIdentifiableType;
        }

        public static bool IsHive(GameObject gameObject)
        {
            if (gameObject == null)
                return false;

            if (!gameObject.GetComponent<ResourceNode>() || !gameObject.GetComponent<ResourceNode>()._model.resourceNodeDefinition)
                return false;

            return gameObject.GetComponent<ResourceNode>()._model.resourceNodeDefinition == wildHoneyNodeDefinition;
        }

        public static bool IsSlimeOrLargo(GameObject gameObject)
        {
            if (gameObject == null)
                return false;

            if (!gameObject.GetComponent<IdentifiableActor>() || !gameObject.GetComponent<IdentifiableActor>().identType)
                return false;

            return slimesGroup.IsMember(gameObject.GetComponent<IdentifiableActor>().identType) || largoGroup.IsMember(gameObject.GetComponent<IdentifiableActor>().identType);
        }

        public List<GameObject> GetNearbyCubs()
        {
            if (gameObject == null)
                return new List<GameObject>();

            List<GameObject> inRadius = new List<GameObject>();
            if (GetCubCount() > 0)
            {
                foreach (GameObject obj in GetCubCache())
                {
                    if (obj == null)
                        continue;

                    if (obj.transform == null)
                        continue;

                    if (obj == gameObject)
                        continue;

                    inRadius.Add(obj);
                }
            }
            return inRadius;
        }

        public List<GameObject> GetNearbyBears()
        {
            if (gameObject == null)
                return new List<GameObject>();

            List<GameObject> inRadius = new List<GameObject>();
            if (GetBearCount() > 0)
            {
                foreach (GameObject obj in GetBearCache())
                {
                    if (obj == null)
                        continue;

                    if (obj.transform == null)
                        continue;

                    if (obj == gameObject)
                        continue;

                    inRadius.Add(obj);
                }
            }
            return inRadius;
        }

        public List<GameObject> GetNearbyHives()
        {
            if (gameObject == null)
                return new List<GameObject>();

            List<GameObject> inRadius = new List<GameObject>();
            if (GetHiveCount() > 0)
            {
                foreach (GameObject obj in GetHiveCache())
                {
                    if (obj == null)
                        continue;

                    if (obj.transform == null)
                        continue;

                    inRadius.Add(obj);
                }
            }
            return inRadius;
        }

        public List<GameObject> GetNearbySlimesOrLargos()
        {
            if (gameObject == null)
                return new List<GameObject>();

            List<GameObject> inRadius = new List<GameObject>();
            if (GetSlimeLargoCount() > 0)
            {
                foreach (GameObject obj in GetSlimeLargoCache())
                {
                    if (obj == null)
                        continue;

                    if (obj.transform == null)
                        continue;

                    if (obj == gameObject)
                        continue;

                    inRadius.Add(obj);
                }
            }
            return inRadius;
        }
    }
}
