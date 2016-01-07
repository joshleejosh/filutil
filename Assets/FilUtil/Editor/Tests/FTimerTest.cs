using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using FilUtil;

namespace UnityTest {
internal class FTimerTest {

    private void AssertState(FTimer t, float e, float n, bool r, bool p, bool d) {
        Assert.That(t.Elapsed, Is.EqualTo(e));
        Assert.That(t.Normalized, Is.EqualTo(n));
        Assert.That(t.Running, Is.EqualTo(r));
        Assert.That(t.Paused, Is.EqualTo(p));
        Assert.That(t.Done, Is.EqualTo(d));
    }

    [Test]
    public void TestBasicState() {
        FTimer t = new FTimer(3f);
        Assert.That(t.Duration, Is.EqualTo(3f));
        // StartTime, Elapsed, and Normalized are garbage values on a non-Running timer.
        //Assert.That(t.StartTime, Is.EqualTo(0));
        //Assert.That(t.Elapsed, Is.EqualTo(0));
        //Assert.That(t.Normalized, Is.EqualTo(0));
        Assert.That(t.Running, Is.False);
        Assert.That(t.Paused, Is.False);
        Assert.That(t.Done, Is.False);

        t.Start();
        Assert.That(t.StartTime, Is.EqualTo(Time.time));
        AssertState(t, 0, 0, true, false, false);

#if FILUTIL_DEBUG
        // _SetNow is a debug hook used to test timer advancement.
        t._SetNow(t.StartTime + 1);
        AssertState(t, 1, 1f/3f, true, false, false);

        t._SetNow(t.StartTime + 3);
        AssertState(t, 3, 1, true, false, true);

        t._SetNow(t.StartTime + 4); // overtime
        AssertState(t, 4, 1, true, false, true);
#endif

        t.Stop();
        // StartTime, Elapsed, and Normalized go back to junk values after Stop.
        //Assert.That(t.StartTime, Is.EqualTo(0));
        //Assert.That(t.Elapsed, Is.EqualTo(0));
        //Assert.That(t.Normalized, Is.EqualTo(0));
        Assert.That(t.Running, Is.False);
        Assert.That(t.Paused, Is.False);
        Assert.That(t.Done, Is.False);
    }

    [Test]
    public void TestPause() {
        FTimer t = new FTimer(3);
        t.Start(4); // override original duration
        float startTime = t.StartTime; // pausing fiddles with the timer's start time, so don't depend on it while faking time.
        Assert.That(t.Duration, Is.EqualTo(4f));
        Assert.That(t.StartTime, Is.EqualTo(Time.time));

#if FILUTIL_DEBUG
        t._SetNow(startTime + 1);
        AssertState(t, 1, 0.25f, true, false, false);

        t.Pause();
        AssertState(t, 1, 0.25f, true, true, false);

        t._SetNow(startTime + 1);
        AssertState(t, 1, 0.25f, true, true, false);

        t._SetNow(startTime + 2);
        t.Unpause();
        AssertState(t, 1, 0.25f, true, false, false);

        t._SetNow(startTime + 3);
        AssertState(t, 2, 0.50f, true, false, false);

        t.Pause(true);
        t._SetNow(startTime + 7);
        AssertState(t, 2, 0.50f, true, true, false);

        t._SetNow(startTime + 9);
        AssertState(t, 2, 0.50f, true, true, false);

        t._SetNow(startTime + 10);
        t.Pause(false);
        AssertState(t, 2, 0.50f, true, false, false);

        t._SetNow(startTime + 12);
        AssertState(t, 4, 1, true, false, true);
#endif
    }

    [Test]
    public void TestDelay() {
        FTimer t = new FTimer(3);
        t.Restart(4); // Restart and Start are exactly the same.
        float startTime = t.StartTime;

        t.Extend(2);
        Assert.That(t.Duration, Is.EqualTo(6));
        Assert.That(t.StartTime, Is.EqualTo(Time.time));

        t.Delay(1);
        Assert.That(t.Duration, Is.EqualTo(6));
        Assert.That(t.StartTime, Is.EqualTo(startTime+1));
        // Delay can cause Elapsed to go negative, but Normalized will always be clamped to [0,1].
        AssertState(t, -1, 0, true, false, false);

#if FILUTIL_DEBUG
        t._SetNow(startTime + 3);
        AssertState(t, 2, 2f/6f, true, false, false);
        t.Delay(2);
        AssertState(t, 0, 0, true, false, false);

        // Extend and Delay should work while Paused.
        t._SetNow(startTime + 6);
        t.Pause();
        AssertState(t, 3, 0.5f, true, true, false);
        t.Delay(2);
        AssertState(t, 1, 1f/6f, true, true, false);
        t.Extend(2);
        AssertState(t, 1, 1f/8f, true, true, false);

        // We can force finish while paused.
        t.FinishImmediately();
        AssertState(t, 8, 1, true, true, true);
#endif

    }

}
}
