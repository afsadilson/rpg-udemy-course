using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform rectTransform;
    private Slider slider;

    private void Start() {
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void UpdateHealthUI() {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void FlipUI() => rectTransform.Rotate(0, 180, 0);

    private void OnDisable() {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
