using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColaTD
{
    class Cola_TDA : ICola_TDA
    {
        int[] a;
        int indice;
        public void Acolar(int x)
        {
            for(int i = indice - 1; i >= 0; i--)
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

        public void InicializarCola()
        {
            a = new int[100];
            indice = 0;
        }

        public int Primero()
        {
            return a[indice - 1];
        }
    }
}
