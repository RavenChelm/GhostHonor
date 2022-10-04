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
        animat.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    void Update()
    {
        if (PlCr._teleportSwitch)
        {
            animat.SetBool("Cast", true);
        }
        else
        {
            animat.SetBool("Cast", false);
        }

    }
}
