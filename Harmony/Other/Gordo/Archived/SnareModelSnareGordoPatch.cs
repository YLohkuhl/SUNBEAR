using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Harmony.Other
{
    /*[HarmonyPatch(typeof(SnareModel), "SnareGordo", new Type[] { })]
    internal class SnareModelSnareGordoPatch
    {
        public static bool Prefix(SnareModel __instance)
        {
            if (!__instance.IsBaited() || __instance.HasSnaredGordo())
                return false;

            IdentifiableType gordoIdForBait = __instance.GetGordoIdForBait();
            if (gordoIdForBait == null)
                return false;

            __instance.SnareGordo(gordoIdForBait);
            return true;
        }
    }*/
}
