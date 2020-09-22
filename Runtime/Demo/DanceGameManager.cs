using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.AddressableAssets;
using Fuxi.Infrastructure.Utility;

namespace Fuxi.DanceGameKit
{
    public class PlayerProfile : JsonIO<PlayerProfile>
    {
        public string playerName;
        public string danceName;
        public float score;
    }

    public class PlayerHistory : JsonIO<PlayerProfile>
    {
        public List<PlayerProfile> profiles;
    }

    public class Grade : JsonIO<Grade>
    {
        public string displayName;
        public string fxAddressable;
        public float gradeRangeMin;
        public float gradeRangeMax;
        public float score;
    }

    public class GradeConfig : JsonIO<GradeConfig>
    {
        public List<Grade> definitions;
    }

    public class DanceGameManager : Singleton<DanceGameManager>
    {
        public GradeConfig m_GradeConfig;
        public PlayerHistory m_PlayerHistory;
        public List<DanceGameDiskData> m_Disks;
        public DanceGameDiskPlayer m_DiscPlayer;

        public void OnDanceClipStart()
        {
            m_DiscPlayer.OnCheckPointStart();
        }

        public void OnDanceClipEnd()
        {
            m_DiscPlayer.OnCheckPointEnd();
        }

        private new void Awake()
        {
            base.Awake();
            m_DiscPlayer = new DanceGameDiskPlayer(m_GradeConfig);
            m_DiscPlayer.GameEventNotifier += (p, e) =>
            {
                if (e != DanceGameDiskPlayer.GameFlowEvent.OnGameLoaded) return;
            //When to play

        };

            m_DiscPlayer.GameEventNotifier += (p, e) =>
            {
                if (e != DanceGameDiskPlayer.GameFlowEvent.OnEnterScoring) return;
            };

            m_DiscPlayer.GameEventNotifier += (p, e) =>
            {
                if (e != DanceGameDiskPlayer.GameFlowEvent.OnEndScoring) return;
            };

            m_DiscPlayer.GameEventNotifier += (p, e) =>
            {
                if (e != DanceGameDiskPlayer.GameFlowEvent.OnGameEnd) return;
            };
        }

        private void Start()
        {
            m_DiscPlayer.LoadDisk(m_Disks[0]);
        }

        private void Update()
        {
            m_DiscPlayer.Update();
        }
    }
}