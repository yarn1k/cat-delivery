using UnityEngine;

namespace Core.Player.Buffs
{
    public class BuffView : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out PlayerView player))
            {

            }
        }
    }
}
