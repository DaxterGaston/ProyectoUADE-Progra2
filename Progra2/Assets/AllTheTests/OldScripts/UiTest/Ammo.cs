using System;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField][Tooltip("Maximum amount of ammo")] 
    private float maxAmmo = 5f;
    [SerializeField][Tooltip("Time to reload")] 
    private float reloadTime = 1f;
    private bool isOutOfAmmo;

    public event Action OnAmmoUse;
    public event Action OnAmmoReload;
    
    public float CurrentAmmo { get; private set; }
    public float MaxAmmo => maxAmmo;
    public float GetRatio => CurrentAmmo / maxAmmo;

    private void Awake()
    {
        CurrentAmmo = maxAmmo;
        isOutOfAmmo = false;
    }

    public void AmmoReload(float ammoReloaded)
    {
        var ammoBefore = CurrentAmmo;
        CurrentAmmo += ammoReloaded;
        CurrentAmmo = Mathf.Clamp(CurrentAmmo, 0f, maxAmmo);

        // call OnReload action
        var trueAmmoGainedAmount = CurrentAmmo - ammoBefore;
        if (trueAmmoGainedAmount > 0f)
        {
            // use this to display amount reloaded on screen
            OnAmmoReload?.Invoke();
        }

        if (CurrentAmmo > 0)
        {
            isOutOfAmmo = false;
        }
    }
   
    public void AmmoUse(float ammoUsed)
    {
        if (isOutOfAmmo) return;
        var ammoBefore = CurrentAmmo;
        CurrentAmmo -= ammoUsed;
        CurrentAmmo = Mathf.Clamp(CurrentAmmo, 0f, maxAmmo);

        // call OnUse action
        var trueAmmoUsedAmount = ammoBefore - CurrentAmmo;
        if (trueAmmoUsedAmount > 0f)
        {
            // use this to display on screen
            OnAmmoUse?.Invoke();
        }
        
        if (CurrentAmmo <= 0f)
        {
            isOutOfAmmo = true;
            Invoke(nameof(Reload),reloadTime);
        }
        
    }
    
    private void Reload()
    {
        CurrentAmmo = maxAmmo;
        OnAmmoReload?.Invoke();
        isOutOfAmmo = false;
    }
}
