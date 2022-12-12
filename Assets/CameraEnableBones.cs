using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnableBones : MonoBehaviour
{
    // Start is called before the first frame update
    bool currentlyPressed = false;
    void Start()
    {
        //GetComponent<Camera>().cullingMask = ~(1 << LayerMask.NameToLayer("TransparentFX")) ;
    }

    // Update is called once per frame
    void Update()
    {
        var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);
        foreach (var device in leftHandedControllers)
        {
            bool toggle;
            if(device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out toggle) && toggle)
            {
                if(!currentlyPressed)
                {
                    //Debug.Log(GetComponent<Camera>().cullingMask);
                    GetComponent<Camera>().cullingMask ^= (1 << LayerMask.NameToLayer("TransparentFX")) ;
                    // = 0x;
                    currentlyPressed = true;
                }
                
            }
            else
            {
                if(currentlyPressed)
                {
                    /*
                    GetComponent<Renderer>().material = originalMaterial;
                    mMaterial = originalMaterial;
                    */
                    GetComponent<Camera>().cullingMask ^= (1 << LayerMask.NameToLayer("TransparentFX")) ;
                    currentlyPressed = false;
                    
                }
            }
        }
    }
}
