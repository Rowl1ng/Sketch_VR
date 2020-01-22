using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SFB;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField username;

    public TMP_InputField savedir;
    public TMP_InputField modeldir;
    public TMP_InputField namelist;

    public TMP_InputField countdown;
    public TMP_InputField index;
    public TextMeshProUGUI message;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void selectModelDir()
    {

        PlayerManager.model_dir = StandaloneFileBrowser.OpenFolderPanel("Select Model Folder", "", true)[0];
        modeldir.text = PlayerManager.model_dir;

    }
    public void selectSaveDir()
    {

        PlayerManager.save_dir = StandaloneFileBrowser.OpenFolderPanel("Select Save Folder", "", true)[0];
        savedir.text = PlayerManager.save_dir;

    }

    public void selectFile()
    {
        PlayerManager.namelist_path = StandaloneFileBrowser.OpenFilePanel("Open Namelist File", "", "txt", true)[0];
        namelist.text = PlayerManager.namelist_path;
    }
    
    public void startSketch()
    {
        if (string.IsNullOrEmpty(username.text))
        {
            message.text = "Username is Null!";
            Debug.LogError("Username is Null!");
            
        }
        else if (string.IsNullOrEmpty(savedir.text))
        {
            message.text = "Save Directory is Null!";
            Debug.LogError("Save Directory is Null!");
        }
        else if (string.IsNullOrEmpty(modeldir.text))
        {
            message.text = "Model Directory is Null!";
            Debug.LogError("Model Directory is Null!");
        }
        else if (string.IsNullOrEmpty(namelist.text))
        {
            message.text = "Name List is Null!";
            Debug.LogError("Name List is Null!");
        }
        else
        {
            PlayerManager.player_id = username.text;
            if (float.TryParse(countdown.text, out float number))
                PlayerManager.countdown = number;
            if (int.TryParse(index.text, out int number_))
                PlayerManager.index = number_ - 1;
            SceneManager.LoadScene(1);
        }

    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
