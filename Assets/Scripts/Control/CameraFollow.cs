using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    public Transform ThirdViewCam;
    public Transform FirstViewCam;

    public Transform ViewPort;
    public Vector3 thirdOffset;
    public Vector3 firstOffset;
    public Vector3 linearOffset;
    
    public Vector3 eulerRotation;

    public float damper;
    private float yRot;

    public int camState = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = eulerRotation;
    }

    // Update is called once per frame
    void Update()
    {
        CameraViewEvent();
        if (Target == null)
        {
            return;
        }
        if (camState == 0)
        {
            transform.parent = null;
            transform.position = Vector3.Lerp(transform.position, Target.position + linearOffset, damper * Time.deltaTime);
            transform.eulerAngles = eulerRotation;

        }
        else if (camState == 1)
        {
            transform.parent = Target;
            transform.position = ThirdViewCam.position;
            transform.eulerAngles = new Vector3(0, Target.eulerAngles.y, 0);
        }
        else if(camState == 2)
        {
            transform.parent = Target;
            transform.position = FirstViewCam.position;
            transform.eulerAngles = new Vector3( Target.eulerAngles.x, Target.eulerAngles.y, Target.eulerAngles.z);
        }

    }


    void CameraViewEvent()
    {
        if (GameManager.Instance.InputController.CameraViewInput == true)
        {
            camState++;
            if(camState == 3)
            {
                camState = 0;   
            }
          
        }
    }
}
