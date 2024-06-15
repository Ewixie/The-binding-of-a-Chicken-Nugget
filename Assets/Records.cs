
using Data;
using TMPro;
using UnityEngine;

public class Records : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalKills;
    [SerializeField] private TextMeshProUGUI maxKillsPerRun;
    [SerializeField] private TextMeshProUGUI maxStageAchieved;

    public void CheckProgress()
    {
        RecordsData data = ProgressManager.Instance.GetRecordsData(); 
        totalKills.text = data.totalEnemiesKilled.ToString();
        maxKillsPerRun.text = data.inRunMaxEnemiesKilled.ToString();
        maxStageAchieved.text = data.maxStageAchieved.ToString();
    }
}
