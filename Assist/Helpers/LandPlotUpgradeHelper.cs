using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppMonomiPark.SlimeRancher.UI.Plot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;

namespace SUNBEAR.Assist
{
    public static class LandPlotUpgradeHelper
    {
        public static List<PlotUpgradePurchaseItemModel> upgradesShopEntriesToPatch = [];

        public static void RegisterUpgradeShopEntry(this PlotUpgradePurchaseItemModel plotUpgradePurchaseItemModel, LandPlotUIRoot landPlotUI)
        {
            if (!upgradesShopEntriesToPatch.Contains(plotUpgradePurchaseItemModel))
                upgradesShopEntriesToPatch.Add(plotUpgradePurchaseItemModel);
            landPlotUI.menuConfig.categories[0].ItemsIncludingHidden.Add(plotUpgradePurchaseItemModel);
        }

        public static PlotUpgradePurchaseItemModel CreateUpgradeShopEntry(LandPlot.Upgrade upgrade, Sprite icon, string name, PurchaseCost purchaseCost, LocalizedString title, LocalizedString description)
        {
            PlotUpgradePurchaseItemModel purchaseItemModel = ScriptableObject.CreateInstance<PlotUpgradePurchaseItemModel>();
            purchaseItemModel.hideFlags |= HideFlags.HideAndDontSave;
            purchaseItemModel.name = name;

            purchaseItemModel._title = title;
            purchaseItemModel._description = description;

            purchaseItemModel._icon = icon;
            purchaseItemModel._upgrade = upgrade;
            purchaseItemModel._purchaseCost = purchaseCost;

            PlotUpgradePurchaseItemModel basePurchaseItemModel = Get<PlotUpgradePurchaseItemModel>("Walls Upgrade");
            purchaseItemModel._promptMessage = basePurchaseItemModel._promptMessage;
            purchaseItemModel._unavailableIcon = basePurchaseItemModel._unavailableIcon;
            purchaseItemModel._infoButtonLabel = basePurchaseItemModel.InfoButtonLabel;
            purchaseItemModel._actionButtonLabel = basePurchaseItemModel.ActionButtonLabel;
            return purchaseItemModel;
        }
    }
}
