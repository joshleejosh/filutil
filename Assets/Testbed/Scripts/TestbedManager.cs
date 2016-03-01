using UnityEngine;
using System.Collections.Generic;
using FilUtil;

public class TestbedManager : MonoBehaviour {

    void Awake() {
        FLog.Mark("hello world");
        FConsole.Instance.Status("wake up");
        FConsole.Instance.AddControlCallback("???", OnConsoleGUI);
    }

    void Update() {
        FConsole.Instance.Status("Game Time = " + Time.time.ToString("F3"));
        FConsole.Instance.Mark();
    }

    void OnConsoleGUI() {
        if (FConsole.Instance.DebugButtonHalf("A", "testbed"))
            FLog.Mark("A");
        if (FConsole.Instance.DebugButtonHalf("B", "testbed"))
            FLog.Mark("B");

        if (FConsole.Instance.DebugButton("Z", ""))
            FLog.Mark("Z");

        if (FConsole.Instance.DebugButtonHalf("C", "testbed"))
            FLog.Mark("C");
        if (FConsole.Instance.DebugButton("D", "testbed"))
            FLog.Mark("D");
        if (FConsole.Instance.DebugButtonHalf("E", "testbed"))
            FLog.Mark("E");
        if (FConsole.Instance.DebugButtonHalf("F", "testbed"))
            FLog.Mark("F");
        if (FConsole.Instance.DebugButton("G", "testbed"))
            FLog.Mark("G");
    }

}
