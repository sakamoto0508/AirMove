using UnityEngine;

public class EnemyRamieru : MonoBehaviour
{
    private EnemyMoveAir _enemyMoveAir;
    private void Awake()
    {
        _enemyMoveAir=GetComponent<EnemyMoveAir>();
        _enemyMoveAir.EnemyDamageAction += PlayAnimation;
    }

    private void OnDestroy()
    {
        _enemyMoveAir.EnemyDamageAction -= PlayAnimation;
    }

    private void PlayAnimation()
    {

    }
}
