#define ANDROID_NATIVE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CUtils
{
#if ANDROID_NATIVE && UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaClass cls_UnityPlayer;
    private static AndroidJavaObject obj_Activity;
#endif

    static CUtils()
    {
#if ANDROID_NATIVE && UNITY_ANDROID && !UNITY_EDITOR
        AndroidJNI.AttachCurrentThread();
        cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
#endif
    }


    public static string ReadFileContent(string path)
    {
        TextAsset file = Resources.Load(path) as TextAsset;
        return file == null ? null : file.text;
    }

    public static Vector3 CopyVector3(Vector3 ori)
    {
        Vector3 des = new Vector3(ori.x, ori.y, ori.z);
        return des;
    }

    public static bool EqualVector3(Vector3 v1, Vector3 v2)
    {
        return Vector3.SqrMagnitude(v1 - v2) <= 0.0000001f;
    }

    public static float GetSign(Vector3 A, Vector3 B, Vector3 M)
    {
        return Mathf.Sign((B.x - A.x) * (M.y - A.y) - (B.y - A.y) * (M.x - A.x));
    }

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    public static void Shuffle<T>(params T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            var temp = array[i];
            var randomIndex = UnityEngine.Random.Range(0, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    public static void Shuffle<T>(List<T> array, int from, int to)
    {
        for (int i = to; i > from; i--)
        {
            int rdIndex = UnityEngine.Random.Range(from, to);
            T tmp = array[i];
            array[i] = array[rdIndex];
            array[rdIndex] = tmp;
        }
    }


    public static string[] SeparateLines(string lines)
    {
        return lines.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n"[0]);
    }

    public static void ChangeSortingLayerRecursively(Transform root, string sortingLayerName, int offsetOrder = 0)
    {
        SpriteRenderer renderer = root.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder += offsetOrder;
        }

        foreach (Transform child in root)
        {
            ChangeSortingLayerRecursively(child, sortingLayerName, offsetOrder);
        }
    }

    public static void ChangeRendererColorRecursively(Transform root, Color color)
    {
        SpriteRenderer renderer = root.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = color;
        }

        foreach (Transform child in root)
        {
            ChangeRendererColorRecursively(child, color);
        }
    }

    public static void ChangeImageColorRecursively(Transform root, Color color)
    {
        Image image = root.GetComponent<Image>();
        if (image != null)
        {
            image.color = color;
        }

        foreach (Transform child in root)
        {
            ChangeImageColorRecursively(child, color);
        }
    }

    public static string GetFacePictureURL(string facebookID, int? width = null, int? height = null, string type = null)
    {
        string url = string.Format("/{0}/picture", facebookID);
        string query = width != null ? "&width=" + width.ToString() : "";
        query += height != null ? "&height=" + height.ToString() : "";
        query += type != null ? "&type=" + type : "";
        query += "&redirect=false";
        if (query != "")
            url += ("?g" + query);
        return url;
    }

    public static double GetCurrentTime()
    {
        TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        return span.TotalSeconds;
    }

    public static double GetCurrentTimeInDays()
    {
        TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        return span.TotalDays;
    }

    public static double GetCurrentTimeInMills()
    {
        TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        return span.TotalMilliseconds;
    }

    public static T GetRandom<T>(params T[] arr)
    {
        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }

    public static string BuildStringFromCollection(ICollection values, char split = '|')
    {
        string results = "";
        int i = 0;
        foreach (var value in values)
        {
            results += value;
            if (i != values.Count - 1)
            {
                results += split;
            }
            i++;
        }
        return results;
    }

    public static List<T> BuildListFromString<T>(string values, char split = '|')
    {
        List<T> list = new List<T>();
        if (string.IsNullOrEmpty(values))
            return list;

        //string[] arr = values.Split(split);
        string[] arr = values.Trim().Split(split);
        foreach (string value in arr)
        {
            if (string.IsNullOrEmpty(value)) continue;
            T val = (T)Convert.ChangeType(value, typeof(T));
            list.Add(val);
        }
        return list;
    }

#if UNITY_EDITOR
    public static string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    public static int[] GetSortingLayerUniqueIDs()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
        return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
    }
#endif


    public static List<T> GetObjectInRange<T>(Vector3 position, float radius, int layerMask = Physics2D.DefaultRaycastLayers) where T : class
    {
        List<T> list = new List<T>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius, layerMask);

        foreach (Collider2D col in colliders)
        {
            list.Add(col.gameObject.GetComponent(typeof(T)) as T);
        }
        return list;
    }

    public static Sprite GetSprite(string textureName, string spriteName)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(textureName);
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == spriteName)
            {
                return sprite;
            }
        }
        return null;
    }

    public static List<Transform> GetActiveChildren(Transform parent)
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform child in parent)
        {
            if (child.gameObject.activeSelf) list.Add(child);
        }
        return list;
    }

    public static List<Transform> GetChildren(Transform parent)
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform child in parent)
        {
            list.Add(child);
        }
        return list;
    }

    public static IEnumerator LoadPicture(string url, Action<Texture2D> callback, int width, int height, bool useCached = true)
    {
#if !UNITY_WSA && !UNITY_WP8 && !UNITY_WEBPLAYER
        string localPath = GetLocalPath(url);
        bool loaded = false;

        if (useCached)
        {
            loaded = LoadFromLocal(callback, localPath, width, height);
        }

        if (!loaded)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                callback(www.texture);
                System.IO.File.WriteAllBytes(localPath, www.bytes);
            }
            else
            {
                LoadFromLocal(callback, localPath, width, height);
            }
        }
#else
        yield return null;
#endif
    }

    private static string GetLocalPath(string url)
    {
#if !UNITY_WSA && !UNITY_WP8 && !UNITY_WEBPLAYER
        string justFilename = System.IO.Path.GetFileName(new Uri(url).LocalPath);
        return Application.persistentDataPath + "/" + justFilename;
#else
        return null;
#endif
    }

    public static IEnumerator CachePicture(string url, Action<bool> result)
    {
#if !UNITY_WSA && !UNITY_WP8 && !UNITY_WEBPLAYER
        string localPath = GetLocalPath(url);
        WWW www = new WWW(url);
        yield return www;
        if (www.isDone && string.IsNullOrEmpty(www.error))
        {
            System.IO.File.WriteAllBytes(localPath, www.bytes);
            if (result != null) result(true);
        }
        else
        {
            if (result != null) result(false);
        }
#else
        yield return null;
#endif
    }

    public static bool IsCacheExists(string url)
    {
#if !UNITY_WSA && !UNITY_WP8
        return System.IO.File.Exists(GetLocalPath(url));
#else
        return false;
#endif
    }

    private static bool LoadFromLocal(Action<Texture2D> callback, string localPath, int width, int height)
    {
#if !UNITY_WSA && !UNITY_WP8 && !UNITY_WEBPLAYER
        if (System.IO.File.Exists(localPath))
        {
            var bytes = System.IO.File.ReadAllBytes(localPath);
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            tex.LoadImage(bytes);
            if (tex != null)
            {
                callback(tex);
                return true;
            }
        }
        return false;
#else
        return false;
#endif
    }

    public static Sprite CreateSprite(Texture2D texture, int width, int height)
    {
        return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    public static List<List<T>> Split<T>(List<T> source, Predicate<T> split)
    {
        List<List<T>> result = new List<List<T>>();
        bool begin = false;
        for (int i = 0; i < source.Count; i++)
        {
            T element = source[i];
            if (split(element))
            {
                begin = false;
            }
            else
            {
                if (begin == false)
                {
                    begin = true;
                    result.Add(new List<T>());
                }
                result[result.Count - 1].Add(element);
            }
        }
        return result;
    }

    public static List<List<T>> GetArrList<T>(List<T> source, Predicate<T> take)
    {
        List<List<T>> result = new List<List<T>>();
        bool begin = false;
        foreach (T element in source)
        {
            if (take(element))
            {
                if (begin == false)
                {
                    begin = true;
                    result.Add(new List<T>());
                }
                result[result.Count - 1].Add(element);
            }
            else
            {
                begin = false;
            }
        }
        return result;
    }

    public static List<T> ToList<T>(T obj)
    {
        List<T> list = new List<T>();
        list.Add(obj);
        return list;
    }

    public static bool IsObjectSeenByCamera(Camera camera, GameObject gameObj, float delta = 0)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(gameObj.transform.position);
        return (screenPoint.z > 0 && screenPoint.x > -delta && screenPoint.x < 1 + delta && screenPoint.y > -delta && screenPoint.y < 1 + delta);
    }

    public static Vector3 GetMiddlePoint(Vector3 begin, Vector3 end, float delta = 0, int dir = 1)
    {
        Vector3 center = Vector3.Lerp(begin, end, 0.5f);
        Vector3 beginEnd = end - begin;
        Vector3 perpendicular = new Vector3(-beginEnd.y, beginEnd.x, 0).normalized;
        perpendicular *= dir;
        Vector3 middle = center + perpendicular * delta;
        return middle;
    }

    public static Vector3 GetMiddlePointOther(Vector3 begin, Vector3 end, float delta = 0)
    {
        Vector3 center = Vector3.Lerp(begin, end, 0.5f);
        Vector3 beginEnd = end - begin;
        Vector3 perpendicular = new Vector3(-beginEnd.y, beginEnd.x, 0).normalized;
        perpendicular *= -1;
        Vector3 middle = center + perpendicular * delta;
        return middle;
    }

    public static AnimationClip GetAnimationClip(Animator anim, string name)
    {
        var ac = anim.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == name) return ac.animationClips[i];
        }
        return null;
    }

    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp = lhs;
        lhs = rhs;
        rhs = temp;
    }




    #region Convert DateTime

    public static string ConvertDateTimeToString(DateTime now)
    {
        int second = now.Second;
        int minute = now.Minute;
        int hour = now.Hour;
        int day = now.Day;
        int month = now.Month;
        int year = now.Year;

        return year + "|" + month + "|" + day + "|" + hour + "|" + minute + "|" + second;
    }

    public static DateTime ConvertStringToDateTime(string time)
    {
        string[] data = time.Split('|');
        if (data.Length == 6)
        {
            return new DateTime(Int32.Parse(data[0]), Int32.Parse(data[1]), Int32.Parse(data[2]), Int32.Parse(data[3]), Int32.Parse(data[4]), Int32.Parse(data[5]));
        }
        return DateTime.Now;
    }
    #endregion
}

