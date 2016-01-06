using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FilUtil {
public class FFactory : MonoBehaviour {

    void Awake() {
    }


    // ====================================================

    private static FFactory _instance;
    private static bool _destroying;

    public static FFactory Instance {
        get {
            if (_instance == null && !_destroying) {
                GameObject gob = GameObject.Find("FFactory");
                if (gob == null) {
                    gob = new GameObject();
                    gob.name = "FFactory";
                }
                _instance = gob.GetComponent<FFactory>();
                if (_instance == null) {
                    gob.AddComponent<FFactory>();
                }
                DontDestroyOnLoad(gob);
            }
            return _instance;
        }
    }

    void OnDestroy() {
        _destroying = true;
    }
}
}
