using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TheWorld : MonoBehaviour  {

    public SceneNode TheRoot;

    private void Start()
    {
        
    }

    private void Update()
    {
        Matrix4x4 i = Matrix4x4.identity;
        Matrix4x4 i2 = Matrix4x4.identity;
        TheRoot.CompositeXform(ref i, ref i2, 0);
    }
}
