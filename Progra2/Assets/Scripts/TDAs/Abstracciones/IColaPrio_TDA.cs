using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColaTD
{
    interface IColaPrio_TDA
    {
        // inicializa la estructura
        void InicializarCola();

        // ingresa un elemento a la estructura, ordenandolo por prioridad
        void AcolarPrioridad(cJugador jugador);

        // elimina el "primer" valor de la estructura (el proximo a salir, el de mayor prioridad) 
        void Desacolar();

        // indica si hay elementos en la estructura
        bool ColaVacia();

        // devuelve el "primer" valor de la estructura (el proximo a salir, el de mayor prioridad)
        cJugador Primero();

        // devuelve la prioridad del "primer" valor de la estructura (el proximo a salir, el de mayor prioridad)
        int Prioridad();
    }
}
