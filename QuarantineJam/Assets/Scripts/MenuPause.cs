using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuPause : MonoBehaviour
{
    public GameObject button;

    public EventSystem eventSystem;

    public void Buton()
    {
        StartCoroutine(FocusEventSystem(button));
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PlayCredit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    public IEnumerator FocusEventSystem(GameObject button)
    {
        yield return null;
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(button);
    }
}
