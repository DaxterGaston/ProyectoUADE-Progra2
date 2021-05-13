using System.Collections.Generic;
using TDAs;
using UnityEngine;

public class BasePool<T> where T : MonoBehaviour
{
    /// <summary>
    /// TODO: Almacenar objetos con enabled = false.
    /// Son los que estarian disponibles para uso.
    /// </summary>
    private Cola_TDA<T> Avaliable;

    /// <summary>
    /// TODO: Almacenar objetos con enabled = true.
    /// Son los que estarian en uso.
    /// </summary>
    private Cola_TDA<T> Using;

    public int Amount { get; private set; }

    public int AvaliableAmount
    {
        get { return Avaliable.Cantidad; }
    }

    /// <summary>
    /// Pool generico.
    /// </summary>
    /// <param name="amount">
    /// Cantidad de ese tipo especifico que se necesitan.
    /// </param>
    public void CreateInitialInstances(List<T> inicial)
    {
        Avaliable = new Cola_TDA<T>();
        Using = new Cola_TDA<T>();
        Amount = inicial.Count;
        Avaliable.InicializarCola(Amount);
        Using.InicializarCola(Amount);
        foreach (var item in inicial) Avaliable.Acolar(item);
    }

    public List<T> GetAll()
    {
        List<T> l = new List<T>();
        for (int i = 1; i < AvaliableAmount; i++)
        {
            l.Add(Avaliable.Primero());
            Avaliable.Desacolar();
        }

        return l;
    }

    public T Get()
    {
        if (Avaliable.ColaVacia()) return default(T);
        if (AvaliableAmount > 0)
        {
            T e;
            e = Avaliable.Primero();
            e.gameObject.SetActive(true);
            Avaliable.Desacolar();
            Using.Acolar(e);
            return e;
        }

        return default(T);
    }

    public void Store(T obj)
    {
        if (AvaliableAmount >= Amount)
        {
            obj = null;
            return;
        }

        if (Using.Cantidad > 0)
        {
            var e = Using.Primero();
            e.gameObject.SetActive(false);
            Using.Desacolar();
            Avaliable.Acolar(e);
        }
    }

    public bool HasAvaliable()
    {
        return Avaliable.Cantidad > 0;
    }
}