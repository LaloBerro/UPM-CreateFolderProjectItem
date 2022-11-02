using System.IO;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [InitializeOnLoad]
    public class CreateFolderProjectItem
    {
        static CreateFolderProjectItem()
        {
            EditorApplication.projectWindowItemOnGUI += DrawAssetDetails;
        }

        private static void DrawAssetDetails(string guid, Rect rect)
        {
            string directoryPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string thisItemPath = AssetDatabase.GUIDToAssetPath(guid);

            if (string.IsNullOrEmpty(directoryPath) || directoryPath != thisItemPath)
                return;

            rect.x += rect.width;
            rect.x -= 20;
            rect.width = 25;

            if (GUI.Button(rect, EditorGUIUtility.IconContent("d_ol_plus"), EditorStyles.iconButton))
            {
                string newFolderPath = Path.Combine(directoryPath, "New Folder");
                Directory.CreateDirectory(newFolderPath);

                AssetDatabase.Refresh();

                Selection.selectionChanged += ActiveRename;

                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(newFolderPath);
            }

        }

        private static void ActiveRename()
        {
            Selection.selectionChanged -= ActiveRename;

            var keyEvent = new Event { keyCode = KeyCode.F2, type = EventType.KeyDown };
            EditorWindow.focusedWindow.SendEvent(keyEvent);
        }
    }
}