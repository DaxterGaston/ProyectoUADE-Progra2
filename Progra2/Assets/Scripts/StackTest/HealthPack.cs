using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : PickUp
{
    [SerializeField] private int healingAmount = 1; // Cantidad de puntos que restaura el item
    private LifeController targetLife;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.GetComponent(typeof(LifeController))) return; // Comprueba si el otro objeto tiene controlador de vida
        targetLife = other.gameObject.GetComponent(typeof(LifeController)) as LifeController; // Guarda la referencia del controlador de vida
    }

    public override void itemAction() // Accion que llama el jugador
    {
        targetLife.HealDamage(healingAmount); // Accion que realiza este item en particular
    }
}
