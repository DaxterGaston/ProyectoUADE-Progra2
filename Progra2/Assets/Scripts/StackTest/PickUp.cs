using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpawnController spawnController;

    public SpriteRenderer SpriteRenderer // Se llama externamente para saber el sprite que va a guardar el display
    {
        get => spriteRenderer;
        set => spriteRenderer = value;
    }

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        spawnController = FindObjectOfType(typeof(SpawnController)) as SpawnController;
    }

    public virtual void itemAction(){} // Reemplaza en sus hijos con la accion que va a realizar el item

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.GetComponent(typeof(SimplePlayer))) return;
        spawnController.ReduceSpawnedItemCount();
        transform.position = new Vector3(-1000,-1000);
    }
}
