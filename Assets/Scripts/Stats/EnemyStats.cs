using System;
using UnityEngine;

public class EnemyStats: CharacterStats {

    private Enemy enemy;

    [Header("Level Details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1)]
    [SerializeField] private float percentageModifier = .4f;

    protected override void Start()
    {
        ApplyLevelModifiers();
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    private void ApplyLevelModifiers() {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);
        Modify(damage);
        Modify(criticalChance);
        Modify(criticalPower);
        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
    }

    private void Modify(Stat _stat) {
        for (int i = 1; i < level; i++) {
            float modifier = _stat.GetValue() * percentageModifier;
            _stat.AddMofifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();

        enemy.Dead();
    }
}