using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    // Assign these in the Inspector
    public GameObject continueButton; // The actual Button GameObject in your Canvas

    private void Awake()
    {
        // Basic singleton pattern, optional
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Check for save data. If no save, disable the continue button.
        bool hasSave = CheckIfSaveFileExists();
        continueButton.SetActive(hasSave);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ContinueGame()
    {
        // Load saved data logic here, or just a debug for now
        Debug.Log("Continue Game clicked - load logic goes here!");
        // Example:
        // SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game clicked - application will close if built.");
    }

    // Example placeholder function
    private bool CheckIfSaveFileExists()
    {
        // Replace with your real save-check logic:
        // e.g. check a file on disk, check PlayerPrefs, etc.
        return false;
    }
}
