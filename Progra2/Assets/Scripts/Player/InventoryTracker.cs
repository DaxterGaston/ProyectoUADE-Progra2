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
    public bool IsItemUsable // Propiedad del bool de uso para agregarle funcionalidad
    {
        get => isItemUsable;
        set
        {
            isItemUsable = value;
            if (!isItemUsable)
            {
                 print("No"); // Se puede cambiar por algun sonido o visualmente para saber que no puedo usar un item
                 return;
            }
            print("Yes"); // Se puede cambiar por algun sonido o visualmente para saber que si puedo usar un item
        }
    } 

    private void Start()
    {
        DisplaySprite(); // Hace el primer update al display del inventario
    }

    private void AddItemToStack(PickUp item) // Pide el item que va a agregar al stack
    {
        inventory.Push(item); // Agrega el item al stack
        DisplaySprite(); // Actualiza el display del inventario
    }

    private void UseItemInStack() // Pide el item que va a utilizar el jugador
    {
        var itemToUse = inventory.Pop(); // Guarda temporalmente el item que sale del stack
        itemToUse.itemAction();// Activa el item que sale del stack
        DisplaySprite(); // Actualiza el display del inventario
        Destroy(itemToUse);
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

    private void DisplaySprite() // Actualiza el display del inventario
    {
        if (inventory.Count < inventorySlots.Length) // Se fija si va a haber espacios vacios
        {
            for (int i = 0; i < inventory.Count; i++) // Cambia cada espacio no vacio por el sprite correspondiente
            {
                inventorySlots[i].sprite = inventory.ToArray()[i].SpriteRenderer.sprite;
                inventorySlots[i].enabled = true;
            }

            for (int j = inventory.Count; j < inventorySlots.Length; j++) // Cambia cada espacio vacio a que no se muestre
            {
                inventorySlots[j].sprite = null;
                inventorySlots[j].enabled = false;
            }
        }
        if (inventory.Count >= inventorySlots.Length) // Continua actualizando cuando el stack es mayor al display
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlots[i].sprite = inventory.ToArray()[i].SpriteRenderer.sprite;
            }
        }
    }
}