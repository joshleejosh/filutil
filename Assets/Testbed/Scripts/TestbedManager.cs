using UnityEngine;
using System.Collections.Generic;
using FilUtil;

public class TestbedManager : MonoBehaviour {

    void Awake() {
        FLog.Mark("hello world");
        FConsole.Instance.Status("wake up");
    }

    void Update() {
        FConsole.Instance.Status("Game Time = " + Time.time.ToString("F3"));
    }


}
