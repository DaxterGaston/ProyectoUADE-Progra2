using System;
using UnityEngine;

namespace ColaTD
{
    public class ColaPrio_TDA : IColaPrio_TDA
    {

        GameObject[] jugadores;
        int indice;

        public void InicializarCola()
        {
            jugadores = new GameObject[50];
            indice = 0;
        }
        public void AcolarPrioridad(GameObject jugador)
        {
            //int j;
            //// al ingresar cada elemento se ingresa en el orden de acuerdo a su prioridad
            //for (j = indice; j > 0 && jugadores[j - 1].puntaje >= jugador.puntaje; j--)
            //{
            //    jugadores[j] = jugadores[j - 1];
            //}
            //jugadores[j] = jugador;

            //indice++;
            throw new NotImplementedException("Implementar dependiendo del uso correspondiente.");
        }

        public void Desacolar()
        {
            indice--;
        }

        public bool ColaVacia()
        {
            return (indice == 0);
        }

        public GameObject Primero()
        {
            return jugadores[indice-1];
        }

        public int Prioridad()
        {
            //return jugadores[indice-1].puntaje;
            throw new NotImplementedException("Implementar dependiendo del uso correspondiente.");
        }
    }
}
