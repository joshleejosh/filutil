using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using FilUtil;

namespace UnityTest {
internal class FAssertTest {

    [Test]
    public void TestAssertTrue() {
        bool o = true;
        FAssert.True(o);
        try {
            o = false;
            FAssert.True(o, "context", 3, true);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException e) {
            Debug.Log(e);
        }
    }

    [Test]
    public void TestAssertFalse() {
        bool o = false;
        FAssert.False(o);
        try {
            o = true;
            FAssert.False(o, "context", 3.14, new Dictionary<string,object>());
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException e) {
            Debug.Log(e);
        }
    }

    [Test]
    public void TestAssertNull() {
        object o = null;
        FAssert.Null(o);
        try {
            o = new object();
            FAssert.Null(o);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestAssertNotNull() {
        object o = new object();
        FAssert.NotNull(o);
        try {
            o = null;
            FAssert.NotNull(o);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestAssertZero() {
        float f = 0.0f;
        FAssert.Zero(f);
        int i = 0;
        FAssert.Zero(i);
        try {
            f = 0.01f;
            FAssert.Zero(f);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
        try {
            i = -1;
            FAssert.Zero(i);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestAssertNonZero() {
        float f = 0.1f;
        FAssert.NonZero(f);
        int i = -1;
        FAssert.NonZero(i);
        try {
            f = 0.0f;
            FAssert.NonZero(f);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
        try {
            i = 0;
            FAssert.NonZero(i);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestAssertEqual() {
        object o = new object();
        object p = o;
        FAssert.Equal(o, p);
        int i = 3;
        int j = 3;
        FAssert.Equal(i, j);
        float f = 3f;
        float g = 3.0f;
        FAssert.Equal(f, g);
        FAssert.Equal(i, (int)g);
        try {
            o = new object();
            FAssert.Equal(o, p);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
        try {
            o = null;
            FAssert.Equal(o, p);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestAssertNotEqual() {
        object o = new object();
        object p = new object();
        FAssert.NotEqual(o, p);
        p = null;
        FAssert.NotEqual(o, p);
        int i = 0;
        float f = 0f;
        FAssert.NotEqual(i, f);
        try {
            o = p;
            FAssert.NotEqual(o, p);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestAssertEmptyString() {
        FAssert.Empty("");
        try {
            FAssert.Empty("fasdf");
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
        try {
            FAssert.Empty((string)null);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestAssertNotNullOrEmptyString() {
        string s = " ";
        FAssert.NotEmpty(s);
        try {
            FAssert.NotEmpty("");
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
        try {
            FAssert.NotEmpty((string)null);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestAssertEmptyCollection() {
        ArrayList ol = new ArrayList();
        FAssert.Empty(ol);
        try {
            ol.Add(new object());
            FAssert.Empty(ol);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }

        List<string> sl = new List<string>();
        FAssert.Empty(sl);
        try {
            sl.Add("hi");
            FAssert.Empty(sl);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }

        try {
            FAssert.Empty((ICollection)null);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestAssertNotNullOrEmptyCollection() {
        ArrayList ol = new ArrayList();
        ol.Add(new object());
        FAssert.NotEmpty(ol);
        try {
            ol.Clear();
            FAssert.NotEmpty(ol);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }

        List<string> sl = new List<string>();
        sl.Add("hi");
        FAssert.NotEmpty(sl);
        try {
            sl.Clear();
            FAssert.NotEmpty(sl);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }

        try {
            FAssert.NotEmpty((ICollection)null);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestGreaterThan() {
        FAssert.GreaterThan(2.0001f, 1.9999f);
        FAssert.GreaterThan(2, 1);
        try {
            FAssert.GreaterThan(1, 1);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
        try {
            FAssert.GreaterThan(1.9999f, 2.0001);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestLessThan() {
        FAssert.LessThan(1.9999f, 2.0001);
        FAssert.LessThan(1, 2);
        FAssert.LessThan((byte)0x23, (long)0x24);
        try {
            FAssert.LessThan(1, 1);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
        try {
            FAssert.LessThan(2.0001f, 1.9999f);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestGreaterThanOrEqual() {
        FAssert.GreaterThanOrEqual(2.0001f, 1.9999f);
        FAssert.GreaterThanOrEqual(2, 1);
        FAssert.GreaterThanOrEqual((byte)234, (long)234);
        try {
            FAssert.GreaterThanOrEqual(1.9999f, 2.0001);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestLessThanOrEqual() {
        FAssert.LessThanOrEqual(1.9999f, 2.0001);
        FAssert.LessThanOrEqual(1, 2);
        FAssert.LessThanOrEqual((byte)0x23, (long)0x24);
        FAssert.LessThanOrEqual(300.1, 300.1);
        try {
            FAssert.LessThanOrEqual(2.0001f, 1.9999f);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

    [Test]
    public void TestContains() {
        List<string> c = new List<string>();
        try {
            FAssert.Contains(c, "hello");
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
        c.Add("blah");
        c.Add((string)null);
        FAssert.Contains(c, "blah");
        FAssert.Contains(c, (string)null);
    }

    [Test]
    public void TestContainsKey() {
        Dictionary<string,string> c = new Dictionary<string,string>();
        try {
            FAssert.ContainsKey(c, "hello");
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
        try {
            FAssert.ContainsKey(c, (string)null);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
        c["blah"] = "hey";
        c["bloo"] = "hoy";
        FAssert.ContainsKey(c, "blah");
    }

    [Test]
    public void TestAssertFail() {
        try {
            FAssert.Fail("Test", "hi", "boo", 33, 44.55);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException e) {
            Debug.Log(e);
        }
    }

    // Equal() is an assertion, but Equals() is the object equality test. That's probably not what you want.
    [Test]
    public void TestAssertEquals() {
        try {
            FAssert.Equals(1, 1);
            Assert.IsTrue(false, "FAssert should have thrown an exception");
        } catch (FAssertException) { }
    }

}
}
