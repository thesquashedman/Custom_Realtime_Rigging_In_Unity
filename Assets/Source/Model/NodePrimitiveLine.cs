using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePrimitiveLine : MonoBehaviour
{
    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    [SerializeField] GameObject newCollider;

    //GameObject newCollider;
    
    

	// Use this for initialization
	void Start () {
        //newCollider = Instantiate(colliderObject);
    }

    void Update()
    {
    }
	
  
	public void LoadShaderMatrix(ref Matrix4x4 parentMatrix, ref Matrix4x4 childMatrix, int boneNumber)
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
        
        if(newCollider != null)
        {
            newCollider.transform.position = ExtractPosition(m);
            newCollider.transform.rotation = ExtractRotation(m);
            Vector3 theScale = ExtractScale(m);
            theScale.x = 4;
            theScale.z = 4;
            newCollider.transform.localScale = theScale;
            newCollider.GetComponent<CheckInsideTheColider>().boneNumber = boneNumber;
        }
        else
        {
            Debug.Log("missing collider");
        }
    }
    //Gotten from here https://forum.unity.com/threads/how-to-assign-matrix4x4-to-transform.121966/
    
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
