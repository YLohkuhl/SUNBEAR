using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppMonomiPark.SlimeRancher.Slime;
using SUNBEAR.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

internal class Utility
{
    public static class PrefabUtils
    {
        public static Transform DisabledParent;
        static PrefabUtils()
        {
            DisabledParent = new GameObject("DeactivedObject").transform;
            DisabledParent.gameObject.SetActive(false);
            UnityEngine.Object.DontDestroyOnLoad(DisabledParent.gameObject);
            DisabledParent.gameObject.hideFlags |= HideFlags.HideAndDontSave;
        }

        public static GameObject CopyPrefab(GameObject prefab)
        {
            var newG = UnityEngine.Object.Instantiate(prefab, DisabledParent);
            return newG;
        }
    }

    public static class Spawner
    {
        public static GameObject ToSpawn(string name) => InstantiationHelpers.InstantiateActor(Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => x.name == name), SRSingleton<SceneContext>.Instance.RegionRegistry.CurrentSceneGroup, SRSingleton<SceneContext>.Instance.Player.transform.position, Quaternion.identity);
    }

    public static T Get<T>(string name) where T : UnityEngine.Object
    {
        return Resources.FindObjectsOfTypeAll<T>().FirstOrDefault(found => found.name.Equals(name));
    }

    public static Color LoadHex(string hexCode)
    {
        ColorUtility.TryParseHtmlString(hexCode, out var returnedColor);
        return returnedColor;
    }

    public static Texture2D GenerateColorTexture(Color color, int textureWidth = 256, int textureHeight = 256)
    {
        // Create a new texture with the specified width and height
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        texture.hideFlags |= HideFlags.HideAndDontSave;

        // Loop through each pixel and set the color
        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                texture.SetPixel(x, y, color);
            }
        }

        // Apply changes and make sure the texture is readable
        texture.Apply();
        texture.wrapMode = TextureWrapMode.Clamp;
        return texture;
    }

    public static class URandom
    {
        public static float GetFloat(float high)
        {
            return (float)(new System.Random().NextDouble() * (double)high);
        }

        public static T Pick<T>(Dictionary<T, float> weightMap, T ifEmpty)
        {
            T result = ifEmpty;
            float num = 0f;
            foreach (KeyValuePair<T, float> keyValuePair in weightMap)
            {
                float value = keyValuePair.Value;
                if ((double)value > 0.0)
                {
                    num += value;
                    if (num == value || GetFloat(num) < value)
                    {
                        result = keyValuePair.Key;
                    }
                }
                else if ((double)value < 0.0)
                {
                    throw new ArgumentException("weightMap", "Weight less than 0: " + keyValuePair);
                }
            }
            return result;
        }
    }
}

/*public static class SUNBEARTEST
{
    public static void TestSunBearAttack()
    {
        GameObject target = Spawner.ToSpawn("slimePink");
        GameObject target2 = Spawner.ToSpawn("slimeSunBear");
        target2.GetComponent<SunBearAttack>().StartAttack(target);
        // target.GetComponent<SunBearAttack>().StartAttack(target2);
    }
}*/

internal static class Extensions
{
    public static SlimeAppearance.SlimeBone[] AddDefaultBones(this SlimeAppearance.SlimeBone[] slimeBones)
    {
        slimeBones = new SlimeAppearance.SlimeBone[]
        {
            SlimeAppearance.SlimeBone.JIGGLE_BACK,
            SlimeAppearance.SlimeBone.JIGGLE_BOTTOM,
            SlimeAppearance.SlimeBone.JIGGLE_FRONT,
            SlimeAppearance.SlimeBone.JIGGLE_LEFT,
            SlimeAppearance.SlimeBone.JIGGLE_RIGHT,
            SlimeAppearance.SlimeBone.JIGGLE_TOP
        };

        return slimeBones;
    }

    public static SlimeAppearanceStructure Clone(this SlimeAppearanceStructure structure)
    {
        SlimeAppearanceStructure slimeAppearanceStructure = new SlimeAppearanceStructure(structure);
        slimeAppearanceStructure.Element.name = slimeAppearanceStructure.Element.name.Replace("(Clone)", string.Empty);
        return slimeAppearanceStructure;
    }

    public static SlimeAppearanceStructure TryGetFace(this IEnumerable<SlimeAppearanceStructure> enumerable)
    {
        return enumerable.FirstOrDefault(x =>
            x.SupportsFaces || 
            x.Element.Type == SlimeAppearanceElement.ElementType.FACE ||
            x.Element.Name.Contains("Face", StringComparison.OrdinalIgnoreCase));
    }

    public static SlimeAppearanceStructure TryGetBody(this IEnumerable<SlimeAppearanceStructure> enumerable)
    {
        return enumerable.FirstOrDefault(x =>
            x.Element.Type == SlimeAppearanceElement.ElementType.BODY ||
            x.Element.Name.Contains("Body", StringComparison.OrdinalIgnoreCase));
    }

    public static SlimeAppearanceStructure TryGetEars(this IEnumerable<SlimeAppearanceStructure> enumerable)
    {
        return enumerable.FirstOrDefault(x =>
            x.Element.Type == SlimeAppearanceElement.ElementType.EARS ||
            x.Element.Name.Contains("Ears", StringComparison.OrdinalIgnoreCase));
    }

    public static SlimeAppearanceStructure TryGetTail(this IEnumerable<SlimeAppearanceStructure> enumerable)
    {
        return enumerable.FirstOrDefault(x =>
            x.Element.Type == SlimeAppearanceElement.ElementType.TAIL ||
            x.Element.Name.Contains("Tail", StringComparison.OrdinalIgnoreCase));
    }

    public static SlimeAppearanceStructure TryGetWings(this IEnumerable<SlimeAppearanceStructure> enumerable)
    {
        return enumerable.FirstOrDefault(x =>
            x.Element.Type == SlimeAppearanceElement.ElementType.WINGS ||
            x.Element.Name.Contains("Wings", StringComparison.OrdinalIgnoreCase));
    }
    
    public static SlimeAppearanceStructure TryGetTop(this IEnumerable<SlimeAppearanceStructure> enumerable)
    {
        return enumerable.FirstOrDefault(x =>
            x.Element.Type == SlimeAppearanceElement.ElementType.TOP ||
            x.Element.Name.Contains("Flower", StringComparison.OrdinalIgnoreCase));
    }

    public static Color AdjustBrightness(this Color color, float amount, bool shouldDarken = false)
    {
        if (shouldDarken)
            return Color.Lerp(color, Color.black, amount);
        return Color.Lerp(color, Color.white, amount);
    }

    public static Sprite ConvertToSprite(this Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1f);
        sprite.hideFlags = texture.hideFlags;
        sprite.name = texture.name;
        return sprite;
    }

    public static T[] TryAdd<T>(this T[] array, T item)
    {
        if (!array.Contains(item))
            return array.AddToArray(item);
        return array;
    }

    public static void TryAdd<T>(this ICollection<T> collection, T item)
    {
        if (!collection.Contains(item))
            collection.Add(item);
    }

    public static void TryAdd<T>(this Il2CppSystem.Collections.Generic.List<T> list, T item)
    {
        if (!list.Contains(item))
            list.Add(item);
    }

    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
        return source?.IndexOf(toCheck, comp) >= 0;
    }

    public static void AddMany<T>(this List<T> list, params T[] values)
    {
        foreach (T item in values)
            list.Add(item);
    }

    public static void AddRange<T>(this Il2CppSystem.Collections.Generic.List<T> list, params T[] values)
    {
        foreach (T item in values)
            list.Add(item);
    }

    public static bool IsNull(this object obj) => obj == null;

    public static bool IsNotNull(this object obj) => !obj.IsNull();

    public static void Prefabitize(this GameObject obj) => obj.transform.SetParent(PrefabUtils.DisabledParent, false);

    public static T LoadFromObject<T>(this AssetBundle bundle, string name) where T : UnityEngine.Object => bundle.LoadAsset(name).Cast<GameObject>().GetComponentInChildren<T>();
}