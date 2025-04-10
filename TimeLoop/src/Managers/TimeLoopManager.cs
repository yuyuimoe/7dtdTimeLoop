using System;
using TimeLoop.Enums;
using TimeLoop.Helpers;
using TimeLoop.Models;
using TimeLoop.Repositories;

namespace TimeLoop.Managers
{
    public class TimeLoopManager
    {
        #region Singleton
        private static TimeLoopManager? _instance;
        public static TimeLoopManager Instance{
            get { return _instance ??= new TimeLoopManager(); }
        }
        public static void Instantiate() => _instance = new TimeLoopManager();
        #endregion
        
        private double _unscaledTimeStamp;
        public bool IsTimeFlowing { get; private set; } = true;

        public void UpdateLoopState()
        {
            var plyDataRepo = new PlayerRepository();
            bool newState = ConfigManager.Instance.Config.Enabled && ConfigManager.Instance.Config.Mode switch
            {
                EMode.Whitelist => plyDataRepo.IsAuthPlayerOnline(),
                EMode.Threshold => plyDataRepo.IsMinPlayerThreshold(),
                EMode.WhitelistedThreshold => plyDataRepo.IsMinAuthPlayerThreshold(),
                EMode.Always => false,
                _ => false
            };
            
            if (newState != IsTimeFlowing)
            {
                switch (newState)
                {
                    case false:
                        MessageHelper.SendGlobalChat("[TimeLoop] You seem to be stuck on the same day.");
                        break;
                    case true:
                        MessageHelper.SendGlobalChat("[TimeLoop] Time flows normally.");
                        break;
                }

                IsTimeFlowing = newState;
            }
            Log.Out("[TimeLoop] Is time flowing? " + IsTimeFlowing);
        }

        public void CheckForTimeLoop()
        {
            if (Math.Abs(_unscaledTimeStamp - UnityEngine.Time.unscaledTimeAsDouble) <= 0.1)
                return;

            if (IsTimeFlowing)
                return;
            
            var worldTime = GameManager.Instance.World.GetWorldTime();
            var dayTime = worldTime % 24000;

            if (dayTime <= 10)
            {
                Log.Out("[TimeLoop] Time Reset.");
                MessageHelper.SendGlobalChat("[TimeLoop] Resetting day");
                var previousDay = GameUtils.WorldTimeToDays(worldTime) - 1;
                GameManager.Instance.World.SetTime(GameUtils.DaysToWorldTime(previousDay) + 20);
            }
            
            _unscaledTimeStamp = UnityEngine.Time.unscaledTimeAsDouble;
        }

        public static implicit operator bool(TimeLoopManager instance)
        {
            return instance != null;
        }
    }
}
