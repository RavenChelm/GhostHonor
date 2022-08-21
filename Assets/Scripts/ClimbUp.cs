using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbUp : MonoBehaviour
{
    [SerializeField] GameObject Player;
    private PlayerContoller PlCr;


    private void Start()
    {
        PlCr = Player.GetComponent<PlayerContoller>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (this.name == "ClimbUp")
        {

            PlCr._climbUp = true;
        }
        else
        {
            PlCr._climbMiddle = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (this.name == "ClimbUp")
        {

            PlCr._climbUp = false;
        }
        else
        {
            PlCr._climbMiddle = false;
        }
    }
}
