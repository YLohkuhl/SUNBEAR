using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.Regions;
using Il2CppSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static HarmonyPatches.Localization;

namespace SUNBEAR.Data.Foods.Archived
{
    internal class Sunberry
    {
        internal static IdentifiableType sunberryFruit;
        internal static Color[] sunberryPalette = new Color[]
        {
            LoadHex("#FFE484"),
            LoadHex("#FFCC33"),
            LoadHex("#FC9601"),
            LoadHex("#D14009"),
        };

        public static void Initialize()
        {
            sunberryFruit = ScriptableObject.CreateInstance<IdentifiableType>();
            sunberryFruit.hideFlags |= HideFlags.HideAndDontSave;
            sunberryFruit.name = "SunberryFruit";
            sunberryFruit.localizationSuffix = "sunberry_fruit";
            sunberryFruit.color = sunberryPalette[0];
        }

        public static void Load(string sceneName)
        {
            switch (sceneName)
            {
                case "GameCore":
                    {
                        sunberryFruit.localizedName = LocalizationDirectorLoadTablePatch.AddTranslation("Actor", "l.sunberry_fruit", "Sunberry");

                        string[] shaderKeywords = new string[]
                        {
                            "_BODYCOLORING_MULTI",
                            "_ENABLEPHOSCORE_ON",
                            "_LIGHTMODEL_DEFAULT",
                            "_STATIC"
                        };

                        Material sunberryMaterial = UnityEngine.Object.Instantiate(Get<SlimeAppearance>("PhosphorDefault").Structures[0].DefaultMaterials[0]);
                        sunberryMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        sunberryMaterial.name = "sunberry";
                        sunberryMaterial.SetColor("_TopColor", sunberryPalette[0]);
                        sunberryMaterial.SetColor("_MiddleColor", sunberryPalette[1]);
                        sunberryMaterial.SetColor("_BottomColor", sunberryPalette[0]);
                        sunberryMaterial.SetColor("_SpecColor", sunberryPalette[1]);

                        sunberryMaterial.SetTexture("_ColorMask", AB.images.LoadAsset("mask_sunberry_multicolor").Cast<Texture2D>());
                        sunberryMaterial.SetColor("_RedTopColor", sunberryPalette[0]);
                        sunberryMaterial.SetColor("_RedMiddleColor", sunberryPalette[1]);
                        sunberryMaterial.SetColor("_RedBottomColor", sunberryPalette[0]);
                        sunberryMaterial.SetColor("_GreenTopColor", sunberryPalette[2]);
                        sunberryMaterial.SetColor("_GreenMiddleColor", sunberryPalette[3]);
                        sunberryMaterial.SetColor("_GreenBottomColor", sunberryPalette[2]);
                        sunberryMaterial.SetColor("_BlueTopColor", sunberryPalette[2]);
                        sunberryMaterial.SetColor("_BlueMiddleColor", sunberryPalette[3]);
                        sunberryMaterial.SetColor("_BlueBottomColor", sunberryPalette[2]);

                        sunberryMaterial.SetColor("_GlowTop", sunberryPalette[1]);
                        // sunberryMaterial.SetFloat("_GlowSpeed", 10);
                        // sunberryMaterial.SetFloat("_GlowStrength", 2);

                        sunberryMaterial.SetShaderKeywords(shaderKeywords);

                        sunberryFruit.prefab = PrefabUtils.CopyPrefab(Get<GameObject>("fruitPogo"));
                        sunberryFruit.prefab.hideFlags |= HideFlags.HideAndDontSave;
                        sunberryFruit.prefab.name = "fruitSunberry";

                        sunberryFruit.prefab.GetComponent<IdentifiableActor>().identType = sunberryFruit;
                        UnityEngine.Object.Destroy(sunberryFruit.prefab.GetComponent<ResourceCycle>());

                        GameObject sunberry_body = sunberryFruit.prefab.transform.FindChild("model_pogofruit").gameObject;
                        sunberry_body.name = "sunberry_body";
                        sunberry_body.transform.parent = sunberryFruit.prefab.transform;
                        sunberry_body.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                        sunberry_body.GetComponent<MeshFilter>().sharedMesh = AB.models.LoadFromObject<MeshFilter>("sunberry_body").sharedMesh;
                        sunberry_body.GetComponent<MeshRenderer>().sharedMaterial = sunberryMaterial;

                        GameObject sunberry_top = new GameObject("sunberry_top");
                        sunberry_top.transform.parent = sunberry_body.transform;
                        sunberry_top.transform.localScale = new Vector3(1, 1, 1);
                        sunberry_top.AddComponent<MeshFilter>().sharedMesh = AB.models.LoadFromObject<MeshFilter>("sunberry_top").sharedMesh;
                        sunberry_top.AddComponent<MeshRenderer>().sharedMaterial = sunberryMaterial;
                        break;
                    }
            }
        }
    }
}
