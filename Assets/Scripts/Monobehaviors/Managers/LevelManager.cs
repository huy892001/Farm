using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    // [SerializeField] TextMeshProUGUI levelText;

    public static Action OnLevelBeginLoaded;
    public static Action OnLevelLoaded;
    public static LevelManager Instance;
    const string levelDataPath = "Levels/Level_";
    LevelData levelData;
    Scene curScene;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void LoadNextLevel()
    {
        OnLevelBeginLoaded?.Invoke();
        Debug.LogError("Load next level");
        StartCoroutine(COLoadNextLevel());
    }
    public IEnumerator COLoadNextLevel()
    {
        int currentLevel = PlayerPrefWrapper.CurrentLevel;
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("level_" + currentLevel));
        Debug.LogError("Unload scene successful");
        if (currentLevel + 1 <= ConstantValue.TOTAL_LEVEL)
        {
            currentLevel++;
            PlayerPrefWrapper.CurrentLevel = currentLevel;
        }
        yield return LoadCurrentLevel();
    }

    public IEnumerator LoadCurrentLevel(bool firstLoad = false)
    {
        OnLevelBeginLoaded?.Invoke();
        FindObjectOfType<LevelTextDisplay>().SetLevel(PlayerPrefWrapper.CurrentLevel);
        // levelText.text = "Level: " + PlayerPrefWrapper.CurrentLevel.ToString();
        Debug.LogError("Current level: " + PlayerPrefWrapper.CurrentLevel);
        curScene = SceneManager.GetSceneByName("level_" + PlayerPrefWrapper.CurrentLevel);
        if (curScene.isLoaded)
        {
            yield return StartCoroutine(UnloadAndLoadNewScene());
        } else
        {
            yield return StartCoroutine(COLoadCurrentLevelScene("level_" + PlayerPrefWrapper.CurrentLevel, firstLoad));
        }
    }
    public IEnumerator UnloadAndLoadNewScene()
    {
        yield return SceneManager.UnloadSceneAsync(curScene);
        yield return COLoadCurrentLevelScene("level_" + PlayerPrefWrapper.CurrentLevel);
    } 
    public IEnumerator COLoadCurrentLevelScene(string sceneName, bool firstLoad = false)
    {
        if (!firstLoad)
        {
            Debug.LogError("In hereeeeeeeeeeeeee");
            yield return ScreenFader.Instance.FadeIn();
        }
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (!firstLoad)
        {
            yield return ScreenFader.Instance.FadeOut();
        }
        OnLevelLoaded?.Invoke();
        Inventory.Instance.Reset();
        levelData = Resources.Load<LevelData>(levelDataPath + GetCurrentLevel()); 
        StartGameDialog startGameDialog = DialogController.Instance.ShowDialog(DialogType.STARTGAME) as StartGameDialog;
        startGameDialog.SetLevelData(levelData);
        FindObjectOfType<GoldManager>().CurrentGold = levelData.GetInitCoin();
        FindObjectOfType<MissionBar>().LoadMissionData(levelData);
        FindObjectOfType<SeedBarManager>().DisplaySeedItems();
        if (levelData.HasTime())
        {
            FindObjectOfType<LevelTimer>().SetTime(levelData.GetTime());
        }
        GameManager.Instance.isFinished = false;
    }

    public int GetCurrentLevel()
    {
        int currentLevel = PlayerPrefWrapper.CurrentLevel;
        return currentLevel;
    }
}
