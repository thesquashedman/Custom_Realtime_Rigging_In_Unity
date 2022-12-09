using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBoneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform boneTransform;          //Transorm the vertices should get parented to

    [SerializeField] Material mMaterial;      //Material with the shader

    public Point mP;                //Just contains the original transform of the bone.

    public Transform mParent;       //Parent transform, for testing how this would work with a parent

    [SerializeField] GameObject[] boneSystem;

    [SerializeField] GameObject colliderObject;

    [SerializeField]bool useColliders;

    Mesh mMesh;

    Material originalMaterial;

    [SerializeField]List<int> boneIgnore;


    //Grouped vertices holds all the indexes of all the mesh vertices that are in the same position, holds what vertex group they are a part of and their weight in a Color, and holds a Gameobject with a collider which will be used for accessing
    class GroupedVertices{
        public List<int> mVertexIndexes = new List<int>();
        public Color group_and_weights; //Color.x is the vertex group, Color.y is the weight

        public GameObject collider;
        public float distanceOverHeight = float.MaxValue; //For how far along it is along the bone, used for determining which bone to use when there is bone overlap.
    }

    //A list of all the grouped vertices
    List<GroupedVertices> mGroupedVerticesList = new List<GroupedVertices>();
    
    //Used for the grouping of the vertices, matches vertex Vector3 with which group they should be a part of.
    Dictionary<Vector3, GroupedVertices> mVertexDictioary = new Dictionary<Vector3, GroupedVertices>();

    const int numberOfBones = 22;

    Matrix4x4[] boneMatrixArray = new Matrix4x4[numberOfBones + 1]; //Important note: boneMatrixArray[0] should always be the identity matrix

    void Start()
    {
        mMaterial = new Material(mMaterial);
        GetComponent<Renderer>().material = mMaterial;
        Mesh mMesh = GetComponent<MeshFilter>().mesh;
        
        Color[] mColors = new Color[mMesh.vertices.Length];     //Creates an array for applying the vertex colors 
        
            
        //Groups all the same vertices of the mesh and saves them. Integral for manipulating the mesh
        for(int i = 0; i < mMesh.vertices.Length; i++)
        {
            if(!mVertexDictioary.ContainsKey(mMesh.vertices[i]))
            {
                //Debug.Log(i);
                GroupedVertices temp = new GroupedVertices();
                temp.mVertexIndexes.Add(i);
                temp.group_and_weights = new Color(0, 0, 0 ,0);
                mGroupedVerticesList.Add(temp);
                mVertexDictioary.Add(mMesh.vertices[i], temp);
                //GameObject newCollider = (GameObject)Instantiate(Resources.Load("Collider"));
                
                if(useColliders)
                {
                    GameObject newCollider = Instantiate(colliderObject);

                    newCollider.transform.localPosition = transform.localToWorldMatrix.MultiplyPoint3x4(mMesh.vertices[i]); //Currently only works with translations.
                    temp.collider = newCollider;
                    newCollider.transform.parent = transform;
                }
                
            }
            else{
                mVertexDictioary[mMesh.vertices[i]].mVertexIndexes.Add(i);
            }
            mColors[i] = new Color(0, 0, 0 ,0);
            
        }
        mMesh.colors = mColors; 

        //Here I am setting all the vertices of a vertex group so that a point is assigned to vertex group 1
        //mGroupedVerticesList[5].group_and_weights = new Color(19, 1, 0, 0);
        //Debug.Log(mGroupedVerticesList[0].mVertexIndexes.Count);
        

        

        for (int i = 0; i < boneMatrixArray.Length; i++)
        {
            boneMatrixArray[i] = Matrix4x4.identity; //Set the initial values of all the bones to the identity matrix.
        }
        //AutoAssignBones();
        StartCoroutine(AutoAssignBones());
        
        //Important note: After assigning the vertices to bones, we will want to hide all the cube colliders that mark the points.
        //We also ideally want to assign all the vertices at once, going from a rigging mode where we assign points to the rig to a manipulating mode where we manipulate the rig.
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            originalMaterial = mMaterial;
            GetComponent<Renderer>().material.shader = Shader.Find("Unlit/BoneShaderColored");
            mMaterial = GetComponent<Renderer>().material;
            
        }
        if(Input.GetKeyUp(KeyCode.X))
        {
             GetComponent<Renderer>().material = originalMaterial;
             mMaterial = originalMaterial;
        }
           //Mesh mMesh = GetComponent<MeshFilter>().mesh;
            
            //I beleive I borrowed this from transform loader

            /*
            Matrix4x4 m = Matrix4x4.identity;  // column major, column first ...
            Matrix4x4 T = Matrix4x4.identity;
            Matrix4x4 R = Matrix4x4.identity;
            Matrix4x4 S = Matrix4x4.identity;
            Vector3 p = boneTransform.localPosition - mP.mT.localPosition;
            Vector3 s = boneTransform.localScale - mP.mT.localScale;
            Quaternion q = mP.mT.localRotation * boneTransform.localRotation;

            Matrix4x4[] matrixArray = new Matrix4x4[] {Matrix4x4.identity, boneTransform.localToWorldMatrix}; //The array of matrices to be loaded to the shader. The first matrix is always the identity matrix.

            T[12] = p.x;    // col-3, row-0
            T[13] = p.y;    // col-3, row-1
            T[14] = p.z;    // col-3, row-2

                
            S[0] = s.x + 1; // col-0, row-0
            S[5] = s.y + 1; // col-1, row-1
            S[10] = s.z + 1; // col-2, row-2

                
            R = Matrix4x4.Rotate(q);

            matrixArray[1] = mParent.localToWorldMatrix * T * boneTransform.transform.localToWorldMatrix * R * S * boneTransform.transform.localToWorldMatrix.inverse ; //Creates the matrix for bone 1. 
            
            mMaterial.SetMatrixArray("MyXformMat", matrixArray); //Loads the matrix for bone 1
            */
            mMaterial.SetMatrixArray("MyXformMat", boneMatrixArray);
            
    }
    public void LoadBone(int boneNumber, Matrix4x4 boneMatrix)
    {
        if(boneNumber == 20)
        {
            //Debug.Log(boneMatrix);
        }
        boneMatrixArray[boneNumber] = boneMatrix;
        
    }
    IEnumerator AutoAssignBones()
    {
        yield return new WaitForSeconds(0.1f);
        Mesh mMesh = GetComponent<MeshFilter>().mesh;
        
        Color[] mColors = new Color[mMesh.vertices.Length];
        GameObject[] boneColliders = GameObject.FindGameObjectsWithTag("BoneCollider");
        //Debug.Log(boneColliders.Length);
        for (int v = 0; v < mGroupedVerticesList.Count; v++)
        {
            foreach (GameObject b in boneColliders)
            {


                    //Debug.Log(mGroupedVerticesList[v].mVertexIndexes[i]);

                    //Debug.Log("Got Inside 1");



                    // Vector3 closest = boneSystem[b].GetComponent<Collider>().
                    //    ClosestPoint(mGroupedVerticesList[v].collider.transform.position);

                    //Debug.Log(mGroupedVerticesList[v].collider.transform.position);
                    //Debug.Log("CLOSESTPOINT" + closest);
                    //if (boneSystem[b].GetComponent<CheckInsideTheColider>().
                    //    CheckColIns(mGroupedVerticesList[v].collider.GetComponent<Collider>()))
                    //{
                    //Debug.Log("Got Inside 2");
                float distanceOverHeight = 0;
                int boneNumber;
                if(useColliders)
                {
                    boneNumber = b.GetComponent<CheckInsideTheColider>().Check(mGroupedVerticesList[v].collider, ref distanceOverHeight);
                    
                }
                else
                {
                    boneNumber = b.GetComponent<CheckInsideTheColider>().CheckNoCollider(transform.localToWorldMatrix.MultiplyPoint3x4(mMesh.vertices[mGroupedVerticesList[v].mVertexIndexes[0]]), ref distanceOverHeight);
                }
                if (boneNumber != 0)
                    {
                        if(!boneIgnore.Contains(boneNumber))
                        {

                        
                            //Debug.Log("INSIDE");
                            if(mGroupedVerticesList[v].group_and_weights == new Color(0,0,0,0) || mGroupedVerticesList[v].distanceOverHeight > distanceOverHeight)
                            {
                                mGroupedVerticesList[v].group_and_weights = new Color(boneNumber, 1.0f, 0, 0);
                                mGroupedVerticesList[v].distanceOverHeight = distanceOverHeight;
                                for (int i = 0; i < mGroupedVerticesList[v].mVertexIndexes.Count; i++) //mGroupedVerticesList[0].mVertexIndexes.Count
                                {
                                    mColors[mGroupedVerticesList[v].mVertexIndexes[i]] = new Color(boneNumber, 1.0f, 0, 0);
                                }
                            }
                        }
                        
                        
                    }
                    
                

                
            }
                
        }
         mMesh.colors = mColors;

    }
}
    
