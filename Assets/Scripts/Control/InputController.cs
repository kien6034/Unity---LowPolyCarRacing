using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    public string inputSteerAxis = "Horizontal";
    public string inputThrottleAxis = "Vertical";

    public float ThrottleInput { get; private set; }
    public float SteerInput { get; private set; }
    public bool BreakInput { get; private set; }
    // Start is called before the first frame update

    public bool CameraViewInput { get; private set; }

    // Update is called once per frame
    void Update()
    {
        SteerInput = Input.GetAxis(inputSteerAxis);
        ThrottleInput = Input.GetAxis(inputThrottleAxis);

        BreakInput = Input.GetKey(KeyCode.Space);

        CameraViewInput = Input.GetKeyDown(KeyCode.Z);

    }
}
