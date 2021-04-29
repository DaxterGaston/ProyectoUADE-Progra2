using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPack : PickUp
{
    [SerializeField] private float speedAmount = 1; // Cantidad de puntos que aumenta el item
    [SerializeField] private float speedDuration = 1; // Duracion del buff 
    private MovementController target;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.GetComponent(typeof(MovementController))) return; // Comprueba si el otro objeto tiene controlador de vida
        target = other.gameObject.GetComponent(typeof(MovementController)) as MovementController; // Guarda la referencia del controlador de vida
    }

    public override void itemAction() // Accion que llama el jugador
    {
        target.ChangeSpeedModifier(speedAmount, speedDuration); // Accion que realiza este item en particular
    }
}
