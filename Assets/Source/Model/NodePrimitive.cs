using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePrimitive: MonoBehaviour {
    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 Pivot;

	// Use this for initialization
	void Start () {
    }

    void Update()
    {
    }
	
  
	public virtual void LoadShaderMatrix(ref Matrix4x4 nodeMatrix, int boneNumber)
    {
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        Matrix4x4 m = nodeMatrix * p * trs * invp;

        float hue = boneNumber * (2 * 3.1415f / 23);
        Vector3 newColor = new Vector3(1, 0 , 0);
        Vector3 k = new Vector3(0.57735f, 0.57735f, 0.57735f);
        float cosAngle = Mathf.Cos(hue);
        newColor = (newColor * cosAngle + Vector3.Cross(k, newColor) * Mathf.Sin(hue) + k * Vector3.Dot(k, newColor) * (1.0f - cosAngle));
        Color theColor = new Color(newColor.x, newColor.y, newColor.z, 1);
                //o.theColor.x = newColor.x * v.mColor.y;
                //o.theColor.y = newColor.y * v.mColor.y;
                //o.theColor.z = newColor.z * v.mColor.y;
        GetComponent<Renderer>().material.SetMatrix("MyXformMat", m);
        GetComponent<Renderer>().material.SetColor("MyColor", theColor);
    }
}