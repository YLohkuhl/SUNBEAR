using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearReproduction : SRBehaviour
    {
        private TimeDirector timeDir;
        private SunBearCache sunBearCache;
        private double nextReproduceTime;

        public int minSpawnCount = 1;
        public int maxSpawnCount = 2;
        public float spawnDistance = 5;
        public float minReproduceHours = 24 * 3;
        public float maxReproduceHours = 24 * 7;

        void Start()
        {
            timeDir = SceneContext.Instance.TimeDirector;
            sunBearCache = GetComponent<SunBearCache>();
            nextReproduceTime = timeDir.HoursFromNowOrStart(UnityEngine.Random.Range(minReproduceHours, maxReproduceHours));
        }

        void Update()
        {
            if (timeDir.HasReached(nextReproduceTime))
            {
                if (sunBearCache.IsCubsNearby())
                {
                    nextReproduceTime = timeDir.HoursFromNowOrStart(UnityEngine.Random.Range(minReproduceHours, maxReproduceHours));
                    return;
                }

                /*if (UnityEngine.Random.Range(0f, 1f) < 0.2f)
                    return;*/

                Reproduce();
                nextReproduceTime = timeDir.HoursFromNowOrStart(UnityEngine.Random.Range(minReproduceHours, maxReproduceHours));
            }
        }

        void Reproduce()
        {
            if (!sunBearCache.IsCubsNearby())
            {
                int spawnCount = UnityEngine.Random.Range(minSpawnCount, maxSpawnCount);
                for (int i = 0; i < spawnCount; i++)
                {
                    float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
                    Vector3 spawnPosition = transform.position + new Vector3(spawnDistance * Mathf.Cos(randomAngle), 0f, spawnDistance * Mathf.Sin(randomAngle));

                    GameObject instantiatedCub = InstantiateActor(Get<SlimeDefinition>("CubSunBear").prefab, SceneContext.Instance.RegionRegistry.CurrentSceneGroup, spawnPosition, Quaternion.identity);
                    try
                    { 
                        instantiatedCub.GetComponent<SunBearFollow>().AttemptSearch(gameObject); 
                    }
                    catch 
                    { 
                        // Nothing???
                    }
                    SpawnAndPlayFX(Get<SlimeDefinition>("Pink").prefab.GetComponent<SlimeEat>().ProduceFX, instantiatedCub.transform.position, Quaternion.identity);
                }
            }
        }
    }
}
