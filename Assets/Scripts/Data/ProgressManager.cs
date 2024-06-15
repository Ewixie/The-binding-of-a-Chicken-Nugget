using System;
using UnityEngine;

namespace Data
{
    public class ProgressManager : MonoBehaviour
    {
        public static ProgressManager Instance;

        private RecordsData _recordsData;
        private CurrentRunData _currentRunData;

        public RecordsData GetRecordsData()
        {
            if (_recordsData is null) _recordsData = (RecordsData)DataBase.Read<RecordsData>();
            if (_recordsData is null)
            {
                _recordsData = new RecordsData();
                DataBase.Save(_recordsData);
            }

            return _recordsData;
        }

        public CurrentRunData GetCurrentRunData()
        {
            if (_currentRunData is null) _currentRunData = (CurrentRunData)DataBase.Read<CurrentRunData>();
            if (_currentRunData is null)
            {
                _currentRunData = new CurrentRunData();
                DataBase.Save(_currentRunData);
            }

            return _currentRunData;
        }

        public void CreateNewRunData()
        {
            _currentRunData = new CurrentRunData();
        }

        public void UpdateRecord()
        {
            if (GetCurrentRunData().enemiesKilled > GetRecordsData().inRunMaxEnemiesKilled)
            {
                GetRecordsData().inRunMaxEnemiesKilled = GetCurrentRunData().enemiesKilled;
            }

            if (GetCurrentRunData().stage > GetRecordsData().maxStageAchieved)
            {
                GetRecordsData().maxStageAchieved = GetCurrentRunData().stage;
            }
        }


        private void OnApplicationQuit()
        {
            UpdateRecord();
            DataBase.Save(_recordsData);
            DataBase.Save(_currentRunData);
        }

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}