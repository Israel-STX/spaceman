using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [Tooltip("Scene to load after win (leave blank to just pause)")]
    public string nextSceneName = "";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || other.isTrigger) return;

        // 1. disable player controls
        other.GetComponent<SpaceManController>().enabled = false;

        // 2. optional: freeze time so he stops moving
        Time.timeScale = 0f;

        // 3. show a simple win panel (spawned from prefab)
        WinScreen.Instance.Show();          // see step 3

        // 4. OR auto‑load next scene after short delay
        if (!string.IsNullOrEmpty(nextSceneName))
            StartCoroutine(LoadNext());
    }

    System.Collections.IEnumerator LoadNext()
    {
        yield return new WaitForSecondsRealtime(2f); // 2 s freeze
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }
}
