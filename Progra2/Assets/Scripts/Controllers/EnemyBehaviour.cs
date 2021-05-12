using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float velocidad;
    public float DistanciaDeParado;
    public float DistanciaDeFueraDeVision;

    public float tiempoDeDisparo;
    public float tiempoInicialDeDisparo;
    Collider2D[] colliders;
    public GameObject bala;

    // Start is called before the first frame update
    void Start()
    {
        tiempoDeDisparo = tiempoInicialDeDisparo;
        Physics2D.queriesStartInColliders = false;
        
    }


    // Update is called once per frame
    void Update()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, DistanciaDeFueraDeVision);
        foreach (var item in colliders)
        {

            if (item.CompareTag("Player"))
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, item.transform.position, DistanciaDeFueraDeVision, 10);
                Debug.DrawRay((Vector2)transform.position, (Vector2)item.transform.position, Color.magenta);
                if (hitInfo)
                {
                    if (Vector2.Distance(transform.position, item.transform.position) <= DistanciaDeFueraDeVision) { }
                    {
                        if (Vector2.Distance(transform.position, item.transform.position) > DistanciaDeParado)
                        {

                            if (hitInfo.collider.CompareTag("Player"))
                                transform.position = Vector2.MoveTowards(transform.position, item.transform.position, velocidad * Time.deltaTime);

                        }
                        else if (Vector2.Distance(transform.position, item.transform.position) <= DistanciaDeParado)
                        {

                            if (hitInfo.collider.CompareTag("Player"))
                                transform.position = transform.position;

                        }
                    }
                }

            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        
    }
}
