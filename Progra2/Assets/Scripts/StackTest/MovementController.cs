using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float baseSpeedModifier = 5;
    [SerializeField] private Camera _camera;
    private float currentSpeedModifier;
    [SerializeField] private Rigidbody2D rb;
    private Animator animator;
    private float horizontal;
    private float vertical;
    private float diagonalLimiter = 0.7f;
    private float speedUpDuration = 0f;
    private bool isSpeedBuffed = false;
    private Vector2 _mouse;
    private Vector3 _scale;
    
    private void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeedModifier = baseSpeedModifier;
        _scale = new Vector3(1, 1, 1);
    }
    
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        _mouse = _camera.ScreenToWorldPoint(Input.mousePosition);

        
        if (isSpeedBuffed) // Timer para la duracion del aumento de velocidad
        {
            speedUpDuration -= Time.deltaTime;
            if (speedUpDuration <= 0)
            {
                isSpeedBuffed = false;
                currentSpeedModifier = baseSpeedModifier;
            }
        }
        UpdateAnimations();
    }
    
    private void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) 
        {
            // Limita el movimiento diagonal a un 70% de la velocidad 
            horizontal *= diagonalLimiter;
            vertical *= diagonalLimiter;
        } 
        rb.velocity = new Vector2(horizontal * currentSpeedModifier, vertical * currentSpeedModifier);
    }

    public void ChangeSpeedModifier(float bonusSpeed, float buffDuration) // Metodo que llama el item para cambiar la velocidad
    {
        currentSpeedModifier += bonusSpeed;
        speedUpDuration = buffDuration;
        isSpeedBuffed = true;
    }
    
    private void UpdateAnimations()
    {
        animator.SetBool("Walking", rb.velocity.magnitude > 0.1f);
        animator.SetFloat("X", horizontal);

        if (_mouse.x > transform.position.x)
            _scale.x = -1;
        else
            _scale.x = 1;
        transform.localScale = _scale;
    }
}
