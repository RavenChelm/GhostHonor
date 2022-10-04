using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private GameObject Camer;
    private Camera main_Camera;
    private CapsuleCollider col;
    private float _rotationX = 0f;
    private float _rotationY = 0f;
    private float MaxR = 90f;
    private float MinR = -90f;
    [SerializeField] private float speed = 5f;
    private Vector2 moveXZ = new Vector2(0, 0);
    [SerializeField] private float jumpForce = 10f;
    private float jumpForceSqrt = 5;
    [SerializeField] private float dragOnGround = 5;
    [SerializeField] private float dragInAir = 0;
    [SerializeField] private float gravityScale = 2.5f;
    private float jumpHeightTime = 0f;
    private float maxJumpHeightTime = 0.4f;
    [SerializeField] private GameObject TeleportBox;
    private SphereCollider TPBoxCollider;
    [SerializeField] private GameObject TeleportSphere;
    private bool teleportSwitch;
    public bool _teleportSwitch { get { return teleportSwitch; } }
    [SerializeField] private float distanceToTeleport = 10;
    private bool inputKey;
    private GameObject TPBox;
    private Vector3 Ray_start_pos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    private float StrafeSpeed = 100;
    private int chargeStrafe = 2;
    private int maxChargeStrafe = 2;
    [SerializeField] private int chargeTeleport = 2;
    [SerializeField] private int maxChargeTeleport = 2;
    private bool oneT = true;
    private bool oneS = true;
    [SerializeField] private float bullet_Force = 100;
    [SerializeField] private float teleportSpeed = 50;
    private bool tMove = false;
    private Vector3 PosToTeleport;
    private bool climbUp = false;
    public bool _climbUp { get { return climbUp; } set { climbUp = value; } }
    private bool climbMiddle = false;
    public bool _climbMiddle { get { return climbMiddle; } set { climbMiddle = value; } }
    private bool moveUp = false;
    [SerializeField] private Transform ClimbUpPoint;
    private Vector3 copyClimbUpPoint;
    private Vector3 teleportOffset;
    [SerializeField] private ParticleSystem fireParticleSystem;
    [SerializeField] private Transform Gun;
    private bool shoot;
    public bool _shoot { get { return shoot; } set { shoot = value; } }
    private Vector3 Slidespeed;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
        col = gameObject.GetComponent<CapsuleCollider>();
        main_Camera = gameObject.transform.GetChild(0).gameObject.GetComponent<Camera>();
        TPBoxCollider = TeleportSphere.GetComponent<SphereCollider>();
        fireParticleSystem.Stop();
    }
    void Update()
    {
        MouseLook();
        Shot();
        Teleport();
        Climb();
        Strafe();
    }
    void FixedUpdate()
    {
        Jump();
        MoveLogick();
    }
    private void Jump()
    {
        var JumpInput = Input.GetButton("Jump");
        var JumpReleased = Input.GetButtonUp("Jump");

        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);

        if (Grounded() && JumpInput)
        {
            rb.drag = dragInAir;
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
        else if (JumpInput && jumpHeightTime < maxJumpHeightTime)
        {
            jumpForceSqrt = Mathf.Sqrt(jumpForceSqrt);
            jumpHeightTime += Time.deltaTime;
            rb.AddForce(new Vector3(0, jumpForceSqrt, 0), ForceMode.Impulse);
        }
        else if (Grounded())
        {

            jumpForceSqrt = jumpForce;
            jumpHeightTime = 0;
        }
    }
    private void MoveLogick()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 move = new Vector3(deltaX, 0, deltaZ);
        move = transform.TransformDirection(move);
        rb.AddForce(move, ForceMode.Impulse);

        moveXZ.x = rb.velocity.x;
        moveXZ.y = rb.velocity.z;
        moveXZ = Vector2.ClampMagnitude(moveXZ, speed);
        rb.velocity = new Vector3(moveXZ.x, rb.velocity.y, moveXZ.y);

        if (Grounded())
        {
            rb.drag = dragOnGround;
        }
        else
        {
            rb.drag = dragInAir;
        }
    }
    private void Strafe()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && chargeStrafe > 0)
        {
            Debug.Log("strafe");
            var velocityVec = Vector3.ClampMagnitude(new Vector3(rb.velocity.x * StrafeSpeed,
            rb.velocity.y, rb.velocity.z * StrafeSpeed), StrafeSpeed);
            rb.AddForce(velocityVec, ForceMode.Impulse);

            chargeStrafe--;
            StartCoroutine(TimeStrafe());
            if (oneS == true)
            {
                oneS = false;
                StartCoroutine(ReloadStrafe());
            }
        }

    }
    private void MouseLook()
    {
        _rotationX += Input.GetAxis("Mouse X");
        _rotationY -= Input.GetAxis("Mouse Y");
        _rotationY = Mathf.Clamp(_rotationY, MinR, MaxR);
        transform.localEulerAngles = new Vector3(0, _rotationX, 0);
        Camer.transform.localEulerAngles = new Vector3(_rotationY, 0, 0);
    }
    private void Teleport()
    {
        int playerIndexLayer = LayerMask.NameToLayer("Player");
        if (playerIndexLayer == -1)
        {
            Debug.LogError("Layer Does not exist");
        }
        else
        {
            int layerMask = (1 << playerIndexLayer);
            layerMask = ~layerMask;
            Ray ray = main_Camera.ScreenPointToRay(Ray_start_pos);
            RaycastHit hit;
            var InputMouseButton1 = Input.GetMouseButton(1);

            //Cast Teleport Mark
            if (!(Input.GetKey(KeyCode.F)) && Input.GetMouseButton(1) && chargeTeleport > 0)
            {
                teleportSwitch = true;
                TeleportSphere.SetActive(true);
                var StatusRay = Physics.Raycast(ray, out hit, distanceToTeleport, layerMask);

                // Time scale
                if (InputKey() == false)
                {
                    Time.timeScale = 1.0f;
                    rb.isKinematic = true;
                }
                else
                {
                    Time.timeScale = 1f;
                    rb.isKinematic = false;
                }

                if (StatusRay && hit.distance <= distanceToTeleport)
                {
                    if (TPBox == null)
                    {
                        TPBox = Instantiate(TeleportBox,
                        new Vector3(hit.point.x, hit.point.y - 0.01f, hit.point.z),
                        Quaternion.identity, TeleportSphere.transform);
                    }
                    // else if (hit.collider != TPBoxCollider)
                    // {
                    TPBox.transform.position = Vector3.MoveTowards(TPBox.transform.position,
                    new Vector3(hit.point.x, hit.point.y - 0.01f, hit.point.z), Time.unscaledDeltaTime * teleportSpeed);
                    // }
                    // else
                    // {
                    // TPBox.transform.position = Vector3.MoveTowards(TPBox.transform.position,
                    // new Vector3(hit.point.x, hit.point.y - 0.01f, hit.point.z), Time.deltaTime * teleportSpeed);
                    //}
                }
                TPBox.transform.rotation = TeleportSphere.transform.localRotation;
            }
            //End cast with teleport
            if (Input.GetMouseButtonUp(1))
            {
                TeleportSphere.SetActive(false);
                teleportSwitch = false;
                rb.isKinematic = false;

                Time.timeScale = 1f;
                if (TPBox != null)
                {
                    chargeTeleport--;
                    if (oneT == true)
                    {
                        oneT = false;
                        StartCoroutine(ReloadTeleport());
                    }
                    Debug.Log(chargeTeleport);
                    PosToTeleport = TPBox.transform.position;
                    PosToTeleport.y = PosToTeleport.y + 1;
                    // transform.position = PosToTeleport;
                    tMove = true;

                    Destroy(TPBox);
                }
            }
            //move to teleport( blink )
            if (tMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, PosToTeleport, 100 * Time.deltaTime);
                rb.isKinematic = true;
                if (transform.position == PosToTeleport)
                {
                    rb.isKinematic = false;
                    tMove = false;
                }
            }
            //End cast with out teleport
            if (Input.GetKeyDown(KeyCode.F))
            {
                TeleportSphere.SetActive(false);
                teleportSwitch = false;
                rb.isKinematic = false;
                Time.timeScale = 1f;
                InputMouseButton1 = false;

                if (TPBox != null)
                {
                    Destroy(TPBox);
                }
            }
        }
    }
    private void Shot()
    {
        //layerMask
        int playerIndexLayer = LayerMask.NameToLayer("Player");
        int teleportSphereIndexLayer = LayerMask.NameToLayer("TeleportSphere");
        if (playerIndexLayer == -1 || teleportSphereIndexLayer == -1)
        {
            Debug.LogError("Layer Does not exist");
        }
        else
        {
            int layerMask = (1 << playerIndexLayer) | (1 << teleportSphereIndexLayer);
            layerMask = ~layerMask;
            //mech
            if (Input.GetMouseButtonDown(0) && shoot == false)
            {
                shoot = true;
                fireParticleSystem.Play();
                Ray ray = main_Camera.ScreenPointToRay(Ray_start_pos);
                RaycastHit hit;
                Physics.SphereCast(ray, 1, out hit, 30, layerMask);
                RaycastHit hitRay;
                Physics.Raycast(ray, out hitRay, 30, layerMask);

                //Physics.Raycast(ray, out hit, 100, layerMask); //100 - distance shot
                if (hit.transform != null || hitRay.transform != null)
                {
                    ArrayList list = new ArrayList() { ray.direction, bullet_Force, hit.collider };
                    if (hit.collider.tag == "enemy")
                    {
                        hit.transform.root.SendMessage("RagDollOn");
                        hit.transform.root.SendMessage("AddForceToBody", list);
                    }
                    if (hitRay.collider.tag == "enemy")
                    {
                        hitRay.transform.root.SendMessage("RagDollOn");
                        hitRay.transform.root.SendMessage("AddForceToBody", list);
                    }
                    if (hit.collider.tag == "item")
                    {
                        hit.rigidbody.AddForce(ray.direction * bullet_Force, ForceMode.Impulse);
                    }
                    if (hitRay.collider.tag == "item")
                    {
                        hitRay.rigidbody.AddForce(ray.direction * bullet_Force, ForceMode.Impulse);
                    }

                }
            }


        }

    }
    private bool Grounded()
    {
        RaycastHit hit;
        return Physics.SphereCast(
        col.bounds.center,
        col.radius - 0.1f,
        Vector3.down,
        out hit,
        col.height / 2 - 0.1f);
    }
    private bool InputKey()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || Input.GetButton("Jump"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Climb()
    {

        if (climbUp == false && climbMiddle == true &&
        Input.GetButton("Jump") == true)
        {
            Debug.Log("Climb!");
            moveUp = true;
            rb.isKinematic = true;
            copyClimbUpPoint = ClimbUpPoint.position;
        }
        if (moveUp == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, copyClimbUpPoint, 10 * Time.deltaTime);
            if (transform.position == copyClimbUpPoint)
            {
                moveUp = false;
                rb.isKinematic = false;
            }
        }
    }

    IEnumerator ReloadTeleport()
    {
        yield return new WaitForSeconds(5f);
        if (chargeTeleport < maxChargeTeleport)
        {
            chargeTeleport++;
            StartCoroutine(ReloadTeleport());
        }
        else
        {
            oneT = true;
        }
        Debug.Log(chargeTeleport);


    }
    IEnumerator ReloadStrafe()
    {
        yield return new WaitForSeconds(5f);
        if (chargeStrafe < maxChargeStrafe)
        {
            chargeStrafe++;
            StartCoroutine(ReloadStrafe());
        }
        else
        {
            oneS = true;
        }
        Debug.Log(chargeStrafe);
    }
    IEnumerator TimeStrafe()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.15f);
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(0.15f);
        Time.timeScale = 1f;
    }

}
