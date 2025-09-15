using UnityEngine;

public class EnemyRamieruChild : MonoBehaviour
{
    private EnemyRamieru _enemyRamieru;
    public void Initialize(EnemyRamieru parent)
    {
        _enemyRamieru = parent;
    }

    /// <summary>
    /// �R�A�Ƀ_���[�W���󂯂�����
    /// </summary>
    public void OnRaycastHit()
    {
        if(_enemyRamieru != null)
        {
            _enemyRamieru.TakeDamageFormChild();
        }
    }
}
