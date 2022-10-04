using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [HideInInspector] public float distToDetect;
    [HideInInspector] public float health;
    private Rigidbody[] rb_arr;
    private Animator animate;
    public PlayerContoller player;
    public NavMeshAgent agent;

    private void Start()
    {
        Init(100, 20);
    }
    public void Init(float distanceToDetectionValue, float healthValue)
    {
        agent = GetComponent<NavMeshAgent>();
        distToDetect = distanceToDetectionValue;
        health = healthValue;
        player = GameObject.Find("Player").GetComponent<PlayerContoller>();
        animate = GetComponent<Animator>();
        rb_arr = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rb_arr)
        {
            rb.tag = "enemy";
            rb.isKinematic = true;
        }
    }
    private void RagDollOn()
    {

        foreach (Rigidbody rb in rb_arr)
        {
            rb.isKinematic = false;
        }
        animate.enabled = false;
    }
    private void AddForceToBody(ArrayList list)
    {
        Debug.Log("rqrqrrqr");

        Vector3 vector = (Vector3)list[0];
        float force = (float)list[1];
        Collider col = (Collider)list[2];
        col.attachedRigidbody.AddForce(vector * force, ForceMode.Impulse);
    }
    public float DistanceToPlayer()
    {
        return Vector3.Distance(this.transform.position, player.transform.position);
    }

}
