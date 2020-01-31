using System;
using UnityEngine;

namespace HorseMoon {

public class CameraController : MonoBehaviour {
    private new Camera camera;
    private void Start() {
        camera = GetComponent<Camera>();
        camera.transparencySortMode = TransparencySortMode.CustomAxis;
        camera.transparencySortAxis = new Vector3(0, 1, 0);
    }
}

}