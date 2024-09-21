using Il2CppInterop.Runtime;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SUNBEAR.Enums.LandPlotUpgrade;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class BearFriendlyUpgrader : MonoBehaviour
    {
        GameObject BearFriendly;

        void Awake()
        {
            BearFriendly = new("BearFriendly");

            BearFriendly.transform.parent = transform;
            BearFriendly.transform.position = transform.position;
            BearFriendly.transform.rotation = new();
            BearFriendly.transform.localPosition = new();

            // like fr just making sure at this point ughdsngohsdogsd,a

            GameObject boxTrigger = new("BoxTrigger", [Il2CppType.Of<BoxCollider>(), Il2CppType.Of<BearFriendlyCorralTrigger>()]);
            boxTrigger.transform.parent = BearFriendly.transform;
            boxTrigger.transform.position = BearFriendly.transform.position;
            boxTrigger.transform.rotation = new();
            boxTrigger.transform.localScale = new(10, 15, 10);
            boxTrigger.GetComponent<BoxCollider>().isTrigger = true;

            // apparently it's not registering the fact that it's being parented needs to change position ugh whatever
            BearFriendly.SetActive(false);
        }

        public void Apply(LandPlot.Upgrade upgrade)
        {
            if (upgrade == BEAR_FRIENDLY)
                BearFriendly.SetActive(true);
        }
    }
}
