using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private Bounds bounds;
    [SerializeField] private float bullet_force = 100;
    private RaycastHit hitBackward;
    private RaycastHit hitUp;
    private RaycastHit hitDown;
    private RaycastHit hitLeft;
    private RaycastHit hitRight;
    private RaycastHit hitForward;
    private Vector3 starPos;
    private Vector3 CurrentPos;
    private float distance;
    private Vector3 Ray_start_pos = new Vector3(Screen.width / 2, Screen.height / 2, 0);

    void Start()
    {
        transform.Rotate(new Vector3(90, 0, 0));
        starPos = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * bullet_force, ForceMode.Impulse);
        StartCoroutine(Time());
        bounds = GetComponent<MeshFilter>().sharedMesh.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentPos = transform.position;
        distance = Vector3.Distance(starPos, CurrentPos);
        Physics.Raycast(transform.position, new Vector3(0, 0, 1), out hitForward, bounds.size.z);
        Physics.Raycast(transform.position, new Vector3(0.5f, 0.5f, 1), out hitRight, bounds.size.z);
        Physics.Raycast(transform.position, new Vector3(-0.5f, 0.5f, 1), out hitLeft, bounds.size.z);
        Physics.Raycast(transform.position, new Vector3(0, -0.5f, 1), out hitDown, bounds.size.z);
        Physics.Raycast(transform.position, new Vector3(0, 0.5f, 1), out hitUp, bounds.size.z);
        //Physics.Raycast(transform.position, new Vector3(0, 0, -1), out hitBackward, bounds.size.z);

        // if (HitRighdBody())
        // {
        //     var obj = HitObj();
        //     if (obj.tag == "enemy")
        //     {
        //         obj.SendMessage("RagDollOn");
        //     }
        //     HitCol().attachedRigidbody.AddForce(rb.velocity / rb.mass, ForceMode.Acceleration);
        // }
        Debug.Log(HitObj().name);

    }

    private bool HitRighdBody()
    {
        if (hitForward.rigidbody || hitRight.rigidbody || hitLeft.rigidbody || hitDown.rigidbody || hitUp.rigidbody /*|| hitBackward.rigidbody*/)
            return true;
        else
            return false;
    }
    private string HitTag()
    {
        // if (hitBackward.collider != null)
        // {
        //     return "item";
        // }
        if (hitUp.collider != null)
        {
            return "item";
        }
        if (hitDown.collider != null)
        {
            return "item";
        }
        if (hitLeft.collider != null)
        {
            return "item";
        }
        if (hitRight.collider != null)
        {
            return "item";
        }
        if (hitForward.collider != null)
        {
            return "item";
        }
        else
        {
            return null;
        }
    }

    private GameObject HitObj()
    {

        // if (hitBackward.transform.gameObject != null)
        // {
        //     return hitBackward.transform.gameObject;
        // }
        if (hitUp.collider != null)
        {
            return hitBackward.transform.gameObject;
        }
        if (hitDown.collider != null)
        {
            return hitBackward.transform.gameObject;
        }
        if (hitLeft.collider != null)
        {
            return hitBackward.transform.gameObject;
        }
        if (hitRight.collider != null)
        {
            return hitBackward.transform.gameObject;
        }
        if (hitForward.collider != null)
        {
            return hitBackward.transform.gameObject;
        }
        else
        {
            return null;
        }
    }


    private Collider HitCol()
    {

        // if (hitBackward.collider != null)
        // {
        //     return hitBackward.collider;
        // }
        if (hitUp.collider != null)
        {
            return hitBackward.collider;
        }
        if (hitDown.collider != null)
        {
            return hitBackward.collider;
        }
        if (hitLeft.collider != null)
        {
            return hitBackward.collider;
        }
        if (hitRight.collider != null)
        {
            return hitBackward.collider;
        }
        if (hitForward.collider != null)
        {
            return hitBackward.collider;
        }
        else
        {
            return null;
        }
    }

    IEnumerator Time()
    {

        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);

    }
}
