using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    private int currentLife;
    private int maxLife;
    private int currentArmorLife;
    private int maxArmorLife;

    public bool IsAlive => currentLife > 0;

    public event Action OnDeath;

    public event Action OnLifeChange;

    public int CurrentLife
    {
        get => currentLife;

        set
        {
            currentLife = value;
            
            if (!IsAlive)
            {
                OnDeath?.Invoke();
            }

            if (currentLife > maxLife)
            {
                currentLife = maxLife;
            }
            print($"{currentLife}/{maxLife} Life Points");
            OnLifeChange?.Invoke();
        }
    }

    public int CurrentArmor
    {
        get => currentArmorLife;

        set
        {
            currentArmorLife = value;
            if (currentArmorLife > maxArmorLife)
            {
                currentArmorLife = maxArmorLife;
            }

            if (currentArmorLife < 0)
            {
                var overflowDamage = -1 * currentArmorLife;
                currentArmorLife = 0;
                CurrentLife -= overflowDamage;
                return;
            }

            print($"{currentArmorLife}/{maxArmorLife} Armor");
            OnLifeChange?.Invoke();
        }
    }

    public int MaxLife => maxLife;
    public int MaxArmorLife => maxArmorLife;

    public void StartingLifePoints(int startingLifePoints, int startingArmorPoints)
    {
        maxLife = startingLifePoints;
        CurrentLife = startingLifePoints;
        maxArmorLife = startingArmorPoints;
        CurrentArmor = startingArmorPoints;
    }

    public void TakeDamage(int damage)
    {
        CurrentArmor -= damage;
    }

    public void HealDamage(int heal)
    {
        CurrentLife += heal;
    }

    public void RepairArmor(int repair)
    {
        CurrentArmor += repair;
    }
}