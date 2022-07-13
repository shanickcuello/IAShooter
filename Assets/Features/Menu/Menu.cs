using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.Menu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button ExitButton;
        [SerializeField] private Button ResetButton;

        private void Awake()
        {
            ResetButton.onClick.AddListener(ResetApplication);
            ExitButton.onClick.AddListener(ExitSimulation);
        }

        private void ResetApplication()
        {
            Debug.Log("Reset app");
            SceneManager.LoadScene(0);
        }

        private void ExitSimulation()
        {
            Debug.Log("Exit app");
            Application.Quit();
        }
    }
}