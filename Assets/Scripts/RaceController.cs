using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceController : MonoBehaviour
{
    //player
    public GameObject player;
    public Transform otherRacers;

    //check points
    public Transform checkPoints;
    public int checkPointsWeight = 1000;

    public GameObject racePanel;
    public GameObject endGamePanel;


    //list
    private List<Transform> marks;
    private List<GameObject> others;

    private bool isEndGame = false;
    private int currentPosition = 0;
    private int score = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Rigidbody[] ors = otherRacers.GetComponentsInChildren<Rigidbody>();
        others = new List<GameObject>();
        others.Add(player);
        for (int i = 0; i < ors.Length; i++)
        {

            others.Add(ors[i].gameObject);

        }
        
        //get the checkpoints position
        Transform[] markPoints = checkPoints.GetComponentsInChildren<Transform>();
        marks = new List<Transform>();

        for (int i = 0; i < markPoints.Length; i++)
        {
            if (markPoints[i] != checkPoints.transform)
            {
                marks.Add(markPoints[i]);
            }

        }

        for(int i =0; i< others.Count; i++)
        {
            others[i].GetComponent<Player>().checkpointCount = marks.Count;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePoint();
        CheckPlayerPosition();
        CheckIfEndGame();
    }

    private void CheckIfEndGame()
    {
        isEndGame = others[0].GetComponent<Player>().isGameCompleted;
        if (isEndGame)
        {
            racePanel.SetActive(false);
            endGamePanel.SetActive(true);

            Text pText = endGamePanel.transform.GetChild(1).gameObject.GetComponent<Text>();

            if (others[0].GetComponent<Player>().ghostMode)
            {
                score = others[0].GetComponent<Player>().score;

                pText.text = $"Greate job racing boy. Here is your score {score}";

            }
            else
            {
               
                if (currentPosition == 1)
                {
                    pText.text = "What a race! Congratulations, my winner! \n 1st ";
                }
                else if (currentPosition == 2)
                {
                    pText.text = "Nearly made it. Cha zo cha zo \n 2nd";
                }
                else
                {
                    pText.text = $"Not satisfied huh? Give it another try? \n {currentPosition}";
                }
            }
            
            
        }
    }

    private void CalculatePoint()
    {
        for (int i = 0; i < others.Count; i++)
        {
            int curCheckPoint = others[i].GetComponent<Player>().lastCheckPointPassed;
            if (curCheckPoint > 0)
            {
                others[i].GetComponent<Player>().Point = others[i].GetComponent<Player>().CurrentLap * 10000 + curCheckPoint * checkPointsWeight +  (Vector3.Distance(others[i].transform.position, marks[curCheckPoint - 1].position));           
            } 
        }
    }

    private void CheckPlayerPosition()
    {
        int curPos = others.Count;
        for (int i=1; i<others.Count; i++)
        {
           
            if(others[0].GetComponent<Player>().Point > others[i].GetComponent<Player>().Point)
            {
                curPos--;    
            }
        }
        currentPosition = curPos;
        others[0].GetComponent<Player>().CurrentPosition = currentPosition;
    }
}
