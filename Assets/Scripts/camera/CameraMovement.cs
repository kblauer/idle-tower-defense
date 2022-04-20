using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private float zoomStep = 1f,
        minCamSize = 3, 
        maxCamSize = 12;

    private Vector3 dragOrigin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandlePan();
        HandleZoom();
    }

    private void HandlePan() 
    {
        // save position of mouse when drag starts
        if (Input.GetMouseButtonDown(1)) {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            // calc the difference bettwen drag origin and new mouse position this frame
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            // add the difference to the current camera position
            cam.transform.position += difference;
        }
    }

    private void HandleZoom()
    {
        // if mousewheel up, zoom in
        if (Input.mouseScrollDelta.y > 0) {
            ZoomCamera(true);
        }
        // if mousewheel up, zoom in
        if (Input.mouseScrollDelta.y < 0) {
            ZoomCamera(false);
        }
    }
    
    private void ZoomCamera(bool zoomIn)
    {
        float newSize = cam.orthographicSize;
        if (zoomIn) {
            newSize -= zoomStep;
        } else {
            newSize += zoomStep;
        }

        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
    }
}
