using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonPlayerControllerWithCentreOfGravity : MonoBehaviour
{
    public GameObject CentreOfGravity;
    public CenterOfGravity CenterOfGravity;
    public GameObject Parent;

    public Vector3 xzUnadjustedVelocity;

    public float CurrentRelativeYVelo;

    public bool UseRealGravity;
    public float RealisticGravityJumpForce = 5.0f;
    public float UnrealisticGravityJumpForce = 5.0f;

    public float UnrealisticGravityAscendingGravity = 14.1f;
    public float UnrealisticGravityDescendingGravity = 28.2f;

    public float MaxWalkSpeed = 5.0f;
    public float MaxRunSpeed = 9.0f;
    public float MaxCrouchSpeed = 2.5f;

    public float AccelerationWalk = 48.0f;
    public float AccelerationRun = 96.0f;

    public Camera Camera;

    public CapsuleCollider StandingCapsuleCollider;
    public CapsuleCollider CrouchingCapsuleCollider;

    public Rigidbody Rigidbody;

    public Vector3 CrouchingCameraLocalPosition = Vector3.zero;

    private float yRot;
    private Vector3 movement;

    private bool IsOnAir;

    private bool SpaceWasPressedDuringLastUpdate;

    // Start is called before the first frame update
    private float groundedExtraTime;

    private bool isRunning;
    private bool isCrouchingButtonDown;

    private float jumpTimer;

    private bool isCrouching;

    private Vector3 cameraOriginalLocalPosition;

    private float originalTimeStep;

    private FirstPersonPlayerControls Controls;
    void Awake()
    {

        Controls = GetComponent<FirstPersonPlayerControls>();
        originalTimeStep = Time.fixedDeltaTime;
        Time.fixedDeltaTime = 0.00833333f;

        CenterOfGravity = FindObjectOfType<CenterOfGravity>();
        UseRealGravity = false;

        if (UseRealGravity)
        {
            Rigidbody.useGravity = true;
        }

        else
        {
            Rigidbody.useGravity = false;
        }

        cameraOriginalLocalPosition = Camera.transform.localPosition;
        yRot = 35.0f;
        Camera.transform.localRotation = Quaternion.Euler(yRot, 0, 0);
    }

    private void OnDestroy()
    {
        Time.fixedDeltaTime = originalTimeStep;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsPaused)
        {
            return;
        }

        //Vector2 mouseInput = ReactToInputWithOldInputSystem();
        Vector2 mouseInput = ReactToInputWithNewInputSystem();

        float xRot;

        MoveHead(mouseInput, out xRot);



        if (isCrouching)
        {
            Camera.transform.localPosition = Vector3.Lerp(Camera.transform.localPosition,
                                                          CrouchingCameraLocalPosition,
                                                          9f * Time.deltaTime);
        }

        else
        {
            Camera.transform.localPosition = Vector3.Lerp(Camera.transform.localPosition,
                                                          cameraOriginalLocalPosition,
                                                          12f * Time.deltaTime);
        }

        //MoveBody(movement);
    }

    private Vector2 ReactToInputWithNewInputSystem()
    {
        movement = new Vector3(Controls.Horizontal,
                               0, Controls.Vertical);

        Vector2 mouseInput = new Vector2(Controls.MouseDeltaX * 0.025f,
                                         Controls.MouseDeltaY * 0.025f);

        if (Controls.RunDown)
        {
            isRunning = true;
        }

        else
        {
            isRunning = false;
        }

        if (Controls.CrouchDown)
        {
            isCrouchingButtonDown = true;

            //Debug.Log("Crouching " + Time.time);
        }

        else
        {
            isCrouchingButtonDown = false;

            //Debug.Log("Not crouhing " + Time.time);
        }

        if (Controls.JumpDownPressed)
        {
            SpaceWasPressedDuringLastUpdate = true;
        }

        return mouseInput;
    }

    private Vector2 ReactToInputWithOldInputSystem()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"),
                               0, Input.GetAxisRaw("Vertical"));

        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"),
                                         Input.GetAxisRaw("Mouse Y"));

        if (Input.GetButton("Run"))
        {
            isRunning = true;
        }

        else
        {
            isRunning = false;
        }

        if (Input.GetButton("Crouch"))
        {
            isCrouchingButtonDown = true;

            //Debug.Log("Crouching " + Time.time);
        }

        else
        {
            isCrouchingButtonDown = false;

            //Debug.Log("Not crouhing " + Time.time);
        }

        if (Input.GetButtonDown("Jump"))
        {
            SpaceWasPressedDuringLastUpdate = true;
        }

        return mouseInput;
    }

    private void FixedUpdate()
    {
        MoveBody(movement);
    }

    private void MoveHead(Vector2 mouseInput, out float xRot)
    {
        float mouseSensitivity = Options.MouseSensitivity;
        //Debug.Log("Mouse sensitivity is " + mouseSensitivity);
        xRot = mouseInput.x;
        float yRotation = mouseInput.y;

        if (!Options.InvertMouse)
        {
            yRotation = -yRotation;
        }

        float newYRot = yRot + yRotation * mouseSensitivity;

        yRot = Mathf.Lerp(yRot, newYRot, 0.9f);

        if (yRot >= 89.999f)
        {
            yRot = 89.999f;
        }

        else if (yRot <= -89.999f)
        {
            yRot = -89.999f;
        }

        Camera.transform.localRotation = Quaternion.Euler(yRot, 0, 0);
        Quaternion previousRot = Parent.transform.localRotation;
        Parent.transform.Rotate(0, xRot * mouseSensitivity, 0, Space.Self);
        Parent.transform.localRotation = Quaternion.Slerp(previousRot, Parent.transform.localRotation, 0.9f);
    }

    private void MoveBody(Vector3 movement)
    {
        Vector3 towardsCenterOfGravity = CenterOfGravity.gameObject.transform.position - transform.position;

        groundedExtraTime -= Time.deltaTime;

        bool isGrounded = false;
        bool hits = false;
        RaycastHit hit;

        Vector3 point1 = StandingCapsuleCollider.transform.position + Rigidbody.transform.up * (StandingCapsuleCollider.height / 2);
        Vector3 point2 = StandingCapsuleCollider.transform.position + -Rigidbody.transform.up * (StandingCapsuleCollider.height / 2);

        hits = Physics.Raycast(Rigidbody.transform.position,
                               -Rigidbody.transform.up,
                               out hit,
                               10.0f);



        if (hits && hit.distance <= 1.001f)
        {
            isGrounded = true;
            groundedExtraTime = 0.4f;
        }

        else
        {
            isGrounded = false;
        }



        bool isOnASlope = false;
        Vector3 slopeNormal = -towardsCenterOfGravity;

        if (Physics.Raycast(Rigidbody.transform.position,
                            -Rigidbody.transform.up,
                            out hit,
                            StandingCapsuleCollider.height / 2 + 0.1f))
        {
            float angle = Vector3.Angle(hit.normal, Rigidbody.transform.up);

            if (angle <= 89.9f
                && angle != 0.0f)
            {
                isOnASlope = true;
                groundedExtraTime = 0.4f;
                slopeNormal = hit.normal;
            }
        }


        if (groundedExtraTime >= 0.0f)
        {
            isGrounded = true;
        }

        if (isOnASlope)
        {
            isGrounded = true;
            Rigidbody.useGravity = false;
        }

        else
        {
            if (UseRealGravity)
            {
                Rigidbody.useGravity = true;
            }
        }

        jumpTimer -= Time.deltaTime;


        // Check if can jump
        bool tryingToStandUp = false;

        if (isCrouchingButtonDown
            && isGrounded
            && CurrentRelativeYVelo <= 0)
        {
            isCrouching = true;
            CrouchingCapsuleCollider.enabled = true;
            StandingCapsuleCollider.enabled = false;
        }

        else
        {
            if (isCrouching)
            {
                tryingToStandUp = true;
            }
        }

        bool canStandUp = true;

        bool hitCeiling = Physics.Raycast(transform.position,
                                          Rigidbody.transform.up,
                                          StandingCapsuleCollider.height / 2 + StandingCapsuleCollider.radius);

        if (hitCeiling)
        {
            canStandUp = false;
        }

        if (tryingToStandUp
            && canStandUp)
        {
            isCrouching = false;
            CrouchingCapsuleCollider.enabled = false;
            StandingCapsuleCollider.enabled = true;
            //Debug.Log("Trying to stand up ");
        }

        if (jumpTimer >= 0
            || CurrentRelativeYVelo > 0)
        {
            isGrounded = false;
        }


        if (isGrounded && !SpaceWasPressedDuringLastUpdate)
        {
            Rigidbody.velocity = towardsCenterOfGravity.normalized * 200.0f * Time.deltaTime;
        }

        Vector3 forward = Rigidbody.transform.forward;

        if (isGrounded
            && SpaceWasPressedDuringLastUpdate
            && jumpTimer <= 0)
        {
            groundedExtraTime = 0;

            if (!UseRealGravity)
            {
                Rigidbody.velocity += UnrealisticGravityJumpForce * -towardsCenterOfGravity.normalized;
                CurrentRelativeYVelo = UnrealisticGravityJumpForce;
            }

            else
            {
                //Rigidbody.AddForce(Vector3.up * RealisticGravityJumpForce, ForceMode.Impulse);
            }

            jumpTimer = 0.2f;
        }

        else
        {
            if (!isGrounded
                && !UseRealGravity)
            {
                if (CurrentRelativeYVelo >= 0)
                {
                    Rigidbody.velocity = -towardsCenterOfGravity.normalized * (CurrentRelativeYVelo - UnrealisticGravityAscendingGravity * Time.deltaTime * CenterOfGravity.GravityMultiplier);

                    CurrentRelativeYVelo -= UnrealisticGravityAscendingGravity * Time.deltaTime * CenterOfGravity.GravityMultiplier;

                }

                else
                {
                    Rigidbody.velocity = -towardsCenterOfGravity.normalized * (CurrentRelativeYVelo - UnrealisticGravityDescendingGravity * Time.deltaTime * CenterOfGravity.GravityMultiplier);

                    CurrentRelativeYVelo -= UnrealisticGravityDescendingGravity * Time.deltaTime * CenterOfGravity.GravityMultiplier;
                }
            }
        }

        if (isOnASlope
            && jumpTimer <= 0)
        {

        }

        Vector3 clampedVelo = DoHorizontalMovements(movement, 
                                                    isGrounded,
                                                    isOnASlope,
                                                    towardsCenterOfGravity,
                                                    slopeNormal);


        Vector3 velo = clampedVelo;



        SpaceWasPressedDuringLastUpdate = false;
        // How about air control?

        Vector3 fwd = Parent.transform.forward;
        Vector3 up = -towardsCenterOfGravity.normalized;

        Rigidbody.transform.rotation = Quaternion.FromToRotation(Rigidbody.transform.up, up) * Rigidbody.transform.rotation;


    }

    private Vector3 DoHorizontalMovements(Vector3 movement, 
                                          bool isGrounded,
                                          bool isOnASlope,
                                          Vector3 towardsCenterOfGravity,
                                          Vector3 slopeNormal)
    {
        Vector3 moveForward = Parent.transform.forward * movement.z;
        Vector3 moveSideWays = Parent.transform.right * movement.x;
        Vector3 move = moveForward + moveSideWays;

        if (isGrounded)
        {
            if (!isRunning)
            {
                move *= AccelerationWalk * Time.deltaTime;
            }

            else
            {
                move *= AccelerationRun * Time.deltaTime;
            }
        }

        else
        {
            move *= 0.2f;
        }

        Vector3 xzVelo = xzUnadjustedVelocity;
        Vector3 clampedVelo;

        if (!isRunning
            && !isCrouching)
        {
            clampedVelo = Vector3.ClampMagnitude(xzVelo + move, MaxWalkSpeed);
        }

        else if (isCrouching)
        {
            clampedVelo = Vector3.ClampMagnitude(xzVelo + move, MaxCrouchSpeed);
            //Debug.Log("Should clamp to max crouch speed");
        }

        else
        {
            clampedVelo = Vector3.ClampMagnitude(xzVelo + move, MaxRunSpeed);
        }

        if (move.magnitude <= 0.1f)
        {
            clampedVelo *= 0.931f;
        }

        //Vector3 velo = clampedVelo;

        //if (isOnASlope)
        //{
        //    Quaternion slopeRotation = Quaternion.FromToRotation(-towardsCenterOfGravity.normalized, slopeNormal);
        //    velo = slopeRotation * velo;
        //    velo = Vector3.ClampMagnitude(velo, MaxRunSpeed);
        //    clampedVelo = velo;
        //}

        xzUnadjustedVelocity = clampedVelo;
        Rigidbody.velocity += xzUnadjustedVelocity;
        return clampedVelo;
    }
}
