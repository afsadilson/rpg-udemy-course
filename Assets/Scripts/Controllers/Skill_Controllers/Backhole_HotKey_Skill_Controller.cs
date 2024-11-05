using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode hotKey;
    private TextMeshProUGUI hotkeyLabel;

    private Transform enemy;
    private Blackhole_Skill_Controller blackHole;
    
    private void Update() {
        if (Input.GetKeyDown(hotKey)) {
            blackHole.AddEnemyToList(enemy);

            hotkeyLabel.color = Color.clear;
            sr.color = Color.clear;
        }
    }

    public void SetupHotKey(KeyCode _newHotKey, Transform _enemy, Blackhole_Skill_Controller _blackHole) {
        sr = GetComponent<SpriteRenderer>();
        hotkeyLabel = GetComponentInChildren<TextMeshProUGUI>();

        hotKey = _newHotKey;
        hotkeyLabel.text = _newHotKey.ToString();

        enemy = _enemy;
        blackHole = _blackHole;
    }

}
