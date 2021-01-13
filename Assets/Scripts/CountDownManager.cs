using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownManager : MonoBehaviour
{
    public GameObject CountDown;
    public AudioSource GetReadyAudio;
    public AudioSource GoAudio;

    private GameObject player;
    public GameObject others;

    private Rigidbody[] ors;
    private void Awake()
    {
        player = GameObject.Find("player");
     

        ors = others.GetComponentsInChildren<Rigidbody>();
        
        player.GetComponent<CarController>().enabled = false;
        for (int i = 0; i < ors.Length; i++)
        {
            ors[i].gameObject.GetComponent<CarController>().enabled = false;
        }
       
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!player.GetComponent<Player>().ghostMode)
        {
            StartCoroutine(CountStart());
        }
       
    }

    // Update is called once per frame
   
    IEnumerator CountStart()
    {
        yield return new WaitForSeconds(0.5f);
        CountDown.GetComponent<Text>().text = "3";
        GetReadyAudio.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<Text>().text = "2";
        GetReadyAudio.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<Text>().text = "1";
        GetReadyAudio.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        GoAudio.Play();

        //TODO: activate the car
      
        player.GetComponent<CarController>().enabled = true;
        for (int i = 0; i < ors.Length; i++)
        {
            Debug.Log("hey");
            ors[i].gameObject.GetComponent<CarController>().enabled = true;
        }
    }
}
