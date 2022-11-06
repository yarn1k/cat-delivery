using UnityEngine;

namespace Core.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [field: SerializeField] public Transform FirePoint { get; private set; }   
    }
}
