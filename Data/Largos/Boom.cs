using SUNBEAR.Assist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Data.Largos
{
    internal class Boom
    {
        public static void Load(string sceneName)
        {
            switch (sceneName)
            {
                case "GameCore":
                    {
                        SlimeDefinition largoDefinition = Get<SlimeDefinition>("SunBearBoom");
                        SlimeDefinition primaryDef = largoDefinition.BaseSlimes[0];
                        SlimeDefinition secondaryDef = largoDefinition.BaseSlimes[1];
                        string largoName = "Boom Sun Bear Largo";

                        LargoHelper.PresetLargo(largoName, largoDefinition);

                        #region BOOM_SUN_BEAR_LARGO
                        // APPEARANCE
                        SlimeAppearance largoAppearance = UnityEngine.Object.Instantiate(primaryDef.AppearancesDefault[0]);
                        SlimeAppearanceApplicator slimeAppearanceApplicator = largoDefinition.prefab.GetComponent<SlimeAppearanceApplicator>();
                        largoAppearance.name = primaryDef.AppearancesDefault[0].name + secondaryDef.AppearancesDefault[0].name;
                        largoAppearance._dependentAppearances = new SlimeAppearance[] { primaryDef.AppearancesDefault[0], secondaryDef.AppearancesDefault[0] };
                        slimeAppearanceApplicator.Appearance = largoAppearance;
                        slimeAppearanceApplicator.SlimeDefinition = largoDefinition;

                        // REST OF APPEARANCE
                        Material primaryMat = primaryDef.AppearancesDefault[0].Structures.TryGetBody().DefaultMaterials[0];
                        Material secondaryMat = secondaryDef.AppearancesDefault[0].Structures.TryGetBody().DefaultMaterials[0];

                        Material largoMaterial = UnityEngine.Object.Instantiate(secondaryMat);
                        largoMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        largoMaterial.name = largoAppearance.name + "_Body";
                        largoMaterial.SetTexture("_ColorMask", LocalAssets.maskSunBearMulticolorGreen);
                        largoMaterial.SetColor("_GreenTopColor", secondaryMat.GetColor("_TopColor").AdjustBrightness(0.3f));
                        largoMaterial.SetColor("_GreenMiddleColor", secondaryMat.GetColor("_MiddleColor").AdjustBrightness(0.3f));
                        largoMaterial.SetColor("_GreenBottomColor", secondaryMat.GetColor("_BottomColor").AdjustBrightness(0.3f));

                        List<string> shaderKeywords = largoMaterial.GetShaderKeywords().ToList();
                        shaderKeywords.Add("_BODYCOLORING_MULTI");
                        shaderKeywords.Remove("_BODYCOLORING_DEFAULT");
                        largoMaterial.SetShaderKeywords(shaderKeywords.ToArray());

                        largoAppearance.Structures[0].DefaultMaterials[0] = largoMaterial;
                        largoAppearance.Structures[2].DefaultMaterials[0] = secondaryMat;
                        largoAppearance._explosionAppearance = secondaryDef.AppearancesDefault[0].ExplosionAppearance;

                        largoAppearance._splatColor = secondaryDef.AppearancesDefault[0].SplatColor;
                        largoAppearance._colorPalette = secondaryDef.AppearancesDefault[0].ColorPalette;

                        largoDefinition.AppearancesDefault = new SlimeAppearance[] { largoAppearance };
                        largoDefinition.prefab.hideFlags |= HideFlags.HideAndDontSave;
                        #endregion
                        break;
                    }
            }
        }
    }
}
