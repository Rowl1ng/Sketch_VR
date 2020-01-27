using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

struct ObjMaterial
{
    public string name;
    public string textureName;
}

public class ObjExporterScript
{
    private static int StartIndex = 0;

    public static void Start()
    {
        StartIndex = 0;
    }
    public static void End()
    {
        StartIndex = 0;
    }


    public static string MeshToString(MeshFilter mf, Transform t)
    {
        Vector3 s = t.localScale;
        Vector3 p = t.localPosition;
        Quaternion r = t.localRotation;


        int numVertices = 0;
        Mesh m = mf.sharedMesh;
        if (!m)
        {
            return "####Error####";
        }
        Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

        StringBuilder sb = new StringBuilder();

        foreach (Vector3 vv in m.vertices)
        {
            Vector3 v = t.TransformPoint(vv);
            numVertices++;
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, -v.z));
        }
        sb.Append("\n");
        foreach (Vector3 nn in m.normals)
        {
            Vector3 v = r * nn;
            sb.Append(string.Format("vn {0} {1} {2}\n", -v.x, -v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }
        for (int material = 0; material < m.subMeshCount; material++)
        {
            sb.Append("\n");
            sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            sb.Append("usemap ").Append(mats[material].name).Append("\n");

            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                                        triangles[i] + 1 + StartIndex, triangles[i + 1] + 1 + StartIndex, triangles[i + 2] + 1 + StartIndex));
            }
        }

        StartIndex += numVertices;
        return sb.ToString();
    }
}

public class ObjExporter : MonoBehaviour
{
    public MeshLineRenderer currLine;

    private static int vertexOffset = 0;
    private static int normalOffset = 0;
    private static int uvOffset = 0;
    public static void DoExportFromGame(GameObject[] saves)
    {
        string meshName = saves[0].tag;
        string fileName = meshName + ".obj";
        bool makeSubmeshes = false;

        ObjExporterScript.Start();
        StringBuilder meshString = new StringBuilder();

        meshString.Append("#" + meshName + ".obj"
                          + "\n#" + System.DateTime.Now.ToLongDateString()
                          + "\n#" + System.DateTime.Now.ToLongTimeString()
                          + "\n#-------"
                          + "\n\n");

        Transform t = saves[0].transform;
        Vector3 originalPosition = t.position;
        t.position = Vector3.zero;
        if (!makeSubmeshes)
        {
            meshString.Append("g ").Append(t.name).Append("\n");
        }
        meshString.Append(ProcessTransform(t, makeSubmeshes));
        WriteToFile(meshString.ToString(), fileName);

        t.position = originalPosition;
        ObjExporterScript.End();
        Debug.Log("Exported Mesh: " + fileName);
    }

    static string ProcessTransform(Transform t, bool makeSubmeshes)
    {
        StringBuilder meshString = new StringBuilder();

        meshString.Append("#" + t.name
                          + "\n#-------"
                          + "\n");

        if (makeSubmeshes)
        {
            meshString.Append("g ").Append(t.name).Append("\n");
        }

        MeshFilter mf = t.GetComponent<MeshFilter>();
        if (mf != null)
        {
            meshString.Append(ObjExporterScript.MeshToString(mf, t));
        }

        for (int i = 0; i < t.childCount; i++)
        {
            meshString.Append(ProcessTransform(t.GetChild(i), makeSubmeshes));
        }

        return meshString.ToString();
    }

    static void WriteToFile(string s, string filename)
    {
        Debug.LogError("Save Scene:"+ filename);
        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write(s);
        }
    }

    public static void DoExportsFromGame(GameObject[] saves)
    {
        int exportedObjects = 0;
        ArrayList mfList = new ArrayList();

        for (int i = 0; i < saves.Length; i++)
        {
            Component[] meshfilter = saves[i].transform.GetComponentsInChildren(typeof(MeshFilter));

            for (int m = 0; m < meshfilter.Length; m++)
            {
                exportedObjects++;
                mfList.Add(meshfilter[m]);
            }
        }
        if (exportedObjects > 0)
        {
            MeshFilter[] mf = new MeshFilter[mfList.Count];

            for (int i = 0; i < mfList.Count; i++)
            {
                mf[i] = (MeshFilter)mfList[i];
            }
            string filename = saves[0].tag;
            string targetFolder = "";

            //string filename = EditorSceneManager.GetActiveScene().name + "_" + exportedObjects;

            //int stripIndex = filename.LastIndexOf(Path.PathSeparator);

            //if (stripIndex >= 0)
            //    filename = filename.Substring(stripIndex + 1).Trim();

            MeshesToFile(mf, targetFolder, filename);


            Debug.LogError("Objects exported: "+"Exported " + exportedObjects + " objects to " + filename);
        }
        else
            Debug.LogError("Objects not exported: "+"Make sure at least some of your selected objects have mesh filters!");

    }

    private static string MeshToString(MeshFilter mf, Dictionary<string, ObjMaterial> materialList)
    {
        Mesh m = mf.sharedMesh;
        
        Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

        StringBuilder sb = new StringBuilder();

        sb.Append("g ").Append(mf.name).Append("\n");
        GameObject space = GameObject.Find("space");

        foreach (Vector3 lv in m.vertices)
        {
            Vector3 wv = mf.transform.TransformPoint(lv);
            Vector3 rv = space.transform.InverseTransformPoint(wv);
            //This is sort of ugly - inverting x-component since we're in
            //a different coordinate system than "everyone" is "used to".
            //sb.Append(string.Format("v {0} {1} {2}\n", -wv.x, wv.y, wv.z));
            sb.Append(string.Format("v {0} {1} {2}\n", rv.x, rv.y, rv.z));
        }
        sb.Append("\n");

        foreach (Vector3 lv in m.normals)
        {
            Vector3 wv = mf.transform.TransformDirection(lv);
            Vector3 rv = space.transform.InverseTransformDirection(wv);

            sb.Append(string.Format("vn {0} {1} {2}\n", -rv.x, rv.y, rv.z));
            //sb.Append(string.Format("vn {0} {1} {2}\n", -wv.x, wv.y, wv.z));
        }
        sb.Append("\n");

        foreach (Vector3 v in m.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }



        for (int material = 0; material < m.subMeshCount; material++)
        {
            string mat_name;
            if (mats[material] == null)
                mat_name = "Default-Material";
            else
                mat_name = mats[material].name;

            sb.Append("\n");
            sb.Append("usemtl ").Append(mat_name).Append("\n");
            sb.Append("usemap ").Append(mat_name).Append("\n");

            //See if this material is already in the materiallist.
            try
            {
                ObjMaterial objMaterial = new ObjMaterial();

                objMaterial.name = mat_name;

                /*
                if (mats[material].mainTexture)
                    //objMaterial.textureName = AssetDatabase.GetAssetPath(mats[material].mainTexture);
                {
                    objMaterial.textureName = "";
                    //#if UNITY_EDITOR
                    objMaterial.textureName = AssetDatabase.GetAssetPath(mats[material].mainTexture);
                    Debug.LogError("objMaterial.textureName : " + AssetDatabase.GetAssetPath(mats[material].mainTexture));
                    //#endif
                }
                else
                {
                    Debug.LogError("mats[material].mainTexture = null ");
                    objMaterial.textureName = null;

                }
                */
                objMaterial.textureName = null;

                materialList.Add(objMaterial.name, objMaterial);
            }
            catch (ArgumentException)
            {
                //Already in the dictionary
            }


            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                //Because we inverted the x-component, we also needed to alter the triangle winding.
                sb.Append(string.Format("f {1}/{1}/{1} {0}/{0}/{0} {2}/{2}/{2}\n",
                    triangles[i] + 1 + vertexOffset, triangles[i + 1] + 1 + normalOffset, triangles[i + 2] + 1 + uvOffset));
            }
        

        }


        vertexOffset += m.vertices.Length;
        normalOffset += m.normals.Length;
        uvOffset += m.uv.Length;

        return sb.ToString();
    }

    private static void Clear()
    {
        vertexOffset = 0;
        normalOffset = 0;
        uvOffset = 0;
    }
    private static Dictionary<string, ObjMaterial> PrepareFileWrite()
    {
        Clear();

        return new Dictionary<string, ObjMaterial>();
    }
    private static void MaterialsToFile(Dictionary<string, ObjMaterial> materialList, string folder, string filename)
    {
        using (StreamWriter sw = new StreamWriter(folder + Path.DirectorySeparatorChar + filename + ".mtl"))
        {
            foreach (KeyValuePair<string, ObjMaterial> kvp in materialList)
            {
                sw.Write("\n");
                sw.Write("newmtl {0}\n", kvp.Key);
                sw.Write("Ka  0.6 0.6 0.6\n");
                sw.Write("Kd  0.6 0.6 0.6\n");
                sw.Write("Ks  0.9 0.9 0.9\n");
                sw.Write("d  1.0\n");
                sw.Write("Ns  0.0\n");
                sw.Write("illum 2\n");

                if (kvp.Value.textureName != null)
                {
                    string destinationFile = kvp.Value.textureName;


                    int stripIndex = destinationFile.LastIndexOf(Path.PathSeparator);

                    if (stripIndex >= 0)
                        destinationFile = destinationFile.Substring(stripIndex + 1).Trim();


                    string relativeFile = destinationFile;

                    destinationFile = folder + Path.DirectorySeparatorChar + destinationFile;

                    Debug.Log("Copying texture from " + kvp.Value.textureName + " to " + destinationFile);

                    try
                    {
                        //Copy the source file
                        File.Copy(kvp.Value.textureName, destinationFile);
                    }
                    catch
                    {

                    }


                    sw.Write("map_Kd {0}", relativeFile);
                }

                sw.Write("\n\n\n");
            }
        }
    }

    private static void MeshToFile(MeshFilter mf, string folder, string filename)
    {
        Dictionary<string, ObjMaterial> materialList = PrepareFileWrite();

        using (StreamWriter sw = new StreamWriter(folder + Path.DirectorySeparatorChar + filename + ".obj"))
        {
            sw.Write("mtllib ./" + filename + ".mtl\n");
            sw.Write(MeshToString(mf, materialList));
        }

        MaterialsToFile(materialList, folder, filename);
    }
    private static void MeshesToFile(MeshFilter[] mf, string folder, string filename)
    {
        Dictionary<string, ObjMaterial> materialList = PrepareFileWrite();

        using (StreamWriter sw = new StreamWriter(folder + Path.DirectorySeparatorChar + filename + ".obj"))
        {
            sw.Write("mtllib ./" + filename + ".mtl\n");

            for (int i = 0; i < mf.Length; i++)
            {
                sw.Write(MeshToString(mf[i], materialList));
            }
        }

        MaterialsToFile(materialList, folder, filename);
    }

    public void DoExportsPointsFromGame(GameObject[] sketchs, GameObject[] reference, string folder, string filename)
    {
        int exportedObjects = 0;
        ArrayList PointsList = new ArrayList();
        GameObject space = GameObject.Find("space");
        for (int i = 0; i < sketchs.Length; i++)
        {
            LineRenderer lr = sketchs[i].GetComponent<LineRenderer>();
            Vector3[] Pts = new Vector3[lr.positionCount];
            lr.GetPositions(Pts);
            for (int j = 0; j < Pts.Length; j++)
            {
                Vector3 wv = sketchs[i].transform.TransformPoint(Pts[j]);
                Vector3 rv = space.transform.InverseTransformPoint(wv);
                PointsList.Add(rv);
            }
            exportedObjects++;
        }

        
        Vector3 point = new Vector3();
        using (StreamWriter sw = new StreamWriter(folder + Path.DirectorySeparatorChar + filename + "_sketch.off"))
        {
            sw.Write("OFF\n");
            sw.Write(PointsList.Count+" 0 0\n");
            for (int i = 0; i < PointsList.Count; i++)
            {
                point = (Vector3)PointsList[i];
                sw.WriteLine(point[0] + " " + point[1] + " " + point[2]);
            }
        }

        Debug.Log("Objects exported: " + "Exported " + exportedObjects + " objects to " + filename + " with " + PointsList.Count + "points.");


        

        if (reference[0].GetComponentInChildren<Renderer>().enabled)
        {
            // Add mesh of lines to meshfilter_obj

            List<MeshFilter> filter_list = new List<MeshFilter>();

            for (int i = 0; i < sketchs.Length; i++)
            {
                GameObject go = new GameObject();
                go.transform.SetParent(space.transform);
                go.tag = "Mesh_Line";
                Mesh ml = go.AddComponent<MeshFilter>().mesh;
                go.AddComponent<MeshRenderer>();
                currLine = go.AddComponent<MeshLineRenderer>();
                currLine.lmat = GameObject.Find("DyLine").GetComponent<LineRenderer>().material;
                currLine.ml = ml;
                currLine.setWidth(0.01f);

                LineRenderer lr = sketchs[i].GetComponent<LineRenderer>();
                Vector3[] points = new Vector3[lr.positionCount];
                lr.GetPositions(points);
                for (int j = 0; j < points.Length; j++)
                {
                    Vector3 wv = sketchs[i].transform.TransformPoint(points[j]);
                    currLine.AddPoint(wv);
                }
                filter_list.Add(go.GetComponent<MeshFilter>());
            }

            MeshFilter[] meshfilter_obj = reference[0].transform.GetComponentsInChildren<MeshFilter>();
            //MeshFilter[] meshfilter_obj = space.transform.GetComponentsInChildren<MeshFilter>();
            filter_list.AddRange(meshfilter_obj);
            //MeshesToFile(meshfilter_obj, folder, filename + "_ref");
            MeshesToFile(filter_list.ToArray(), folder, filename + "_ref");
            Debug.Log("Save Reference into OBJ: " + reference[0].name);

            //Delete Meshes of Lines
            GameObject[] delete = GameObject.FindGameObjectsWithTag("Mesh_Line");
            int deleteCount = delete.Length;//.Length();
            for (int i = deleteCount - 1; i >= 0; i--)
                Destroy(delete[i]);

        }

    }

    //save stroke info
    public static void DoExportsMetaInfo(List<List<List<float>>> all_timestamps, string folder, string filename)
    {
        int point_num = 0;
        using (StreamWriter sw = new StreamWriter(folder + Path.DirectorySeparatorChar + filename + "_timestamp.txt"))
        {
            for (int i = 0; i < all_timestamps.Count; i++)
            {
                List<List<float>> timestamps = all_timestamps[i];
                point_num += timestamps.Count;
                for (int j = 0; j < timestamps.Count; j++)
                {
                    List<float> point = timestamps[j];

                    sw.WriteLine(point[0] + " " + point[1] + " " + point[2] + " " + point[3]);
                }
            }

        }
        Debug.Log("Timestamps exported to " + filename + "_timestamp.txt" + " with " + all_timestamps.Count + " strokes and " + point_num + " points.");

    }
}