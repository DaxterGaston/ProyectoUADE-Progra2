using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDAs.Abstracciones
{
    interface ICola_TDA<T>
    {
        void InicializarCola(int amount);

        void Acolar(T x);

        void Desacolar();

        bool ColaVacia();

        T Primero();
    }
}
