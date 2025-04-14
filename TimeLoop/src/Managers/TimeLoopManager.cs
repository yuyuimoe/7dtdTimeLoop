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
        private int _timesLooped = 0;
        public bool IsTimeFlowing { get; private set; } = true;
        private bool IsDaySkippable() => !IsTimeFlowing && ConfigManager.Instance.Config.DaysToSkip > 0;

        private bool IsLoopLimitReached() => this._timesLooped >= ConfigManager.Instance.Config.LoopLimit && ConfigManager.Instance.Config.LoopLimit != 0;

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
                        if(ConfigManager.Instance.Config.DaysToSkip > 0)
                            Log.Out("[TimeLoop] Resetting days to skip the loop.");
                        ConfigManager.Instance.Config.DaysToSkip = 0;
                        ConfigManager.Instance.SaveToFile();
                        MessageHelper.SendGlobalChat("[TimeLoop] Time flows normally.");
                        break;
                }

                IsTimeFlowing = newState;
            }
            Log.Out($"[TimeLoop] Is Time Flowing? {IsTimeFlowing} | Days to skip: {ConfigManager.Instance.Config.DaysToSkip}");
        }

        private void SkipLoop()
        {
            GameManager.Instance.World.worldTime += 20;
            MessageHelper.SendGlobalChat("[TimeLoop] Skipping the loop for today.");
            if (ConfigManager.Instance.DecreaseDaysToSkip() > 0)
                MessageHelper.SendGlobalChat($"[TimeLoop] The following {ConfigManager.Instance.Config.DaysToSkip} day(s) will NOT loop");
            Log.Out("[TimeLoop] Skipping the loop for day. Remaining: {0} days", ConfigManager.Instance.Config.DaysToSkip);
        }
        
        private void LoopDay()
        {
            if (IsDaySkippable())
            {
                SkipLoop();
                return;
            }
            Log.Out("[TimeLoop] Time Reset.");
            MessageHelper.SendGlobalChat("[TimeLoop] Resetting day");
            var previousDay = GameUtils.WorldTimeToDays(GameManager.Instance.World.GetWorldTime()) - 1;
            GameManager.Instance.World.SetTime(GameUtils.DaysToWorldTime(previousDay) + 20);
        }
        
        private void LimitedLoop()
        {
            if (!IsLoopLimitReached())
            {
                LoopDay();
                this._timesLooped++;
                return;
            }
            Log.Out("[TimeLoop] Loop limit reached.");
            MessageHelper.SendGlobalChat("[TimeLoop] Loop limit reached.");
            SkipLoop();
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
                LimitedLoop();
            }
            _unscaledTimeStamp = UnityEngine.Time.unscaledTimeAsDouble;
        }

        public static implicit operator bool(TimeLoopManager? instance)
        {
            return instance != null;
        }
    }
}
