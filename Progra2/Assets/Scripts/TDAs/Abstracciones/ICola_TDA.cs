using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColaTD
{
    interface ICola_TDA
    {
        void InicializarCola();

        void Acolar(int x);

        void Desacolar();

        bool ColaVacia();

        int Primero();
    }
}
