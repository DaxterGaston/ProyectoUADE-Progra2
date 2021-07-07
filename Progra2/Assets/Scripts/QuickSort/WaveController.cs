using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Unity.Mathematics;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    private WaveXMLCreator xml;
    private XmlDocument waveXml;
    private int[] enemyDistribution = new int[3];
    private Dictionary<int, DijkstraPathing> waveReferenceDic = new Dictionary<int, DijkstraPathing>();
    private Queue<DijkstraPathing> waveQueue = new Queue<DijkstraPathing>();

    public bool IsQueueEmpty => waveQueue.Count <= 0;

    [SerializeField][Tooltip("Asignar el prefab del enemigo 'rojo'")]
    private DijkstraPathing redEnemyPrefab;
    [SerializeField][Tooltip("Asignar el prefab del enemigo 'verde'")]
    private DijkstraPathing greenEnemyPrefab;
    [SerializeField][Tooltip("Asignar el prefab del enemigo 'azul'")]
    private DijkstraPathing blueEnemyPrefab;
    

    private void Awake()
    {
        // Guardo la referencia del script que crea el archivo xml
        xml = GetComponent<WaveXMLCreator>();
    }
    
    public void LoadXml(int waveNumber)
    {
        // Creo el archivo xml
        xml.CreateXml(waveNumber);
        // Si no existe el archivo no continua
        if (!File.Exists(xml.FullPath)) return;
        // crea una referencia a un xml
        waveXml = new XmlDocument();
        // guarda el archivo creado anteriormente en su referencia
        waveXml.Load(xml.FullPath);
    }

    public void WaveSetup(int waveToSetup, int maxWaves)
    {
        // Confirma si el numero es valido 
        if (waveToSetup < 1 || waveToSetup > maxWaves) return;
        // Guardo todos los nodos del xml con el nombre wave
        var waveNodes = waveXml.GetElementsByTagName("Wave");
        // Accedo a cada nodo guardado
        foreach (XmlNode node in waveNodes)
        {
            // Me fijo en los nodos cual es su valor de wave
            var waveNumberParsedValue = Int32.Parse(node.Attributes.GetNamedItem("WaveNumber").Value);
            // Si el valor de wave es el que yo busco
            if (waveNumberParsedValue == waveToSetup)
            {
                // Guardo el atributo Red(cantidad de enemigos de este color que tengo que spawnear) temporalmente
                var redEnemiesV = Int32.Parse(node.FirstChild.Attributes.GetNamedItem("Red").Value);
                // Guardo el atributo Green(cantidad de enemigos de este color que tengo que spawnear) temporalmente
                var greenEnemiesV = Int32.Parse(node.FirstChild.Attributes.GetNamedItem("Green").Value);
                // Guardo el atributo Blue(cantidad de enemigos de este color que tengo que spawnear) temporalmente
                var blueEnemiesV = Int32.Parse(node.FirstChild.Attributes.GetNamedItem("Blue").Value);

                //Guardo cada valor en un array
                enemyDistribution[0] = redEnemiesV;
                enemyDistribution[1] = greenEnemiesV;
                enemyDistribution[2] = blueEnemiesV;
                WaveDataReference();
                WaveQuickSort();
                break;
            }
        }
    }
    
    private void WaveDataReference()
    {
        // Limpio el diccionario para evitar errores
        waveReferenceDic.Clear();
        // Agrego al diccionario cada par Key(cantidad) Value(prefab correspondiente) como referencia
        // Se que 0 = red / 1 = Green / 2 = Blue  siempre
        waveReferenceDic.Add(enemyDistribution[0],redEnemyPrefab);
        waveReferenceDic.Add(enemyDistribution[1],greenEnemyPrefab);
        waveReferenceDic.Add(enemyDistribution[2],blueEnemyPrefab);
    }
    
    private void WaveQuickSort()
    {
        // Implemento el algoritmo de ordenamiento QuickSort para que ordene el array de menor a mayor
        AlgQuickSort.quickSort(enemyDistribution, 0 , enemyDistribution.Length - 1);
        // Revierto el orden para que este de mayor a menor
        Array.Reverse(enemyDistribution);
        // Limpio la queue para evitar errores
        waveQueue.Clear();
        // Agrego la cantidad adecuada del tipo de enemigos con mayor cantidad de spawns a la queue de spawns
        for (int i = 0; i < enemyDistribution[0]; i++)
        {
            waveQueue.Enqueue(waveReferenceDic[enemyDistribution[0]]);
        }
        // Agrego la siguiente cantidad
        for (int i = 0; i < enemyDistribution[1]; i++)
        {
            waveQueue.Enqueue(waveReferenceDic[enemyDistribution[1]]);
        }
        // Agrego la ultima cantidad
        for (int i = 0; i < enemyDistribution[2]; i++)
        {
            waveQueue.Enqueue(waveReferenceDic[enemyDistribution[2]]);
        }
    }

    public void WaveSpawn(Transform transf)
    {
        // Si no hay enemigos en la queue no continuo
        if (waveQueue.Count <= 0) return;
        // Instancio el primer objeto de la queue
        var spawned = Instantiate(waveQueue.Peek(),transf.position,transf.rotation);
        // Guardo su spawn inicial
        spawned.SetSpawnPoint(transf);
        // Lo saco de la queue
        waveQueue.Dequeue();
    }
}