using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonOption : MonoBehaviour
{
    private GlobalData globalScript;
    private void Awake()
    {
        GameObject globalData = GameObject.Find("GlobalData");
        if(globalData == null)
        {
            Debug.Log("Cannot found global data");
        }
        else
        {
            globalScript = globalData.GetComponent<GlobalData>();
        }
       
    }
    public void TrackSelect()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }


    //game mode
    public void AgainstAI()
    {
        globalScript.ghostMode = false;
    }

    public void AgainstGhost()
    {
        globalScript.ghostMode = true;
    }

    public void MediumMode()
    {
        globalScript.gameLevel = 1;
    }

    public void EasyMode()
    {
        globalScript.gameLevel = 0;
    }

    public void HardMode()
    {
        globalScript.gameLevel = 2;
    }

    //Track scene 
    public void Track01()
    {
        SceneManager.LoadScene(2);
    }
    
    public void Track02()
    {
        SceneManager.LoadScene(3);
    }
    public void Track03()
    {

    }
}
