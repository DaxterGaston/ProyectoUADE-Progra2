﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    // Declaro un controlador de vida
    private LifeController lifeController;
    // Declaro un valor de vida maximo
    [SerializeField] private int maxLifePoints = 100;
    // Declaro un valor de armor maximo
    [SerializeField] private int maxArmorPoints = 100;

    private void Start()
    {
        // Obtengo el controlador de vida
        lifeController = GetComponent<LifeController>();
        // Inicializo el controlador con el numero maximo de vida que va a tener
        lifeController.StartingLifePoints(maxLifePoints,maxArmorPoints);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            lifeController.TakeDamage(20);
        }
    }
}