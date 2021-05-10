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

    public Transform jugador;
    public GameObject bala;

    // Start is called before the first frame update
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        tiempoDeDisparo = tiempoInicialDeDisparo;
        
    }
    

    // Update is called once per frame
    void Update()
    {
        tiempoDeDisparo -= Time.deltaTime;
        if (Vector3.Distance(transform.position, jugador.position) <= DistanciaDeFueraDeVision)
        {

            if (Vector3.Distance(transform.position, jugador.position) > DistanciaDeParado)
            {
                transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
            }
            else if (Vector3.Distance(transform.position, jugador.position) <= DistanciaDeParado)
            {
                transform.position = this.transform.position;
                if (tiempoDeDisparo <= 0)
                {
                    Instantiate(bala, transform.position, Quaternion.identity);
                    tiempoDeDisparo = tiempoInicialDeDisparo;
                }
            }
        }
        
    }
}
