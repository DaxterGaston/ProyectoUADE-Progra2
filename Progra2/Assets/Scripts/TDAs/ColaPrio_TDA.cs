using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColaTD
{


    class ColaPrio_TDA : IColaPrio_TDA
    {

        cJugador[] jugadores;
        int indice;

        public void InicializarCola()
        {
            jugadores = new cJugador[50];
            indice = 0;
        }
        public void AcolarPrioridad(cJugador jugador)
        {
            int j;
            // al ingresar cada elemento se ingresa en el orden de acuerdo a su prioridad
            for (j = indice; j > 0 && jugadores[j - 1].puntaje >= jugador.puntaje; j--)
            {
                jugadores[j] = jugadores[j - 1];
            }
            jugadores[j] = jugador;

            indice++;
        }

        public void Desacolar()
        {
            indice--;
        }

        public bool ColaVacia()
        {
            return (indice == 0);
        }

        public cJugador Primero()
        {
            return jugadores[indice-1];
        }

        public int Prioridad()
        {
            return jugadores[indice-1].puntaje;
        }
    }
}
