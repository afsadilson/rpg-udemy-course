using UnityEngine;

public class CharacterStats: MonoBehaviour {
  [Header("Major Stats")]
  [Tooltip("Increase damage and Critical Power")]
  public Stat strength;
  [Tooltip("Increase evasion and Critical Chance")]
  public Stat agility;
  [Tooltip("Increase magic damage and magic resistance")]
  public Stat intelligence;
  [Tooltip("Increase health")]
  public Stat vitality;

  [Header("Offensive Stats")]
  [Tooltip("Base damage")]
  public Stat damage;
  [Tooltip("Chances to hit Critical Power")]
  public Stat criticalChance;
  [Tooltip("Critical Damage")]
  public Stat criticalPower;

  [Header("Defensive Stats")]
  [Tooltip("Maximum Health")]
  public Stat maxHealth;
  [Tooltip("Decrease Damage")]
  public Stat armor;
  [Tooltip("Increase chance to avoid Attack")]
  public Stat evasion;
  public Stat magicResistance;

  [Header("Magic Stats")]
  public Stat fireDamage;
  public Stat iceDamage;
  public Stat lightningDamage;

  [Tooltip("Cause damage over time")]
  public bool isIgnited;
  [Tooltip("Reduce armor")]
  public bool isChilled;
  [Tooltip("Reduce accuracy")]
  public bool isShocked;

  private float ignitedTimer;
  private float chilledTimer;
  private float shockedTimer;

  private float igniteDamageCooldown = .3f;
  private float igniteDamageTimer;
  private int igniteDamage;

  [SerializeField] private int currentHealth;
  
  protected virtual void Start() {
    currentHealth = maxHealth.GetValue();
    criticalPower.SetDefaultValue(150);
  }

  protected virtual void Update() {
    ignitedTimer -= Time.deltaTime;
    chilledTimer -= Time.deltaTime;
    shockedTimer -= Time.deltaTime;
    igniteDamageTimer -= Time.deltaTime;

    if (ignitedTimer < 0)
      isIgnited = false;

    if (chilledTimer < 0)
      isChilled = false;

    if (shockedTimer < 0)
      isShocked = false;

    if (igniteDamageTimer < 0 && isIgnited) {
      currentHealth -= igniteDamage;

      if (currentHealth < 0)
        Die();
      
      igniteDamageTimer = igniteDamageCooldown;
    }
  }

  public virtual void DoDamage(CharacterStats _targetStats) {
    if (TargetCanAvoidAttack(_targetStats))
        return;

    int totalDamage = damage.GetValue() + strength.GetValue();

    if (CanCrit())
      totalDamage = CalculateCriticalDamage(totalDamage);

    totalDamage = CheckTargetArmor(_targetStats, totalDamage);

    _targetStats.TakeDamage(totalDamage);
  }

  public virtual void TakeDamage(int _damage) {
    currentHealth -= _damage;

    if (currentHealth <= 0)
      Die();
  }

  protected virtual void Die() {}

  public virtual void DoMagicalDamage(CharacterStats _targetStats) {
    int _intelligence = intelligence.GetValue();
    int _fireDamage = fireDamage.GetValue();
    int _iceDamage = iceDamage.GetValue();
    int _lightningDamage = lightningDamage.GetValue();

    int totalMagicDamage = _intelligence + _fireDamage + _iceDamage + _lightningDamage;
    totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);

    _targetStats.TakeDamage(totalMagicDamage);

    if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
      return;

    bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
    bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
    bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

    if (_fireDamage == _iceDamage && _fireDamage == _lightningDamage) {
        // Escolhe aleatoriamente um efeito para aplicar
        int randomChoice = Random.Range(0, 3);
        canApplyIgnite = randomChoice == 0;
        canApplyChill = randomChoice == 1;
        canApplyShock = randomChoice == 2;
    }

    if (canApplyIgnite)
      _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

    _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
  }

  public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

  public void ApplyAilments(bool _ignite, bool _chill, bool _shock) {
    if (isIgnited || isChilled || isShocked)
      return;
    
    if (_ignite) {
      isIgnited = _ignite;
      ignitedTimer = 4;
    }

    if (_chill) {
      isChilled = _chill;
      chilledTimer = 4;
    }
    
    if (_shock) {
      isShocked = _shock;
      shockedTimer = 4;
    }
  }

  private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage) {
    totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
    totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);

    return totalMagicDamage;
  }

  private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage) {
    if (_targetStats.isChilled) {
      totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f); // Reduce 20% armor
    } else {
      totalDamage -= _targetStats.armor.GetValue();
    }

    totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
    return totalDamage;
  }

  private bool TargetCanAvoidAttack(CharacterStats _targetStats) {
    int totalEvasion = _targetStats.agility.GetValue() + _targetStats.evasion.GetValue();

    if (isShocked)
      totalEvasion += 20; // 20% more bonus chance

    if (Random.Range(1, 100) <= totalEvasion)
      return true;

    return false;
  }

  private bool CanCrit() {
    int totalCriticalChance = criticalChance.GetValue() + agility.GetValue();

    if (Random.Range(1, 100) <= totalCriticalChance)
      return true;

    return false;
  }

  private int CalculateCriticalDamage(int _damage) {
    float totalCriticalPower = (criticalPower.GetValue() + strength.GetValue()) * .01f;

    float criticalDamage = _damage * totalCriticalPower;

    return Mathf.RoundToInt(criticalDamage);
  }
}