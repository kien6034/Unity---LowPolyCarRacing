using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject UIRacePanel;

    public Text UICurrentTime;
    public Text UICurrentPosition;
    public Text UICurrentLap;
    public Text UIBestLapTime;

    private GameObject player;
    private Player UpdateUIForPlayer;
    // Start is called before the first frame update

    private int currentLap = -1;
    private float currentTime;
    private float bestLapTime;
    private int currentPosition;
    private int score = 0;
    // Update is called once per frame

    private void Awake()
    {
        //get the player 
        player = GameObject.Find("player");
        UpdateUIForPlayer = player.GetComponent<Player>();
    }

    void Update()
    {
        if (UpdateUIForPlayer == null)
        {
            return;
        }
        if (player.GetComponent<Player>().ghostMode) 
        {
            if (UpdateUIForPlayer.CurrentLap != currentLap)
            {
                currentLap = UpdateUIForPlayer.CurrentLap;
                UICurrentLap.text = $"LAP : {currentLap}";

                score = score + (int)currentTime / 5 * currentLap;
                player.GetComponent<Player>().score = score;
                UICurrentPosition.text = $"SCORE: {score}";

            }
            if (UpdateUIForPlayer.CurrentLapTime != currentTime)
            {
                currentTime = UpdateUIForPlayer.CurrentLapTime;
                UICurrentTime.text = $"TIME: {(int)currentTime / 60}:{(currentTime) % 60:00.00}";
            }
            if (UpdateUIForPlayer.BestLapTime != bestLapTime)
            {
                bestLapTime = UpdateUIForPlayer.BestLapTime;
                if (bestLapTime == Mathf.Infinity)
                {
                    UIBestLapTime.text = "BEST: 0:00.0";
                }
                else
                {
                    UIBestLapTime.text = $"BEST: {(int)bestLapTime / 60}:{(bestLapTime) % 60:00.00}";
                }
            }
            

        }
        else
        {
            if (UpdateUIForPlayer.CurrentLap != currentLap)
            {
                currentLap = UpdateUIForPlayer.CurrentLap;
                UICurrentLap.text = $"LAP : {currentLap}";
            }

            if (UpdateUIForPlayer.CurrentLapTime != currentTime)
            {
                currentTime = UpdateUIForPlayer.CurrentLapTime;
                UICurrentTime.text = $"TIME: {(int)currentTime / 60}:{(currentTime) % 60:00.00}";
            }

            if (UpdateUIForPlayer.CurrentPosition != currentPosition)
            {
                currentPosition = UpdateUIForPlayer.CurrentPosition;
                UICurrentPosition.text = $"RANK: {currentPosition}";


            }
            if (UpdateUIForPlayer.BestLapTime != bestLapTime)
            {
                bestLapTime = UpdateUIForPlayer.BestLapTime;
                if (bestLapTime == Mathf.Infinity)
                {
                    UIBestLapTime.text = "BEST: 0:00.0";
                }
                else
                {
                    UIBestLapTime.text = $"BEST: {(int)bestLapTime / 60}:{(bestLapTime) % 60:00.00}";
                }
            }
        }

        
    }
}
