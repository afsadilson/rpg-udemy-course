using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;
    private SpriteRenderer sr;

    [Header("Ailments color")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    private void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    public void MakeTransparent(bool _transparent) {
        if (_transparent) {
            sr.color = Color.clear;
        } else {
            sr.color = Color.white;
        }
    }

    private IEnumerator FlashFX() {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(.15f);

        sr.color = currentColor;

        sr.material = originalMat;
    }

    private void RedColorBlink() {
        if (sr.color != Color.white) {
            sr.color = Color.white;
        } else {
            sr.color = Color.red;
        }
    }

    private void CancelColorChange() {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void IgniteFxFor(float _seconds) {
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx() {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    public void ChillFxFor(float _seconds) {
        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void ChillColorFx() {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

    public void ShockFxFor(float _seconds) {
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void ShockColorFx() {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

}
