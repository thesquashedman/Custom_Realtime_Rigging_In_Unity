using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHeadsetTrack : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform headBone;
    public Transform rootBone;

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
            headBone.localRotation = this.transform.localRotation;
            //headBone.localRotation = Quaternion.Euler(Vector3.Scale(this.transform.localEulerAngles, new Vector3(1, -1, -1)));
        }
    }
    IEnumerator IsReady()
    {
        yield return new WaitForSeconds(1.0f);
        ready = true;
    }
}
