using UnityEngine;
using System.Collections.Generic;

/*
 * I am a tickless timer class.
 * I calculate my values based on UnityEngine.Time.time, so I will only "advance" once per frame.
 */
namespace FilUtil {
public class FTimer {
    private const float STOP_TIME = float.MinValue;
    private const float PAUSE_TIME = float.MaxValue;

    private float duration;
    private float start;
    private float bookmark, oldstart;

    // ====================================================

    public float Duration {
        get {
            return duration;
        }
    }

    // I am usually the time you called Start, but I am subject to change due to Pause, Delay, etc.
    public float StartTime {
        get {
            return start;
        }
    }

    // Total time passed since the timer started.
    // This value can be greater than duration.
    // This value is only meaningful when the timer is Running.
    public float Elapsed {
        get {
            if (Paused)
                return (bookmark - oldstart);
            else
                return (Now - start);
        }
    }

    // Time of the timer on a range of [0, 1].
    public float Normalized {
        get {
            return Mathf.Clamp((Elapsed / duration), 0, 1);
        }
    }

    // The timer is only Running if it has been Started.
    // Once Started, it will only become false when the timer is Stopped.
    // Running is still true when the timer is Paused.
    public bool Running {
        get {
            return (start != STOP_TIME);
        }
    }

    public bool Paused {
        get {
            return (start == PAUSE_TIME);
        }
    }

    // The timer is Done if it is Running and the Elapsed time is longer than the duration.
    public bool Done {
        get {
            return (start != STOP_TIME && Elapsed >= Duration);
        }
    }

    // ====================================================

    public FTimer(float d) {
        duration = d;
        Stop();
    }

    // Restart the timer using the duration I was constructed with.
    // Restart and Start are synonymous.
    public void Restart() {
        start = Now;
        bookmark = start;
        oldstart = start;
    }

    // Start the timer using the duration I was constructed with.
    // Start and Restart are synonymous.
    public void Start() {
        start = Now;
        bookmark = start;
        oldstart = start;
    }

    // Restart the timer using the given duration.
    // Restart and Start are synonymous.
    public void Restart(float d) {
        duration = d;
        start = Now;
        bookmark = start;
        oldstart = start;
    }

    // Start the timer using the given duration.
    // Start and Restart are synonymous.
    public void Start(float d) {
        duration = d;
        start = Now;
        bookmark = start;
        oldstart = start;
    }

    // Explicitly stop the timer so that Running, Paused, and Done are all false.
    // Stop and Cancel are synonymous.
    public void Stop() {
        start = STOP_TIME;
        oldstart = STOP_TIME;
        bookmark = STOP_TIME;
    }

    // Explicitly stop the timer so that Running, Paused, and Done are all false.
    // Stop and Cancel are synonymous.
    public void Cancel() {
        start = STOP_TIME;
        oldstart = STOP_TIME;
        bookmark = STOP_TIME;
    }

    // ====================================================

    // If you never Unpause, the timer will be full of junk.
    public void Pause() {
        bookmark = Now;
        oldstart = start;
        start = PAUSE_TIME;
    }

    // Unpausing without a prior Pause may result in junk.
    public void Unpause() {
        start = Now - (bookmark - oldstart);
    }

    // Alternate form for calling Pause/Unpause.
    public void Pause(bool p) {
        if (p)
            Pause();
        else
            Unpause();
    }

    // ====================================================

    // Extend the duration of the timer.
    public void Extend(float t) {
        duration += t;
    }

    // Edit state to fudge Elapsed time. This can result in a negative Elapsed value (though Normalized will never be less than 0).
    public void Delay(float t) {
        if (Paused) {
            oldstart += t;
        } else {
            start += t;
        }
    }

    // Edit state to register as Done right now.
    public void FinishImmediately() {
        Delay(Elapsed-duration);
    }

    // ====================================================

#if FILUTIL_DEBUG
    // Enable faking of time for unit test purposes.
    private float faketime = float.MinValue;
    public void _SetNow(float newt) {
        faketime = newt;
    }

    private float Now {
        get {
            if (faketime == float.MinValue)
                return Time.time;
            else
                return faketime;
        }
    }

#else
    private float Now {
        get {
            return Time.time;
        }
    }

#endif

}
}
