using System;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelBounds : MonoBehaviour
    {
        public Vector2 Size { get; private set; }

        private void Awake()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            Camera camera = Camera.main;
            Size = new Vector2(camera.orthographicSize * camera.aspect * 2f, camera.orthographicSize * 2f);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDisposable disposableObj))
            {
                disposableObj.Dispose();
            }
        }
    }
}
