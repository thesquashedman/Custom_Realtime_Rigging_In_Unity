using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XFormHelperForCenter : MonoBehaviour
{
    // Start is called before the first frame update
    public XformControl x;
    public Transform center;
    void Start()
    {
        x.SetSelectedObject(center);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
