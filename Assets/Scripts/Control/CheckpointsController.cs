using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsController : MonoBehaviour
{
    public Transform checkPoints;
    public bool ghostMode = false;

    private List<Transform> marks;

    private int curCheckpoint =0;
    // Start is called before the first frame update
    void Start()
    {
        Transform[] markPoints = checkPoints.GetComponentsInChildren<Transform>();
        marks = new List<Transform>();

        for (int i= 0; i < markPoints.Length; i++)
        {
            if(markPoints[i] != checkPoints.transform)
            {
                marks.Add(markPoints[i]);
            }
        }

        //set main check points to the start point.
        transform.position = marks[0].position;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        curCheckpoint++;

        Debug.Log(marks.Count);
        if(curCheckpoint == marks.Count - 1)
        {
            curCheckpoint = 0;
        }

        transform.position = marks[curCheckpoint].position;

        if(curCheckpoint == 0)
        {
            StartLap();
        }
    }

    void StartLap()
    {
        if (ghostMode)
        {
            MakeGhost();
        }
    }

    void MakeGhost()
    {
        
    }

    void EndLap()
    {

    }
}
