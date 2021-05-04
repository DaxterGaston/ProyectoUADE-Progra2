using System;
using System.Collections.Generic;
using System.Text;

namespace Pilas_Ej1
{
    public interface IPila_TDA
    {
        void InicializarPila(int c);

        int Apilar(int x);

        int Desapilar();

        bool PilaVacia();

        int Tope();

        void imprimoPila();
    }
}
