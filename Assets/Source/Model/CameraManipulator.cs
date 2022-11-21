using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManipulator : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform center;
    public float rotSpeed = 0.1f;
    public float speed = 0.02f;
    public float zoomSpeed = 0.1f;
    
    Vector3 previousMousePosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            if(Input.GetMouseButtonDown(1))
            {
                previousMousePosition = Input.mousePosition;
            }
            if(Input.GetMouseButton(1))
            {
                Vector3 newMousePosition = Input.mousePosition;
                Vector3 pan = new Vector3();
                pan = ((newMousePosition.x - previousMousePosition.x) * transform.right + (newMousePosition.y - previousMousePosition.y) * transform.up) * speed;
                center.localPosition += pan;
                this.transform.localPosition += pan;
                previousMousePosition = newMousePosition;
            }
            if(Input.GetMouseButtonDown(0))
            {
                previousMousePosition = Input.mousePosition;
            }
            if(Input.GetMouseButton(0))
            {
                Vector3 newMousePosition = Input.mousePosition;
                //Borrowed from the camera tumble example
                // 1. Rotation of the viewing direction by right axis and up axis
                Quaternion q = Quaternion.AngleAxis(rotSpeed * (newMousePosition.x - previousMousePosition.x), transform.up) * Quaternion.AngleAxis(rotSpeed * -(newMousePosition.y - previousMousePosition.y), transform.right);

                // 2. we need to rotate the camera position
                Matrix4x4 r = Matrix4x4.Rotate(q);
                Matrix4x4 invP = Matrix4x4.TRS(-center.localPosition, Quaternion.identity, Vector3.one);
                r = invP.inverse * r * invP;
                Vector3 newCameraPos = r.MultiplyPoint(transform.localPosition);

                if (Mathf.Abs(Vector3.Dot((center.localPosition - newCameraPos).normalized, Vector3.up)) < 0.9)
                {
                    transform.localPosition = newCameraPos;
                }
                
                
                // transform.LookAt(LookAtPosition);
                //transform.localRotation = q * transform.localRotation;
                
                

                previousMousePosition = newMousePosition;
            }

            Vector3 posToMov = (center.localPosition - transform.localPosition).normalized * Input.mouseScrollDelta.y * zoomSpeed;
            if((this.transform.localPosition + posToMov - center.localPosition).magnitude > 1)
            {
                this.transform.localPosition += posToMov;
            }
            
        }
        this.transform.forward = (center.localPosition - transform.localPosition).normalized;
        
    }
}
