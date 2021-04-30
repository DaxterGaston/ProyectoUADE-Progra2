using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float baseSpeedModifier = 5;
    private float currentSpeedModifier;
    private Rigidbody2D rb;
    private float horizontal;
    private float vertical;
    private float diagonalLimiter = 0.7f;
    private float speedUpDuration = 0f;
    private bool isSpeedBuffed = false;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeedModifier = baseSpeedModifier;
    }
    
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        
        if (isSpeedBuffed) // Timer para la duracion del aumento de velocidad
        {
            speedUpDuration -= Time.deltaTime;
            if (speedUpDuration <= 0)
            {
                isSpeedBuffed = false;
                currentSpeedModifier = baseSpeedModifier;
            }
        }
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
}
