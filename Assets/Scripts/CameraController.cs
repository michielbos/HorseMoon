using System;
using UnityEngine;

namespace HorseMoon {

public class CameraController : MonoBehaviour {
    public Transform target;

    public float borderMargin = 0.3f;

    private new Camera camera;

    private void Start() {
        camera = GetComponent<Camera>();
        camera.transparencySortMode = TransparencySortMode.CustomAxis;
        camera.transparencySortAxis = new Vector3(0, 1, 0);
    }

    private void LateUpdate() {
        FollowTarget();
    }

    private void FollowTarget() {
        if (target == null)
            return;
        float cameraSize = camera.orthographicSize;
        float maxYDist = cameraSize * (1f - borderMargin);
        float maxXDist = maxYDist * camera.aspect;
        Vector2 targetPos = target.position;
        Vector2 cameraPos = camera.transform.position;
        camera.transform.position = new Vector3(
            Mathf.Clamp(cameraPos.x, targetPos.x - maxXDist, targetPos.x + maxXDist),
            Mathf.Clamp(cameraPos.y, targetPos.y - maxYDist, targetPos.y + maxYDist),
            -10
        );
    }
}

}