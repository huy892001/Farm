using TMPro;
using UnityEngine;

public class StartGameDialog : Dialog 
{
    [SerializeField] TextMeshProUGUI timeTxt, goldText;
    [SerializeField] RectTransform startItemParent, targetItemParent;
    [SerializeField] ItemUI startItemPrefab, targetItemPrefab; 

    protected override void Start()
    {
        base.Start();
        title.text = "LEVEL " + LevelManager.Instance.GetCurrentLevel();
    }
    public void SetLevelData(LevelData levelData) {
        for (int i = 0; i < levelData.GetInitSeeds().Length; i++) {
            ItemUI instance = Instantiate(startItemPrefab, startItemParent); 
            instance.SetItemHolder(levelData.GetInitSeeds()[i]);
        }
        MissionBase[] missions = levelData.GetMissions();;
        foreach (var mission in missions) {
            ItemHolder[] missionItems = mission.GetItems(); 
            for (int i = 0; i < missionItems.Length; i++) {
                ItemUI instance = Instantiate(targetItemPrefab, targetItemParent); 
                instance.SetItemHolder(missionItems[i]);
            }
        } 
    }
    public override void Close() {
        Debug.Log("Close dialog");
        FindObjectOfType<LevelTimer>().CountDownTime();
        base.Close();
    }
}
