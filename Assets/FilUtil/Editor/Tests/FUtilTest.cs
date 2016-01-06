using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using FilUtil;

namespace UnityTest {
internal class FUtilTest {
    // note that Shuffle, PickRandom, and Join exist as both utility and extension methods; we call them both ways just to make sure that's all cool.

    [Test]
    public void TestPickRandom() {
        List<int> listi = new List<int>();
        List<string> lists = new List<string>();

        // test empty lists.
        Assert.That(FUtil.PickRandom(listi), Is.EqualTo(default(int)));
        Assert.That(lists.PickRandom(), Is.EqualTo(default(string)));

        // test full lists.
        for (int i=100; i<120; i++)
            listi.Add(i);
        for (int i=100; i<120; i++)
            lists.Add(i.ToString());
        for (int i=0; i<100; i++) {
            Assert.That(FUtil.PickRandom(listi), Is.GreaterThanOrEqualTo(100));
            Assert.That(listi.PickRandom(), Is.LessThan(120));
            Assert.That(System.Convert.ToInt32(FUtil.PickRandom(lists)), Is.GreaterThanOrEqualTo(100));
            Assert.That(System.Convert.ToInt32(lists.PickRandom()), Is.LessThan(120));
        }

        // null
        Assert.That(FUtil.PickRandom((List<float>)null), Is.EqualTo(default(float)));
        Assert.That(FUtil.PickRandom((List<string>)null), Is.EqualTo(default(string)));
    }

    [Test]
    public void TestListToString() {
        // null
        Assert.That(FUtil.Join((List<float>)null), Is.EqualTo(""));
        Assert.That(FUtil.Join((List<string>)null), Is.EqualTo(""));

        List<int> listi = new List<int>();
        List<string> lists = new List<string>();

        // test empty lists.
        Assert.That(FUtil.Join(listi, ","), Is.EqualTo(""));
        Assert.That(lists.Join(), Is.EqualTo(""));

        // test full lists.
        for (int i=100; i<110; i++)
            listi.Add(i);
        Assert.That(FUtil.Join(listi, ","), Is.EqualTo("100,101,102,103,104,105,106,107,108,109"));

        for (int i=100; i<103; i++)
            lists.Add(i.ToString());
        lists.Add(null);
        for (int i=103; i<106; i++)
            lists.Add(i.ToString());
        lists.Add("");
        for (int i=106; i<110; i++)
            lists.Add(i.ToString());
        Assert.That(lists.Join(","), Is.EqualTo("100,101,102,,103,104,105,,106,107,108,109"));

    }

    [Test]
    public void TestShuffle() {
        FUtil.Shuffle((List<string>)null); // don't crash.

        // test empty.
        List<int> listi = new List<int>();
        listi.Shuffle();
        Assert.That(listi.Count, Is.EqualTo(0));

        // test full.
        for (int i=100; i<120; i++)
            listi.Add(i);
        listi.Shuffle();
        Assert.That(listi.Count, Is.EqualTo(20));
        for (int i=100; i<120; i++)
            Assert.That(listi.Contains(i), Is.True);
        listi.Shuffle();
        Assert.That(listi.Count, Is.EqualTo(20));
        for (int i=100; i<120; i++)
            Assert.That(listi.Contains(i), Is.True);

        // There are probably ways to unit test the quality of a shuffle,
        // but I'm not that ambitious.
    }


}
}
