using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GearShiftBehaviour : MonoBehaviour
{

    private bool _clutchDown = false;

    [Header("Dev References")]
    public TMP_Text txtClutchStatus;


    void Start()
    {
        //
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
            txtClutchStatus.text = "Clutch: DOWN!";
        }

        if ( Input.GetMouseButtonUp(0) )
        {
            _clutchDown = false;
            txtClutchStatus.text = "Clutch: UP!";
        }
    }

    void ProcessInput_GearShift()
    {
        if ( _clutchDown )
        {

        }
    }
    
}
