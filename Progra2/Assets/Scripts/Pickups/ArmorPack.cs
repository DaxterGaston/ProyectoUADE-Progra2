using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPack : PickUp
{
    [SerializeField] private int repairAmount = 1; // Cantidad de puntos que restaura el item
    private Armor targetArmor;

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (!other.gameObject.GetComponent<Armor>()) return; // Comprueba si el otro objeto tiene controlador de vida
        targetArmor = other.gameObject.GetComponent<Armor>(); // Guarda la referencia del controlador de vida
    }

    public override void ItemAction() // Accion que llama el jugador
    {
        targetArmor.ArmorUp(repairAmount); // Accion que realiza este item en particular
    }
}
