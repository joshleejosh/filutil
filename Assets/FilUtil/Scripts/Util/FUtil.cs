using UnityEngine;
using System.Collections.Generic;

/*
 * I am a big bucket of miscellaneous functions.
 */
namespace FilUtil {
public abstract class FUtil {

    public static float Round(float i, int places) {
        float m = Mathf.Pow(10, places);
        return (float)Mathf.RoundToInt(i * m) / m;
    }

    public static float Map(float i, float imin, float imax, float omin, float omax) {
        if (imax==imin)
            return omin;
        return ((i - imin) / (imax - imin)) * (omax - omin) + omin;
    }

    public static float MapClamp(float i, float imin, float imax, float omin, float omax) {
        if (omin > omax)
            return Mathf.Clamp(Map(i, imin, imax, omin, omax), omax, omin);
        else
            return Mathf.Clamp(Map(i, imin, imax, omin, omax), omin, omax);
    }

    // Given an angle in radians, returns a unit Vector2 pointing in that direction.
    public static Vector2 UnitAtAngle(float a) {
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    // The returned vector's Z will be 0.
    public static Vector3 PolarToCartesian(float a, float r) {
        return new Vector3(r * Mathf.Cos(a), r * Mathf.Sin(a), 0);
    }

    // returns either -1 or +1.
    public static int Coinflip() {
        return (Random.Range(0f,1f) < .5f)?1:-1;
    }

    public static bool IsUpper(string s) {
        bool rv = true;
        for (int i=0; i<s.Length; i++) {
            if (!System.Char.IsUpper(s[i])) {
                rv = false;
                break;
            }
        }
        return rv;
    }

    // ====================================================

    public static void Shuffle<T>(List<T> deck) {
        if (deck == null)
            return;
        for (int i=deck.Count-1; i>=0; i--) {
            int j = Random.Range(0,i);
            T t = deck[j];
            deck[j] = deck[i];
            deck[i] = t;
        }
    }

    public static T PickRandom<T>(List<T> deck) {
        if (deck == null)
            return default(T);
        if (deck.Count == 0)
            return default(T);
        return deck[Random.Range(0,deck.Count)];
    }

    // ====================================================

    public static string Join<T>(List<T> list, string delimiter="") {
        if (list == null)
            return "";
        if (list.Count == 0)
            return "";
        return Join(list.ToArray(), delimiter);
    }

    public static string Join<T>(Queue<T> queue, string delimiter="") {
        if (queue == null)
            return "";
        if (queue.Count == 0)
            return "";
        return Join(queue.ToArray(), delimiter);
    }

    public static string Join<T>(T[] a, string delimiter="") {
        if (a == null)
            return "";
        if (a.Length == 0)
            return "";
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append((a[0]==null)?"":a[0].ToString());
        for (int i=1; i<a.Length; i++) {
            sb.Append(delimiter);
            sb.Append((a[i]==null)?"":a[i].ToString());
        }
        return sb.ToString();
    }

    // ====================================================

    // If a Rect has negative width or height, flip it and reposition it.
    public static Rect RectAbs(Rect r) {
        Rect s = r;
        if (s.width < 0) {
            s.width *= -1;
            s.x -= s.width;
        }
        if (s.height < 0) {
            s.height *= -1;
            s.x -= s.height;
        }
        return s;
    }

    // ====================================================

    public static string GetObjectPath(GameObject gob) {
        if (gob == null)
            return "";
        Transform t = gob.transform;
        string rv = t.name;
        while (t.parent != null) {
            t = t.parent;
            rv = t.name + "/" + rv;
        }
        return rv;
    }

    // ====================================================

    // t in range [0,1]
    public static float EaseOutQuad(float t) {
        return -1 * t * (t-2);
    }
    public static float EaseInQuad(float t) {
        return t*t;
    }
    public static float EaseInOutQuad(float t) {
        float rv;
        t *= 2;
        if (t <= 1) {
            rv = EaseInQuad(t) * .5f;
        } else {
            t--;
            rv = EaseOutQuad(t) * .5f + .5f;
        }
        return rv;
    }

    public static float EaseOutExpo(float t) {
        return -Mathf.Pow(2, -10 * t) + 1;
    }
    public static float EaseInExpo(float t) {
        return Mathf.Pow(2, 10 * (t-1));
    }
    public static float EaseInOutExpo(float t) {
        float rv;
        t *= 2;
        if (t <= 1) {
            rv = EaseInExpo(t) * .5f;
        } else {
            t--;
            rv = EaseOutExpo(t) * .5f + .5f;
        }
        return rv;
    }

    // ====================================================

}
}
