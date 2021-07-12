using System;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    #region GetInfo

    private Health health;
    private Armor armor;
    private Ammo ammo;
    private SpawnController spawnController;
    private WaveController waveController;

    #endregion

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private TextMeshProUGUI redEnemyNumberText;
    [SerializeField] private TextMeshProUGUI greenEnemyNumberText;
    [SerializeField] private TextMeshProUGUI blueEnemyNumberText;
    [SerializeField] private TextMeshProUGUI waveTotalEnemyNumberText;
    
    private void Start()
    {
        health = GetComponent<Health>();
        armor = GetComponent<Armor>();
        ammo = GetComponent<Ammo>();
        spawnController = SpawnController.Instance;
        waveController = spawnController.GetComponent<WaveController>();
     
        health.OnDamaged += UpdateHealth;
        health.OnHealed += UpdateHealth;
        armor.OnDamaged += UpdateArmor;
        armor.OnHealed += UpdateArmor;
        ammo.OnAmmoUse += UpdateAmmo;
        ammo.OnAmmoReload += UpdateAmmo;
        waveController.OnWaveSetup += UpdateWave;
        spawnController.OnKillConfirmed += UpdateObjective;
        FirstUpdate();
    }
    
    private void FirstUpdate()
    {
        UpdateHealth();
        UpdateArmor();
        UpdateAmmo();
        switch (spawnController.enemySpawnMethod)
        {
            case SpawnController.EnemySpawningMethod.QueuePool:
                break;
            case SpawnController.EnemySpawningMethod.Waves:
                UpdateWave();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        UpdateObjective();
    }

    #region PlayerUpdates

    private void UpdateHealth()
    {
        healthText.text = $"{health.CurrentHealth}/{health.MaxHealth}";
    }

    private void UpdateArmor()
    {
        armorText.text = $"{armor.CurrentArmor}/{armor.MaxArmor}";
    }
    private void UpdateAmmo()
    {
        ammoText.text = $"{ammo.CurrentAmmo}/{ammo.MaxAmmo}";
    }

    #endregion

    private void UpdateWave()
    {
        waveNumberText.text = $"Wave  {waveController.CurrentWave}";
        redEnemyNumberText.text = $"{waveController.RedEnemies}";
        greenEnemyNumberText.text = $"{waveController.GreenEnemies}";
        blueEnemyNumberText.text = $"{waveController.BlueEnemies}";
    }

    private void UpdateObjective()
    {
        switch (spawnController.enemySpawnMethod)
        {
            case SpawnController.EnemySpawningMethod.QueuePool:
                waveTotalEnemyNumberText.text = $"{spawnController.QueuePoolObjective - SpawnController.killedAmount}";
                break;
            case SpawnController.EnemySpawningMethod.Waves:
                waveTotalEnemyNumberText.text = $"{waveController.WaveTotalEnemies - SpawnController.killedAmount}";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
