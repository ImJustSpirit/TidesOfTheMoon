using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{
    // Named this way so you don't mix up positionConstraint and PositionConstraint
    public PositionConstraint moveConstraint;
    public LookAtConstraint watchConstraint;

    void cameraTopDownV()
    {
        // Disable Position and LookAt constraints
        moveConstraint.enabled = false;
        watchConstraint.enabled = false;

        Camera.main.transform.position = new Vector3(0, 5, 4);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        Camera.main.orthographic = true;
    }
    void cameraTopDownH()
    {
        // Disable Position and LookAt constraints
        moveConstraint.enabled = false;
        watchConstraint.enabled = false;

        Camera.main.transform.position = new Vector3(0, 5, 8);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 90));
        Camera.main.orthographic = true;
    }

    void cameraDefault()
    {
        // Enable Position and LookAt constraints
        moveConstraint.enabled = true;
        watchConstraint.enabled = true;

        Camera.main.transform.position = new Vector3(0, 0, -5);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Camera.main.orthographic = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) { cameraTopDownV(); } // Top Down Vertical
        if (Input.GetKeyDown(KeyCode.H)) { cameraTopDownH(); } // Top Down Horizontal
        if (Input.GetKeyDown(KeyCode.U)) { cameraDefault(); } // Default Cam
    }
}
