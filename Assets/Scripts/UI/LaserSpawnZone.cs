using Core.Models;
using UnityEngine;

namespace Core.UI
{
    public class LaserSpawnZone : MonoBehaviour
    {
        private Camera _camera;
        [SerializeField]
        private GameSettingsInstaller _settings;
        private GUIContent _content = new GUIContent("Laser Spawn Zone");

        private float Height => _settings.EnemySettings.LaserSpawnZone;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_settings == null) return;

            if (_camera == null) _camera = Camera.main;

            Vector2 size = new Vector2(_camera.orthographicSize * _camera.aspect * 2f, Height);
            Vector3 offset = new Vector3(5f, size.y / 2f, 0f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_camera.transform.position, size);
            UnityEditor.Handles.Label(_camera.transform.position + offset, _content, UnityEditor.EditorStyles.whiteLargeLabel);
        }
#endif
    }
}