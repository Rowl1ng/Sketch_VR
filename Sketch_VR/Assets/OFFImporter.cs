using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Dummiesman;


public class OFFImporter
{
    public Material matVertex;
    private string filename;

    private GameObject pointCloud;

    public float scale = 1;
    public bool invertYZ = false;

    public int numPoints;
    public int numPointGroups;
    private int limitPoints = 65000;

    private Vector3[] points;
    private Color[] colors;

    private Vector3 minValue;
    private FileInfo _objInfo;

    private Dictionary<string, List<int>> _materialIndices = new Dictionary<string, List<int>>();



    public GameObject Load(string dPath)
    {
        _objInfo = new FileInfo(dPath);
        //StreamReader sr = new StreamReader(Application.dataPath + dPath);
        StreamReader sr = new StreamReader(dPath);
        sr.ReadLine(); // OFF
        string[] buffer = sr.ReadLine().Split(); // nPoints, nFaces
        numPoints = int.Parse(buffer[0]);
        Debug.LogError(numPoints);
        points = new Vector3[numPoints];
        colors = new Color[numPoints];
        minValue = new Vector3();

        for (int i = 0; i < numPoints; i++)
        {
            buffer = sr.ReadLine().Split();

            if (!invertYZ)
                points[i] = new Vector3(float.Parse(buffer[0]) * scale, float.Parse(buffer[1]) * scale, float.Parse(buffer[2]) * scale);
            else
                points[i] = new Vector3(float.Parse(buffer[0]) * scale, float.Parse(buffer[2]) * scale, float.Parse(buffer[1]) * scale);

            if (buffer.Length >= 5)
                colors[i] = new Color(int.Parse(buffer[3]) / 255.0f, int.Parse(buffer[4]) / 255.0f, int.Parse(buffer[5]) / 255.0f);
            else
                colors[i] = Color.cyan;

        }

        numPointGroups = Mathf.CeilToInt(numPoints * 1.0f / limitPoints * 1.0f);


        GameObject off = new GameObject(_objInfo != null ? Path.GetFileNameWithoutExtension(_objInfo.Name) : "WavefrontObject");
        var mf = off.AddComponent<MeshFilter>();
        var mr = off.AddComponent<MeshRenderer>();
        
        mr.material = OBJLoaderHelper.CreateNullMaterial();
        //off.GetComponent<Renderer>().material = matVertex;

        off.GetComponent<MeshFilter>().sharedMesh = CreateMesh(numPoints);
        //off.transform.parent = pointCloud.transform;
        return off;
    }

    Mesh CreateMesh(int nPoints)
    {

        Mesh mesh = new Mesh();

        Vector3[] myPoints = new Vector3[nPoints];
        int[] indecies = new int[nPoints];
        Color[] myColors = new Color[nPoints];

        for (int i = 0; i < nPoints; ++i)
        {
            myPoints[i] = points[i] - minValue;
            indecies[i] = i;
            myColors[i] = colors[i];
        }


        mesh.vertices = myPoints;
        mesh.colors = myColors;
        mesh.SetIndices(indecies, MeshTopology.Points, 0);
        mesh.uv = new Vector2[nPoints];
        mesh.normals = new Vector3[nPoints];


        return mesh;
    }
}
