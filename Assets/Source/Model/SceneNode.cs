using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneNode : MonoBehaviour {

    protected Matrix4x4 mCombinedParentXform;
    protected Matrix4x4 mCombinedParentXformFromOrigin;
    
    public Vector3 NodeOrigin = Vector3.zero;
    public List<NodePrimitive> PrimitiveList;
    public List<NodePrimitiveLine> LinePrimitiveList;

    public List<NodePrimitive> specialPrimitive; //Old code, used for putting the axis frame at the node location

    public Vector3 oPosition;
    public Quaternion oRotation;
    public Vector3 oScale;

    public MeshBoneLoader myMesh;
    public int boneNumber;

    bool firstCall = true;
    Vector3 WorldPositionUponAssignment;

	// Use this for initialization
	protected void Start () {

        InitializeSceneNode();
        // Debug.Log("PrimitiveList:" + PrimitiveList.Count);
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void InitializeSceneNode()
    {
        mCombinedParentXform = Matrix4x4.identity;
    }

    // This must be called _BEFORE_ each draw!! 
    public void CompositeXform(ref Matrix4x4 parentXform, ref Matrix4x4 parentXFormFromOrigin, int previousBone)
    {
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        //Matrix4x4 trsOrigin = Matrix4x4.TRS(transform.localPosition - oPosition, transform.localRotation * Quaternion.Inverse(oRotation), transform.localScale - oScale + Vector3.one);
        

        Matrix4x4 T = Matrix4x4.identity;
        Matrix4x4 R = Matrix4x4.identity;
        Matrix4x4 S = Matrix4x4.identity;
        Matrix4x4 TD = Matrix4x4.identity;

        
        

        mCombinedParentXform = parentXform * orgT * trs;

        if(firstCall)
        {
            WorldPositionUponAssignment = new Vector3(mCombinedParentXform[12], mCombinedParentXform[13], mCombinedParentXform[14]);
            firstCall = false;
        }
        T[12] =  WorldPositionUponAssignment.x;
        T[13] = WorldPositionUponAssignment.y;
        T[14] = WorldPositionUponAssignment.z;
        R = Matrix4x4.Rotate(transform.localRotation * Quaternion.Inverse(oRotation));
        S[0] = (transform.localScale - oScale + Vector3.one).x;
        S[5] = (transform.localScale - oScale + Vector3.one).y;
        S[10] = (transform.localScale - oScale + Vector3.one).z;
        TD[12] =  (transform.localPosition.x - oPosition.x);
        TD[13] = (transform.localPosition.y - oPosition.y);
        TD[14] = (transform.localPosition.z - oPosition.z);

        mCombinedParentXformFromOrigin = parentXFormFromOrigin * TD * T * R * S * T.inverse;
        if(boneNumber == 1)
        {
            //Debug.Log(mCombinedParentXformFromOrigin.ToString());
            Debug.Log(WorldPositionUponAssignment);
            Debug.Log(T);
        }
        

        // propagate to all children
        foreach (Transform child in transform)
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                cn.CompositeXform(ref mCombinedParentXform, ref mCombinedParentXformFromOrigin, boneNumber);
            }
        }
        
        // disenminate to primitives
        foreach (NodePrimitive p in PrimitiveList)
        {
            p.LoadShaderMatrix(ref mCombinedParentXform, boneNumber);
        }
        foreach (NodePrimitiveLine p in LinePrimitiveList)
        {
            p.LoadShaderMatrix(ref parentXform, ref mCombinedParentXform, previousBone);
        }
        foreach (NodePrimitive p in specialPrimitive)
        {
            p.LoadShaderMatrix(ref mCombinedParentXform, boneNumber);
        }
        
        myMesh.LoadBone(boneNumber, mCombinedParentXformFromOrigin);

    }
    
}