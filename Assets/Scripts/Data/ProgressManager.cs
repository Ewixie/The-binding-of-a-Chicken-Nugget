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
            if (_recordsData is null) _recordsData = (RecordsData) DataBase.Read<RecordsData>();
            if (_recordsData is null)
            {
                _recordsData = new RecordsData();
                DataBase.Save(_recordsData);
            }
            return _recordsData;
        }
        public CurrentRunData GetCurrentRunData()
        {
            if (_currentRunData is null) _currentRunData = (CurrentRunData) DataBase.Read<CurrentRunData>();
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


        private void OnApplicationQuit()
        {
            if (_currentRunData.enemiesKilled > _recordsData.inRunMaxEnemiesKilled)
            {
                _recordsData.inRunMaxEnemiesKilled = _currentRunData.enemiesKilled;
            }

            if (_currentRunData.stage > _recordsData.maxStageAchieved)
            {
                _recordsData.maxStageAchieved = _currentRunData.stage;
            }
            
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
        
        
    }
}