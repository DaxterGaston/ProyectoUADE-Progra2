using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Components

    [SerializeField] private Camera cam;
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private Transform crosshair;

    #endregion

    #region Variables

    private Vector3 scale;
    private Vector3 mousePoint;
    private float h;
    [SerializeField] private float crosshairDistance = 5f;
    private Vector3 crosshairPosition;

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        scale = transform.localScale;
    }

    private void Update()
    {
        crosshairPosition = crosshair.position;
        mousePoint = cam.ScreenToWorldPoint(Input.mousePosition);
        h = Input.GetAxisRaw("Horizontal");
        Crosshair();
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        animator.SetBool("Walking", rb.velocity.magnitude > 0.1f);
        animator.SetFloat("X", h);
        if (mousePoint.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }

        if (mousePoint.x < transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);
        }
       
        transform.localScale = scale;
    }

    private void Crosshair()
    {
        mousePoint.z = 0;
        var auxPoint = cam.WorldToScreenPoint(transform.position);
        auxPoint.z = 0;
        var playerPoint = cam.ScreenToWorldPoint(auxPoint);
        playerPoint.z = 0;
        var target = mousePoint;
        var diff = mousePoint - playerPoint;
        diff.z = 0;
        var mouseDistance = diff.magnitude;
        if (mouseDistance > crosshairDistance)
        {
            target = playerPoint + (diff.normalized * crosshairDistance);
        }

        target.z = crosshairPosition.z;
        crosshair.position = target;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, crosshairDistance);
    }
}
