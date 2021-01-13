using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct GhostTransform
{
    public Vector3 position;
    public Quaternion rotation;

    public GhostTransform(Transform transform)
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
    }
}


public class GhostCarManager : MonoBehaviour
{
    public GameObject target;
   
    private int crLap {get; set; }
    private int lastLap { get; set; }


    private List<GhostTransform> recordedGhostTransforms = new List<GhostTransform>();
    private GhostTransform lastRecorededGhostTransform;


    // Update is called once per frame
    void Update()
    {
        crLap = target.GetComponent<Player>().CurrentLap;

        if (crLap == int.Parse(name))
        {
            if (target.transform.position != lastRecorededGhostTransform.position || target.transform.rotation != lastRecorededGhostTransform.rotation)
            {
                var newGhostTransform = new GhostTransform(target.transform);
                recordedGhostTransforms.Add(newGhostTransform);

                lastRecorededGhostTransform = newGhostTransform;
            }
        }

        if (crLap != lastLap)
        {
            if(crLap - int.Parse(name) > 0)
            {
                Play();
            }
        }
    
        lastLap = crLap;
    }

    void Play()
    {
        StartCoroutine(StartGhost());
       
    }

    IEnumerator StartGhost()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < recordedGhostTransforms.Count; i++)
        {

            transform.position = recordedGhostTransforms[i].position;
            transform.rotation = recordedGhostTransforms[i].rotation;
            yield return new WaitForFixedUpdate();
        }
    }
}
