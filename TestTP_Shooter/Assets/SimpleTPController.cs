using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTPController : MonoBehaviour
{

    private Rigidbody rb;
    private Animator animator;
    private float RotX;

    
    private float MouseX_Raw;
    private float MouseY_Raw;

    private float RawVertical;
    private float RawHorizontal;

    private Vector3 CamOrigin;

    [Header("Refs")]
    public GameObject CamAttachment;
    private Camera LeCam;
    public GameObject Spine;
    public GameObject CamHelperOrigin;
    public GameObject CamHelper;

    [Header("Movements")]
    public float MoveSpeed;

    [Header("Rotations")]
    public float TurnSpeed;
    public float RotAngle;
    public bool ShouldAlsoRotChar;
    public Vector3 SpineOffset;

    [Header("Camera Collision")]
    public LayerMask TriggLayerMask;
    public float Cam_Offset;

    [Header("Aiming Stuff")]
    private bool IsAiming;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        LeCam = Camera.main;

        RotX = CamAttachment.transform.localRotation.eulerAngles.x;
        CamOrigin = LeCam.transform.localPosition;
    }


    void Update()
    {
        MouseY_Raw = -Input.GetAxisRaw("Mouse Y");
        MouseX_Raw = Input.GetAxisRaw("Mouse X");

        RawVertical = Input.GetAxisRaw("Vertical");
        RawHorizontal = Input.GetAxisRaw("Horizontal");

        CharacterAndCamRotations();

        CharacterMovement();
        CharacterAnimation();

        CamWallCollisionHelper();

        Aiming();
    }

    private void LateUpdate()
    {
        if (IsAiming)
            Spine.transform.Rotate(SpineOffset);
    }

    private void Aiming()
    {
        IsAiming = Input.GetMouseButton(1);
        animator.SetBool("IsAiming", IsAiming);
    }

    private void CamWallCollisionHelper()
    {
        RaycastHit Hit;
        if(Physics.Linecast(CamHelperOrigin.transform.position,
            CamHelper.transform.position, out Hit, TriggLayerMask))
        {
            if(Hit.transform)
            {
                Vector3 Dir = CamHelperOrigin.transform.position - Hit.point;
                LeCam.transform.position = Hit.point + (Dir * Cam_Offset) + (Hit.normal * Cam_Offset);
            }
        }
        else
        {
            LeCam.transform.localPosition = CamOrigin;
        }
    }

    private void CharacterAndCamRotations()
    {
        RotX += MouseY_Raw * TurnSpeed;
        RotX = Mathf.Clamp(RotX, -RotAngle, RotAngle);
        Quaternion localRot = Quaternion.Euler(RotX, CamAttachment.transform.eulerAngles.y, 0f);
        CamAttachment.transform.rotation = localRot;

        Transform TransformToRot = null;
        if (ShouldAlsoRotChar)
        {
            TransformToRot = transform;
            Quaternion localrot2 = Quaternion.Euler(RotX, Spine.transform.eulerAngles.y, 0f);
            Spine.transform.rotation = localrot2;
        }
        else
        {
            TransformToRot = CamAttachment.transform;
        }

        TransformToRot.Rotate(new Vector3(
            0,
            TurnSpeed * MouseX_Raw,
            0), Space.Self);

    }

    private void CharacterAnimation()
    {
        animator.SetFloat("MoveSpeed", Mathf.Max(Mathf.Abs(RawVertical), Mathf.Abs(RawHorizontal)));
        animator.SetFloat("Horiz", RawHorizontal);
        animator.SetFloat("Vert", RawVertical);
    }

    private void CharacterMovement()
    {
        Vector3 MoveVec = Vector3.zero;

        MoveVec += transform.forward * RawVertical;
        MoveVec += transform.right * RawHorizontal;
        MoveVec = MoveVec.normalized * MoveSpeed;

        rb.velocity = new Vector3(MoveVec.x, rb.velocity.y, MoveVec.z);
    }
}
