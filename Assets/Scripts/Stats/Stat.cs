using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat {
  [SerializeField] private int baseValue;
  public List<int> modifiers;

  public int GetValue() {
    int finalValue = baseValue;

    foreach (var modifier in modifiers)
    {
      finalValue += modifier;
    }

    return finalValue;
  }

  public void SetDefaultValue(int _defaultValue) {
    baseValue = _defaultValue;
  }

  public void AddMofifier(int _modifier) {
    modifiers.Add(_modifier);
  }

  public void RemoveModifier(int _modifier) {
    modifiers.RemoveAt(_modifier);
  }
}