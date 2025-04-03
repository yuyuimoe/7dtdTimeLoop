using System;
using ContentData = TimeLoop.Serializer.XmlContentData;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TimeLoop.Enums;
using TimeLoop.Helpers;
using TimeLoop.Repository;


namespace TimeLoop.Modules.TimeLoop
{
    public class TimeLooper
    {
        private readonly ContentData _contentData;
        private double _unscaledTimeStamp;
        public bool isLooping { get; private set; } = true;

        public TimeLooper(ContentData contentData)
        {
            _contentData = contentData;
        }

        public void UpdateLoopState()
        {
            var plyDataRepo = new PlayerDataRepository(_contentData);
            isLooping = _contentData.Mode switch
            {
                EMode.Whitelist => !plyDataRepo.IsAuthPlayerOnline(),
                EMode.MinPlayerCount => !plyDataRepo.IsMinPlayerThreshold(),
                EMode.MinWhitelistPlayerCount => !plyDataRepo.IsMinAuthPlayerThreshold(),
                _ => false
            };
            Log.Out("[TimeLoop] Loop state updated to: " + isLooping);
        }

        public void CheckForTimeLoop()
        {
            if (Math.Abs(_unscaledTimeStamp - UnityEngine.Time.unscaledTimeAsDouble) <= 0.1)
                return;

            if (!isLooping)
                return;
            
            var worldTime = GameManager.Instance.World.GetWorldTime();
            var dayTime = worldTime % 24000;

            if (dayTime <= 10)
            {
                Log.Out("[TimeLoop] Time Reset.");
                MessageHelper.SendGlobalChat("Resetting day. Please wait.");
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
