using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public InputController InputController { get; private set; }
   
    //runberfore start
    private void Awake()
    {
        Instance = this;
        //get input controller script
        InputController = GetComponentInChildren<InputController>();
    }

 
}
