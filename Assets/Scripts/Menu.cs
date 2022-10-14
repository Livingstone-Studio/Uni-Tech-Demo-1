using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private GameObject menu;

    private void Start()
    {
        if (menu)
        {
            menu.SetActive(false);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleMenu()
    {
        if (menu)
        {
            menu.SetActive(true);
        }
    }
}
