using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [Header("Crystal info")]
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    [SerializeField] private float growSpeed;
    private GameObject currentCrystal;
    
    [Header("Crystal Mirage")]
    [SerializeField] private bool CloneInsteadOfCrystal;

    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi crystal")]
    [SerializeField] private bool canUseMultiCrystal;
    [SerializeField] private int amountOfCrystals;
    [SerializeField] private float multiCrystalCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalsLeft = new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystals()) return;

        if (currentCrystal == null) {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);

            Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, growSpeed, FindClosestEnemy(currentCrystal.transform));
        } else {
            if (canMoveToEnemy) return;

            Vector2 playerPosition = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPosition;

            if (CloneInsteadOfCrystal) {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            } else {
                currentCrystal.GetComponent<Crystal_Skill_Controller>().FinishCrystal();
            }
        }
    }

    private bool CanUseMultiCrystals() {
        if (canUseMultiCrystal) {
            if (crystalsLeft.Count > 0) {
                if (crystalsLeft.Count == amountOfCrystals)
                    Invoke("ResetAbility", useTimeWindow);

                cooldown = 0;
                GameObject crystalToSpawn = crystalsLeft[crystalsLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalsLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, growSpeed, FindClosestEnemy(newCrystal.transform));

                if (crystalsLeft.Count <= 0) {
                    cooldown = multiCrystalCooldown;
                    RefilCrystal();
                }

                return true;
            }
        }

        return false;
    }

    private void RefilCrystal() {
    int amountToAdd = amountOfCrystals - crystalsLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalsLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility() {
        if (cooldownTimer > 0) return;

        cooldownTimer = multiCrystalCooldown;
        RefilCrystal();
    }
}
