using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;


public class PlayerManager : MonoBehaviour
{
    public static string model_dir = @"C:\Users\ll00931\Documents\Datasets\VR_sketch\chair";
    public static string save_dir = @"C:\Users\ll00931\Documents\Datasets\VR_sketch";
    public static string player_id = "Sketcher";
    public static string model_id;
    public static string namelist_path = @"C:\Users\ll00931\Documents\Datasets\VR_sketch\namelist\subset3.txt";
    public static int index = 0;
    public static float countdown = 5;

    public List<string> namelist;
    public TextMeshProUGUI username;
    public TextMeshProUGUI modelname;

    private void Start()
    {
        username.text = "Welcome! " + player_id;
        if (!File.Exists(namelist_path))
        {
            Debug.LogError("File doesn't exist: " + namelist_path);
        }
        else
        {
            //Read the text from directly from the test.txt file
            using (StreamReader sr = new StreamReader(namelist_path))
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    namelist.Add(line);
                }
            }
        }
        model_id = namelist[index];
        modelname.text = "Model: "+ (index + 1) + "/" + namelist.Count + "  " + model_id;
    }

    public void NextModel()
    {
        if (index < namelist.Count -1)
        {
            index += 1;
            model_id = namelist[index];
            modelname.text = "Model: " + (index + 1) + "/" + namelist.Count + "  " + model_id;
            GameObject line_manager = GameObject.Find("PointLineManager");
            Destroy(line_manager.GetComponent<PointLineManager>()); //toggle this script to re-invoke it

            GameObject[] reference = GameObject.FindGameObjectsWithTag("reference");
            for (int i = 0; i < reference.Length; i++)
            {
                Destroy(reference[i]);
            }

            GameObject[] sketch = GameObject.FindGameObjectsWithTag("Dynamic_Line");
            for (int i = 0; i < sketch.Length; i++)
            {
                Destroy(sketch[i]);
            }

            line_manager.AddComponent<PointLineManager>();

            GameObject count_down = GameObject.Find("CountDown");
            count_down.GetComponent<CountDownScript>().init(); 
        }
    }
    public void PreviousModel()
    {
        if (index > 0)
        {
            index -= 1;
            model_id = namelist[index];
            modelname.text = "Model: " + (index + 1) + "/" + namelist.Count + "  " + model_id;
            GameObject line_manager = GameObject.Find("PointLineManager");
            Destroy(line_manager.GetComponent<PointLineManager>()); //toggle this script to re-invoke it

            GameObject[] reference = GameObject.FindGameObjectsWithTag("reference");
            for (int i = 0; i < reference.Length; i++)
            {
                Destroy(reference[i]);
            }

            GameObject[] sketch = GameObject.FindGameObjectsWithTag("Dynamic_Line");
            for (int i = 0; i < sketch.Length; i++)
            {
                Destroy(sketch[i]);
            }

            line_manager.AddComponent<PointLineManager>();

            GameObject count_down = GameObject.Find("CountDown");
            count_down.GetComponent<CountDownScript>().init();
        }
    }
}
