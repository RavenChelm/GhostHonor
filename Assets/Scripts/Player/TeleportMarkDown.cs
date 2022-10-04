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
    [SerializeField] private Transform helpSphere;

    private bool col_ask = false;
    private bool temp = false;
    private Vector3 Point;
    int maskRay;
    int maskRayUp;
    float time = 0;
    float _rotationX = 0;
    float _rotationY = 0;
    float moveSpeed = 0;

    void Start()
    {
        int teleportSphereIndexLayer = LayerMask.NameToLayer("TeleportSphere");
        int playerIndexLayer = LayerMask.NameToLayer(layerName: "Player");

        if (teleportSphereIndexLayer == -1 || playerIndexLayer == -1)
        {
            Debug.LogError("Layer does not exist");
        }
        else
        {
            maskRay = ~((1 << teleportSphereIndexLayer) | (1 << playerIndexLayer));
            maskRayUp = ~(1 << teleportSphereIndexLayer);
        }

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitSphere;
        var fallFromSphere = Physics.Raycast(transform.position,
            Vector3.down, out hitSphere, Mathf.Infinity, maskRay);
        RaycastHit hitUp;
        var Fall = Physics.Raycast(origin: helpSphere.position,
            Vector3.up, hitInfo: out hitUp, 3f, maskRayUp);
        //Если луч никуда не попал(?)
        _rotationX += Input.GetAxis("Mouse X");
        _rotationY -= Input.GetAxis("Mouse Y");
        if (hitSphere.point != Vector3.zero)
        {
            Point = transform.InverseTransformDirection(hitSphere.point);
        }

        //Если луч попал в сферу
        if (hitUp.collider == shpereCollider)
        {
            downSystem.transform.position = Vector3.Lerp(downSystem.transform.position, Point, 0.07f);
        }
        //Если между основной сферой и вспомогательной есть предметы 
        else if (hitUp.collider != shpereCollider && Mathf.Abs(_rotationX) < 0.1 && Mathf.Abs(_rotationY) < 0.1)
        {
            downSystem.transform.position = Vector3.Lerp(downSystem.transform.position, transform.position, 0.07f);
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

