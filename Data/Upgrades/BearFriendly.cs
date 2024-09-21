using Il2CppMonomiPark.SlimeRancher.Pedia;
using Il2CppMonomiPark.SlimeRancher.UI.Plot;
using Il2CppMonomiPark.SlimeRancher.UI;
using SUNBEAR.Assist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SUNBEAR.Enums.LandPlotUpgrade;
using SUNBEAR.Components;

namespace SUNBEAR.Data.Upgrades
{
    internal static class BearFriendly
    {
        public static void Load(string sceneName)
        {
            switch (sceneName)
            {
                case "GameCore":
                    {
                        PurchaseCost purchaseCost = PurchaseCost.CreateEmpty();
                        purchaseCost.newbuckCost = 750;

                        PlotUpgradePurchaseItemModel upgradeShopEntry = LandPlotUpgradeHelper.CreateUpgradeShopEntry(BEAR_FRIENDLY, LocalAssets.iconSlimeSunBearSpr, "Bear Friendly Corral", purchaseCost,
                            GeneralizedHelper.CreateTranslation("Pedia", "m.upgrade.name.corral.bear_friendly", "Bear-Friendly Corral"),
                            GeneralizedHelper.CreateTranslation("Pedia", "m.upgrade.desc.corral.bear_friendly", "Induces a more friendly environment, heavily reducing the chances of bear attacks. However, a bear going savage cannot be prevented.")
                        );

                        upgradeShopEntry._pediaEntry = Get<PediaEntry>("Corral");
                        upgradeShopEntry.RegisterUpgradeShopEntry(Get<LandPlotUIRoot>("CorralUI"));

                        GameContext.Instance.LookupDirector.GetPlotPrefab(LandPlot.Id.CORRAL).AddComponent<BearFriendlyUpgrader>();
                        break;
                    }
            }
        }
    }
}
