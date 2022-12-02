using UnityEngine;

namespace Core.UI
{
    public sealed class UIManager
    {
        public enum CursorType : byte
        {
            Default = 0
        }

        public void ShowCursor(bool isShow)
        {
            Cursor.visible = isShow;
        }
        public void SetCursor(CursorType type)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
