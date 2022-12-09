using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHeadsetTrack : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform headBone;
    public Transform rootBone;

    bool ready = false;

    float speed = 20;

    float angleSpeed = 120;

    bool bigHead = false;
    void Start()
    {
        StartCoroutine(IsReady());
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if(ready) 
        {
            headBone.localRotation = this.transform.localRotation;
            //headBone.localRotation = Quaternion.Euler(Vector3.Scale(this.transform.localEulerAngles, new Vector3(1, -1, -1)));
            var rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
            var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);
            foreach (var device in rightHandedControllers)
            {
                Vector2 direction;
                if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out direction))
                {
                    rootBone.transform.localPosition +=  rootBone.forward * direction.y * speed * Time.deltaTime + rootBone.right * direction.x * speed * Time.deltaTime;
                }
                bool toggle;
                if(device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out toggle) && toggle)
                {
                    if(!bigHead)
                    {
                        headBone.localScale = new Vector3(3, 3, 3);
                    }
                    else
                    {
                        headBone.localScale = Vector3.one;
                    }
                }
            }
            var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
            desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);
            foreach (var device in leftHandedControllers)
            {
                Vector2 direction;
                if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out direction))
                {
                    rootBone.transform.localRotation *= Quaternion.AngleAxis(direction.x * angleSpeed * Time.deltaTime, Vector3.up);
                }
            }
        }
    }
    IEnumerator IsReady()
    {
        yield return new WaitForSeconds(1.0f);
        ready = true;
    }
}
