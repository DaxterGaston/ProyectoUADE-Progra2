using System;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField][Tooltip("Maximum amount of armor")] 
    private float maxArmor = 100f;
    private Health health;

    public event Action OnDamaged;
    public event Action OnHealed;
    
    public float CurrentArmor { get; private set; }
    public float MaxArmor => maxArmor;
    public float GetRatio => CurrentArmor / maxArmor;

    private void Awake()
    {
        CurrentArmor = maxArmor;
    }

    private void Start()
    {
        health = GetComponent<Health>();
    }

    public void ArmorUp(float armorAmount)
    {
        var armorBefore = CurrentArmor;
        CurrentArmor += armorAmount;
        CurrentArmor = Mathf.Clamp(CurrentArmor, 0f, maxArmor);

        // call OnHeal action
        var trueArmorAmount = CurrentArmor - armorBefore;
        if (trueArmorAmount > 0f)
        {
            // use this to display amount healed on screen
            OnHealed?.Invoke();
        }
    }
   
    public void TakeArmorDamage(float damage)
    {
        var armorBefore = CurrentArmor;
        CurrentArmor -= damage;
        CurrentArmor = Mathf.Clamp(CurrentArmor, 0f, maxArmor);

        // call OnDamage action
        var trueDamageAmount = armorBefore - CurrentArmor;
        if (trueDamageAmount > 0f)
        {
            // use this to display on screen
            OnDamaged?.Invoke();
        }
        
        HandlePierce(damage - trueDamageAmount);
    }
    
    private void HandlePierce(float pierceDamage)
    {
        if (!(CurrentArmor <= 0f)) return;
        if (pierceDamage > 0)
        {
            health.TakeDamage(pierceDamage);
        }
    }
}
