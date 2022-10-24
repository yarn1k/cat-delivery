using System.Collections;
using UnityEngine;

namespace Core.Enemy
{
    public class Enemy : MonoBehaviour
    {
        private float _speed = 3f;
        private float _waitTime = 0.5f;
        [SerializeField]
        private Transform[] _moveSpots;
        private Vector3 _toPosition;
        private bool _isMoveUp;

        void Start()
        {
            StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            if (_moveSpots.Length != 2) yield break;

            while (true)
            {
                if (_isMoveUp)
                {
                    _isMoveUp = false;
                    _toPosition = _moveSpots[0].position;
                }
                else
                {
                    _isMoveUp = true;
                    _toPosition = _moveSpots[1].position;
                }

                while (Vector3.Distance(transform.position, _toPosition) > 0.2f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _toPosition, _speed * Time.deltaTime);
                    yield return null;
                }
                yield return new WaitForSeconds(_waitTime);
            }
        }
    }
}
