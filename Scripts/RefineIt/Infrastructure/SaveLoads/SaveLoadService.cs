using System;
using System.Globalization;
using Infrastructure.PersistenceProgress;
using UnityEngine;

namespace Infrastructure.SaveLoads
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IProgressService _progressService;

        public SaveLoadService(IProgressService progressService)
        {
            _progressService = progressService;
        }

        private const string SavesKey = "Saves";

        public void Load()
        {
            _progressService.InitializeProgress(GetOrCreate());
        }

        public void Save()
        {
            _progressService.PlayerProgress.LastSession = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            PlayerPrefs.SetString(SavesKey, JsonUtility.ToJson(_progressService.PlayerProgress));
        }

        private PlayerProgress GetOrCreate()
        {
            if(PlayerPrefs.HasKey(SavesKey))
            {
                var saves = PlayerPrefs.GetString(SavesKey);
                return JsonUtility.FromJson<PlayerProgress>(saves);
            }
            
            return new PlayerProgress();
        }
    }
}