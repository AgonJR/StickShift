using UnityEngine;

public class CarAccelerator : MonoBehaviour
{
    public Transform carTransform;

    [Space]
    [Header("Acceleration Per Gear")]
    public float accelerationNuetral    = 0.0f;
    public float accelerationFirst      = 1.0f;
    public float accelerationSecond     = 2.0f;
    public float accelerationThird      = 3.0f;
    public float accelerationFourth     = 4.0f;
    public float accelerationFifth      = 5.0f;
    public float accelerationReverse    =-1.0f;

    [Space]
    public float decelerationFlat       = 1.0f;

    // //To Do - Implement a Curve instead of a set value
    // [Space]
    // [Header("Gear Acceleration Curves")]
    // public AnimationCurve neutralGearAccelerationCurve ;
    // public AnimationCurve firstGearAccelerationCurve   ;
    // public AnimationCurve secondGearAccelerationCurve  ;
    // public AnimationCurve thirdGearAccelerationCurve   ;
    // public AnimationCurve fourthGearAccelerationCurve  ;
    // public AnimationCurve fifthGearAccelerationCurve   ;
    // public AnimationCurve reverseGearAccelerationCurve ;

    private bool _accDown = false;
    private GearShiftBehaviour gear;

    private float _carSpeed = 0.0f;


    void Start()
    {
        gear = gameObject.GetComponent<GearShiftBehaviour>();
    }

    void Update()
    {
        ProcessInput_Accelerator();
    }

    void FixedUpdate()
    {
        Process_Acceleration();
        Process_Movement();
        Process_Deceleration();
    }

    
    void ProcessInput_Accelerator()
    {
        _accDown = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);
    }

    private void Process_Acceleration()
    {
        // Increase car speed based on current gear

        if ( _accDown )
        {
            float gearAcc = EvaluateGearAcceleration();
            _carSpeed = gearAcc >= 0.0f ? Mathf.Max(_carSpeed, gearAcc) : Mathf.Min(_carSpeed, gearAcc); //Min for reverse
        }
    }

    private void Process_Deceleration()
    {
        // Car alwasys decelerates a bit, so tht it stops if no acceleration is applied
        if ( _carSpeed > 0.0f )
        {
            _carSpeed -= decelerationFlat * Time.deltaTime;
            
            if ( _carSpeed < 0.0f ) 
            {
                _carSpeed = 0.0f;
            }
        }
        // If it's in reverse ...
        else if ( _carSpeed < 0.0f )
        {
            _carSpeed += decelerationFlat * Time.deltaTime;
            
            if ( _carSpeed > 0.0f ) 
            {
                _carSpeed = 0.0f;
            }
        }
    }
    
    private void Process_Movement()
    {
        // What about turning ?

        if ( _carSpeed != 0.0f )
        {
            transform.Translate(Vector3.forward * _carSpeed * Time.deltaTime);
        }
    }

    private float EvaluateGearAcceleration()
    {
        switch (gear.currentGear)
        {
            case GearShiftBehaviour.Gear.First   : return accelerationFirst     ;
            case GearShiftBehaviour.Gear.Second  : return accelerationSecond    ;
            case GearShiftBehaviour.Gear.Third   : return accelerationThird     ;
            case GearShiftBehaviour.Gear.Fourth  : return accelerationFourth    ;
            case GearShiftBehaviour.Gear.Fifth   : return accelerationFifth     ;
            case GearShiftBehaviour.Gear.Reverse : return accelerationReverse   ;
        }

        return accelerationNuetral;
    }

}
