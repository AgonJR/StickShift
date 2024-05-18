using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearShiftBehaviour : MonoBehaviour
{

    private bool _clutchDown = false;


    void Start()
    {
        Debug.Log("Hello Banana!");
    }

    void Update()
    {
        ProcessInput_Clutch();
    }


    void ProcessInput_Clutch()
    {
        if ( Input.GetMouseButtonDown(0) )
        {
            _clutchDown = true;
        }

        if ( Input.GetMouseButtonUp(0) )
        {
            _clutchDown = false;
        }
    }

    void ProcessInput_GearShift()
    {
        if ( _clutchDown )
        {
            
        }
    }
    
}
