using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR
{
    internal class LocalAssets
    {
        // MESH
        internal static Mesh sunBearEars;
        internal static Mesh gordoSunBearEars;

        // TEXTURE2D
        internal static Texture2D loadingCharsSBA;
        internal static Texture2D loadingCharsSBB;
        internal static Texture2D iconPlortSunBear;
        internal static Texture2D iconSlimeSunBear;
        internal static Texture2D iconGordoSunBear;
        internal static Texture2D stripesSunBearPlort;
        internal static Texture2D maskSunBearMulticolor;
        internal static Texture2D maskSunBearEarsMulticolor;
        internal static Texture2D maskSunBearMulticolorGreen;

        // SPRITE
        internal static Sprite loadingCharsSBASpr;
        internal static Sprite loadingCharsSBBSpr;
        internal static Sprite iconPlortSunBearSpr;
        internal static Sprite iconSlimeSunBearSpr;
        internal static Sprite iconGordoSunBearSpr;

        // LARGO TEXTURE2D
        // internal static Texture2D bodyStripesSunBear;
        // internal static Texture2D bodyStripesSunBearTabby;
        // internal static Texture2D bodyStripesSunBearSaber;
        // internal static Texture2D bodyStripesSunBearHunter;
        internal static Texture2D maskSunBearRingtailMulticolor;
        internal static Texture2D maskSunBearHunterMulticolor;
        internal static Texture2D maskSunBearSaberMulticolor;
        internal static Texture2D bodyStripesSunBearDervish;

        public static void Load(string sceneName)
        {
            switch (sceneName)
            {
                case "SystemCore":
                    {
                        iconSlimeSunBear = AB.images.LoadAsset("iconSlimeSunBear").Cast<Texture2D>();
                        iconSlimeSunBear.name = "iconSlimeSunBear";

                        iconSlimeSunBearSpr = iconSlimeSunBear.ConvertToSprite();
                        break;
                    }
                case "GameCore":
                    {
                        // MESH
                        sunBearEars = AB.models.LoadFromObject<MeshFilter>("sunbear_ears").sharedMesh;
                        gordoSunBearEars = AB.models.LoadFromObject<MeshFilter>("gordo_sunbear_ears").sharedMesh;

                        // TEXTURE2D
                        loadingCharsSBA = AB.images.LoadAsset("LoadingCharsSBA").Cast<Texture2D>();
                        loadingCharsSBB = AB.images.LoadAsset("LoadingCharsSBB").Cast<Texture2D>();
                        iconPlortSunBear = AB.images.LoadAsset("iconPlortSunBear").Cast<Texture2D>();
                        iconGordoSunBear = AB.images.LoadAsset("iconGordoSunBear").Cast<Texture2D>();
                        stripesSunBearPlort = AB.images.LoadAsset("stripes_sunBearPlort").Cast<Texture2D>();
                        maskSunBearMulticolor = AB.images.LoadAsset("mask_sunbear_multicolor").Cast<Texture2D>();
                        maskSunBearEarsMulticolor = AB.images.LoadAsset("mask_sunbear_ears_multicolor").Cast<Texture2D>();
                        maskSunBearMulticolorGreen = AB.images.LoadAsset("mask_sunbear_multicolor_green").Cast<Texture2D>();

                        iconPlortSunBear.name = "iconPlortSunBear";
                        iconGordoSunBear.name = "iconGordoSunBear";

                        loadingCharsSBA.hideFlags |= HideFlags.HideAndDontSave;
                        loadingCharsSBB.hideFlags |= HideFlags.HideAndDontSave;

                        // SPRITE
                        loadingCharsSBASpr = loadingCharsSBA.ConvertToSprite();
                        loadingCharsSBBSpr = loadingCharsSBB.ConvertToSprite();
                        iconPlortSunBearSpr = iconPlortSunBear.ConvertToSprite();
                        iconGordoSunBearSpr = iconGordoSunBear.ConvertToSprite();

                        // LARGO TEXTURE2D
                        // bodyStripesSunBear = AB.images.LoadAsset("body_stripes_sunBear").Cast<Texture2D>();
                        // bodyStripesSunBearTabby = AB.images.LoadAsset("body_stripes_sunBearTabby").Cast<Texture2D>();
                        // bodyStripesSunBearSaber = AB.images.LoadAsset("body_stripes_sunBearSaber").Cast<Texture2D>();
                        // bodyStripesSunBearHunter = AB.images.LoadAsset("body_stripes_sunBearHunter").Cast<Texture2D>();
                        maskSunBearRingtailMulticolor = AB.images.LoadAsset("mask_sunbear_ringtail_multicolor").Cast<Texture2D>();
                        maskSunBearHunterMulticolor = AB.images.LoadAsset("mask_sunbear_hunter_multicolor").Cast<Texture2D>();
                        maskSunBearSaberMulticolor = AB.images.LoadAsset("mask_sunbear_saber_multicolor").Cast<Texture2D>();
                        bodyStripesSunBearDervish = GenerateColorTexture(LoadHex("#5A595A"));

                        bodyStripesSunBearDervish.name = "body_stripes_sunBearDervish";
                        break;
                    }
            }
        }
    }
}
