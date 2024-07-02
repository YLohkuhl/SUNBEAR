using Il2CppMonomiPark.SlimeRancher.Pedia;
using Il2CppMonomiPark.SlimeRancher.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUNBEAR.Harmony;
using UnityEngine.Localization;
using Il2CppMonomiPark.SlimeRancher;

namespace SUNBEAR.Assist
{
    internal static class GeneralizedHelper
    {
        // internal static HashSet<GameIconDefinition> gameIconsToPatch = new HashSet<GameIconDefinition>();
        internal static List<MarketUI.PlortEntry> plortsToPatch = new List<MarketUI.PlortEntry>();
        internal static List<EconomyDirector.ValueMap> valueMapsToPatch = new List<EconomyDirector.ValueMap>();

        // public static void RegisterGameIcon(GameIconDefinition gameIcon) => gameIconsToPatch.TryAdd(gameIcon);

        public static void RegisterPlortEntry(MarketUI.PlortEntry plortEntry) => plortsToPatch.TryAdd(plortEntry);

        public static void RegisterValueMap(EconomyDirector.ValueMap valueMap) => valueMapsToPatch.TryAdd(valueMap);

        public static LocalizedString CreateTranslation(string table, string key, string localized) => LocalizationDirectorLoadTablesPatch.AddTranslation(table, key, localized);
    }
}
