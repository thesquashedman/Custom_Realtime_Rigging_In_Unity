using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public TextAsset JSON;

    public MeshBoneLoader meshBoneLoader;


    //public int listSize = 0;


    //public int[] arr = new int[10];
    string path = "";
    string persistantPath = "";

    [System.Serializable]
    public class Vertex
    {
        public int vertIndex = 0;
        public int bouneIndex = 0;
        public int waight = 0;

        public int groupedVertexNumber = 0;

        
    }

    Vertex[] vertexList;

    //VertexList vertexList = new VertexList();


    // Start is called before the first frame update
    void Start()
    {
        SetPaths();
        vertexList = new Vertex[FindObjectOfType<MeshBoneLoader>().GetComponent<MeshFilter>().mesh.vertices.Length];
        for(int i = 0; i< vertexList.Length; i++)
        {
            vertexList[i] = new Vertex();
        }
        Debug.Log(vertexList.Length);
    }
    void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        persistantPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    // Update is called once per frame
    void Update()
    {
        //vertexList = JsonUtility.FromJson<VertexList>(JSON.text);
    }

    public void WriteTOJSON()
    {
        
        string strOutput = JsonHelper.ToJson(vertexList);
        Debug.Log(strOutput);
        File.WriteAllText(persistantPath, strOutput);
    }

    public void ClearSaves()
    {
        string strOutput = "";

        File.WriteAllText(Application.dataPath + "/JSON.txt", strOutput);

        Array.Clear(vertexList, 0, vertexList.Length);

    }

    public void AddVertexToList(int vertexInd, int boneInd, int weight, int groupedIndex)
    {
        //Debug.Log(vertexList.countList);
        //vertexList.list[0] = new Vertex();
        //Vertex newVert = new Vertex();
        

        vertexList[vertexInd].vertIndex = vertexInd;
        vertexList[vertexInd].bouneIndex = boneInd;
        vertexList[vertexInd].waight = weight;
        vertexList[vertexInd].groupedVertexNumber = groupedIndex;


        //int cunt2 = vertexList.count;
        //vertexList.list[vertexInd] = newVert;
        //vertexList.countList++;
        //Debug.Log(vertexList.count);

        //List<int>[] arr = new List<int>;



    }

    public void LoadFormJSON()
    {
        using StreamReader reader = new StreamReader(persistantPath);
        string json = reader.ReadToEnd();
        vertexList = JsonHelper.FromJson<Vertex>(json);
        Debug.Log(vertexList.Length);
        for(int i = 0; i < vertexList.Length; i++)
        {
            Debug.Log(vertexList[i].vertIndex);
            
        }

        meshBoneLoader.LoadFormJSONFile();
    }

    public Vertex getVertex(int vertInd)
    {
        

        return vertexList[vertInd];
    }
    public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
}
