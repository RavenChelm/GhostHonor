using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animat : MonoBehaviour
{
    [SerializeField] private Animator animat;
    [SerializeField] GameObject Player;
    private PlayerContoller PlCr;
    private void Start()
    {
        PlCr = Player.GetComponent<PlayerContoller>();
    }
    void Update()
    {
        Debug.Log(PlCr.teleportSwitch);
        if (PlCr.teleportSwitch)
        {
            animat.SetBool("Teleport", true);
        }
        else
        {
            animat.SetBool("Teleport", false);

        }
    }
}
