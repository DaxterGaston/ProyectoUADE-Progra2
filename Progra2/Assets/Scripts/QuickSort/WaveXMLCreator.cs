using System;
using System.IO;
using System.Xml;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveXMLCreator : MonoBehaviour
{
    #region XMLBase

    private XmlDocument doc;
    private XmlDeclaration xmlDeclaration;
    private string path = Directory.GetCurrentDirectory();
    private string documentName = "WaveDisplay.xml";
    public string FullPath { get; private set; }

    #endregion

    #region WaveVariables
    
    [SerializeField] [Tooltip("First wave base number of enemies to spawn")]
    private int lastWaveEnemies = 10;
    private int firstSetupWaveEnemies;
    private int differentEnemies = 3;

    #endregion

    private void Awake()
    {
        // Guardo el valor para poder reiniciarlo despues
        firstSetupWaveEnemies = lastWaveEnemies;
    }

    public void CreateXml(int waveNumber)
    {
        // Creo el nuevo documento
        doc = new XmlDocument();
        // Declaro el formato
        xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        // Tomo el nodo raiz del documento
        var root = doc.DocumentElement;
        // Agrego la declaracion de formato al inicio del documento
        doc.InsertBefore(xmlDeclaration, root);
        // Creo la raiz
        var waveDisplay = doc.CreateElement(String.Empty, "WaveDisplay", String.Empty);
        // Atributos del elemento
        waveDisplay.SetAttribute("NumberOfWaves", waveNumber.ToString());
        // Agrego la raiz al documento
        doc.AppendChild(waveDisplay);
        // Creo objetos que van dentro de la raiz
        for (int i = 1; i <= waveNumber; i++)
        {
            // Elemento Contenedor
            var wave = doc.CreateElement("Wave");
            // Atributos del elemento
            wave.SetAttribute("WaveNumber", i.ToString());
            var waveEnemies = TotalEnemiesInWave();
            wave.SetAttribute("TotalEnemies", waveEnemies.ToString());

            // Anido el elemento a su padre
            wave.AppendChild(EnemyDistributionInWave(waveEnemies));
            waveDisplay.AppendChild(wave);
        }

        SaveDocument();
        // Reinicio el valor para que no genere errores
        lastWaveEnemies = firstSetupWaveEnemies;
    }

    private void SaveDocument()
    {
        // Agrego a la ruta el nombre del documento
        FullPath = Path.Combine(path, documentName);
        // cuando termino guardo el archivo en la ruta que se eligio
        doc.Save(FullPath);
    }

    private int TotalEnemiesInWave()
    {
        // Cambio el numero de enemigos que va a spawnear en la wave
        var nextWaveEnemies = lastWaveEnemies + Random.Range(5, 11);
        // Guardo el valor para tenerlo en el siguiente llamado
        lastWaveEnemies = nextWaveEnemies;
        return nextWaveEnemies;
    }

    private XmlElement EnemyDistributionInWave(int enemiesToDistribute)
    {
        // Creo el elemento que va a devolver el metodo
        var enemies = doc.CreateElement("Enemies");
        // Variable auxiliar para crear un minimo de unidades que van a sobrar
        var currentEnemiesDistributed = 1;
        // Cantidad maxima de enemigos teniendo en cuenta el minimo que necesita sobrar (Total - (enemigosDiferentes - 1)) resultado +1 porque excluye .Range -1
        var maxToSpawn = (enemiesToDistribute - (differentEnemies - currentEnemiesDistributed)) + 1;
        // Cantidad final de enemigos que va a spawnear este tipo
        var numberToSpawn = Random.Range(1, maxToSpawn);
        // Variable auxiliar para facilitar el calculo de maximo
        var remainingEnemiesToSpawn = enemiesToDistribute - numberToSpawn;
        // Variable aux
        currentEnemiesDistributed++;
        // Checkeo que no quede el valor minimo(2) para distribuir entre 2 tipos de enemigos
        if (remainingEnemiesToSpawn <= currentEnemiesDistributed)
        {
            numberToSpawn--;
            remainingEnemiesToSpawn++;
        }
        // Checkeo que no quede el mismo valor para los siguientes enemigos
        if (remainingEnemiesToSpawn == numberToSpawn)
        {
            numberToSpawn--;
            remainingEnemiesToSpawn++;
        }
        // Variable aux
        var redEnemiesAux = numberToSpawn;
        // Guardo el valor como atributo
        enemies.SetAttribute("Red", numberToSpawn.ToString());
        // Hago el calculo del maximo de enemigos
        maxToSpawn = (remainingEnemiesToSpawn - (differentEnemies - currentEnemiesDistributed)) + 1;
        // Calculo un valor entre 1 y el maximo de enemigos para spawnear
        numberToSpawn = Random.Range(1, maxToSpawn);
        // Variable aux
        remainingEnemiesToSpawn -= numberToSpawn;
        // Checkeo que no quede el mismo valor para los siguientes enemigos
        if (remainingEnemiesToSpawn == numberToSpawn)
        {
            numberToSpawn++;
            remainingEnemiesToSpawn--;
        }
        // Checkeo que el valor de enemigos sobrantes no sea igual al primero numero de enemigos
        if (redEnemiesAux == remainingEnemiesToSpawn || redEnemiesAux == numberToSpawn)
        {
            numberToSpawn++;
            remainingEnemiesToSpawn--;
            if (remainingEnemiesToSpawn == 0)
            {
                numberToSpawn -= 2;
                remainingEnemiesToSpawn += 2;
            }
        }
        // Guardo valor como att
        enemies.SetAttribute("Green", numberToSpawn.ToString());
        // Como este es el ultimo tipo de enemigo le asigno todos los que sobran
        numberToSpawn = remainingEnemiesToSpawn;
        // Guardo valor como att
        enemies.SetAttribute("Blue", numberToSpawn.ToString());

        // Devuelve el elemento ya con todos los atributos asignados
        return enemies;
    }
}