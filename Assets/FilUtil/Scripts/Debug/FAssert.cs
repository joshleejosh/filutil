#define RAISE_EXCEPTIONS
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * I contain a bunch of functions that throw exceptions if conditions fail.
 *
 * For this to work, FILUTIL_ASSERT must be defined system-wide, either in Player Settings or in gmcs.rsp/smcs.rsp.
 */
namespace FilUtil {

public class FAssertException : System.Exception {
    public FAssertException(string msg) : base(msg) { }
}

public abstract class FAssert {

    // Override object.Equals to avoid confusion with the Equal assertion.
    public static new bool Equals(object a, object b) {
        Fail("You want Equal, not Equals", null);
        return false;
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void True(bool o, params object[] ctx) {
        if (!o)
            Fail("True", o, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void False(bool o, params object[] ctx) {
        if (o)
            Fail("False", o, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void Null(object o, params object[] ctx) {
        if (o != null)
            Fail("Null", o, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void NotNull(object o, params object[] ctx) {
        if (o == null)
            Fail("NotNull", o, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void Zero(double o, params object[] ctx) {
        if (o != 0)
            Fail("Zero", o, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void NonZero(double o, params object[] ctx) {
        if (o == 0)
            Fail("NotZero", o, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void Equal(object o, object p, params object[] ctx) {
        if (o == null && p != null)
            Fail("Equal", o, p, ctx);
        if (o != null && p == null)
            Fail("Equal", o, p, ctx);
        if (!object.Equals(o, p))
            Fail("Equal", o, p, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void NotEqual(object o, object p, params object[] ctx) {
        if (o == null && p == null)
            Fail("NotEqual", o, p, ctx);
        if (object.Equals(o, p))
            Fail("NotEqual", o, p, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void Empty(string s, params object[] ctx) {
        if (s == null)
            Fail("Empty", s, ctx);
        if (s != "")
            Fail("Empty", s, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void NotEmpty(string s, params object[] ctx) {
        if (s == null)
            Fail("NotEmpty", s, ctx);
        if (s == "")
            Fail("NotEmpty", s, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void Empty(ICollection c, params object[] ctx) {
        if (c == null)
            Fail("Empty", c, ctx);
        if (c.Count != 0)
            Fail("Empty", c, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void NotEmpty(ICollection c, params object[] ctx) {
        if (c == null)
            Fail("NotEmpty", c, ctx);
        if (c.Count == 0)
            Fail("NotEmpty", c, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void GreaterThan(double d, double e, params object[] ctx) {
        if (d <= e)
            Fail("GreaterThan", d, e, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void LessThan(double d, double e, params object[] ctx) {
        if (d >= e)
            Fail("LessThan", d, e, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void GreaterThanOrEqual(double d, double e, params object[] ctx) {
        if (d < e)
            Fail("GreaterThanOrEqual", d, e, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void LessThanOrEqual(double d, double e, params object[] ctx) {
        if (d > e)
            Fail("LessThanOrEqual", d, e, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void Contains<T>(ICollection<T> c, T o, params object[] ctx) {
        if (c == null)
            Fail("Contains", c, o, ctx);
        if (!c.Contains(o))
            Fail("Contains", c, o, ctx);
    }

    [System.Diagnostics.Conditional("FILUTIL_ASSERT")]
    public static void ContainsKey<T,U>(IDictionary<T,U> c, T o, params object[] ctx) {
        if (c == null)
            Fail("ContainsKey", c, o, ctx);
        if (o == null)
            Fail("ContainsKey", c, o, ctx);
        if (!c.ContainsKey(o))
            Fail("ContainsKey", c, o, ctx);
    }

    // ====================================================

    public static void Fail(string condition, object o, params object[] ctx) {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("ASSERT! ");
        sb.Append(FLog.GetStamp());
        sb.Append(FLog.Bracketize(condition));
        sb.Append(FLog.Bracketize(o));
        foreach (object co in ctx)
            sb.Append(FLog.Bracketize(co));
        FLog.LogError(sb.ToString());
#if RAISE_EXCEPTIONS
        throw new FAssertException(sb.ToString());
#endif
    }

    public static void Fail(string condition, object o, object p, params object[] ctx) {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("ASSERT! ");
        sb.Append(FLog.GetStamp());
        sb.Append(FLog.Bracketize(condition));
        sb.Append(FLog.Bracketize(o));
        sb.Append(FLog.Bracketize(p));
        foreach (object co in ctx)
            sb.Append(FLog.Bracketize(co));
        FLog.LogError(sb.ToString());
#if RAISE_EXCEPTIONS
        throw new FAssertException(sb.ToString());
#endif
    }

}
}
