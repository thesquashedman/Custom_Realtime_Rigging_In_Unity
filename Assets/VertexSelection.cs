using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexSelection : MonoBehaviour
{
    public int vertexId;
    public SceneNodeControl sceneNodeControl;

    public int GetBoneId()
    {
        return sceneNodeControl.GetSelectedSceneNode().boneNumber;
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneNodeControl = GameObject.FindObjectOfType<SceneNodeControl>().GetComponent<SceneNodeControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
