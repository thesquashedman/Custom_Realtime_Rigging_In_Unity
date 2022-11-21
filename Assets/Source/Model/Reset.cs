using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    // Start is called before the first frame update
    List<SceneNode> sceneNodes;
    public Transform Camera;
    public Transform Center;
    void Start()
    {
        sceneNodes = new List<SceneNode>(Object.FindObjectsOfType<SceneNode>());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void resetSceneNode()
    {
        foreach (SceneNode s in sceneNodes)
        {
            s.transform.localPosition = s.oPosition;
            s.transform.localRotation = s.oRotation;
            s.transform.localScale = s.oScale;
        }
        
        Center.localPosition = Vector3.zero;
        Center.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Center.localRotation = Quaternion.identity;
        Camera.localPosition = new Vector3(0, 0, -16);
    }
}
