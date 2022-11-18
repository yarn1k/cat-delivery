using Core.Models;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.UI
{
    public class SceneGizmos : MonoBehaviour
    {
        private Camera _camera;
        [SerializeField]
        private GameSettingsInstaller _settings;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_settings == null) return;
            
            if (Selection.activeObject != null && Selection.activeObject.Equals(_settings))
            {
                if (_camera == null) _camera = Camera.main;

                DrawLaserSpawnZone();
                DrawCatsSpawnZone();
            }
        }

        private void DrawLaserSpawnZone()
        {
            Vector3 size = new Vector3(_camera.orthographicSize * _camera.aspect * 2f, _settings.EnemySettings.LaserSpawnHeight);
            Vector3 offset = new Vector3(5f, size.y / 2f, 0f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_camera.transform.position, size);
            Handles.Label(_camera.transform.position + offset, "Laser Spawn Zone", EditorStyles.whiteLargeLabel);

            float x1 = Mathf.Cos(Mathf.Deg2Rad * _settings.EnemySettings.LaserAngle) * 5f;
            float y1 = Mathf.Sin(Mathf.Deg2Rad * _settings.EnemySettings.LaserAngle) * 5f;
            Vector3 startPos = _camera.transform.position + Vector3.left * size.x / 2f;
            Vector3 endPos1 = new Vector3(x1, y1);

            float x2 = Mathf.Cos(-Mathf.Deg2Rad * _settings.EnemySettings.LaserAngle) * 5f;
            float y2 = Mathf.Sin(-Mathf.Deg2Rad * _settings.EnemySettings.LaserAngle) * 5f;
            Vector3 endPos2 = new Vector3(x2, y2);

            Handles.DrawDottedLine(startPos, startPos + endPos1, 2f);
            Handles.DrawDottedLine(startPos, startPos + endPos2, 2f);
            Handles.DrawWireArc(startPos, Vector3.forward, endPos2, _settings.EnemySettings.LaserAngle * 2f, 5f);
            Handles.Label(startPos, $"Angle: {_settings.EnemySettings.LaserAngle}°", EditorStyles.whiteLargeLabel);
        }
        private void DrawCatsSpawnZone()
        {
            Vector2 size = new Vector2(_settings.CatsSettings.SpawnWidth, 0.5f);
            Vector3 offset = new Vector3(0f, _camera.orthographicSize, 0f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_camera.transform.position + offset, size);
        }
#endif
    }
}