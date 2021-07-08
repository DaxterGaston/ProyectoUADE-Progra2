using System;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraPathing : MonoBehaviour
{
    private Transform[] pathPoints; // Array con todos los puntos del grafo
    private GrafoMA grafoMa; // Matriz de adyacencia donde se guardan todos los datos para utilizar en Dijkstra
    private Queue<Transform> activePathing = new Queue<Transform>(); // Lista de puntos que tiene que recorrer
    private Transform pathCurrent; // Punto al que tiene que llegar 
    private Transform pathLast; // Quizas se usa para algo despues (si patrulla sobre una camino especifico)
    private bool isAtPoint; // Llegue al punto que me corresponde?
    [SerializeField] private int pathEnemyIndex; // El punto final de la ruta
    [SerializeField] private float moveSpeed = 5f; // Velocidad de movimiento del objeto
    private EnemyController enemyController; // Referencia al script que tiene los metodos de tracking


    private void Awake()
    {
        // Guardo referencia al controlador del enemigo
        enemyController = GetComponent<EnemyController>();
    }

    public void SetSpawnPoint(Transform transf)
    {
        // Guardo la posicion
        pathCurrent = transf;
        // Aplico la posicion
        transform.position = new Vector3(pathCurrent.position.x,pathCurrent.position.y,-10f);
        // Guarda la posicion anterior
        pathLast = pathCurrent;
        // Inicializa
        Startup();
    }

    private void Startup()
    {
        // Var aux para hacer el array en el que voy a copiar
        var arrLength = SpawnController.Instance.dijkstraPathPoints.Length;
        // Genero el array con el tamaño 
        pathPoints = new Transform[arrLength];
        // Copio el array para poder realizar todas las funciones del script
        Array.Copy(SpawnController.Instance.dijkstraPathPoints, pathPoints, arrLength);
        // Genera el grafo con sus aristas y pesos
        GraphCreation();
        // Usa el algoritmo de dijkstra sobre el grafo generado anteriormente
        RunDijkstra();
        // Actualiza la siguiente posicion a la que tiene que ir
        NodeUpdate();
    }

    private void Update()
    {
        // Comprueba si puede moverse por dijkstra
        if (enemyController.DijkstraMove)
        {
            // Movimiento utilizando el camino generado por dijkstra
            DijkstraMove();
        }
    }

    private void GraphCreation()
    {
        // Tamaño que va a tener la matriz de adyacencia
        GrafoMA.n = pathPoints.Length;
        // Crea una nueva matriz
        grafoMa = new GrafoMA();
        // Inicializa los valores iniciales de la matriz
        grafoMa.InicializarGrafo();
        // Agrega los vertices a la matriz
        for (int i = 0; i < pathPoints.Length; i++)
        {
            grafoMa.AgregarVertice(int.Parse(pathPoints[i].name));
        }

        // Declara el origen de las aristas
        int[] aristas_origen =
        {
            0, 0, 0, 1, 1, 1, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 8, 9, 9, 9, 10, 10, 10, 10,
            11, 11, 11, 12, 12, 13, 13, 14, 14, 14, 15, 15, 15, 16, 16, 16, 16, 17, 17, 18, 18, 18, 19, 19, 19, 19,
            20, 21, 22, 22, 22, 23, 24, 24, 25, 25, 25, 26, 26, 26, 26, 27, 27, 28, 28, 29, 29, 29, 135, 135, 135,
            30, 30, 30, 31, 31, 31, 32, 32, 32, 33, 33, 33, 34, 34, 34, 35, 35, 36, 36, 36, 37, 37, 37, 38, 38, 38,
            39, 39, 40, 40, 40, 41, 41, 41, 42, 42, 42, 43, 43, 43, 44, 44, 44, 45, 45, 46, 46, 46, 47, 47, 47, 48,
            48, 48, 48, 49, 49, 49, 50, 50, 50, 51, 51, 51, 52, 52, 53, 53, 53, 54, 54, 54, 55, 55, 55, 56, 56, 56,
            56, 57, 57, 57, 58, 58, 58, 59, 59, 59, 60, 60, 60, 61, 61, 61, 61, 62, 62, 62, 62, 63, 63, 63, 63, 64,
            64, 64, 65, 65, 65, 66, 66, 66, 67, 67, 67, 68, 68, 68, 69, 69, 69, 70, 70, 71, 71, 71, 72, 72, 72, 73,
            73, 73, 73, 74, 74, 74, 75, 75, 75, 75, 76, 76, 76, 77, 77, 78, 78, 78, 79, 79, 79, 80, 80, 80, 81, 81,
            81, 82, 82, 82, 83, 83, 83, 84, 84, 85, 85, 85, 85, 86, 86, 86, 86, 87, 87, 87, 87, 88, 88, 88, 89, 89,
            89, 89, 136, 136, 90, 90, 91, 91, 91, 92, 92, 93, 93, 93, 94, 94, 94, 95, 95, 95, 96, 96, 96, 97, 97,
            97, 98, 98, 98, 99, 99, 99, 100, 100, 100, 101, 101, 101, 102, 102, 102, 102, 103, 103, 103, 104, 104,
            104, 104, 105, 105, 105, 106, 106, 107, 107, 107, 108, 108, 108, 109, 109, 109, 110, 110, 110, 111, 111,
            111, 112, 112, 112, 113, 113, 113, 114, 114, 114, 115, 115, 115, 116, 116, 117, 117, 117, 118, 118, 118,
            118, 119, 119, 119, 119, 120, 120, 120, 120, 121, 121, 121, 121, 122, 122, 122, 122, 123, 123, 123, 123,
            124, 124, 124, 124, 125, 125, 125, 125, 125, 126, 126, 126, 127, 127, 127, 127, 128, 128, 128, 128, 129,
            129, 129, 129, 130, 130, 130, 130, 131, 131, 131, 131, 132, 132, 132, 132, 133, 133, 133, 133, 134, 134,
            134, 134, 19, 21, 135, 89, 136, 134
        };
        // Declara el destino de las aristas
        int[] aristas_destino =
        {
            1, 12, 13, 0, 2, 15, 1, 3, 2, 4, 15, 3, 16, 5, 4, 6, 5, 7, 16, 6, 17, 8, 7, 9, 18, 19, 8, 10, 19, 9, 11,
            18, 19, 10, 12, 20, 0, 11, 0, 14, 13, 15, 16, 1, 3, 14, 4, 6, 14, 17, 7, 16, 8, 10, 19, 8, 9, 10, 18,
            11, 22, 21, 23, 24, 22, 22, 25, 24, 26, 29, 25, 27, 28, 135, 26, 135, 26, 135, 25, 30, 46, 26, 27, 28,
            29, 31, 48, 30, 32, 49, 31, 33, 50, 32, 66, 34, 33, 51, 35, 34, 36, 35, 52, 37, 36, 38, 53, 37, 39, 54,
            38, 40, 39, 41, 61, 40, 42, 62, 41, 43, 63, 42, 44, 55, 43, 45, 56, 44, 46, 29, 45, 47, 46, 48, 56, 30,
            47, 49, 57, 31, 48, 50, 32, 49, 58, 34, 52, 59, 51, 36, 37, 54, 60, 38, 53, 61, 43, 56, 64, 44, 47, 55,
            57, 48, 56, 58, 50, 57, 65, 51, 60, 62, 53, 59, 61, 40, 54, 60, 62, 41, 59, 61, 63, 42, 62, 64, 66, 55,
            63, 65, 58, 64, 66, 33, 65, 63, 68, 80, 136, 67, 69, 136, 68, 70, 83, 69, 71, 70, 83, 72, 71, 73, 84,
            72, 74, 85, 89, 73, 75, 89, 74, 76, 85, 89, 75, 77, 86, 76, 78, 77, 79, 86, 78, 80, 87, 67, 79, 81, 80,
            82, 87, 81, 83, 88, 69, 71, 82, 72, 88, 73, 75, 86, 89, 76, 78, 85, 87, 79, 81, 86, 88, 82, 84, 87, 73,
            74, 75, 85, 67, 68, 91, 115, 90, 92, 117, 91, 93, 92, 94, 117, 93, 118, 95, 94, 119, 96, 95, 120, 97,
            96, 130, 98, 97, 121, 99, 98, 122, 100, 99, 123, 101, 100, 124, 102, 101, 125, 103, 134, 102, 134, 104,
            103, 134, 125, 105, 104, 126, 106, 105, 107, 106, 127, 108, 107, 128, 109, 108, 122, 110, 109, 129, 111,
            110, 130, 112, 111, 131, 113, 112, 132, 114, 113, 133, 115, 90, 114, 116, 115, 133, 91, 93, 118, 94,
            117, 133, 119, 118, 99, 132, 120, 119, 96, 131, 121, 98, 120, 129, 122, 121, 99, 123, 109, 100, 122,
            128, 124, 101, 123, 127, 125, 124, 102, 104, 126, 134, 105, 125, 127, 124, 126, 107, 128, 108, 123, 127,
            129, 121, 128, 110, 130, 129, 111, 131, 97, 120, 112, 132, 130, 113, 119, 131, 133, 132, 118, 114, 116,
            102, 103, 104, 125, 21, 19, 89, 135, 134, 136
        };
        // Declara el peso de las aristas
        int[] aristas_peso = new int[aristas_origen.Length];
        for (int i = 0; i < aristas_peso.Length; i++)
        {
            aristas_peso[i] = 1;
        }

        // Agrega las aristas a la matriz
        for (int i = 0; i < aristas_peso.Length; i++)
        {
            grafoMa.AgregarArista(aristas_origen[i], aristas_destino[i], aristas_peso[i]);
        }
    }

    private void RunDijkstra()
    {
        // Corre el algoritmo de dijkstra sobre la matriz creada y tiene como inicio la posicion actual del objeto
        AlgDijkstra.Dijkstra(grafoMa, TransformToIndex(pathCurrent));
        // Guarda los datos del algoritmo como una lista de puntos a recorrer
        UpdateCurrentRoute();
    }

    private void UpdateCurrentRoute()
    {
        // Convierte el array de string que dice el camino a uno de int
        var path = Array.ConvertAll(AlgDijkstra.nodos[pathEnemyIndex].Split(','), Convert.ToInt32);
        // Limpia la ruta anterior 
        activePathing.Clear();
        // Agrega el array de int a la Queue de ruta despues de transformarlo a un Transform
        for (int i = 0; i < path.Length; i++)
        {
            activePathing.Enqueue(IndexToTransform(path[i]));
        }
    }

    private void DijkstraMove()
    {
        // Se fija si no llego al punto actual de la ruta
        if (!isAtPoint)
        {
            // Como no llego, trata de ir hacia el punto
            transform.position = Vector3.MoveTowards(transform.position, pathCurrent.position, moveSpeed * Time.deltaTime);
            // Cuando llega a la posicion cambia el bool
            if (transform.position == pathCurrent.position)
            {
                isAtPoint = true;
            }
        }

        // Se fija si llego al punto de la ruta
        if (isAtPoint)
        {
            // Actualiza el siguiente punto de la ruta
            NodeUpdate();
        }
    }

    private void NodeUpdate()
    {
        // Si no queda mas ruta (llego al final)
        if (activePathing.Count <= 0)
        {
            // Guarda el ultimo como su posicion actual
            pathLast = transform;
            // Guarda el actual como su posicion actual
            pathCurrent = transform;
            return;
        }

        // Agarra el siguiente punto de la ruta
        pathCurrent = activePathing.Peek();
        // Saca el punto de la queue para no repetirlo
        activePathing.Dequeue();
        // Cambia el bool para que trate de llegar al punto devuelta
        isAtPoint = false;
    }

    private Transform IndexToTransform(int index)
    {
        // Cambia el int sacado del Dijkstra a su Transform correspondiente
        return pathPoints[index];
    }

    private int TransformToIndex(Transform transf)
    {
        // Cambia el Transform a su int correspondiente usable por Dijkstra
        return Int32.Parse(transf.gameObject.name);
    }
}