using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryTracker : MonoBehaviour
{
    private Stack<PickUp> inventory = new Stack<PickUp>(); // Stack de powerups
    private float itemCooldown = 5f; // Cooldown entre cada uso de item
    private float currentItemCooldown; // Cooldown actual
    private bool isItemUsable = true; // Bool para saber si puedo usar el siguiente item

    public Image[] inventorySlots; // Array de imagenes para mostrar los items
    [SerializeField] private Sprite emptySlot;
    public bool IsItemUsable // Propiedad del bool de uso para agregarle funcionalidad
    {
        get => isItemUsable;
        set
        {
            isItemUsable = value;
            if (!isItemUsable)
            {
                foreach (var image in inventorySlots)
                {
                    image.color = Color.red;
                }
                return;
            }
            foreach (var image in inventorySlots)
            {
                image.color = Color.white;
            }
        }
    } 

    private void Start()
    {
        // Hace el primer update al display del inventario
        foreach (var image in inventorySlots)
        {
            if (image.sprite == null)
            {
                image.sprite = emptySlot;
            }
        } 
    }

    private void AddItemToStack(PickUp item) // Pide el item que va a agregar al stack
    {
        inventory.Push(item); // Agrega el item al stack
        DisplaySprites(); // Actualiza el display del inventario
    }

    private void UseItemInStack() // Pide el item que va a utilizar el jugador
    {
        var itemToUse = inventory.Pop(); // Guarda temporalmente el item que sale del stack
        itemToUse.ItemAction();// Activa el item que sale del stack
        Destroy(itemToUse);
        EmptySlotCheck();
        DisplaySprites(); // Actualiza el display del inventario
    }

    private void Update()
    {
        if (!IsItemUsable) // Pregunta si puedo utilizar el item
        {
            currentItemCooldown += Time.deltaTime; // Como no se puede utilizar empieza un contador
            if (currentItemCooldown >= itemCooldown) // Cuando el contador llega al Cooldown entre cada uso de item
            {
                currentItemCooldown = 0; // Reinicia el Cooldown entre cada uso de item a 0
                IsItemUsable = true; // Cambia el bool a verdadero para poder utilizar el siguiente item
            }
        }
        if (IsItemUsable && Input.GetKeyDown(KeyCode.Space)) // Pregunta si puedo utilizar el item Y si aprete la barra espaciadora
        {
            if (inventory.Count > 0) // Compruebo que el stack no este vacio
            {
                UseItemInStack(); // Pide el item que va a utilizar el jugador
                IsItemUsable = false; // Cambia el bool a falso para no poder utilizar el siguiente item
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) // Compruebo colision con otro objeto
    {
        if (!other.gameObject.GetComponent(typeof(PickUp))) return; // Compruebo si el otro objeto tiene un componente del tipo PickUp
        var itemToAdd = other.gameObject.GetComponent(typeof(PickUp)) as PickUp; // Guardo el componente PickUp en una variable 
        AddItemToStack(itemToAdd); // Pide el item que va a agregar al stack
    }
    
    private void DisplaySprites()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventory.Count > i && i < inventorySlots.Length)
            {
                inventorySlots[i].sprite = inventory.ToArray()[i].SpriteRenderer.sprite;
            }
        }
    }

    private void EmptySlotCheck()
    {
        if (inventory.Count < inventorySlots.Length)
        {
            for (int i = inventory.Count; i < inventorySlots.Length; i++)
            {
                inventorySlots[i].sprite = emptySlot;
            }
        }
    }
}