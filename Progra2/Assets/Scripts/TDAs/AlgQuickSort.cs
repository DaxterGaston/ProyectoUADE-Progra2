using System;
using System.Collections.Generic;

class AlgQuickSort
{
    static public int Partition(int[] arr, int left, int right)
    {
        int pivot;
        int aux = (left + right) / 2; //tomo el valor central del vector
        pivot = arr[aux];

        // en este ciclo debo dejar todos los valores menores al pivot
        // a la izquierda y los mayores a la derecha
        while (true)
        {
            while (arr[left] < pivot)
            {
                left++;
            }

            while (arr[right] > pivot)
            {
                right--;
            }

            if (left < right)
            {
                int temp = arr[right];
                arr[right] = arr[left];
                arr[left] = temp;
            }
            else
            {
                // este es el valor que devuelvo como proxima posicion de
                // la particion en el siguiente paso del algoritmo
                return right;
            }
        }
    }

    static public void quickSort(int[] arr, int left, int right)
    {
        int pivot;
        if (left < right)
        {
            pivot = Partition(arr, left, right);
            if (pivot > 1)
            {
                // mitad del lado izquierdo del vector
                quickSort(arr, left, pivot - 1);
            }

            if (pivot + 1 < right)
            {
                // mitad del lado derecho del vector
                quickSort(arr, pivot + 1, right);
            }
        }
    }

    static void imprimirVector(int[] vec)
    {
        for (int i = 0; i < vec.Length; i++)
        {
            Console.Write(vec[i] + " ");
        }
    }

    static void Main(string[] args)
    {
        // creo el vector de enteros para ordenar
        int[] vectorEnteros = {67, 12, 95, 56, 85, 1, 100, 23, 60, 9};

        Console.WriteLine("Inicio Programa: Quick Sort");

        // muestro vector desordenado
        Console.Write("\nLista Desordenada: ");
        imprimirVector(vectorEnteros);

        // algoritmo de ordenamiento
        // inicialmente los parametros left y right son los extremos del vector
        quickSort(vectorEnteros, 0, vectorEnteros.Length - 1);

        // muestro vector ordenado
        Console.Write("\nLista Ordenada: ");
        imprimirVector(vectorEnteros);

        Console.ReadKey();
    }

    private static Random rng = new Random();

    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}