using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{

    public Transform path;
    private List<Transform> nodes;
    public Transform checkPoints;
    private int currentNode = 0;

    public float Steer { get; set; }
    public float Throttle { get; set; }

    [Header("Sensors")]
    public Vector3 frontSensorPosition = new Vector3(0f, 0f, 2f);
    public float sensorSidePosition = 1f;
    public float frontSensorAngle = 30;
    public int timeToUnstuck = 80;

    private float targetSteerAngle = 0;
    public float steerDamper = 1;

    private bool isAvoiding = false;
    private int timeStuck = 0;
    private int timeBackCar = 0;

    private int gameLevel = 0;
    public float motorTorque { get; set; }
    public float MAXSPEED { get; set; }
    public float sensorLenght { get; set; } = 10f;

    private GameObject globalData;
    private void Awake()
    {
        globalData = GameObject.Find("GlobalData");
        if(globalData == null)
        {
            Debug.Log("cannot find global data");
        }
        else
        {
            gameLevel = globalData.GetComponent<GlobalData>().gameLevel;
        }


        //set value for car AI
        if (gameLevel == 0)
        {
            motorTorque = 400;
            MAXSPEED = 500;
            sensorLenght = 8;
            
        }
        else if (gameLevel == 1)
        {
            motorTorque = 500;
            MAXSPEED = 600;
            sensorLenght = 10;
        }
        else if (gameLevel == 2) {
            motorTorque = 600;
            MAXSPEED = 700;
            sensorLenght = 15;
        }
    }

    void Start()
    {
        Throttle = 1;

        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i< pathTransforms.Length; i++)
        {
            if(pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
       
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Sensors();
        ApplySteer();
        CheckWayPoint();
        CheckBackThrottle();
        
    }


    private void Sensors()
    {
        RaycastHit hit;
        float avoidMultiplier = 0;
        isAvoiding = false;

        //move sensor point to the head of the car
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;  //transform.forward: move the object while considering its rotation
        sensorStartPos += transform.up * frontSensorPosition.y;

        //front left sensor
        sensorStartPos -= transform.right * sensorSidePosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLenght))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isAvoiding = true;
                avoidMultiplier += 1f;
            }
        }
        
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLenght))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isAvoiding = true;
                avoidMultiplier += 0.5f;
            }
        }
      

       
        //front right sensor
        sensorStartPos += 2 * transform.right * sensorSidePosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLenght))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isAvoiding = true;
                avoidMultiplier -= 1f; // turn left
            }
        }
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLenght))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isAvoiding = true;
                avoidMultiplier -= 0.5f;
            }
        }


        //front sensor - center
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLenght) && avoidMultiplier == 0) // both 2 sensor hit
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isAvoiding = true;
                if(hit.normal.x < 0)
                {
                    avoidMultiplier -= 1;
                }
                else
                {
                    avoidMultiplier += 1;
                }
            }

        }

        if (isAvoiding)
        {
            float preSteer = Steer;
            targetSteerAngle = avoidMultiplier;
            Steer = Mathf.Lerp(preSteer, targetSteerAngle, Time.deltaTime * steerDamper);
       
        }
      
    }


    private void CheckWayPoint()
    {
       
        if(Vector3.Distance(transform.position, nodes[currentNode].position)< 5f)
        {
            if(currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
       
    }

   

    private void ApplySteer()
    {
        //dont apply normal steer if avoiding
        if (isAvoiding)
        {
            return;
        }
        //get the relative vector
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        relativeVector = relativeVector / relativeVector.magnitude; // magnitude is the same as the length of the vector\

        float newSteer = (relativeVector.x / relativeVector.magnitude);

        Steer = newSteer;
    }

    private void OnCollisionStay(Collision collision)
    {
        //back the car after amount of stuck time 
        timeStuck++;
        if(timeStuck > timeToUnstuck)
        {
            Throttle = -1;
            timeStuck = 0;
        }
    }

    //re-move the car forward after amount of back thorttle
    private void CheckBackThrottle()
    {
        if(Throttle == -1)
        {
            timeBackCar++;
            if(timeBackCar > 100)
            {
                Throttle = 1;
                timeBackCar = 0;
            }
        }
    }
  
}
