using UnityEngine;
using Core.Weapons;

namespace Core.Enemy
{
    public class EnemyModel
    {
        public readonly float MovementSpeed;
        public Vector2 AttackCooldownInterval;
        public IWeapon PrimaryWeapon { get; set; }

        public EnemyModel(float movementSpeed, Vector2 attackCooldownInterval)
        {
            MovementSpeed = movementSpeed;
            AttackCooldownInterval = attackCooldownInterval;
        }
    }
}
