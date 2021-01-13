using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMaker : MonoBehaviour
{
    private GameObject gc;
    // Start is called before the first frame update
    public CarMaker(GameObject gc)
    {
        this.gc = gc;
        Instantiate(this.gc, new Vector3(90, 1.04f, 0.69f), Quaternion.identity);
    }
   

}
