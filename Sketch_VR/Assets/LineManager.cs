using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour {


    public Transform anchor;
    private MeshLineRenderer currLine;
    public Material lmat;
    public float lwidth = 0.01f;

    private bool pressing = false;

    // Update is called once per frame
    void Update () {
        // float RI = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        // float RI = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch);
        bool clearA = OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Touch);
        bool undoX = OVRInput.GetDown(OVRInput.RawButton.X);
        bool saveY = OVRInput.GetDown(OVRInput.RawButton.Y);
        if (pressing == false && clearA == true)
        {
            Debug.Log("clear all");
            GameObject[] delete = GameObject.FindGameObjectsWithTag("Dynamic_Line");
            int deleteCount = delete.Length;//.Length();
            for (int i = deleteCount - 1; i >= 0; i--)
                Destroy(delete[i]);
            
        }

        if (pressing == false && undoX == true)
        {
            
            GameObject[] delete = GameObject.FindGameObjectsWithTag("Dynamic_Line");
            int deleteCount = delete.Length;//.Length();
            if(deleteCount > 0)
                Destroy(delete[deleteCount - 1]);
        }

        if (pressing == false && saveY == true)
        {


            GameObject[] save = GameObject.FindGameObjectsWithTag("Dynamic_Line");
            ObjExporter.DoExportsFromGame(save);

            Debug.LogError("Save "+save.Length+" Line!");
        }

        if (pressing == false && OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch))
        {
            pressing = true;

            GameObject go = new GameObject();
            go.tag = "Dynamic_Line";
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            currLine = go.AddComponent<MeshLineRenderer>();
            currLine.lmat = new Material(lmat);
            currLine.setWidth(lwidth);
            
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch))
        {
            pressing = true;
            currLine.AddPoint(anchor.position);
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch))
        {
            pressing = false;
            currLine = null;
        }
        if (currLine != null)
        {
            currLine.lmat.color = ColorManager.Instance.GetCurrentColor();
        }
    }

}
