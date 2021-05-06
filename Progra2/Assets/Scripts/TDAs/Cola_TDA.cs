using TDAs.Abstracciones;
using UnityEngine;

namespace TDAs
{
    public class Cola_TDA<T> : ICola_TDA<T> where T : MonoBehaviour
    {
        T[] a;
        int indice;

        public int Cantidad { get => a.Length; }

        public void Acolar(T x)
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

        public void InicializarCola(int amount)
        {
            a = new T[amount];
            indice = 0;
        }

        public T Primero()
        {
            return a[indice - 1];
        }
    }
}
