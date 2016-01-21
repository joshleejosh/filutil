using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using FilUtil;

namespace UnityTest {
internal class FExtensionsTest {

    private GameObject parent, child;
    private Transform tparent, tchild;
    private Component cparent, cchild;

    [SetUp]
    public void SetUp() {
        parent = new GameObject("test");
        tparent = parent.transform;
        cparent = parent.AddComponent<SpriteRenderer>();
        child = new GameObject("testchild");
        child.transform.parent = parent.transform;
        tchild = child.transform;
        cchild = child.AddComponent<SpriteRenderer>();
    }

    [TearDown]
    public void TearDown() {
        UnityEngine.Object.DestroyImmediate(child);
        UnityEngine.Object.DestroyImmediate(parent);
    }

    // ====================================================

    [Test]
    public void TestBoundsZero() {
        Bounds b = new Bounds();
        Assert.That(b.TopLeft(), Is.EqualTo(Vector3.zero));
        Assert.That(b.TopCenter(), Is.EqualTo(Vector3.zero));
        Assert.That(b.TopRight(), Is.EqualTo(Vector3.zero));
        Assert.That(b.MiddleLeft(), Is.EqualTo(Vector3.zero));
        Assert.That(b.MiddleCenter(), Is.EqualTo(Vector3.zero));
        Assert.That(b.MiddleRight(), Is.EqualTo(Vector3.zero));
        Assert.That(b.BottomLeft(), Is.EqualTo(Vector3.zero));
        Assert.That(b.BottomCenter(), Is.EqualTo(Vector3.zero));
        Assert.That(b.BottomRight(), Is.EqualTo(Vector3.zero));
        Assert.That(b.TopLeft(2),      Is.EqualTo(new Vector3(-2,  2, 0)));
        Assert.That(b.TopCenter(2),    Is.EqualTo(new Vector3( 0,  2, 0)));
        Assert.That(b.TopRight(2),     Is.EqualTo(new Vector3( 2,  2, 0)));
        Assert.That(b.MiddleLeft(2),   Is.EqualTo(new Vector3(-2,  0, 0)));
        Assert.That(b.MiddleCenter(2), Is.EqualTo(new Vector3( 0,  0, 0)));
        Assert.That(b.MiddleRight(2),  Is.EqualTo(new Vector3( 2,  0, 0)));
        Assert.That(b.BottomLeft(2),   Is.EqualTo(new Vector3(-2, -2, 0)));
        Assert.That(b.BottomCenter(2), Is.EqualTo(new Vector3( 0, -2, 0)));
        Assert.That(b.BottomRight(2),  Is.EqualTo(new Vector3( 2, -2, 0)));
    }

    [Test]
    public void TestBounds() {
        Bounds b = new Bounds(new Vector3(-2, 2, -2), new Vector3(4, 6, 8));
        Assert.That(b.TopLeft(),       Is.EqualTo(new Vector3(-4,  5, -2)));
        Assert.That(b.TopCenter(),     Is.EqualTo(new Vector3(-2,  5, -2)));
        Assert.That(b.TopRight(),      Is.EqualTo(new Vector3( 0,  5, -2)));
        Assert.That(b.MiddleLeft(),    Is.EqualTo(new Vector3(-4,  2, -2)));
        Assert.That(b.MiddleCenter(),  Is.EqualTo(new Vector3(-2,  2, -2)));
        Assert.That(b.MiddleRight(),   Is.EqualTo(new Vector3( 0,  2, -2)));
        Assert.That(b.BottomLeft(),    Is.EqualTo(new Vector3(-4, -1, -2)));
        Assert.That(b.BottomCenter(),  Is.EqualTo(new Vector3(-2, -1, -2)));
        Assert.That(b.BottomRight(),   Is.EqualTo(new Vector3( 0, -1, -2)));
        Assert.That(b.TopLeft(2),      Is.EqualTo(new Vector3(-6,  7, -2)));
        Assert.That(b.TopCenter(2),    Is.EqualTo(new Vector3(-2,  7, -2)));
        Assert.That(b.TopRight(2),     Is.EqualTo(new Vector3( 2,  7, -2)));
        Assert.That(b.MiddleLeft(2),   Is.EqualTo(new Vector3(-6,  2, -2)));
        Assert.That(b.MiddleCenter(2), Is.EqualTo(new Vector3(-2,  2, -2)));
        Assert.That(b.MiddleRight(2),  Is.EqualTo(new Vector3( 2,  2, -2)));
        Assert.That(b.BottomLeft(2),   Is.EqualTo(new Vector3(-6, -3, -2)));
        Assert.That(b.BottomCenter(2), Is.EqualTo(new Vector3(-2, -3, -2)));
        Assert.That(b.BottomRight(2),  Is.EqualTo(new Vector3( 2, -3, -2)));
    }

    // ====================================================

    void AssertParentPosition(float ax, float ay, float az) {
      // positions are super-drifty, and Is.EqualTo doesn't account for that.
      Assert.That(parent.transform.position == new Vector3(ax, ay, az), Is.True);
    }

    void AssertChildPosition(float lx, float ly, float lz, float gx, float gy, float gz) {
      Assert.That(child.transform.localPosition == new Vector3(lx, ly, lz), Is.True);
      Assert.That(child.transform.position == new Vector3(gx, gy, gz), Is.True);
    }

    [Test]
    public void TestMove1() {
        AssertParentPosition(0, 0, 0);
        parent.MoveX(-3.4f); AssertParentPosition(-3.4f, 0, 0);
        parent.MoveY( 2.5f); AssertParentPosition(-3.4f, 2.5f, 0);
        parent.MoveZ( 1.1f); AssertParentPosition(-3.4f, 2.5f, 1.1f);

        tparent.MoveZ( 3.2f); AssertParentPosition(-3.4f, 2.5f, 3.2f);
        tparent.MoveY(-7.1f); AssertParentPosition(-3.4f, -7.1f, 3.2f);
        tparent.MoveX( 9.3f); AssertParentPosition(9.3f, -7.1f, 3.2f);

        cparent.MoveX( 8.4f); AssertParentPosition(8.4f, -7.1f, 3.2f);
        cparent.MoveY( 4.7f); AssertParentPosition(8.4f, 4.7f, 3.2f);
        cparent.MoveZ(-5.6f); AssertParentPosition(8.4f, 4.7f, -5.6f);
    }

    [Test]
    public void TestMove1Local() {
        parent.Move3(-1.5f, 2.0f, -3.5f); AssertParentPosition(-1.5f, 2.0f, -3.5f);

        child.MoveXLocal( 2.5f); AssertChildPosition(2.5f,    0f,   0f, 1.0f,  2.0f, -3.5f);
        child.MoveYLocal(-3.0f); AssertChildPosition(2.5f, -3.0f,   0f, 1.0f, -1.0f, -3.5f);
        child.MoveZLocal( 2.5f); AssertChildPosition(2.5f, -3.0f, 2.5f, 1.0f, -1.0f, -1.0f);

        tchild.MoveXLocal(-1.5f); AssertChildPosition(-1.5f, -3.0f,  2.5f, -3.0f, -1.0f, -1.0f);
        tchild.MoveYLocal( 2.0f); AssertChildPosition(-1.5f,  2.0f,  2.5f, -3.0f,  4.0f, -1.0f);
        tchild.MoveZLocal(-2.0f); AssertChildPosition(-1.5f,  2.0f, -2.0f, -3.0f,  4.0f, -5.5f);

        cchild.MoveXLocal(  12); AssertChildPosition(12, 2.0f, -2.0f, 10.5f, 4.0f, -5.5f);
        cchild.MoveYLocal( -30); AssertChildPosition(12,  -30, -2.0f, 10.5f, -28f, -5.5f);
        cchild.MoveZLocal(-150); AssertChildPosition(12,  -30, -150f, 10.5f, -28f, -153.5f);
    }

    [Test]
    public void TestMove2Vector2() {
        parent.Move3(-1.5f, 2.0f, -3.5f); AssertParentPosition(-1.5f, 2.0f, -3.5f);

        parent.MoveXY (new Vector2(    2,    -4)); AssertParentPosition(    2,    -4, -3.5f);
        tparent.MoveXY(new Vector2(-7.3f, -1.1f)); AssertParentPosition(-7.3f, -1.1f, -3.5f);
        cparent.MoveXY(new Vector2( 2.9f,  1.2f)); AssertParentPosition( 2.9f,  1.2f, -3.5f);

        child.MoveXYLocal (new Vector2(    3,     3)); AssertChildPosition(    3,     3, 0,  5.9f,  4.2f, -3.5f);
        tchild.MoveXYLocal(new Vector2(-6.6f, 11.3f)); AssertChildPosition(-6.6f, 11.3f, 0, -3.7f, 12.5f, -3.5f);
        cchild.MoveXYLocal(new Vector2( 2.7f, -0.1f)); AssertChildPosition( 2.7f, -0.1f, 0,  5.6f,  1.1f, -3.5f);
    }

    [Test]
    public void TestMove2() {
        parent.Move3(-1.5f, 2.0f, -0.1f); AssertParentPosition(-1.5f, 2.0f, -0.1f);

        parent.MoveXY (   32,    -44); AssertParentPosition(   32,    -44,  -0.1f);
        tparent.MoveXY(-2.3f, -71.3f); AssertParentPosition(-2.3f, -71.3f,  -0.1f);
        cparent.MoveXY(-3.3f,  12.3f); AssertParentPosition(-3.3f,  12.3f,  -0.1f);
        parent.MoveXZ (   32,    -44); AssertParentPosition(   32,  12.3f,    -44);
        tparent.MoveXZ(-2.3f, -71.3f); AssertParentPosition(-2.3f,  12.3f, -71.3f);
        cparent.MoveXZ(-3.3f,  12.3f); AssertParentPosition(-3.3f,  12.3f,  12.3f);
        parent.MoveYZ (   32,    -44); AssertParentPosition(-3.3f,     32,    -44);
        tparent.MoveYZ(-2.3f, -71.3f); AssertParentPosition(-3.3f,  -2.3f, -71.3f);
        cparent.MoveYZ(-3.3f,  12.3f); AssertParentPosition(-3.3f,  -3.3f,  12.3f);

        parent.Move3(-1.5f, 2.0f, -0.1f); AssertParentPosition(-1.5f, 2.0f, -0.1f);
        child.MoveXYLocal ( -62,    15); AssertChildPosition(  -62,    15,     0, -63.5f,   17, -0.1f);
        tchild.MoveXYLocal(2.7f, -1.9f); AssertChildPosition( 2.7f, -1.9f,     0,   1.2f, 0.1f, -0.1f);
        cchild.MoveXYLocal(-9.4f, 5.8f); AssertChildPosition(-9.4f,  5.8f,     0, -10.9f, 7.8f, -0.1f);
        child.MoveXZLocal ( -62,    15); AssertChildPosition(  -62,  5.8f,    15, -63.5f, 7.8f, 14.9f);
        tchild.MoveXZLocal(2.7f, -1.9f); AssertChildPosition( 2.7f,  5.8f, -1.9f,   1.2f, 7.8f, -2.0f);
        cchild.MoveXZLocal(-9.4f, 5.8f); AssertChildPosition(-9.4f,  5.8f,  5.8f, -10.9f, 7.8f,  5.7f);
        child.MoveYZLocal ( -62,    15); AssertChildPosition(-9.4f,   -62,    15, -10.9f,  -60, 14.9f);
        tchild.MoveYZLocal(2.7f, -1.9f); AssertChildPosition(-9.4f,  2.7f, -1.9f, -10.9f, 4.7f, -2.0f);
        cchild.MoveYZLocal(-9.4f, 5.8f); AssertChildPosition(-9.4f, -9.4f,  5.8f, -10.9f,-7.4f,  5.7f);
    }

    [Test]
    public void TestMove3() {
        parent.Move3 (  -1.5f,   2.0f,   -0.1f); AssertParentPosition(  -1.5f,   2.0f,   -0.1f);
        tparent.Move3(     39,    -41,      27); AssertParentPosition(     39,    -41,      27);
        cparent.Move3(-193.7f, 926.1f, -542.8f); AssertParentPosition(-193.7f, 926.1f, -542.8f);
        parent.Move3 (new Vector3(  -1.5f,   2.0f,   -0.1f)); AssertParentPosition(  -1.5f,   2.0f,   -0.1f);
        tparent.Move3(new Vector3(     39,    -41,      27)); AssertParentPosition(     39,    -41,      27);
        cparent.Move3(new Vector3(-193.7f, 926.1f, -542.8f)); AssertParentPosition(-193.7f, 926.1f, -542.8f);

        parent.Move3 (  -1.5f,   2.0f,   -0.1f); AssertParentPosition(  -1.5f,   2.0f,   -0.1f);
        child.Move3Local (   -24,   722,   986); AssertChildPosition(   -24,   722,   986, -25.5f,   724, 985.9f);
        tchild.Move3Local(  6.2f, -9.6f, -1.8f); AssertChildPosition(  6.2f, -9.6f, -1.8f,   4.7f, -7.6f,  -1.9f);
        cchild.Move3Local(-53.6f, 87.7f, 39.7f); AssertChildPosition(-53.6f, 87.7f, 39.7f, -55.1f, 89.7f,  39.6f);
        child.Move3Local (new Vector3(   -24,   722,   986)); AssertChildPosition(   -24,   722,   986, -25.5f,   724, 985.9f);
        tchild.Move3Local(new Vector3(  6.2f, -9.6f, -1.8f)); AssertChildPosition(  6.2f, -9.6f, -1.8f,   4.7f, -7.6f,  -1.9f);
        cchild.Move3Local(new Vector3(-53.6f, 87.7f, 39.7f)); AssertChildPosition(-53.6f, 87.7f, 39.7f, -55.1f, 89.7f,  39.6f);
    }

    // ====================================================

    void AssertScale(float sx, float sy, float sz) {
      Assert.That(parent.transform.localScale == new Vector3(sx, sy, sz), Is.True);
    }

    [Test]
    public void TestScale1() {
      AssertScale(1, 1, 1);
      parent.ScaleX (3.4f  ) ; AssertScale(3.4f  , 1     , 1     ) ;
      tparent.ScaleX(-79   ) ; AssertScale(-79   , 1     , 1     ) ;
      cparent.ScaleX(-4.6f ) ; AssertScale(-4.6f , 1     , 1     ) ;
      parent.ScaleY (3.4f  ) ; AssertScale(-4.6f , 3.4f  , 1     ) ;
      tparent.ScaleY(-79   ) ; AssertScale(-4.6f , -79   , 1     ) ;
      cparent.ScaleY(-4.6f ) ; AssertScale(-4.6f , -4.6f , 1     ) ;
      parent.ScaleZ (3.4f  ) ; AssertScale(-4.6f , -4.6f , 3.4f  ) ;
      tparent.ScaleZ(-79   ) ; AssertScale(-4.6f , -4.6f , -79   ) ;
      cparent.ScaleZ(-4.6f ) ; AssertScale(-4.6f , -4.6f , -4.6f ) ;
    }

    [Test]
    public void TestScale2() {
      AssertScale(1, 1, 1);
      parent.ScaleXY (new Vector2(-604   , 196   )  ) ; AssertScale(-604   , 196   , 1 ) ;
      tparent.ScaleXY(new Vector2(6.2f   , -1.8f )  ) ; AssertScale(6.2f   , -1.8f , 1 ) ;
      cparent.ScaleXY(new Vector2(-27.7f , 90.3f )  ) ; AssertScale(-27.7f , 90.3f , 1 ) ;
      parent.ScaleXY (  -604       ); AssertScale(  -604,   -604,      1);
      tparent.ScaleXY(  6.2f       ); AssertScale(  6.2f,   6.2f,      1);
      cparent.ScaleXY(-27.7f       ); AssertScale(-27.7f, -27.7f,      1);
      parent.ScaleXY (  -604,   196); AssertScale(  -604,    196,      1);
      tparent.ScaleXY(  6.2f, -1.8f); AssertScale(  6.2f,  -1.8f,      1);
      cparent.ScaleXY(-27.7f, 90.3f); AssertScale(-27.7f,  90.3f,      1);
      parent.ScaleXZ (  -604       ); AssertScale(  -604,  90.3f,   -604);
      tparent.ScaleXZ(  6.2f       ); AssertScale(  6.2f,  90.3f,   6.2f);
      cparent.ScaleXZ(-27.7f       ); AssertScale(-27.7f,  90.3f, -27.7f);
      parent.ScaleXZ (  -604,   196); AssertScale(  -604,  90.3f,    196);
      tparent.ScaleXZ(  6.2f, -1.8f); AssertScale(  6.2f,  90.3f,  -1.8f);
      cparent.ScaleXZ(-27.7f, 90.3f); AssertScale(-27.7f,  90.3f,  90.3f);
      parent.ScaleYZ (  -604       ); AssertScale(-27.7f,   -604,   -604);
      tparent.ScaleYZ(  6.2f       ); AssertScale(-27.7f,   6.2f,   6.2f);
      cparent.ScaleYZ(-27.7f       ); AssertScale(-27.7f, -27.7f, -27.7f);
      parent.ScaleYZ (  -604,   196); AssertScale(-27.7f,   -604,    196);
      tparent.ScaleYZ(  6.2f, -1.8f); AssertScale(-27.7f,   6.2f,  -1.8f);
      cparent.ScaleYZ(-27.7f, 90.3f); AssertScale(-27.7f, -27.7f,  90.3f);
    }

    [Test]
    public void TestScale3() {
      AssertScale(1, 1, 1);
      parent.Scale3 (new Vector3(834, -562, 620)); AssertScale(834, -562, 620);
      tparent.Scale3(new Vector3(-3.7f, 6.2f, 1.9f)); AssertScale(-3.7f, 6.2f, 1.9f);
      cparent.Scale3(new Vector3(82.7f, -66.1f, 37.9f)); AssertScale(82.7f, -66.1f, 37.9f);
      parent.Scale3 (834, -562, 620); AssertScale(834, -562, 620);
      tparent.Scale3(-3.7f, 6.2f, 1.9f); AssertScale(-3.7f, 6.2f, 1.9f);
      cparent.Scale3(82.7f, -66.1f, 37.9f); AssertScale(82.7f, -66.1f, 37.9f);
      parent.Scale3 (834); AssertScale(834, 834, 834);
      tparent.Scale3(-3.7f); AssertScale(-3.7f, -3.7f, -3.7f);
      cparent.Scale3(82.7f); AssertScale(82.7f, 82.7f, 82.7f);
    }

}
}

