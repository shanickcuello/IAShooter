using UnityEngine;

namespace Features.DebugSystem
{
   public class TimeChanger : MonoBehaviour
   {
      [SerializeField] private float timeScale = 1;

      private void Update()
      {
         Time.timeScale = timeScale;
      }
   }
}
