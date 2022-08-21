using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody[] rb_arr;

    void Start()
    {
        rb_arr = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rb_arr)
        {
            rb.tag = "enemy";
            rb.isKinematic = true;
        }

    }
    private void RagDollOn()
    {
        Debug.Log("qwqwqwqwqw");

        foreach (Rigidbody rb in rb_arr)
        {
            rb.isKinematic = false;
        }
    }
    private void AddForceToBody(ArrayList list)
    {
        Debug.Log("rqrqrrqr");

        Vector3 vector = (Vector3)list[0];
        float force = (float)list[1];
        Collider col = (Collider)list[2];
        col.attachedRigidbody.AddForce(vector * force, ForceMode.Impulse);
    }
}
