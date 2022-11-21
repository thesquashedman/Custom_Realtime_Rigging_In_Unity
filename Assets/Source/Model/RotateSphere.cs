using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSphere : MonoBehaviour
{
    // Start is called before the first frame update
    float direction = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation *= Quaternion.AngleAxis(1.6f * direction, Vector3.forward);
        if(transform.localRotation.eulerAngles.z > 180 || transform.localRotation.eulerAngles.z< 0)
        {
            direction *= -1;
        }
    }
}
