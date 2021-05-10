using System;
using System.Collections.Generic;
using System.Text;

namespace TDAs.Abrstacciones
{
    public interface ICola_TDA<T>
    {
        void InicializarCola(int d);
        // siempre que la cola este inicializada
        void Acolar(T x);
        // siempre que la cola este inicializada y no este vacıa
        void Desacolar();
        // siempre que la cola este inicializada
        bool ColaVacia();
        // siempre que la cola este inicializada y no este vacıa
        T Primero();
    }
}
