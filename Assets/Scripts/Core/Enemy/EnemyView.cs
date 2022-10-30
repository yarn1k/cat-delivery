using System.Collections;
using UnityEngine;

namespace Core.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField]
        private Laser _laser;

        public void ShootLaser()
        {
            StartCoroutine(LaserCoroutine(1.5f));
        }
        private IEnumerator LaserCoroutine(float duration)
        {
            _laser.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            _laser.gameObject.SetActive(false);
        }    
    }
}
