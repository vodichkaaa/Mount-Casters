using UnityEngine;

public class DamageBoss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BossManager bossManager))
        {
            BossManager.Instance.health--;
            BossManager.Instance.healthText.text = BossManager.Instance.health.ToString();
            BossManager.Instance.healthBar.value = BossManager.Instance.health;
        }
    }
}
