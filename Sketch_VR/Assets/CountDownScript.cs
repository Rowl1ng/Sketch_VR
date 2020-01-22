using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownScript : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI uiText;
    //[SerializeField] private float mainTimer;

    private float timer;
    private bool canCount = false;
    public bool doOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        timer = PlayerManager.countdown;//mainTimer;
        uiText.text = "Countdown: "+ timer.ToString("F");
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0.0f && canCount)
        {
            timer -= Time.deltaTime;
            uiText.text = "Countdown: " + timer.ToString("F");
        }
        else if(timer <= 0.0f && !doOnce)
        {
            canCount = false;
            doOnce = true;
            uiText.text = "Countdown: 0.00";
            timer = 0.0f;
            GameObject[] reference = GameObject.FindGameObjectsWithTag("reference");

            for (int i = 0; i < reference.Length; i++)
            {
                MeshRenderer[] meshrenderer_obj = reference[i].transform.GetComponentsInChildren<MeshRenderer>();
                for (int j = 0; j < meshrenderer_obj.Length; j++)
                    meshrenderer_obj[j].enabled = false;
            }

        }
    }
    public void init()
    {
        canCount = false;
        doOnce = false;
        timer = PlayerManager.countdown;//mainTimer;
    }


    public void StartCount()
    {
        canCount = true;
        doOnce = false;
        GameObject save_sketch_logic = GameObject.Find("SaveSketchLogic");
        save_sketch_logic.GetComponent<SaveSketchLogic>().ClearSketch();
    }
}
