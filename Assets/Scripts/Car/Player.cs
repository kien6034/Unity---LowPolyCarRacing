using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public enum ControlType {HumanInput, AI }

    public ControlType controlType = ControlType.HumanInput;

    public int LapPerGame = 2;
    private int LapCompleted = 0;

    //UI    
    public float BestLapTime { get; private set; } = Mathf.Infinity;

    public float LastLapTime { get; private set; } = 0;

    public float CurrentLapTime { get; private set; } = 0;

    public int CurrentLap { get; private set; } = 0;

    public int CurrentPosition { get; set; } = 0;

    public bool isGameCompleted { get; set; } = false;

    private GameObject others;

    //ghost mode
    public GameObject ghostCar;
    public bool ghostMode = false;
  

    private float lapTimerTimeStamp;
    
    //race position 
    public float Point { get; set; }
    public int lastCheckPointPassed { get; private set; } = 0;
    public int score { get; set; } = 0;


    //number of checkpoints
    public int checkpointCount { get; set; } = 0;
    private int checkpointLayer;

    //car Controller script
    private CarController carController;

    //car AI script
    private CarAI carAI;
    private GameObject globalData;
    

    private void Awake()
    {
        others = GameObject.Find("others");
        globalData = GameObject.Find("GlobalData");
        if (globalData == null)
        {
            Debug.Log("cannot find global data");
        }
        else
        {
           
            
        }
        //start value
        Point = 0;
    
        checkpointLayer = LayerMask.NameToLayer("Checkpoint");
        carController = GetComponent<CarController>();
     
        carAI = GetComponent<CarAI>();
    }


    void StartLap()
    {
        CurrentLap++;
        lastCheckPointPassed = 1;

        lapTimerTimeStamp = Time.time;

        if (ghostMode == true)
        {
            MakeGhost();
        }
        
    }

    void MakeGhost()
    {
        if (ghostCar == null)
        {
            return;
        }
        GameObject gc = Instantiate(ghostCar, new Vector3(90, -20f, 0.69f), Quaternion.identity);
        gc.GetComponent<GhostCarManager>().target = gameObject;
        gc.name = CurrentLap.ToString();
        gc.layer = LayerMask.NameToLayer("Enemy");
    }

    void EndLap()
    {
        LapCompleted++;

        //update the best laptime
        LastLapTime = Time.time - lapTimerTimeStamp;
        BestLapTime = Mathf.Min(LastLapTime, BestLapTime);

        //stop the engiene when complete the game 
        if (ghostMode)
        {
            
        }
        else
        {
            if (LapCompleted == LapPerGame)
            {
                //Stop the engine
                gameObject.GetComponent<CarController>().isBraking = true;
                gameObject.GetComponent<CarController>().maxBreak *= 2;
                if (gameObject.name == "player")
                {
                    isGameCompleted = true;
                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
 
        if (other.gameObject.layer != checkpointLayer)
        {  
            return;
        }

        //if is the checkpoint layer
        if (other.gameObject.name == "1")
        {
            //if ..., we've completed a lap 
            if(lastCheckPointPassed == checkpointCount)
            {
                EndLap();
            }

            //if ..., we 're on our first lap, or just complete the lap
            if (CurrentLap == 0 || lastCheckPointPassed == checkpointCount)
            {
                StartLap();
            }
           
            return;
        }

        // if we've passed the next checkpoint in the sequence, update the latest checkpoint
        if (other.gameObject.name == (lastCheckPointPassed + 1).ToString())
        {
            lastCheckPointPassed++;
          
        }
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
          
            if (ghostMode) {
                isGameCompleted = true;
            }
        }
    }


    void Update()
    {
     

        CurrentLapTime = lapTimerTimeStamp > 0 ? Time.time - lapTimerTimeStamp : 0;
        if(controlType == ControlType.HumanInput)
        {
            carController.Steer = GameManager.Instance.InputController.SteerInput;
            
            carController.Throttle = GameManager.Instance.InputController.ThrottleInput;
            carController.isBraking = GameManager.Instance.InputController.BreakInput;
           
        }

        else if (controlType == ControlType.AI)
        {
            carController.Steer = carAI.Steer;
            carController.Throttle = carAI.Throttle;
            carController.motorTorque = carAI.motorTorque;
            carController.MAXSPEED = carAI.MAXSPEED;
        }   
    }  
}
