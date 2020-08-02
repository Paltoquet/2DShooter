using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNavigationController : MonoBehaviour
{
    // Start is called before the first frame update

    private string m_mainMenuSceneName;

    void Start()
    {
        m_mainMenuSceneName = "Scenes/LoginScreen"; 
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.keyCode == KeyCode.Escape)
        {
            SceneManager.LoadScene(m_mainMenuSceneName, LoadSceneMode.Single);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
