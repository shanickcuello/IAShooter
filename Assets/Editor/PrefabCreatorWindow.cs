using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PrefabCreatorWindow : EditorWindow
    {
        GameObject prefab;
        private float startPositionX, startPositionY, startPositionZ;
        private int amountOfTilesX, amountOfTilesY;

        [MenuItem("Window/Prefab Creator")]
        public static void ShowWindow()
        {
            var window = GetWindow<PrefabCreatorWindow>();
            window.position = new Rect(0, 0, 80, 30);
            window.Show();
        }


        void OnGUI()
        {
            GUILayout.Label("Tile Creator", EditorStyles.boldLabel);
            GUILayout.Space(50);
            prefab = EditorGUI.ObjectField(new Rect(
                    10,
                    20,
                    position.width - 20, 20),
                "Tile", prefab, typeof(GameObject),
                false) as GameObject;
            //space 
            GUILayout.Space(20);

            startPositionX = EditorGUI.FloatField(new Rect(10, 50, 200, 20),
                "Start position X",
                startPositionX);

            GUILayout.Space(20);


            startPositionY = EditorGUI.FloatField(new Rect(10, 70, 200, 20),
                "Start position y",
                startPositionY);

            startPositionZ = EditorGUI.FloatField(new Rect(10, 90, 200, 20),
                "Start position z",
                startPositionZ);

            amountOfTilesX = EditorGUI.IntField(new Rect(10, 110, 200, 20),
                "Amount tiles in X",
                amountOfTilesX);

            amountOfTilesY = EditorGUI.IntField(new Rect(10, 130, 200, 20),
                "Amount tiles in Y",
                amountOfTilesY);


            GUILayout.Space(80);

            if (GUILayout.Button("Create tiles"))
            {
                if (prefab == null)
                {
                    Debug.Log("Please put a prefab on the field");
                    return;
                }

                CreatePrefabs();
            }
        }

        private void CreatePrefabs()
        {
            var tileFather = new GameObject("TileContainer");
            var currentPositionX = startPositionX;
            var currentPositionY = startPositionX;
            var currentPositionZ = startPositionZ;
            
            for (int i = 0; i < amountOfTilesX; i++)
            {
                for (int j = 0; j < amountOfTilesY; j++)
                {
                    var tile = Instantiate(prefab, tileFather.transform, true);
                    tile.transform.position = new Vector3(currentPositionX + i, currentPositionY, startPositionZ + j);
                }
                
            }
        }
    }
}