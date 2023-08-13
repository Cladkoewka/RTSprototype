using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera RaycastCamera;

    private Vector3 _startPoint;
    private Vector3 _startCameraPosition;
    private Plane _plane;

    private void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);

    }

    private void Update()
    {
        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance); 
        
        if (Input.GetMouseButtonDown(2))
        {
            _startPoint = point;
            _startCameraPosition = transform.position;
        }

        if (Input.GetMouseButton(2)) 
        {
            Vector3 offset = point - _startPoint;
            transform.position = _startCameraPosition - offset;
        }

        transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
        RaycastCamera.transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
        
    }
}
