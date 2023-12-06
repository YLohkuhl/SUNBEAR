using Il2CppAssets.Script.Util.Extensions;
using Il2CppSystem.Linq;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearPlortonomics : SRBehaviour
    {
        private Rigidbody rigidBody;

        private LandPlot targetPlot;
        private SpawnResource targetResource;
        private bool hasActivated;
        private bool isDescending;

        public float growthChance = 0.02f;
        public float lowerSpeed = 1;

        void Awake() => rigidBody = GetComponent<Rigidbody>();

        void Update()
        {
            if (IsDescending())
                DescendPlort();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (HasActivated())
                return;

            if (!SunBearGlobalStatistics.GetPlortonomicsUsable())
                return;

            GameObject obj = collision.gameObject;
            if (!obj)
                return;

            LandPlot foundPlot = GetPlotIfLandPlot(obj);
            if (!foundPlot)
                return;

            if (foundPlot.TypeId != LandPlot.Id.GARDEN || !foundPlot._attached)
                return;

            Joint[] spawnJoints = foundPlot._attached?.GetComponent<SpawnResource>()?.SpawnJoints;
            if (spawnJoints == null)
                return;

            if (spawnJoints.Length < 1)
                return;

            if (spawnJoints.All(x => x.connectedBody.IsNull() || x.connectedBody?.GetComponent<ResourceCycle>()?._model?.state == ResourceCycle.State.RIPE))
                return;

            targetPlot = foundPlot;
            targetResource = targetPlot._attached.GetComponent<SpawnResource>();
            ImmediatelyRipenAll();
        }

        public bool IsDescending() => isDescending;

        public bool HasActivated() => hasActivated;

        public void DestroyPlort() => Destroyer.DestroyActor(gameObject, "SunBearPlortonomics.DestroyPlort");

        public void DescendPlort()
        {
            if (rigidBody.constraints != RigidbodyConstraints.FreezeAll)
                rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = transform.position - Vector3.up * lowerSpeed * Time.deltaTime;
        }
        
        public IEnumerator DestroyOnlyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (IsDescending())
                isDescending = false;
            DestroyPlort();
            yield break;
        }

        public LandPlot GetPlotIfLandPlot(GameObject gameObject)
        {
            if (!gameObject.name.Contains("Soil"))
            {
                if (!gameObject.name.Contains("Bed"))
                    return null;
            }

            if (!gameObject.GetComponent<LandPlot>())
            {
                if (gameObject.GetComponentInParent<LandPlot>())
                    return gameObject.GetComponentInParent<LandPlot>();
                return null;
            }
            else if (gameObject.GetComponent<LandPlot>())
                return gameObject.GetComponent<LandPlot>();

            return null;
        }

        public void ImmediatelyRipenAll()
        {
            isDescending = true;
            hasActivated = true;
            foreach (Joint joint in targetResource.SpawnJoints)
            {
                var connectedBody = joint.connectedBody;
                if (!connectedBody)
                    continue;

                var resourceCycle = connectedBody.GetComponent<ResourceCycle>();
                if (!connectedBody.GetComponent<ResourceCycle>())
                    continue;

                if (resourceCycle._model.state == ResourceCycle.State.RIPE)
                    continue;

                resourceCycle.ImmediatelyRipen(0);
            }
            SunBearGlobalStatistics.SetPlortonomicsUsable(false);
            MelonCoroutines.Start(DestroyOnlyAfterDelay(1));
        }

        /*public void TransitionSoil()
        {
            if (!targetPlot || !targetResource)
                return;

            if (transitioningSoil.Length < 1)
            {
                transitioningSoil.TryAdd(targetPlot.transform?.Find("Soil"));
                foreach (Transform transform in targetResource.transform)
                {
                    if (!transform)
                        continue;

                    if (!transform.gameObject)
                        continue;

                    if (transform.name == "Bed")
                        transitioningSoil.TryAdd(transform.Find("Dirt"));
                }
            }

            if (originalSoilMaterials.Length < 1)
            {
                foreach (Transform transform in transitioningSoil)
                {
                    if (!transform)
                        continue;

                    if (!transform.gameObject)
                        continue;

                    originalSoilMaterials.TryAdd(transform.gameObject.GetComponent<MeshRenderer>()?.material);
                }
            }

            foreach (Transform transform in transitioningSoil)
            {
                if (!transform)
                    continue;

                if (!transform.gameObject)
                    continue;

                float time = Mathf.PingPong(Time.time / 2, 1);
                var material = transform.gameObject.GetComponent<MeshRenderer>().material;
                transform.gameObject.GetComponent<MeshRenderer>()?.material?.Lerp(material, meshRenderer.material, time);
            }
        }*/
    }
}
