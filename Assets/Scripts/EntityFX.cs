using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;
    private SpriteRenderer sr;

    private void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX() {
        sr.material = hitMat;

        yield return new WaitForSeconds(.15f);

        sr.material = originalMat;
    }

    private void RedColorBlink() {
        if (sr.color != Color.white) {
            sr.color = Color.white;
        } else {
            sr.color = Color.red;
        }
    }

     private void CancelRedColorBlink() {
        CancelInvoke();
        sr.color = Color.white;
    }

}
