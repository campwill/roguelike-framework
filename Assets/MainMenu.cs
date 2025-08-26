
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load your dungeon scene (replace "GameScene" with actual scene name)
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Debug.Log("Will Quit the game once its built, in the editor apparently it does nothing"); 
        Application.Quit(); 
    }
}
