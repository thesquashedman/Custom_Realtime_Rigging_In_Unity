using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerTrack : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform handBone;
    public Transform armBone;

    public Transform elbowBone;
    public Transform headset;


    public int direction;

    float maxReach = 6;

    bool bigArm= false;

    bool currentlyPressed = false;
    bool ready = false;
    void Start()
    {
        StartCoroutine(IsReady());
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if(ready) 
        {
            
            Quaternion x = Quaternion.FromToRotation(-Vector3.right * direction, (headset.localPosition - this.transform.localPosition).normalized) * Quaternion.AngleAxis(direction * 80, Vector3.forward);

            armBone.localRotation = x;
            float distance = Mathf.Min(maxReach, (headset.localPosition - this.transform.localPosition).magnitude);
            float degrees = (maxReach - distance) * 110/maxReach;
            x *= Quaternion.AngleAxis(degrees, x * -Vector3.up * direction);
            elbowBone.localRotation = Quaternion.AngleAxis(degrees, -Vector3.right) ;
            armBone.localRotation = x;
            //Debug.Log(Mathf.Acos(Vector3.Dot(headset.right * -direction, (headset.localPosition - this.transform.localPosition).normalized)) * 180 / Mathf.PI);
            //armBone.localRotation = 
            
            handBone.localRotation = Quaternion.Inverse(x) * this.transform.localRotation * Quaternion.AngleAxis(-90, Vector3.right) ;

            var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
            var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);
            foreach (var device in leftHandedControllers)
            {
                bool toggle;
                if(device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out toggle) && toggle)
                {
                    if(!currentlyPressed)
                    {
                        if(!bigArm)
                        {
                            handBone.localPosition = Vector3.down * 5;
                            bigArm = true;
                        }
                        else
                        {
                            handBone.localPosition = Vector3.zero;
                            bigArm = false;
                        }
                    }
                    currentlyPressed = true;
                   
                }
                else
                {
                    currentlyPressed = false;
                }
            }
        }
    }
    IEnumerator IsReady()
    {
        yield return new WaitForSeconds(1.0f);
        ready = true;
        maxReach = (headset.position - this.transform.position).magnitude;
    }
}
