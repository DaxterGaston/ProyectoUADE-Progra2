using System;
using System.Collections.Generic;
using System.Text;
using TDAs.Abrstacciones;

namespace TDAs
{
    public class Cola_TDA<T> : ICola_TDA<T>
    {
        T[] a; 
        int indice;

        public int Cantidad { get {return a.Length; } }

        public void Acolar(T x)
        {
            for (int i = indice - 1; i >= 0; i--)
            {
                a[i + 1] = a[i];
            }
            a[0] = x;
            
            indice++;
        }

        public bool ColaVacia()
        {
            return (indice == 0);
        }

        public void Desacolar()
        {
            indice--;
        }

        public void InicializarCola(int dimension)
        {
            a = new T[dimension];
            indice = 0;
        }

        public T Primero()
        {
            return a[indice - 1];
        }
    }
}
