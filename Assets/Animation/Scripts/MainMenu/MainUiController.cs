using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUiController : MonoBehaviour
{

    public Button arenaButton;
    public Button characterCreationButton;

    private string m_arenaSceneName;
    private string m_characterCreationSceneName;

    // Start is called before the first frame update
    void Start()
    {
        m_arenaSceneName = "Scenes/Main";
        m_characterCreationSceneName = "Scenes/CharacterCreation";

        arenaButton.onClick.AddListener(startArena);
        characterCreationButton.onClick.AddListener(startCharacterCreation);
    }


    void startArena()
    {
        SceneManager.LoadScene(m_arenaSceneName, LoadSceneMode.Single);
    }

    void startCharacterCreation()
    {
        SceneManager.LoadScene(m_characterCreationSceneName, LoadSceneMode.Single);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
