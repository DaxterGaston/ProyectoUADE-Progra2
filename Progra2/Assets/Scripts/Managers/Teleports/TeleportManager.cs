using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    private List<Teleport> _teleports;

    private TeleportManager()
    {
    }

    private void Awake()
    {
        _teleports = new List<Teleport>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Teleport");
        foreach (var item in gos)
            _teleports.Add(item.GetComponent<Teleport>());
    }

    #region Singleton

    private static TeleportManager _instance;

    public static TeleportManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var instance = FindObjectOfType<TeleportManager>();

                if (instance == null)
                {
                    var obj = new GameObject
                    {
                        name = typeof(TeleportManager).Name
                    };

                    _instance = obj.AddComponent<TeleportManager>();
                }
                else
                {
                    _instance = instance;
                }
            }
            return _instance;
        }
    }

    #endregion

    public Vector3 GetTargetPosition(int id)
    {
        foreach (var item in _teleports)
            if (item.Id == id)
                return item.transform.position;
        throw new ArgumentException("El teleport de id " + id + " no existe.");
    }
}
