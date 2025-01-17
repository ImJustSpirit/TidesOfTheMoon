using System;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    // Named this way so you don't mix up positionConstraint and PositionConstraint
    public float transitionSpeed = 10;
    public Transform player;
    public PositionConstraint moveConstraint;
    public LookAtConstraint watchConstraint;
    private float cameraDistance;
    private float targetOrthSize;

    // Markers
    private GameObject targetMarker;
    public GameObject originMarker;
    public GameObject topDownVMarker;
    public GameObject topDownHMarker;
    public GameObject sideScrollerMarker;

    void cameraOrigin()
    {
        targetOrthSize = 3;
        targetMarker = originMarker;
        cameraDistance = Vector3.Distance(cam.transform.position, targetMarker.transform.position);
    }

    void cameraTopDownV()
    {
        // Disable Position and LookAt constraints
        moveConstraint.enabled = false;
        watchConstraint.enabled = false;

        targetOrthSize = 5;
        targetMarker = topDownVMarker;
        cameraDistance = Vector3.Distance(cam.transform.position, targetMarker.transform.position);
    }
    void cameraTopDownH()
    {
        // Disable Position and LookAt constraints
        moveConstraint.enabled = false;
        watchConstraint.enabled = false;

        targetOrthSize = 5;
        targetMarker = topDownHMarker;
        cameraDistance = Vector3.Distance(cam.transform.position, targetMarker.transform.position);
    }
    void cameraSideScroller()
    {
        // Disable Position and LookAt constraints
        moveConstraint.enabled = false;
        watchConstraint.enabled = false;

        targetOrthSize = 5;
        targetMarker = sideScrollerMarker;
        cameraDistance = Vector3.Distance(cam.transform.position, targetMarker.transform.position);
    }

    private void Start()
    {
        targetMarker = originMarker;
        if (player == null) Debug.LogError("Player reference is not set in CameraController.");
        cam = Camera.main;
    }
    
    
     /*Oliver - I disabled this because the ship looks fuzzy the whole time 
     because I think the cam is lerping to the player all the time since the player is always moving? 
     Feel free to fix and switch back to this*/
     
     void Update()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetOrthSize, transitionSpeed * Time.deltaTime);
        if ((-0.2f < targetOrthSize - cam.orthographicSize) && (targetOrthSize - cam.orthographicSize < 0.2f)) { cam.orthographicSize = targetOrthSize; }

        cam.transform.position = Vector3.Lerp(cam.transform.position, targetMarker.transform.position, transitionSpeed * Time.deltaTime);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, targetMarker.transform.rotation, transitionSpeed * Time.deltaTime);

        if (Vector3.Distance(cam.transform.position, targetMarker.transform.position) < 0.05f)
        {
            cam.transform.position = targetMarker.transform.position;
            cam.transform.rotation = targetMarker.transform.rotation;
            if (targetMarker == originMarker) { watchConstraint.enabled = true; moveConstraint.enabled = true; }
        }
        if ((cameraDistance / 2 > Vector3.Distance(cam.transform.position, targetMarker.transform.position)) && (targetMarker == originMarker)) { cam.orthographic = false; }
        if (((cameraDistance / 5)*4 > Vector3.Distance(cam.transform.position, targetMarker.transform.position)) && (targetMarker != originMarker)) { cam.orthographic = true; }

        if (Input.GetKeyDown(KeyCode.T)) { cameraTopDownV(); } // Top Down Vertical
        if (Input.GetKeyDown(KeyCode.H)) { cameraTopDownH(); } // Top Down Horizontal
        if (Input.GetKeyDown(KeyCode.U)) { cameraOrigin(); } // Default Cam
        if (Input.GetKeyDown(KeyCode.Y)) { cameraSideScroller(); } // Side Scroller
    }

    /*private void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(cam.transform.position.x, 0, player.position.z + 15); // Maintain offset
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, transitionSpeed * Time.deltaTime);
        }
    }*/
}
