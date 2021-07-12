using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : PickUp
{
    [SerializeField] private int healingAmount = 1; // Cantidad de puntos que restaura el item
    private Health targetLife;

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (!other.gameObject.GetComponent<Health>()) return; // Comprueba si el otro objeto tiene controlador de vida
        targetLife = other.gameObject.GetComponent<Health>(); // Guarda la referencia del controlador de vida
    }

    public override void ItemAction() // Accion que llama el jugador
    {
        targetLife.Heal(healingAmount); // Accion que realiza este item en particular
    }
}
