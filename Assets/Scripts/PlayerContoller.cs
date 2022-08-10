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
    [SerializeField] private float gravityScale = 2.5f;
    private float jumpHeightTime = 0f;
    private float maxJumpHeightTime = 0.4f;
    [SerializeField] private GameObject TeleportBox;
    [SerializeField] private GameObject TeleportSphere;
    [SerializeField] public bool teleportSwitch;

    public bool _teleportSwitch
    {
        get;
    }
    private float distanceToTeleport = 10;
    private bool inputKey;
    private GameObject TPBox;
    private Vector3 Ray_start_pos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    [SerializeField] private GameObject bullet_prefab;
    [SerializeField] private float StrafeSpeed = 100;
    [SerializeField] private int chargeStrafe = 2;
    [SerializeField] private int maxChargeStrafe = 2;
    [SerializeField] private int chargeTeleport = 2;
    [SerializeField] private int maxChargeTeleport = 2;
    private bool oneT = true;
    private bool oneS = true;



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
    }
    void Update()
    {
        MouseLook();
        Jump();
        MoveLogick();
        Teleport();
        Shot();
        Strafe();
    }
    private void Jump()
    {
        var JumpInput = Input.GetButton("Jump");
        var JumpReleased = Input.GetButtonUp("Jump");

        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);

        if (Grounded() && JumpInput)
        {
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
        rb.AddForce(move, ForceMode.VelocityChange);

        moveXZ.x = rb.velocity.x;
        moveXZ.y = rb.velocity.z;
        moveXZ = Vector2.ClampMagnitude(moveXZ, speed);
        rb.velocity = new Vector3(moveXZ.x, rb.velocity.y, moveXZ.y);
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
        Ray ray = main_Camera.ScreenPointToRay(Ray_start_pos);
        RaycastHit hit;
        Vector3 posToTp = new Vector3(Camer.transform.position.x,
        Camer.transform.position.y, Camer.transform.position.z + 10);
        TeleportSphere.transform.localEulerAngles = new Vector3(_rotationY, 0, 0);
        var InputMouseButton1 = Input.GetMouseButton(1);

        if (!(Input.GetKey(KeyCode.F)) && Input.GetMouseButton(1) && chargeTeleport > 0)
        {
            teleportSwitch = true;
            TeleportSphere.SetActive(true);
            Physics.Raycast(ray, out hit);

            if (InputKey() == false)
            {
                Time.timeScale = 0f;
                rb.isKinematic = true;
            }
            else
            {
                Time.timeScale = 1f;
                rb.isKinematic = false;
            }

            if (hit.distance <= distanceToTeleport)
            {
                if (TPBox == null)
                {
                    TPBox = Instantiate(TeleportBox, hit.point, Quaternion.identity);
                }
                else if (hit.collider != TPBox.GetComponent<Collider>())
                {
                    TPBox.transform.position = hit.point;
                }
            }
            else
            {
                //TPBox = Instantiate(TeleportBox, TeleportSphere.transform.position, Quaternion.identity);
                TPBox.transform.position = TeleportSphere.transform.position;
            }
            if (TPBox.transform.position == Vector3.zero)
            {
                TPBox.transform.position = TeleportSphere.transform.position;

            }
        }

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
                var PosToTeleport = TPBox.transform.position;
                PosToTeleport.y = PosToTeleport.y + 1;
                transform.position = PosToTeleport;

                Destroy(TPBox);
            }


        }

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
    private void Shot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var bullet = Instantiate(bullet_prefab, Camer.transform.position, Camer.transform.rotation);
        }
    }
    private bool Grounded()
    {
        RaycastHit hit;
        return Physics.SphereCast(
        col.bounds.center,
        col.radius,
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


}
