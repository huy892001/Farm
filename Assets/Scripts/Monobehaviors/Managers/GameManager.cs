using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance;
    public bool isFinished = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(Instance.gameObject);
    }

    private void Start()
    {
        // Inventory.Instance.Reset();
        // LevelManager.Instance.LoadCurrentLevel(true);
    }

    public void Win()
    {
        if (isFinished) return;
        isFinished = true;
        FindObjectOfType<LevelTimer>().Pause();
        DialogController.Instance.ShowDialog(DialogType.WIN);
        Inventory.Instance.Reset();
    }
    public void Lose()
    {
        if (isFinished) return;
        isFinished = true;
        Inventory.Instance.Reset();
        DialogController.Instance.ShowDialog(DialogType.LOSE);
    }
}
