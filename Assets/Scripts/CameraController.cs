using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{
    // Named this way so you don't mix up positionConstraint and PositionConstraint
    public float transitionSpeed = 10;
    public PositionConstraint moveConstraint;
    public LookAtConstraint watchConstraint;

    // Markers
    private GameObject targetMarker;
    public GameObject originMarker;
    public GameObject topDownVMarker;
    public GameObject topDownHMarker;
    public GameObject sideScrollerMarker;

    void cameraOrigin()
    {
        // Enable Position and LookAt constraints
        moveConstraint.enabled = true;
        watchConstraint.enabled = true;

        targetMarker = originMarker;
        Camera.main.orthographic = false;
    }

    void cameraTopDownV()
    {
        // Disable Position and LookAt constraints
        moveConstraint.enabled = false;
        watchConstraint.enabled = false;

        targetMarker = topDownVMarker;
        Camera.main.orthographic = true;
    }
    void cameraTopDownH()
    {
        // Disable Position and LookAt constraints
        moveConstraint.enabled = false;
        watchConstraint.enabled = false;

        targetMarker = topDownHMarker;
        Camera.main.orthographic = true;
    }
    void cameraSideScroller()
    {
        // Disable Position and LookAt constraints
        moveConstraint.enabled = false;
        watchConstraint.enabled = false;

        targetMarker = sideScrollerMarker;
        Camera.main.orthographic = true;
    }

    private void Start()
    {
        targetMarker = originMarker;
    }
    void Update()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetMarker.transform.position, transitionSpeed * Time.deltaTime);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, targetMarker.transform.rotation, transitionSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.T)) { cameraTopDownV(); } // Top Down Vertical
        if (Input.GetKeyDown(KeyCode.H)) { cameraTopDownH(); } // Top Down Horizontal
        if (Input.GetKeyDown(KeyCode.U)) { cameraOrigin(); } // Default Cam
        if (Input.GetKeyDown(KeyCode.Y)) { cameraSideScroller(); } // Side Scroller
    }
}
