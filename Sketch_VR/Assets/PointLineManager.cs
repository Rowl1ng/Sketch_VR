using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using System.IO;

public class PointLineManager : MonoBehaviour
{

    public GameObject rightController;

    public float lineWidth;

    public float Max_width;
    public float Min_width;

    public bool with_reference;

    private bool pressing;
    private LineRenderer lr;
    private List<Vector3> verts;
    public List<List<float>> timestamps;
    public List<List<List<float>>> all_timestamps;
    private float start_time;

    // Use this for initialization
    private GameObject loadedObject;
    private GameObject space;

    private Material material;
    private GameObject laser;
    private GameObject ColorManager;
    private GameObject Grab1;
    private GameObject Grab2;

    void Start()
    {
        init();
    }

    private void init()
    {
        ColorManager = GameObject.Find("ColorPicker");
        Grab1 =
        rightController = GameObject.Find("RightHandAnchor");
        lineWidth = 0.03f;
        start_time = 0.0f;
        verts = new List<Vector3>();
        timestamps = new List<List<float>>();
        all_timestamps = new List<List<List<float>>>();
        with_reference = true;
        pressing = false;
        laser = GameObject.Find("LaserPointer");
        
        material = GameObject.Find("DyLine").GetComponent<LineRenderer>().material;
        string targetPath = PlayerManager.model_dir + Path.DirectorySeparatorChar + PlayerManager.model_id + ".obj";
        space = GameObject.Find("space");



        //file path
        if (!File.Exists(targetPath))
        {
            Debug.LogError("File doesn't exist: " + targetPath);
        }
        else
        {
            if (loadedObject != null)
                Destroy(loadedObject);
            loadedObject = new OBJLoader().Load(targetPath);
            loadedObject.tag = "reference";
            GameObject anchor = GameObject.Find("ReferenceAnchor");
            loadedObject.transform.SetParent(space.transform);

            loadedObject.transform.SetPositionAndRotation(anchor.transform.position, anchor.transform.rotation);
            loadedObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        }

    }
    // Update is called once per frame
    void Update()
    {
        float RI = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        bool undoX = OVRInput.GetDown(OVRInput.RawButton.X);
        bool undoY = OVRInput.GetDown(OVRInput.RawButton.Y);


        //Undo last stroke
        if (pressing == false && undoX == true)
        {
            GameObject[] delete = GameObject.FindGameObjectsWithTag("Dynamic_Line");
            int deleteCount = delete.Length;
            if (deleteCount > 0)
            {
                Destroy(delete[deleteCount - 1]);
                if (all_timestamps.Count > 0)
                    all_timestamps.RemoveAt(all_timestamps.Count - 1);
            }

        }
        if (pressing == false && undoY == true)
        {
            GameObject countdown = GameObject.Find("CountDown");
            
            if (!countdown.GetComponent<CountDownScript>().doOnce)
            {
                GameObject[] reference = GameObject.FindGameObjectsWithTag("reference");

                for (int i = 0; i < reference.Length; i++)
                {
                    MeshRenderer[] meshrenderer_obj = reference[i].transform.GetComponentsInChildren<MeshRenderer>();
                    for (int j = 0; j < meshrenderer_obj.Length; j++)
                        meshrenderer_obj[j].enabled = !meshrenderer_obj[j].enabled;
                }
            }

        }

        
        //press-start a new stroke
        if (pressing == false && RI > 0)
        {
            pressing = true;
            GameObject go = new GameObject();
            go.transform.SetParent(space.transform);
            go.tag = "Dynamic_Line";
            lr = go.AddComponent<LineRenderer>();
            lr.useWorldSpace = false;
            lr.material = material;
            lr.material.color = ColorManager.GetComponent<ColorManager>().GetCurrentColor();
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.positionCount = 0;
            lr.sortingLayerName = "ForeGround";
            lr.sortingOrder = 2000;
            start_time = Time.time;
            /* linePrefab.c1 = c1;
             linePrefab.c2 = c2;
             linePrefab.lineWidth = lineWidth;
             */
        }

        // release
        if (pressing == true && RI == 0)
        {
            pressing = false;

            //add stroke info to list of strokes
            if (timestamps.Count > 0)
            {
                all_timestamps.Add(new List<List<float>>(timestamps));
                timestamps.Clear();
            }

            verts.Clear();
            start_time = 0.0f;
        }

        //press-keep drawing on this stroke
        if (pressing == true && RI > 0)
        {
            Vector3 pos = rightController.transform.position;
            if (verts.Count == 0 || verts[verts.Count - 1] != pos)
            {
                verts.Add(pos);
                Vector3 relaPt = space.transform.InverseTransformPoint(pos);

                timestamps.Add(new List<float>{ relaPt[0], relaPt[1], relaPt[2], Time.time - start_time });
            }

            lr.positionCount = verts.Count;
            lr.SetPositions(verts.ToArray());
        }

        if (pressing == false)
        {
            laser.GetComponent<LineRenderer>().enabled = true;

            float moveHOrizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            if (moveVertical > 0 && lineWidth < Max_width)
            {
                lineWidth += 0.001f;
                rightController.gameObject.transform.localScale = new Vector3(lineWidth, lineWidth, lineWidth);
            }

            if (moveVertical < 0 && lineWidth > Min_width)
            {
                lineWidth -= 0.001f;
                rightController.gameObject.transform.localScale = new Vector3(lineWidth, lineWidth, lineWidth);
            }
        }
        else
            laser.GetComponent<LineRenderer>().enabled = false;

    }
}