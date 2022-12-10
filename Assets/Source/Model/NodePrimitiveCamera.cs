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
        //Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        //Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        //Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        Matrix4x4 m = nodeMatrix;

        /*
        Vector3 forward;
        forward.x = m.m02;
        forward.y = m.m12;
        forward.z = m.m22;
        
        Vector3 position;
        position.x = m.m03;
        position.y = m.m13;
        position.z = m.m23;
        */

        //position += -forward.normalized * 0.5f;
        //transform.forward = -forward;
        //Debug.Log(ExtractPosition(m));
        transform.localPosition = ExtractPosition(m);
        transform.localRotation = ExtractRotation(m);
        transform.localScale = ExtractScale(m);
        //Debug.Log(position);
        
    }
    Quaternion ExtractRotation(Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;
 
        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;
 
        return Quaternion.LookRotation(forward, upwards);
    }
 
    Vector3 ExtractPosition(Matrix4x4 matrix)
    {
        Vector3 position;
        position.x = matrix.m03;
        position.y = matrix.m13;
        position.z = matrix.m23;
        return position;
    }
 
    Vector3 ExtractScale(Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
        return scale;
    }
}
