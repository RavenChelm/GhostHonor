using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatRightArm : MonoBehaviour
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
        if (PlCr._shoot)
        {
            animat.SetBool("Shoot", true);
        }
        else
        {
            PlCr._shoot = false;
            animat.SetBool("Shoot", false);
        }
    }
    public void ShootToggle()
    {
        PlCr._shoot = false;
        animat.SetBool("Shoot", false);
    }
}
