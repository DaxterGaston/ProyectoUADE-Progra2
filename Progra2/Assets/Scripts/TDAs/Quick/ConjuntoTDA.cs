using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ConjuntoTDA
{
    void InicializarConjunto();
    bool ConjuntoVacio();
    void Agregar(int x);
    int Elegir();
    void Sacar(int x);
    bool Pertenece(int x);
}