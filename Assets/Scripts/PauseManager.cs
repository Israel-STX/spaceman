using UnityEngine;
using UnityEngine.SceneManagement; 

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("UI")]
    [SerializeField] GameObject pausePanel;     
    [SerializeField] GameObject pauseCanvasPrefab; 

    bool isPaused;



    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 🔽 guarantee hidden before first frame
        if (pausePanel && pausePanel.activeSelf)
            pausePanel.SetActive(false);
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void Start() => EnsurePanelExists();

    void OnSceneLoaded(Scene scn, LoadSceneMode mode)
    {
        isPaused = false;
        Time.timeScale = 1f;         
        AudioListener.pause = false;
        EnsurePanelExists();
    }

    void EnsurePanelExists()
    {
        if (pausePanel) return;                         // already valid

        // try tag
        var panel = GameObject.FindWithTag("PausePanel");
        if (panel)
        {
            pausePanel = panel;
            pausePanel.SetActive(false);
            return;
        }

        // fallback prefab
        if (pauseCanvasPrefab)
        {
            var canvas = Instantiate(pauseCanvasPrefab);
            pausePanel = canvas.transform.Find("PausePanel").gameObject;
            canvas.SetActive(false);
            DontDestroyOnLoad(canvas);
        }
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (!pausePanel) EnsurePanelExists();  
        if (!pausePanel)                        
        {
            Debug.LogWarning("PauseManager: no PausePanel in this scene.");
            return;
        }

        if (WinScreen.Instance && WinScreen.Instance.gameObject.activeInHierarchy)
            return;

        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        AudioListener.pause = isPaused;
    }


    public void QuitToMenu() => SceneManager.LoadScene("MainMenuScene");
}
