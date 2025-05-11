using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public static WinScreen Instance;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject winCanvasPrefab;
    [SerializeField] string mainMenuScene = "MainMenuScene";


    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (winPanel && winPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;              // in case you paused time
            SceneManager.LoadScene(mainMenuScene);
        }
    }

    public void Show()
    {
        if (!winPanel) EnsureCanvas();
        winPanel.SetActive(true);
    }

    void EnsureCanvas()
    {
        var panel = GameObject.FindWithTag("WinPanel");
        if (panel) { winPanel = panel; return; }

        var canvas = Instantiate(winCanvasPrefab);
        winPanel = canvas.transform.Find("WinPanel").gameObject;
        winPanel.SetActive(false);
    }
}
