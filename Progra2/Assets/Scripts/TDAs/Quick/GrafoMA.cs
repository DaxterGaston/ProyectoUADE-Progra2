using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrafoMA : GrafoTDA
{
    public static int n = 0;
    public int[,] MAdy;
    public int[] Etiqs;
    public int cantNodos;

    public void InicializarGrafo()
    {
        MAdy = new int[n, n];
        Etiqs = new int[n];
        cantNodos = 0;
    }

    public void AgregarVertice(int v)
    {
        Etiqs[cantNodos] = v;
        for (int i = 0; i <= cantNodos; i++)
        {
            MAdy[cantNodos, i] = 0;
            MAdy[i, cantNodos] = 0;
        }

        cantNodos++;
    }

    public void EliminarVertice(int v)
    {
        int ind = Vert2Indice(v);

        for (int k = 0; k < cantNodos; k++)
        {
            MAdy[k, ind] = MAdy[k, cantNodos - 1];
        }

        for (int k = 0; k < cantNodos; k++)
        {
            MAdy[ind, k] = MAdy[cantNodos - 1, k];
        }

        Etiqs[ind] = Etiqs[cantNodos - 1];
        cantNodos--;
    }

    public int Vert2Indice(int v)
    {
        int i = cantNodos - 1;
        while (i >= 0 && Etiqs[i] != v)
        {
            i--;
        }

        return i;
    }

    public ConjuntoTDA Vertices()
    {
        ConjuntoTDA Vert = new ConjuntoLD();
        Vert.InicializarConjunto();
        for (int i = 0; i < cantNodos; i++)
        {
            Vert.Agregar(Etiqs[i]);
        }

        return Vert;
    }

    public void AgregarArista(int v1, int v2, int peso)
    {
        int o = Vert2Indice(v1);
        int d = Vert2Indice(v2);
        MAdy[o, d] = peso;
    }

    public void EliminarArista(int v1, int v2)
    {
        int o = Vert2Indice(v1);
        int d = Vert2Indice(v2);
        MAdy[o, d] = 0;
    }

    public bool ExisteArista(int v1, int v2)
    {
        int o = Vert2Indice(v1);
        int d = Vert2Indice(v2);
        return MAdy[o, d] != 0;
    }

    public int PesoArista(int v1, int v2)
    {
        int o = Vert2Indice(v1);
        int d = Vert2Indice(v2);
        return MAdy[o, d];
    }
}