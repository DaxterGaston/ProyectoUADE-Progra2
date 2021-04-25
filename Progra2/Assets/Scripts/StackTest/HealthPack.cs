using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : PickUp
{
    public override void itemAction() // Accion que llama el jugador
    {
        print("health"); // Accion que realiza este item en particular
    }
}
