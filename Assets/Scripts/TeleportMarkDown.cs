using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMarkDown : MonoBehaviour
{
    //повесить скрипт на sphere
    [SerializeField] private ParticleSystem VerticalGlow;
    [SerializeField] private Transform upSystem;
    [SerializeField] private Transform downSystem;
    [SerializeField] private Collider shpereCollider;
    private bool col_ask = false;
    private bool temp = false;
    private Vector3 Point;
    int MaskRaySphere;
    int MaskRayUp;
    void Start()
    {
        int FirstIndexLayer = LayerMask.NameToLayer("TeleportSphere");
        int SecondIndexLayer = LayerMask.NameToLayer("Player");
        int ThirdIndexLayer = LayerMask.NameToLayer("HelpTeleport");

        if (FirstIndexLayer == -1 || SecondIndexLayer == -1 || ThirdIndexLayer == -1)
        {
            Debug.Log("Layer doesn't exist");
        }
        else
        {
            int mMaskRaySphereask1 = ~(1 << FirstIndexLayer);
            int MaskRayUp = ~((1 << SecondIndexLayer) | (1 << ThirdIndexLayer));
        }



    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitSphere;
        var fallFromSphere = Physics.Raycast(transform.position,
            Vector3.down, out hitSphere, Mathf.Infinity, MaskRaySphere);
        RaycastHit hitUp;
        var Fall = Physics.Raycast(origin: new Vector3(transform.position.x, transform.position.y + 1, transform.position.z),
            Vector3.down, out hitUp, 3f, MaskRayUp);

        if (Fall)
        {
            Debug.Log("fall");
            if (fallFromSphere && temp == false)
            {
                Point = hitSphere.point;
                temp = true;
            }
            else
            {
                Point = transform.position;
            }
        }
        else
        {
            Point = transform.position;
        }

        if (temp)
        {
            downSystem.transform.position = Vector3.MoveTowards(downSystem.transform.position, Point, Time.deltaTime * 10);
            if (downSystem.transform.position == Point)
            {
                temp = false;
            }
        }
        // Расположение вертикального свечения
        var distanceVerticalGlow = Vector3.Distance(downSystem.position, upSystem.position);
        VerticalGlow.transform.position = new Vector3(transform.position.x, distanceVerticalGlow / 2f, transform.position.z);
        var main = VerticalGlow.main;
        main.startSizeYMultiplier = distanceVerticalGlow;
        VerticalGlow.transform.rotation = transform.localRotation;

    }
























    // {
    //     //луч из сферы
    //     RaycastHit hitSphere;

    //     var fallFromSphere = Physics.Raycast(new Vector3(upSystem.position.x,
    //     upSystem.position.y - Vector3.Distance(transform.position, upSystem.transform.position),
    //     upSystem.position.z),
    //     transform.TransformDirection(Vector3.down), out hitSphere);

    //     RaycastHit hitFromFall;
    //     var upFromFall = Physics.Raycast(origin: hitSphere.point,
    //     Vector3.up, out hitFromFall);
    //     if (hitFromFall.collider == shpereCollider && upFromFall && fallFromSphere && temp == false)
    //     {
    //         Debug.Log("qq");
    //         downSystem.transform.position = Vector3.MoveTowards(downSystem.transform.position,
    //         new Vector3(hitSphere.point.x, hitSphere.point.y + 0.2f, hitSphere.point.z), Time.deltaTime * 10);
    //         if (hitSphere.point != Vector3.zero)
    //         {
    //             temp = true;
    //             tempPoint = new Vector3(hitSphere.point.x, hitSphere.point.y + 0.2f, hitSphere.point.z);
    //         }
    //     }
    //     else if (temp)
    //     {
    //         downSystem.transform.position = tempPoint;

    //         if (downSystem.transform.position == tempPoint)
    //         {
    //             temp = false;
    //         }
    //     }
    //     else
    //     {
    //         downSystem.transform.position = Vector3.MoveTowards(downSystem.transform.position,
    //         transform.position, Time.deltaTime * 10);
    //     }



    //     //Расположение вертикального свечения
    //     var distanceVerticalGlow = Vector3.Distance(downSystem.position, upSystem.position);
    //     VerticalGlow.transform.position = new Vector3(VerticalGlow.transform.position.x, distanceVerticalGlow / 1.5f, VerticalGlow.transform.position.z);
    //     var main = VerticalGlow.main;
    //     main.startSizeYMultiplier = distanceVerticalGlow;
    //     VerticalGlow.transform.rotation = transform.localRotation;

    // }
    // private Collider OnTriggerEnter(Collider other)
    // {

    //     Debug.Log(other.name);
    //     if (other == null && other.tag != "TeleportSphere")
    //         col_ask = false;
    //     else
    //         col_ask = true;
    //     return other;
    // }

}

