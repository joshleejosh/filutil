using UnityEngine;
using System.Collections.Generic;

namespace FilUtil {
public class FConfig {
    private string manifestPath;
    private Dictionary<string,object> root;

    public FConfig() {
        manifestPath = "";
        root = null;
    }

    public void LoadPath(string path) {
        manifestPath = path;
        TextAsset manass = (TextAsset)Resources.Load(path);
        root = DarktableJSON.Json.Deserialize(manass.text) as Dictionary<string,object>;
    }

    public void LoadString(string s) {
        manifestPath = "";
        root = DarktableJSON.Json.Deserialize(s) as Dictionary<string,object>;
    }

    /*
     * Pick a value out of the JSON blob containing config values.
     * path is a slash-separated path used to traverse the JSON tree.
     *
     * If you're drilling into an array, use numbers to index.
     * e.g., for { "foo": [ "A", "B", "C", "D" ] }
     *     "foo/0" -> "A"
     *     "foo/2" -> "C"
     */
    public object Get(string path) {
        return Get(root, path);
    }

    // Assuming the given object is a Dictionary<string,object>, traverse it as a JSON-y thing using the given path.
    public object Get(object root, string path) {
        object rv = null;
        FAssert.NotNull(root);
        object cur = root;
        string[] a = path.Split(new char[]{'/',});
        foreach (string s in a) {
            string key = s.Trim();
            if (key == "")
                continue;
            if (cur is Dictionary<string,object>) {
                Dictionary<string,object> dcur = cur as Dictionary<string,object>;
                FAssert.NotNull(dcur);
/*
                if (key == "%DEVICE")
                    key = (LayoutManager.Instance.isPhone)?"phone":"tablet";
                if (key == "%ASPECT") {
                    key = CameraManager.Instance.GetAspectKey();
                    if (key != CameraManager.ASPECT_WILDCARD && !dcur.ContainsKey(key))
                        key = CameraManager.ASPECT_WILDCARD;
                }
*/
                cur = dcur[key];
            } else if (cur is List<object>) {
                List<object> lcur = cur as List<object>;
                FAssert.NotNull(lcur);
                int ki = System.Convert.ToInt32(key);
                cur = lcur[ki];
            } else {
                // should we warn here, or just silently stop drilling
                // and return the last successful hit?
                FLog.Mark(path, key, cur.GetType().ToString());
            }
        }
        if (cur != root)
            rv = cur;
        return rv;
    }

    // ====================================================

    public Dictionary<string,object> GetDict(string path) {
        object o = Get(path);
        return (Dictionary<string,object>)o;
    }

    public Dictionary<string,object> GetDict(object r, string path) {
        object o = Get(r, path);
        return (Dictionary<string,object>)o;
    }

    public List<object> GetList(string path) {
        object o = Get(path);
        return (List<object>)o;
    }

    public List<object> GetList(object r, string path) {
        object o = Get(r, path);
        return (List<object>)o;
    }

    public Vector3 GetVector(string path) {
        object o = Get(path);
        return jtov(o);
    }

    public Vector3 GetVector(object r, string path) {
        object o = Get(r, path);
        return jtov(o);
    }

    public Vector3 GetVector(string path, Vector3 dv) {
        object o = Get(path);
        return jtov(o, dv);
    }

    public Vector3 GetVector(object r, string path, Vector3 dv) {
        object o = Get(r, path);
        return jtov(o, dv);
    }

    public string GetString(string path) {
        object o = Get(path);
        return (string)o;
    }

    public string GetString(object r, string path) {
        object o = Get(r, path);
        return (string)o;
    }

    public float GetFloat(string path) {
        object o = Get(path);
        return System.Convert.ToSingle(o);
    }

    public float GetFloat(object r, string path) {
        object o = Get(r, path);
        return System.Convert.ToSingle(o);
    }

    public int GetInt(string path) {
        object o = Get(path);
        return System.Convert.ToInt32(o);
    }

    public int GetInt(object r, string path) {
        object o = Get(r, path);
        return System.Convert.ToInt32(o);
    }

    public bool GetBool(string path) {
        object o = Get(path);
        return System.Convert.ToBoolean(o);
    }

    public bool GetBool(object r, string path) {
        object o = Get(r, path);
        return System.Convert.ToBoolean(o);
    }


    // Convert a JSON object to a vector.
    // The object can be either of two forms:
    //     { "x": 0, "y": 0, "z": 0 }
    //     [ 0, 0, 0 ]
    // In the object form, all three fields are optional.
    // In the array form, you can have two or three fields.
    public static Vector3 jtov(object j, Vector3 defaultValue) {
        Vector3 rv = defaultValue;
        if (j is List<object>) {
            List<object> jl = j as List<object>;
            if (jl.Count > 0)
                rv.x = System.Convert.ToSingle(jl[0]);
            if (jl.Count > 1)
                rv.y = System.Convert.ToSingle(jl[1]);
            if (jl.Count > 2)
                rv.z = System.Convert.ToSingle(jl[2]);
        } else if (j is Dictionary<string,object>) {
            Dictionary<string,object> jd = j as Dictionary<string,object>;
            if (jd.ContainsKey("x"))
                rv.x = System.Convert.ToSingle(jd["x"]);
            if (jd.ContainsKey("y"))
                rv.y = System.Convert.ToSingle(jd["y"]);
            if (jd.ContainsKey("z"))
                rv.z = System.Convert.ToSingle(jd["z"]);
        } else {
            FLog.LogError("invalid JSON vector object [" + j.ToString() + "]");
        }
        return rv;
    }

    public static Vector3 jtov(object j) {
        return jtov(j, Vector3.zero);
    }


    // ====================================================

#if UNITY_EDITOR
    public void Reload() {
        if (string.IsNullOrEmpty(manifestPath))
            return;
        string fn = "Assets/Resources/" + manifestPath + ".txt";
        Debug.Log("Reload layout data from [" + fn + "]");
        string s = System.IO.File.ReadAllText(fn);
        if (root != null) {
            root.Clear();
            root = null;
        }
        root = DarktableJSON.Json.Deserialize(s) as Dictionary<string,object>;
    }
#else
    public void Reload() { }
#endif

}
}
