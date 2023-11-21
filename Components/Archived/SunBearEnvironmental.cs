using Il2Cpp;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components.Archived
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearEnvironmental : SRBehaviour
    {
        private List<GameObject> fruitCache = new List<GameObject>();
        private IdentifiableTypeGroup fruitGroup;
        private GameObject environmentalEffectIndicator;

        void Start()
        {
            fruitGroup = Get<IdentifiableTypeGroup>("FruitGroup");
            environmentalEffectIndicator = LocalInstances.environmentalEffectIndicator;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject == null)
                return;

            if (!fruitCache.Contains(collider.gameObject))
            {
                fruitCache.Add(collider.gameObject);
                ApplyBenefits(collider.gameObject);
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject == null)
                return;

            if (fruitCache.Contains(collider.gameObject))
            {
                fruitCache.Remove(collider.gameObject);
                RemoveBenefits(collider.gameObject);
            }
        }

        void ApplyBenefits(GameObject fruit)
        {
            if (fruit == null)
                return;

            if (!fruit.GetComponent<IdentifiableActor>() || !fruit.GetComponent<IdentifiableActor>().identType)
                return;

            IdentifiableType fruitIdent = fruit.GetComponent<IdentifiableActor>().identType;

            if (!fruitGroup.IsMember(fruitIdent))
                return;

            var resourceCycle = fruit.GetComponent<ResourceCycle>();
            resourceCycle.edibleGameHours += resourceCycle.ripeGameHours;
            resourceCycle.ripeGameHours -= resourceCycle.ripeGameHours / 2;
            resourceCycle.rottenGameHours -= resourceCycle.ripeGameHours / 2;
            resourceCycle.unripeGameHours -= resourceCycle.ripeGameHours / 2;

            Instantiate(environmentalEffectIndicator).gameObject.transform.SetParent(fruit.transform);
        }

        void RemoveBenefits(GameObject fruit)
        {
            if (fruit == null)
                return;

            if (!fruit.GetComponent<IdentifiableActor>() || !fruit.GetComponent<IdentifiableActor>().identType)
                return;

            IdentifiableType fruitIdent = fruit.GetComponent<IdentifiableActor>().identType;

            if (!fruitGroup.IsMember(fruitIdent))
                return;

            var defaultResourceCycle = fruitIdent.prefab.GetComponent<ResourceCycle>();
            var resourceCycle = fruit.GetComponent<ResourceCycle>();
            resourceCycle.edibleGameHours = defaultResourceCycle.edibleGameHours;
            resourceCycle.ripeGameHours = defaultResourceCycle.ripeGameHours;
            resourceCycle.rottenGameHours = defaultResourceCycle.ripeGameHours;
            resourceCycle.unripeGameHours = defaultResourceCycle.ripeGameHours;

            Destroy(fruit.transform.FindChild("EnvironmentalEffectIndicator(Clone)").gameObject);
        }
    }
}
