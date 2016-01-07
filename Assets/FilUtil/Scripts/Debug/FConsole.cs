using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * I am a debug console that can display both realtime and logged information.
 */
namespace FilUtil {
public class FConsole : MonoBehaviour {
#if FILUTIL_DEBUG
    private const float BUTTON_SIZE_FACTOR = 0.08f;

    private bool isup, mode, needMetrics;
    private System.Text.StringBuilder bufStatus;

    private float triggerSize;
    private Vector2 buttonSize;
    private Vector2 logScrollPosition;
    private float margin, xContent, wContent;
    private GUIStyle boxStyle;

    void Awake() {
        bufStatus = new System.Text.StringBuilder();
        isup = false;
        mode = false;
        needMetrics = true;
    }

    void Update() {
        StartCoroutine(ClearStatus());
    }

    private void InitMetrics() {
        triggerSize = Camera.main.pixelHeight * BUTTON_SIZE_FACTOR;
        margin = triggerSize * 0.1f;
        buttonSize = new Vector2(triggerSize * 3 + margin * 2, triggerSize);
        xContent = buttonSize.x + margin;
        wContent = Camera.main.pixelWidth - xContent;

        boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.alignment = TextAnchor.UpperLeft;
    }

    void OnGUI() {
        if (needMetrics) {
            needMetrics = false;
            InitMetrics();
        }
        if (isup) {
            if (mode) {
                OnGUILog();
            } else {
                OnGUIStatus();
            }

            Rect r = new Rect(0, 0, buttonSize.x, buttonSize.y);
            r = AddOption(r, "Console/Log", OnConsoleLogToggle);
        }
        OnGUIToggle();
    }

    private void OnGUIStatus() {
        GUIContent content = new GUIContent(bufStatus.ToString());
        Rect dmr = GUILayoutUtility.GetRect(content, GUI.skin.box);
        dmr.x = xContent;
        dmr.width = wContent;
        dmr.height = Mathf.Max(triggerSize, Mathf.Min(dmr.height * 1.2f, Camera.main.pixelHeight));
        GUI.Box(dmr, content, boxStyle);
    }

    private void OnGUIToggle() {
        float w = triggerSize;
        float h = triggerSize;
        float x = Camera.main.pixelWidth - w;
        float y = 0;
        if (isup) {
            if (GUI.Button(new Rect(x, y, w, h), "X")) {
                isup = false;
            }
        } else {
            GUI.color = new Color(1, 1, 1, 0);
            if (GUI.Button(new Rect(x, y, w, h), "")) {
                isup = true;
            }
            GUI.color = new Color(1, 1, 1, 1);
        }
    }

    private Rect AddOption(Rect r, string s, System.Action f) {
        if (GUI.Button(r, s)) {
            f();
        }
        r.y += margin;
        return r;
    }

    // ====================================================

    private void OnConsoleLogToggle() {
        mode = !mode;
    }

    // ====================================================
    // Status stuff

    public void Status(string s) {
        bufStatus.Append('\n');
        bufStatus.Append(s);
    }

    private IEnumerator ClearStatus() {
        yield return new WaitForEndOfFrame();
        bufStatus.Length = 0;
    }

    // ====================================================
    // Log stuff

    private void OnGUILog() {
        var report = FLog.Instance.GetLog();
        Rect viewr = new Rect(xContent, 0, wContent, Screen.height);
        logScrollPosition = GUIScrollBox(report, viewr, logScrollPosition);
    }

    public Vector2 GUIScrollBox(string text, Rect viewRect, Vector2 scrollPosition) {
        Vector2 rv;
        GUIContent gc = new GUIContent(text);
        Rect r = GUILayoutUtility.GetRect(gc, boxStyle);
        r.width = Mathf.Max(r.width * 1.1f, viewRect.width);
        rv = GUI.BeginScrollView(viewRect, scrollPosition, r);
        GUI.Box(r, gc, boxStyle);
        GUI.EndScrollView();
        return rv;
    }

    // ====================================================
    // Disable me when not in debug mode

#else
    [System.Diagnostics.Conditional("FILUTIL_DEBUG")]
    public void Status(string s) {}
#endif

    // ====================================================
    // Singleton stuff

    private static FConsole _instance;
    private static bool _destroying;

    public static FConsole Instance {
        get {
            if (_instance == null && !_destroying) {
                GameObject gob = GameObject.Find("FConsole");
                if (gob == null) {
                    gob = new GameObject();
                    gob.name = "FConsole";
                }
                _instance = gob.GetComponent<FConsole>();
                if (_instance == null) {
                    _instance = gob.AddComponent<FConsole>();
                }
                DontDestroyOnLoad(gob);
            }
            return _instance;
        }
    }

    void OnDestroy() {
        _destroying = true;
    }
}
}
