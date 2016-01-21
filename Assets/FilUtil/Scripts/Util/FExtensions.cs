using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Extension methods on various classes.
 */
namespace FilUtil {
public static class FExtensions {

    // Quick access to various points around a Bounds.
    // These functions are 2D-oriented; they all return the bound's center Z value as a placeholder.
    // TODO: Front/Center/Back for Z?
    public static Vector3 TopLeft      (this Bounds b) { return new Vector3( b.min.x,    b.max.y,    b.center.z ); }
    public static Vector3 TopCenter    (this Bounds b) { return new Vector3( b.center.x, b.max.y,    b.center.z ); }
    public static Vector3 TopRight     (this Bounds b) { return new Vector3( b.max.x,    b.max.y,    b.center.z ); }
    public static Vector3 MiddleLeft   (this Bounds b) { return new Vector3( b.min.x,    b.center.y, b.center.z ); }
    public static Vector3 MiddleCenter (this Bounds b) { return new Vector3( b.center.x, b.center.y, b.center.z ); }
    public static Vector3 MiddleRight  (this Bounds b) { return new Vector3( b.max.x,    b.center.y, b.center.z ); }
    public static Vector3 BottomLeft   (this Bounds b) { return new Vector3( b.min.x,    b.min.y,    b.center.z ); }
    public static Vector3 BottomCenter (this Bounds b) { return new Vector3( b.center.x, b.min.y,    b.center.z ); }
    public static Vector3 BottomRight  (this Bounds b) { return new Vector3( b.max.x,    b.min.y,    b.center.z ); }

    // Same as above, but extended outward by some distance.
    public static Vector3 TopLeft      (this Bounds b, float d) { return new Vector3( b.min.x - d , b.max.y + d , b.center.z ); }
    public static Vector3 TopCenter    (this Bounds b, float d) { return new Vector3( b.center.x  , b.max.y + d , b.center.z ); }
    public static Vector3 TopRight     (this Bounds b, float d) { return new Vector3( b.max.x + d , b.max.y + d , b.center.z ); }
    public static Vector3 MiddleLeft   (this Bounds b, float d) { return new Vector3( b.min.x - d , b.center.y  , b.center.z ); }
    public static Vector3 MiddleCenter (this Bounds b, float d) { return new Vector3( b.center.x  , b.center.y  , b.center.z ); }
    public static Vector3 MiddleRight  (this Bounds b, float d) { return new Vector3( b.max.x + d , b.center.y  , b.center.z ); }
    public static Vector3 BottomLeft   (this Bounds b, float d) { return new Vector3( b.min.x - d , b.min.y - d , b.center.z ); }
    public static Vector3 BottomCenter (this Bounds b, float d) { return new Vector3( b.center.x  , b.min.y - d , b.center.z ); }
    public static Vector3 BottomRight  (this Bounds b, float d) { return new Vector3( b.max.x + d , b.min.y - d , b.center.z ); }

    // Move objects in 1 or 2 directions while preserving their other dimensions.
    public static void MoveX       (this Transform  t, float   x          ) { t.position                = new Vector3(x,                           t.position.y,                t.position.z                ); }
    public static void MoveX       (this Component  c, float   x          ) { c.transform.position      = new Vector3(x,                           c.transform.position.y,      c.transform.position.z      ); }
    public static void MoveX       (this GameObject g, float   x          ) { g.transform.position      = new Vector3(x,                           g.transform.position.y,      g.transform.position.z      ); }
    public static void MoveXLocal  (this Transform  t, float   x          ) { t.localPosition           = new Vector3(x,                           t.localPosition.y,           t.localPosition.z           ); }
    public static void MoveXLocal  (this Component  c, float   x          ) { c.transform.localPosition = new Vector3(x,                           c.transform.localPosition.y, c.transform.localPosition.z ); }
    public static void MoveXLocal  (this GameObject g, float   x          ) { g.transform.localPosition = new Vector3(x,                           g.transform.localPosition.y, g.transform.localPosition.z ); }
    public static void MoveY       (this Transform  t, float   y          ) { t.position                = new Vector3(t.position.x,                y,                           t.position.z                ); }
    public static void MoveY       (this Component  c, float   y          ) { c.transform.position      = new Vector3(c.transform.position.x,      y,                           c.transform.position.z      ); }
    public static void MoveY       (this GameObject g, float   y          ) { g.transform.position      = new Vector3(g.transform.position.x,      y,                           g.transform.position.z      ); }
    public static void MoveYLocal  (this Transform  t, float   y          ) { t.localPosition           = new Vector3(t.localPosition.x,           y,                           t.localPosition.z           ); }
    public static void MoveYLocal  (this Component  c, float   y          ) { c.transform.localPosition = new Vector3(c.transform.localPosition.x, y,                           c.transform.localPosition.z ); }
    public static void MoveYLocal  (this GameObject g, float   y          ) { g.transform.localPosition = new Vector3(g.transform.localPosition.x, y,                           g.transform.localPosition.z ); }
    public static void MoveZ       (this Transform  t, float   z          ) { t.position                = new Vector3(t.position.x,                t.position.y,                z                           ); }
    public static void MoveZ       (this Component  c, float   z          ) { c.transform.position      = new Vector3(c.transform.position.x,      c.transform.position.y,      z                           ); }
    public static void MoveZ       (this GameObject g, float   z          ) { g.transform.position      = new Vector3(g.transform.position.x,      g.transform.position.y,      z                           ); }
    public static void MoveZLocal  (this Transform  t, float   z          ) { t.localPosition           = new Vector3(t.localPosition.x,           t.localPosition.y,           z                           ); }
    public static void MoveZLocal  (this Component  c, float   z          ) { c.transform.localPosition = new Vector3(c.transform.localPosition.x, c.transform.localPosition.y, z                           ); }
    public static void MoveZLocal  (this GameObject g, float   z          ) { g.transform.localPosition = new Vector3(g.transform.localPosition.x, g.transform.localPosition.y, z                           ); }

    public static void MoveXY      (this Transform  t, Vector2 p          ) { t.position                = new Vector3(p.x,                         p.y,                         t.position.z                ); }
    public static void MoveXY      (this Component  c, Vector2 p          ) { c.transform.position      = new Vector3(p.x,                         p.y,                         c.transform.position.z      ); }
    public static void MoveXY      (this GameObject g, Vector2 p          ) { g.transform.position      = new Vector3(p.x,                         p.y,                         g.transform.position.z      ); }
    public static void MoveXYLocal (this Transform  t, Vector2 p          ) { t.localPosition           = new Vector3(p.x,                         p.y,                         t.localPosition.z           ); }
    public static void MoveXYLocal (this Component  c, Vector2 p          ) { c.transform.localPosition = new Vector3(p.x,                         p.y,                         c.transform.localPosition.z ); }
    public static void MoveXYLocal (this GameObject g, Vector2 p          ) { g.transform.localPosition = new Vector3(p.x,                         p.y,                         g.transform.localPosition.z ); }

    public static void MoveXY      (this Transform  t, float   x, float y ) { t.position                = new Vector3(x,                           y,                           t.position.z                ); }
    public static void MoveXY      (this Component  c, float   x, float y ) { c.transform.position      = new Vector3(x,                           y,                           c.transform.position.z      ); }
    public static void MoveXY      (this GameObject g, float   x, float y ) { g.transform.position      = new Vector3(x,                           y,                           g.transform.position.z      ); }
    public static void MoveXYLocal (this Transform  t, float   x, float y ) { t.localPosition           = new Vector3(x,                           y,                           t.localPosition.z           ); }
    public static void MoveXYLocal (this Component  c, float   x, float y ) { c.transform.localPosition = new Vector3(x,                           y,                           c.transform.localPosition.z ); }
    public static void MoveXYLocal (this GameObject g, float   x, float y ) { g.transform.localPosition = new Vector3(x,                           y,                           g.transform.localPosition.z ); }
    public static void MoveXZ      (this Transform  t, float   x, float z ) { t.position                = new Vector3(x,                           t.position.y,                z                           ); }
    public static void MoveXZ      (this Component  c, float   x, float z ) { c.transform.position      = new Vector3(x,                           c.transform.position.y,      z                           ); }
    public static void MoveXZ      (this GameObject g, float   x, float z ) { g.transform.position      = new Vector3(x,                           g.transform.position.y,      z                           ); }
    public static void MoveXZLocal (this Transform  t, float   x, float z ) { t.localPosition           = new Vector3(x,                           t.localPosition.y,           z                           ); }
    public static void MoveXZLocal (this Component  c, float   x, float z ) { c.transform.localPosition = new Vector3(x,                           c.transform.localPosition.y, z                           ); }
    public static void MoveXZLocal (this GameObject g, float   x, float z ) { g.transform.localPosition = new Vector3(x,                           g.transform.localPosition.y, z                           ); }
    public static void MoveYZ      (this Transform  t, float   y, float z ) { t.position                = new Vector3(t.position.x,                y,                           z                           ); }
    public static void MoveYZ      (this Component  c, float   y, float z ) { c.transform.position      = new Vector3(c.transform.position.x,      y,                           z                           ); }
    public static void MoveYZ      (this GameObject g, float   y, float z ) { g.transform.position      = new Vector3(g.transform.position.x,      y,                           z                           ); }
    public static void MoveYZLocal (this Transform  t, float   y, float z ) { t.localPosition           = new Vector3(t.localPosition.x,           y,                           z                           ); }
    public static void MoveYZLocal (this Component  c, float   y, float z ) { c.transform.localPosition = new Vector3(c.transform.localPosition.x, y,                           z                           ); }
    public static void MoveYZLocal (this GameObject g, float   y, float z ) { g.transform.localPosition = new Vector3(g.transform.localPosition.x, y,                           z                           ); }

    public static void Move3       (this Transform  t, float x, float y, float z ) { t.position                = new Vector3(x, y, z); }
    public static void Move3       (this Component  c, float x, float y, float z ) { c.transform.position      = new Vector3(x, y, z); }
    public static void Move3       (this GameObject g, float x, float y, float z ) { g.transform.position      = new Vector3(x, y, z); }
    public static void Move3Local  (this Transform  t, float x, float y, float z ) { t.localPosition           = new Vector3(x, y, z); }
    public static void Move3Local  (this Component  c, float x, float y, float z ) { c.transform.localPosition = new Vector3(x, y, z); }
    public static void Move3Local  (this GameObject g, float x, float y, float z ) { g.transform.localPosition = new Vector3(x, y, z); }
    public static void Move3       (this Transform  t, Vector3 p) { t.position                = p; }
    public static void Move3       (this Component  c, Vector3 p) { c.transform.position      = p; }
    public static void Move3       (this GameObject g, Vector3 p) { g.transform.position      = p; }
    public static void Move3Local  (this Transform  t, Vector3 p) { t.localPosition           = p; }
    public static void Move3Local  (this Component  c, Vector3 p) { c.transform.localPosition = p; }
    public static void Move3Local  (this GameObject g, Vector3 p) { g.transform.localPosition = p; }

    public static void ScaleX  (this Transform  t, float   s            ) { t.localScale           = new Vector3(s,                        t.localScale.y,           t.localScale.z           ); }
    public static void ScaleX  (this GameObject g, float   s            ) { g.transform.localScale = new Vector3(s,                        g.transform.localScale.y, g.transform.localScale.z ); }
    public static void ScaleX  (this Component  c, float   s            ) { c.transform.localScale = new Vector3(s,                        c.transform.localScale.y, c.transform.localScale.z ); }
    public static void ScaleY  (this Transform  t, float   s            ) { t.localScale           = new Vector3(t.localScale.x,           s,                        t.localScale.z           ); }
    public static void ScaleY  (this GameObject g, float   s            ) { g.transform.localScale = new Vector3(g.transform.localScale.x, s,                        g.transform.localScale.z ); }
    public static void ScaleY  (this Component  c, float   s            ) { c.transform.localScale = new Vector3(c.transform.localScale.x, s,                        c.transform.localScale.z ); }
    public static void ScaleZ  (this Transform  t, float   s            ) { t.localScale           = new Vector3(t.localScale.x,           t.localScale.y,           s                        ); }
    public static void ScaleZ  (this GameObject g, float   s            ) { g.transform.localScale = new Vector3(g.transform.localScale.x, g.transform.localScale.y, s                        ); }
    public static void ScaleZ  (this Component  c, float   s            ) { c.transform.localScale = new Vector3(c.transform.localScale.x, c.transform.localScale.y, s                        ); }

    public static void ScaleXY (this Transform  t, Vector2 s            ) { t.localScale           = new Vector3(s.x,                      s.y,                      t.localScale.z           ); }
    public static void ScaleXY (this GameObject g, Vector2 s            ) { g.transform.localScale = new Vector3(s.x,                      s.y,                      g.transform.localScale.z ); }
    public static void ScaleXY (this Component  c, Vector2 s            ) { c.transform.localScale = new Vector3(s.x,                      s.y,                      c.transform.localScale.z ); }
    public static void ScaleXY (this Transform  t, float   s            ) { t.localScale           = new Vector3(s,                        s,                        t.localScale.z           ); }
    public static void ScaleXY (this GameObject g, float   s            ) { g.transform.localScale = new Vector3(s,                        s,                        g.transform.localScale.z ); }
    public static void ScaleXY (this Component  c, float   s            ) { c.transform.localScale = new Vector3(s,                        s,                        c.transform.localScale.z ); }
    public static void ScaleXY (this Transform  t, float   sx, float sy ) { t.localScale           = new Vector3(sx,                       sy,                       t.localScale.z           ); }
    public static void ScaleXY (this GameObject g, float   sx, float sy ) { g.transform.localScale = new Vector3(sx,                       sy,                       g.transform.localScale.z ); }
    public static void ScaleXY (this Component  c, float   sx, float sy ) { c.transform.localScale = new Vector3(sx,                       sy,                       c.transform.localScale.z ); }
    public static void ScaleXZ (this Transform  t, float   s            ) { t.localScale           = new Vector3(s,                        t.localScale.y,           s                        ); }
    public static void ScaleXZ (this GameObject g, float   s            ) { g.transform.localScale = new Vector3(s,                        g.transform.localScale.y, s                        ); }
    public static void ScaleXZ (this Component  c, float   s            ) { c.transform.localScale = new Vector3(s,                        c.transform.localScale.y, s                        ); }
    public static void ScaleXZ (this Transform  t, float   sx, float sz ) { t.localScale           = new Vector3(sx,                       t.localScale.y,           sz                       ); }
    public static void ScaleXZ (this GameObject g, float   sx, float sz ) { g.transform.localScale = new Vector3(sx,                       g.transform.localScale.y, sz                       ); }
    public static void ScaleXZ (this Component  c, float   sx, float sz ) { c.transform.localScale = new Vector3(sx,                       c.transform.localScale.y, sz                       ); }
    public static void ScaleYZ (this Transform  t, float   s            ) { t.localScale           = new Vector3(t.localScale.x,           s,                        s                        ); }
    public static void ScaleYZ (this GameObject g, float   s            ) { g.transform.localScale = new Vector3(g.transform.localScale.x, s,                        s                        ); }
    public static void ScaleYZ (this Component  c, float   s            ) { c.transform.localScale = new Vector3(c.transform.localScale.x, s,                        s                        ); }
    public static void ScaleYZ (this Transform  t, float   sy, float sz ) { t.localScale           = new Vector3(t.localScale.x,           sy,                       sz                       ); }
    public static void ScaleYZ (this GameObject g, float   sy, float sz ) { g.transform.localScale = new Vector3(g.transform.localScale.x, sy,                       sz                       ); }
    public static void ScaleYZ (this Component  c, float   sy, float sz ) { c.transform.localScale = new Vector3(c.transform.localScale.x, sy,                       sz                       ); }

    public static void Scale3  (this GameObject g, Vector3 v                     ) { g.transform.localScale = v;                        }
    public static void Scale3  (this Transform  t, Vector3 v                     ) { t.localScale           = v;                        }
    public static void Scale3  (this Component  c, Vector3 v                     ) { c.transform.localScale = v;                        }
    public static void Scale3  (this GameObject g, float   s                     ) { g.transform.localScale = new Vector3(s,  s,  s  ); }
    public static void Scale3  (this Transform  t, float   s                     ) { t.localScale           = new Vector3(s,  s,  s  ); }
    public static void Scale3  (this Component  c, float   s                     ) { c.transform.localScale = new Vector3(s,  s,  s  ); }
    public static void Scale3  (this GameObject g, float   sx, float sy, float sz) { g.transform.localScale = new Vector3(sx, sy, sz ); }
    public static void Scale3  (this Transform  t, float   sx, float sy, float sz) { t.localScale           = new Vector3(sx, sy, sz ); }
    public static void Scale3  (this Component  c, float   sx, float sy, float sz) { c.transform.localScale = new Vector3(sx, sy, sz ); }

    public static void Shuffle<T>(this List<T> o) { FUtil.Shuffle(o); }
    public static T PickRandom<T>(this List<T> o) { return FUtil.PickRandom(o); }
    public static string Join<T>(this List<T> o, string delimiter="") { return FUtil.Join(o, delimiter); }

}
}
