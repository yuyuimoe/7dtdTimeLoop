using System;
using TimeLoop.Enums;
using TimeLoop.Helpers;
using TimeLoop.Repositories;
using ContentData = TimeLoop.Serializer.XmlContentData;


namespace TimeLoop.Modules
{
    public class TimeLooper
    {
        private readonly ContentData _contentData;
        private double _unscaledTimeStamp;
        public bool IsTimeFlowing { get; private set; } = true;

        public TimeLooper(ContentData contentData)
        {
            _contentData = contentData;
        }

        public void UpdateLoopState()
        {
            var plyDataRepo = new PlayerDataRepository(_contentData);
            bool newState = _contentData.EnableTimeLooper && _contentData.Mode switch
            {
                EMode.Whitelist => plyDataRepo.IsAuthPlayerOnline(),
                EMode.Threshold => plyDataRepo.IsMinPlayerThreshold(),
                EMode.WhitelistedThreshold => plyDataRepo.IsMinAuthPlayerThreshold(),
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

        public static implicit operator bool(TimeLooper instance)
        {
            return instance != null;
        }
    }
}
