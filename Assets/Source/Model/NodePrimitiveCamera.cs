using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePrimitiveCamera : NodePrimitive
{
    // Start is called before the first frame update
    public Vector3 startPosition;
    public Quaternion startRotation;
    public Vector3 startScale;
    void Awake()
    {
        //startPosition = transform.localPosition;
        //startRotation = transform.localRotation;
        //startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void LoadShaderMatrix(ref Matrix4x4 nodeMatrix, int boneNumber)
    {
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(startPosition, startRotation, startScale);
        Matrix4x4 m = nodeMatrix * p * trs * invp;

        Vector3 forward;
        forward.x = m.m02;
        forward.y = m.m12;
        forward.z = m.m22;
        
        Vector3 position;
        position.x = m.m03;
        position.y = m.m13;
        position.z = m.m23;

        position += -forward.normalized * 0.5f;
        transform.forward = -forward;
        
        transform.localPosition = position;
        //Debug.Log(position);
    }
}
