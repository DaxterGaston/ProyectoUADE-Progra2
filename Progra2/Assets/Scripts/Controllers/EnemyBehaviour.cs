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
        
    }


    // Update is called once per frame
    void Update()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, DistanciaDeFueraDeVision);
        foreach (var item in colliders)
        {
            if (item.CompareTag("Player"))
            {
                if (Vector3.Distance(transform.position, item.transform.position) <= DistanciaDeFueraDeVision)
                {

                    if (Vector3.Distance(transform.position, item.transform.position) > DistanciaDeParado)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, item.transform.position, velocidad * Time.deltaTime);
                    }
                    else if (Vector3.Distance(transform.position, item.transform.position) <= DistanciaDeParado)
                    {
                        transform.position = this.transform.position;
                    }
                }

            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        
    }
}
