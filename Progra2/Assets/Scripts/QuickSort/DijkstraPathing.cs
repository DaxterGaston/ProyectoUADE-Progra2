using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraPathing : MonoBehaviour
{
    private Transform[] pathPoints;
    private GrafoMA grafoMa;
    private Queue<Transform> activePathing = new Queue<Transform>();
    private Transform pathStart;
    private Transform pathCurrent;
    private Transform pathLast;
    private bool isAtPoint;
    [SerializeField] private int pathEnemyIndex;
    [SerializeField] private float moveSpeed = 5f;

    public void SetSpawnPoint(Transform transf)
    {
        // Cambiar a la posicion donde spawnea
        pathCurrent = transf;
        transform.position = pathCurrent.position;
        pathLast = pathCurrent;
        Startup();
    }
    private void Startup()
    {
        var arrLength = SpawnController.Instance.dijkstraPathPoints.Length;
        pathPoints = new Transform[arrLength];
        Array.Copy(SpawnController.Instance.dijkstraPathPoints,pathPoints,arrLength);
        GraphCreation();
        RunDijkstra();
        NodeUpdate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RunDijkstra();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NodeUpdate();
        }
        print(pathCurrent);
        DijkstraMove();
    }

    private void GraphCreation()
    {
        GrafoMA.n = pathPoints.Length;
        grafoMa = new GrafoMA();
        grafoMa.InicializarGrafo();
        for (int i = 0; i < pathPoints.Length; i++)
        {
            grafoMa.AgregarVertice(int.Parse(pathPoints[i].name));
        }

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
        int[] aristas_peso = new int[aristas_origen.Length];
        for (int i = 0; i < aristas_peso.Length; i++)
        {
            aristas_peso[i] = 1;
        }

        for (int i = 0; i < aristas_peso.Length; i++)
        {
            grafoMa.AgregarArista(aristas_origen[i], aristas_destino[i], aristas_peso[i]);
        }
    }

    private void RunDijkstra()
    {
        AlgDijkstra.Dijkstra(grafoMa, TransformToIndex(pathCurrent));
        UpdateCurrentRoute();
    }

    private void UpdateCurrentRoute()
    {
        var path = Array.ConvertAll(AlgDijkstra.nodos[pathEnemyIndex].Split(','), Convert.ToInt32);
        print(AlgDijkstra.nodos[pathEnemyIndex]);
        activePathing.Clear();
        for (int i = 0; i < path.Length; i++)
        {
            activePathing.Enqueue(IndexToTransform(path[i]));
        }
    }

    private void DijkstraMove()
    {
        if (!isAtPoint)
        {
            transform.position =Vector2.MoveTowards(transform.position, pathCurrent.position, moveSpeed * Time.deltaTime);
            if (transform.position == pathCurrent.position)
            {
                isAtPoint = true;
            }
        }

        if (isAtPoint)
        {
            NodeUpdate();
        }
        // if (!isAtNextPoint)
        // {
        //     // Moverse al nodo
        //     transform.position = Vector2.MoveTowards(transform.position, pathCurrent.position, moveSpeed * Time.deltaTime);
        //     if (transform.position == pathCurrent.position)
        //     {
        //         isAtNextPoint = true;
        //     }
        // }
        //
        // if (isAtNextPoint)
        // {
        //     transform.position = Vector2.MoveTowards(transform.position, pathNext.position, moveSpeed * Time.deltaTime);
        //     if (transform.position == pathNext.position)
        //     {
        //         isAtNextPoint = false;
        //         NodeUpdate();
        //     }
        // }
    }

    private void NodeUpdate()
    {
        if (activePathing.Count <= 0)
        {
            pathLast = transform;
            pathCurrent = transform;
            return;
        }
        pathCurrent = activePathing.Peek();
        activePathing.Dequeue();
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