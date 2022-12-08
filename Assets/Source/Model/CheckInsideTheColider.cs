using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInsideTheColider : MonoBehaviour
{
    public List<GameObject> ObjectsWithinCollider = new List<GameObject>();
    public int boneNumber = 0;
    void Update()
    {
        //Collider[] hitColidliders = Physics.OverlapSphere(transform.position, 0f);
        //foreach (Collider hitColider in hitColidliders)
        //{
        //    Debug.Log(hitColider.gameObject.name);
        //}
    }

    public bool CheckColIns(Collider c)
    {
        Collider[] hitColidliders = Physics.OverlapSphere(transform.position, 0f);
        for (int i = 0; i < hitColidliders.Length; i++)
        {
            if (hitColidliders[i] == c)
            {
                return true;
            }
        }
        return false;
    }

    public int Check(GameObject collider, ref float distanceOverHeight)
    {
        //Debug.Log(ObjectsWithinCollider.Count);
        distanceOverHeight = (collider.transform.position - this.transform.position).magnitude / 2;
        if(ObjectsWithinCollider.Contains(collider))
        {
            return boneNumber;
        }
        return 0;

        /*
        if ((this.transform.position - point).magnitude < 1)
        {
            return true;
        }
        return false;
        */
    }
    public int CheckNoCollider(Vector3 position, ref float distanceOverHeight)
    {
        
        distanceOverHeight = (position - this.transform.position).magnitude / 2;
        if(GetComponent<Collider>().bounds.Contains(position))
        {
            return boneNumber;
        }
        return 0;
    }

    public bool IsInside(Vector3 point)
    {
        
        Vector3 closest = this.GetComponent<Collider>().ClosestPoint(point);
        // Because closest=point if point is inside - not clear from docs I feel
        return closest == point;
    }
    //Gotten From https://answers.unity.com/questions/1875398/check-for-objects-inside-a-collider.html
    private void OnTriggerEnter(Collider other) {
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.name == "ColiderObject(Clone)")
        {
            //Debug.Log("moo");
            if(!ObjectsWithinCollider.Contains(other.gameObject))
            {
                //Debug.Log("added");
                ObjectsWithinCollider.Add(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other) {

        if(other.gameObject.name == "ColiderObject(Clone)")
        {
            if(ObjectsWithinCollider.Contains(other.gameObject))
            {
                //Debug.Log("removing");
                ObjectsWithinCollider.Remove(other.gameObject);
            }
        }
    }
}
