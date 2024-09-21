using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class BearFriendlyCorralTrigger : MonoBehaviour
    {
        IdentifiableType sunBearIdentifiableType;

        void Start() => sunBearIdentifiableType = Get<IdentifiableType>("SunBear");

        public void OnTriggerEnter(Collider collider)
        {
            IdentifiableType identifiableType = collider.gameObject?.GetComponent<IdentifiableActor>()?.identType;

            if (identifiableType.IsNotNull() && identifiableType == sunBearIdentifiableType)
            {
                var attack = collider.gameObject.GetComponent<SunBearAttack>();
                if (attack)
                    attack.isInBearFriendlyArea = true;
            }
        }

        public void OnTriggerExit(Collider collider)
        {
            IdentifiableType identifiableType = collider.gameObject?.GetComponent<IdentifiableActor>()?.identType;

            if (identifiableType.IsNotNull() && identifiableType == sunBearIdentifiableType)
            {
                var attack = collider.gameObject.GetComponent<SunBearAttack>();
                if (attack)
                    attack.isInBearFriendlyArea = false;
            }
        }
    }
}
