using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSwitch : MonoBehaviour
{
    public GameObject upperHalf;
    public GameObject lowerHalf;

    private bool controllingUpper = true;

    void Start()
    {
        SetControlState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            controllingUpper = !controllingUpper;
            SetControlState();
        }
    }

    void SetControlState()
    {
        if (upperHalf != null && lowerHalf != null)
        {
            var upperController = upperHalf.GetComponent<UpperHalfController>();
            var lowerController = lowerHalf.GetComponent<LowerHalf>();

            upperController.canControl = controllingUpper;
            lowerController.canControl = !controllingUpper;

            if (!controllingUpper)
            {
                upperController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else
            {
                lowerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
}