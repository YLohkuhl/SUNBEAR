using HarmonyLib;
using SUNBEAR.Assist;
using SUNBEAR.Data.Slimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Data.Largos
{
    internal class Ringtail
    {
        public static void Load(string sceneName)
        {
            switch (sceneName)
            {
                case "GameCore":
                    {
                        SlimeDefinition largoDefinition = Get<SlimeDefinition>("SunBearRingtail");
                        SlimeDefinition primaryDef = largoDefinition.BaseSlimes[0];
                        SlimeDefinition secondaryDef = largoDefinition.BaseSlimes[1];
                        string largoName = "Ringtail Sun Bear Largo";

                        LargoHelper.PresetLargo(largoName, largoDefinition);

                        #region RINGTAIL_SUN_BEAR_LARGO
                        // APPEARANCE
                        SlimeAppearance largoAppearance = UnityEngine.Object.Instantiate(primaryDef.AppearancesDefault[0]);
                        SlimeAppearanceApplicator slimeAppearanceApplicator = largoDefinition.prefab.GetComponent<SlimeAppearanceApplicator>();
                        largoAppearance.name = primaryDef.AppearancesDefault[0].name + secondaryDef.AppearancesDefault[0].name;
                        largoAppearance._dependentAppearances = new SlimeAppearance[] { primaryDef.AppearancesDefault[0], secondaryDef.AppearancesDefault[0] };
                        slimeAppearanceApplicator.Appearance = largoAppearance;
                        slimeAppearanceApplicator.SlimeDefinition = largoDefinition;

                        // REST OF APPEARANCE
                        foreach (SlimeAppearanceStructure secondaryStruct in secondaryDef.AppearancesDefault[0].Structures)
                        {
                            if (secondaryStruct.SupportsFaces || secondaryStruct.Element.Type == SlimeAppearanceElement.ElementType.FACE || secondaryStruct.Element.Name.Contains("Face", StringComparison.OrdinalIgnoreCase))
                                continue;

                            if (secondaryStruct.Element.Type == SlimeAppearanceElement.ElementType.BODY || secondaryStruct.Element.Name.Contains("Body", StringComparison.OrdinalIgnoreCase))
                                continue;

                            if (secondaryStruct.Element.Type == SlimeAppearanceElement.ElementType.EARS || secondaryStruct.Element.Name.Contains("Ears", StringComparison.OrdinalIgnoreCase))
                                continue;

                            largoAppearance.Structures = largoAppearance.Structures.ToArray().AddToArray(new SlimeAppearanceStructure(secondaryStruct));
                        }

                        /*List<SlimeAppearanceStructure> structures = largoAppearance.Structures.ToList();
                        structures.Remove(structures.TryGetEars());
                        structures.Add(new SlimeAppearanceStructure(secondaryDef.AppearancesDefault[0].Structures.TryGetEars()));
                        largoAppearance.Structures = structures.ToArray();*/

                        Material primaryMat = primaryDef.AppearancesDefault[0].Structures.TryGetBody().DefaultMaterials[0];
                        Material secondaryMat = secondaryDef.AppearancesDefault[0].Structures.TryGetBody().DefaultMaterials[0];

                        Material largoMaterial = UnityEngine.Object.Instantiate(primaryMat);
                        largoMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        largoMaterial.name = largoAppearance.name + "_Body";
                        largoMaterial.SetColor("_RedTopColor", secondaryMat.GetColor("_RedTopColor").AdjustBrightness(0.5f, true));
                        largoMaterial.SetColor("_RedMiddleColor", secondaryMat.GetColor("_RedMiddleColor").AdjustBrightness(0.5f, true));
                        largoMaterial.SetColor("_RedBottomColor", secondaryMat.GetColor("_RedBottomColor").AdjustBrightness(0.5f, true));
                        largoMaterial.SetColor("_GreenTopColor", secondaryMat.GetColor("_RedTopColor"));
                        largoMaterial.SetColor("_GreenMiddleColor", secondaryMat.GetColor("_MiddleColor"));
                        largoMaterial.SetColor("_GreenBottomColor", secondaryMat.GetColor("_RedBottomColor"));

                        Material earsMaterial = UnityEngine.Object.Instantiate(primaryDef.AppearancesDefault[0].Structures[2].DefaultMaterials[0]);
                        earsMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        earsMaterial.name = largoAppearance.name + "_Ears";
                        earsMaterial.SetColor("_RedTopColor", secondaryMat.GetColor("_RedTopColor").AdjustBrightness(0.5f, true));
                        earsMaterial.SetColor("_RedMiddleColor", secondaryMat.GetColor("_RedMiddleColor").AdjustBrightness(0.5f, true));
                        earsMaterial.SetColor("_RedBottomColor", secondaryMat.GetColor("_RedBottomColor").AdjustBrightness(0.5f, true));

                        Material tailMaterial = UnityEngine.Object.Instantiate(primaryMat);
                        tailMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        tailMaterial.name = largoAppearance.name + "_Tail";
                        tailMaterial.SetTexture("_ColorMask", LocalAssets.maskSunBearRingtailMulticolor);
                        tailMaterial.SetColor("_TopColor", secondaryMat.GetColor("_RedTopColor"));
                        tailMaterial.SetColor("_MiddleColor", secondaryMat.GetColor("_RedMiddleColor"));
                        tailMaterial.SetColor("_BottomColor", secondaryMat.GetColor("_RedBottomColor"));

                        tailMaterial.SetColor("_RedTopColor", secondaryMat.GetColor("_RedTopColor").AdjustBrightness(0.5f, true));
                        tailMaterial.SetColor("_RedMiddleColor", secondaryMat.GetColor("_RedMiddleColor").AdjustBrightness(0.5f, true));
                        tailMaterial.SetColor("_RedBottomColor", secondaryMat.GetColor("_RedBottomColor").AdjustBrightness(0.5f, true));
                        tailMaterial.SetColor("_BlueTopColor", secondaryMat.GetColor("_RedTopColor"));
                        tailMaterial.SetColor("_BlueMiddleColor", secondaryMat.GetColor("_RedMiddleColor"));
                        tailMaterial.SetColor("_BlueBottomColor", secondaryMat.GetColor("_RedBottomColor"));

                        largoAppearance.Structures[0].DefaultMaterials[0] = largoMaterial;
                        largoAppearance.Structures[2].DefaultMaterials[0] = earsMaterial;
                        largoAppearance.Structures.TryGetTail().DefaultMaterials[0] = tailMaterial;
                        largoAppearance._stoneFormAppearance = Get<StoneFormAppearance>("StoneFormDefaultLargo");

                        largoAppearance._splatColor = secondaryMat.GetColor("_RedTopColor").AdjustBrightness(0.5f, true);
                        largoAppearance._colorPalette = new SlimeAppearance.Palette()
                        {
                            Ammo = secondaryMat.GetColor("_RedTopColor").AdjustBrightness(0.5f, true),
                            Top = secondaryMat.GetColor("_RedTopColor").AdjustBrightness(0.5f, true),
                            Middle = secondaryMat.GetColor("_RedMiddleColor").AdjustBrightness(0.5f, true),
                            Bottom = secondaryMat.GetColor("_RedBottomColor").AdjustBrightness(0.5f, true)
                        };

                        largoDefinition.AppearancesDefault = new SlimeAppearance[] { largoAppearance };
                        largoDefinition.prefab.hideFlags |= HideFlags.HideAndDontSave;
                        #endregion
                        break;
                    }
            }
        }
    }
}
