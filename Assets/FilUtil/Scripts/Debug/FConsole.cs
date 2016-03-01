#define ENABLE_KEYBOARD
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
    private List<System.Action> controlCallbacks;

    private List<string> contexts;
    private int context;

    void Awake() {
        bufStatus = new System.Text.StringBuilder();
        controlCallbacks = new List<System.Action>();
        isup = false;
        mode = false;
        needMetrics = true;
        contexts = new List<string>();
        contexts.Add("");
        context = 0;
    }

    void Update() {
#if ENABLE_KEYBOARD
        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                FLog.Instance.Clear();
            else
                isup = !isup;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
            OnConsoleLogToggle();
#endif

        StartCoroutine(ClearStatus());
    }

    // Add a control action that will add buttons to the debug menu.
    // The action will be called during OnGUI.
    // context is currently unused.
    public void AddControlCallback(string context, System.Action callback) {
        if (!controlCallbacks.Contains(callback))
            controlCallbacks.Add(callback);
    }

    // ====================================================

    private void InitMetrics() {
        triggerSize = Camera.main.pixelHeight * BUTTON_SIZE_FACTOR;
        margin = triggerSize * 0.1f;
        buttonSize = new Vector2(triggerSize * 3 + margin * 2, triggerSize);
        xContent = buttonSize.x + margin;
        wContent = Camera.main.pixelWidth - xContent;

        boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.alignment = TextAnchor.UpperLeft;
    }

    private Rect controlRect;
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

            controlRect = new Rect(0, -(buttonSize.y+margin), buttonSize.x, buttonSize.y);
            if (DebugButtonForReal("CONSOLE/LOG"))
                OnConsoleLogToggle();
            if (DebugButtonForReal("CONTROLS: " + ((contexts[context]=="")?"_":contexts[context])))
                if (++context >= contexts.Count)
                    context = 0;

            // Call actions from anyone that wants to add their own control buttons.
            foreach (var cb in controlCallbacks)
                cb();
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

    private void OnConsoleLogToggle() {
        mode = !mode;
    }

    // ====================================================

    private bool DebugButtonForReal(string label) {
        controlRect.width = buttonSize.x;
        controlRect.x = 0;
        controlRect.y += controlRect.height + margin;
        bool rv = GUI.Button(controlRect, label);
        return rv;
    }

    // Display a button in the console control panel.
    // Returns true when the button is activated.
    public bool DebugButton(string label, string filter="") {
        if (!contexts.Contains(filter))
            contexts.Add(filter);
        if (contexts[context] != filter)
            return false;
        return DebugButtonForReal(label);
    }

    // Display a half-width button in the console button tray.
    // If this is called multiple times in a row, alternate between the left
    // and right half.
    public bool DebugButtonHalf(string label, string filter="") {
        if (!contexts.Contains(filter))
            contexts.Add(filter);
        if (contexts[context] != filter)
            return false;
        // guess what half this is based on the previous state of the control rect.
        if (controlRect.width == buttonSize.x) {
            // previous was a full-width button, put us on the left side.
            controlRect.width = buttonSize.x / 2 - margin / 2;
            controlRect.x = 0;
            controlRect.y += controlRect.height + margin;
        } else {
            if (controlRect.x == 0) {
                controlRect.x = controlRect.width + margin / 2;
            } else {
                controlRect.x = 0;
                controlRect.y += controlRect.height + margin;
            }
        }
        bool rv = GUI.Button(controlRect, label);
        return rv;
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

    public void Mark(params object[] msgs) {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(FLog.GetStamp(1));
        foreach (object o in msgs)
            sb.Append(FLog.Bracketize(o));
        Status(sb.ToString());
    }

    // ====================================================
    // Log stuff

    private void OnGUILog() {
        var report = FLog.Instance.GetLog();
        Rect viewr = new Rect(xContent, 0, wContent, Screen.height);
        logScrollPosition = GUIScrollBox(report, viewr, logScrollPosition);

        Rect cr = new Rect(Screen.width - triggerSize - margin - buttonSize.x, 0, buttonSize.x, buttonSize.y);
        if(GUI.Button(cr, "CLEAR LOG"))
            FLog.Instance.Clear();
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
    [System.Diagnostics.Conditional("FILUTIL_DEBUG")]
    public void Mark(params object[] msgs) {}
    [System.Diagnostics.Conditional("FILUTIL_DEBUG")]
    public void AddControlCallback(string context, System.Action callback) {}
    [System.Diagnostics.Conditional("FILUTIL_DEBUG")]
    public bool DebugButton(string label) {}
    [System.Diagnostics.Conditional("FILUTIL_DEBUG")]
    public bool DebugButtonHalf(string label) {}
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
