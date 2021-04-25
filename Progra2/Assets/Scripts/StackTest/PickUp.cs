using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public SpriteRenderer SpriteRenderer // Se llama externamente para saber el sprite que va a guardar el display
    {
        get => spriteRenderer;
        set => spriteRenderer = value;
    }

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void itemAction(){} // Reemplaza en sus hijos con la accion que va a realizar el item
}
