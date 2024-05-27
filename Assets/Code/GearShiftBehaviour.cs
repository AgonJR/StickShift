using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GearShiftBehaviour : MonoBehaviour
{
    [Header("Dev References")]
    public TMP_Text txtClutchStatus;
    public TMP_Text txtGearStatus;

    public Gear     currentGear = Gear.NeutralMid;
    
    public float    mouseSlideThreshold = 10f ;
    public float    gearShiftCooldown   = 0.3f;

    [Space]
    [Header("Gear Bulbs")]
    public Material SelectedGearBulbColor;
    public Material DefaultGearBulbColour;
    public MeshRenderer[] GearBulbRenders;
    [Space]

    private bool    _clutchDown = false;
    private Vector3 _lastMousePos;


    void Start()
    {
        UpdateGearBulbColors();
    }

    void Update()
    {
        ProcessInput_Clutch();
        ProcessInput_GearShift();
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

            _lastMousePos = Vector3.zero;
        }
    }

    void ProcessInput_GearShift()
    {
        if (_clutchDown)
        {
            if ( _lastMousePos == Vector3.zero ) { _lastMousePos = Input.mousePosition; }

            Vector3 currentMousePos = Input.mousePosition;
            Vector3 shiftDirection  = currentMousePos - _lastMousePos;

                 if (Mathf.Abs(shiftDirection.x) >= mouseSlideThreshold) 
                 { ShiftGear(shiftDirection.x > 0 ? GearShiftDirection.Right : GearShiftDirection.Left); }
            else if (Mathf.Abs(shiftDirection.y) >= mouseSlideThreshold) 
                 { ShiftGear(shiftDirection.y > 0 ? GearShiftDirection.Up    : GearShiftDirection.Down); }

            _lastMousePos = currentMousePos;
        }
    }


    public enum Gear
    {
        NeutralLft, NeutralMid,  NeutralRgt,
        First, Second, Third, Fourth, Fifth,
        Reverse
    }

    private enum GearShiftDirection
    {
        Up, Down, Left, Right
    }

    [SerializeField] private bool _shiftingGears = false;

    void ShiftGear(GearShiftDirection gsDir)
    {
        if ( _shiftingGears )
        {
            return;
        }

        _shiftingGears = true;
        
        if (gsDir == GearShiftDirection.Up)
        {
            switch (currentGear)
            {
                case Gear.NeutralLft: currentGear = Gear.First; break;
                case Gear.NeutralMid: currentGear = Gear.Third; break;
                case Gear.NeutralRgt: currentGear = Gear.Fifth; break;
                
                case Gear.Second : currentGear = Gear.NeutralLft; break;
                case Gear.Fourth : currentGear = Gear.NeutralMid; break;
                case Gear.Reverse: currentGear = Gear.NeutralRgt; break;
            }
        }
        else if (gsDir == GearShiftDirection.Down)
        {
            switch (currentGear)
            {
                case Gear.NeutralLft: currentGear = Gear.Second ; break;
                case Gear.NeutralMid: currentGear = Gear.Fourth ; break;
                case Gear.NeutralRgt: currentGear = Gear.Reverse; break;
                
                case Gear.First: currentGear = Gear.NeutralLft; break;
                case Gear.Third: currentGear = Gear.NeutralMid; break;
                case Gear.Fifth: currentGear = Gear.NeutralRgt; break;
            }
        }
        else if (gsDir == GearShiftDirection.Left)
        {
            switch (currentGear)
            {
                case Gear.NeutralMid: currentGear = Gear.NeutralLft; break;
                case Gear.NeutralRgt: currentGear = Gear.NeutralMid; break;
            }
        }
        else if (gsDir == GearShiftDirection.Right)
        {
            switch (currentGear)
            {
                case Gear.NeutralMid: currentGear = Gear.NeutralRgt; break;
                case Gear.NeutralLft: currentGear = Gear.NeutralMid; break;
            }
        }

        UpdateGearBulbColors();
        UpdateGearStatusText();
        
        StartCoroutine(ResetGearShiftDelay());
    }

    IEnumerator ResetGearShiftDelay()
    {
        //Delay to avoid quick multi-shifts
        yield return new WaitForSeconds(gearShiftCooldown);
        _shiftingGears = false;
    }

    private void UpdateGearBulbColors()
    {
        for (int i = 0; i < 9; i++ )
        {
            GearBulbRenders[i].material = currentGear == (Gear) i ? SelectedGearBulbColor : DefaultGearBulbColour;
        }
    }

    private void UpdateGearStatusText()
    {
        txtGearStatus.text = "G: " + currentGear;
    }

}
