using UnityEngine;
using System.Collections.Generic;

/*
 * I am responsible for maintaining a log that can be printed to console, displayed in game, dumped to a file, etc.
 *
 * This depends on the FILUTIL_DEBUG and FILUTIL_LOG flags, which should be set in Player Settings or in gmcs.rsp/smcs.rsp.
 */
namespace FilUtil {

public class FLog {
    public const int MAX_LOG_SIZE = 10000;

    private static FLog _instance;
    public static FLog Instance {
        get {
            if (_instance == null)
                _instance = new FLog();
            return _instance;
        }
    }

    private System.Text.StringBuilder logData;

    // 0 = Nothing to console
    // 1 = Errors to console
    // 2 = Errors and warnings to console
    // 3 = Everything to console
    public int verbosity;

    public FLog() {
        logData = new System.Text.StringBuilder();
        verbosity = 2;
    }

    public void Clear() {
        logData.Length = 0;
    }

    public string GetLog() {
        return logData.ToString();
    }

    // ====================================================

    [System.Diagnostics.Conditional("FILUTIL_LOG")]
    public void Log(int vlevel, string prefix, string msg) {
        logData.Append(prefix);
        logData.Append(msg);
        logData.Append('\n');
        if (logData.Length > MAX_LOG_SIZE)
            logData.Remove(0, logData.Length - MAX_LOG_SIZE);
        if (verbosity > vlevel)
            Debug.Log(prefix + msg);
    }

    [System.Diagnostics.Conditional("FILUTIL_LOG")]
    public static void Log(string s) {
        Instance.Log(2, "", s);
    }

    [System.Diagnostics.Conditional("FILUTIL_LOG")]
    public static void LogWarning(string s) {
        Instance.Log(1, "WARNING: ", s);
    }

    [System.Diagnostics.Conditional("FILUTIL_LOG")]
    public static void LogError(string s) {
        Instance.Log(0, "ERROR: ", s);
    }

    // ====================================================

    // For when you just want some quick debug tracer spam.
    // Ignores the verbosity setting and always prints to console.
    [System.Diagnostics.Conditional("FILUTIL_DEBUG")]
    public static void Mark(params object[] msgs) {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(GetStamp());
        foreach (object o in msgs)
            sb.Append(Bracketize(o));
        Debug.Log(sb.ToString());
        Instance.Log(99, "", sb.ToString());
    }

    // Use this when you want to fiddle with the depth of the stack trace, e.g., when you want the mark to mention a method's caller instead of itself.
    [System.Diagnostics.Conditional("FILUTIL_DEBUG")]
    public static void MarkModifiedStack(int extraStackDepth, params object[] msgs) {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(GetStamp(extraStackDepth));
        foreach (object o in msgs)
            sb.Append(Bracketize(o));
        Debug.Log(sb.ToString());
        Instance.Log(99, "", sb.ToString());
    }

    /*
     * Gets a stamp for debug messages that contains the time and the calling method.
     *
     * The calling method is judged to be the lowest frame in the stack that's
     * *not* in the FLog class. If depth is set, we will walk further up the
     * stack.
     */
    public static string GetStamp(int extraDepth=0) {
        string rv = "";
        System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();

        // Walk up the stack to the first frame that's *not* in this class (i.e., the lowest caller) and append it.
        System.Diagnostics.StackFrame sf = null;
        int i = 0;
        for ( ; i<trace.FrameCount; i++) {
            sf = trace.GetFrame(i);
            if (sf.GetMethod().DeclaringType != typeof(FLog))
                break;
        }

        // Walk up some extra frames as needed.
        i += extraDepth;

        sf = trace.GetFrame(i);
        if (sf != null) {
            string m = sf.GetMethod().DeclaringType.ToString();
            rv += System.String.Format("{0}:{1}.{2}", Time.realtimeSinceStartup.ToString("F1"), m, sf.GetMethod().Name);
        } else {
            rv = Time.realtimeSinceStartup.ToString("F1");
        }
        return rv;
    }

    public static string Bracketize(object o) {
        return " [" + ((o==null)?"":o.ToString()) + "]";
    }

}
}
