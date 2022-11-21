using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePrimitiveLine : MonoBehaviour
{
    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);

    
    

	// Use this for initialization
	void Start () {
    }

    void Update()
    {
    }
	
  
	public void LoadShaderMatrix(ref Matrix4x4 parentMatrix, ref Matrix4x4 childMatrix)
    {
        Vector3 startPoint = new Vector3(parentMatrix[0, 3], parentMatrix[1, 3], parentMatrix[2, 3]);
        Vector3 endPoint = new Vector3(childMatrix[0, 3], childMatrix[1, 3], childMatrix[2, 3]);

        Vector3 distance = endPoint - startPoint;
        Vector3 direction = distance.normalized;
        float scaleY = distance.magnitude;
        Vector3 pos = startPoint + (scaleY/2 * direction);

        // Viewing vector is from transform.localPosition to the lookat position
        Vector3 V = (endPoint - startPoint).normalized;
        //Vector3 W = Vector3.Cross(V, Vector3.up);
        //Vector3 U = Vector3.Cross(W, V);
        
        Quaternion q2 = Quaternion.AngleAxis(-Vector3.Angle(V, Vector3.up), Vector3.Cross(V, Vector3.up).normalized);
        
        
        //Calculate position for cylinder
        //Calculate scale for cylinder.
        //Calculate rotation for cylinder
        Matrix4x4 trs = Matrix4x4.TRS(pos, q2, new Vector3(transform.localScale.x, scaleY/2, transform.localScale.z));
        Matrix4x4 m = trs;
        GetComponent<Renderer>().material.SetMatrix("MyXformMat", m);
        GetComponent<Renderer>().material.SetColor("MyColor", MyColor);
    }
}
