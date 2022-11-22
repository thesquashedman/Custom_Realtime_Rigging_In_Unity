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
    public void CompositeXform(ref Matrix4x4 parentXform, ref Matrix4x4 parentXFormFromOrigin)
    {
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        Matrix4x4 trsOrigin = Matrix4x4.TRS(transform.localPosition - oPosition, transform.localRotation * Quaternion.Inverse(oRotation), transform.localScale - oScale + Vector3.one);
        
        mCombinedParentXform = parentXform * orgT * trs;

        mCombinedParentXformFromOrigin = parentXFormFromOrigin * trsOrigin;

        // propagate to all children
        foreach (Transform child in transform)
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                cn.CompositeXform(ref mCombinedParentXform, ref mCombinedParentXformFromOrigin);
            }
        }
        
        // disenminate to primitives
        foreach (NodePrimitive p in PrimitiveList)
        {
            p.LoadShaderMatrix(ref mCombinedParentXform);
        }
        foreach (NodePrimitiveLine p in LinePrimitiveList)
        {
            p.LoadShaderMatrix(ref parentXform, ref mCombinedParentXform);
        }
        foreach (NodePrimitive p in specialPrimitive)
        {
            p.LoadShaderMatrix(ref mCombinedParentXform);
        }
        
        myMesh.LoadBone(boneNumber, mCombinedParentXformFromOrigin);

    }
    
}