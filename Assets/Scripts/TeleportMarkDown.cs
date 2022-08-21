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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //луч из сферы
        RaycastHit hitSphere;

        var fallFromSphere = Physics.Raycast(new Vector3(upSystem.position.x,
        upSystem.position.y - Vector3.Distance(transform.position, upSystem.transform.position),
        upSystem.position.z),
        transform.TransformDirection(Vector3.down), out hitSphere);

        RaycastHit hitFromFall;
        // var upFromFall = Physics.Raycast(origin: hitSphere.point,
        // Vector3.up, out hitFromFall);
        var upFromFall = Physics.SphereCast(hitSphere.point, 0.1f, transform.TransformDirection(Vector3.up), out hitFromFall);
        if (hitFromFall.collider == shpereCollider && upFromFall && fallFromSphere)
        {
            Debug.Log("qq");
            downSystem.transform.position = Vector3.MoveTowards(downSystem.transform.position,
            new Vector3(hitSphere.point.x, hitSphere.point.y + 0.2f, hitSphere.point.z), Time.deltaTime * 10);
        }
        else
        {
            downSystem.transform.position = Vector3.MoveTowards(downSystem.transform.position,
            transform.position, Time.deltaTime * 10);
        }



        //Расположение вертикального свечения
        var distanceVerticalGlow = Vector3.Distance(downSystem.position, upSystem.position);
        VerticalGlow.transform.position = new Vector3(VerticalGlow.transform.position.x, distanceVerticalGlow / 1.5f, VerticalGlow.transform.position.z);
        var main = VerticalGlow.main;
        main.startSizeYMultiplier = distanceVerticalGlow;
        VerticalGlow.transform.rotation = transform.localRotation;

    }
    private Collider OnTriggerEnter(Collider other)
    {

        Debug.Log(other.name);
        if (other == null && other.tag != "TeleportSphere")
            col_ask = false;
        else
            col_ask = true;
        return other;
    }

}

