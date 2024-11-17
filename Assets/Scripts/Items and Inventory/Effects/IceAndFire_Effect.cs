using UnityEngine;

[CreateAssetMenu(fileName = "IceAndFire_Effect", menuName = "Data/Item Effect/Ice and Fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform _respawnPosition) {
        Player player = PlayerManager.instance.player;
        bool thirdAttack = player.primaryAttackState.comboCounter == 2;

        if (thirdAttack) {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(xVelocity * player.facingDir, 0);

            Destroy(newIceAndFire, 10);
        }
    }
}
