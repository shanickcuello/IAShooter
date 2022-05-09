using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PrefabCreatorWindow : EditorWindow
    {
        GameObject prefab;

        [MenuItem("Window/Prefab Creator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(PrefabCreatorWindow));
        }


        void OnGUI()
        {
            GUILayout.Label("Tile Creator", EditorStyles.boldLabel);
            GUILayout.Space(50);
            prefab = EditorGUI.ObjectField(new Rect(
                    10,
                    10,
                    position.width - 20, 20),
                "Tile", prefab, typeof(GameObject),
                false) as GameObject;
            //space 
            GUILayout.Space(20);

            if (GUILayout.Button("Create Prefab"))
            {
                if (prefab == null)
                {
                    Debug.Log("Pls put a prefab on the field");
                    return;
                }
                CreatePrefabs();
            }
        }

        private void CreatePrefabs()
        {
            var tileFather = new GameObject("TileContainer");
            
            var tile =  Instantiate(prefab);
            tile.transform.position = Vector3.zero;

        }
    }
}