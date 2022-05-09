using UnityEngine;
using UnityEditor;

namespace Editor
{
    public class GameObjectCreatorTool : MonoBehaviour
    {
        static void CreateGameObject(GameObject gameObject)
        {
            gameObject = new GameObject("New GameObject");
            
        }
    }
}