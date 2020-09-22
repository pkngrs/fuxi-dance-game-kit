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

    public interface IInputPostureProvider
    {
        Vector3[] GetLandmarkPoints();
    }

    public interface IScoringMechanism
    {
        float GetRawScore(Vector3[] referenceLandmarks, Vector3[] targetLandmarks, float[] weights);
    }

    public class DanceGameDiskPlayer
    {
        public GameFlowEvent currentState { get { return m_CurrentState; } }
        public double currentGameTime { get { return BetweenFlowState(currentState, GameFlowEvent.OnGameStart, GameFlowEvent.OnGameEnd) ? m_CurrentTime : -1; } }
        public float currentCheckPointScore;
        public IScoringMechanism scoreMechanism { set { m_ScoringMechanism = value; } }
        public IInputPostureProvider inputProvider { set { m_InputProvider = value; } }
        public bool freezed = true;
        public Action<DanceGameDiskPlayer, GameFlowEvent> GameEventNotifier;

        public enum GameFlowEvent
        {
            OnInitialized = 0,
            OnGameLoaded,
            OnGameStart,
            OnEnterScoring,
            OnEndScoring,
            OnGameEnd,
            OnExit
        }

        public enum InputMode
        {
            Manual,
            Simulated,
            Automatical
        }

        public DanceGameDiskPlayer(GradeConfig config)
        {
            m_GradeConfig = config;
            EnterFlowState(GameFlowEvent.OnInitialized);
        }

        public void LoadDisk(DanceGameDiskData disk)
        {
            if (freezed) return;
            var request = Addressables.InstantiateAsync(disk.diskPrefabAddressable);
            request.Completed += (x) =>
            {
                m_CurrentDisk = disk;
                m_CurrentDisk.Mount();
                EnterFlowState(GameFlowEvent.OnGameLoaded);
            };
        }

        public void UnloadDisk()
        {
            if (freezed) return;
            if (m_CurrentDisk != null)
            {
                m_CurrentDisk.Unmount();
                m_CurrentDisk = null;
                EnterFlowState(GameFlowEvent.OnExit);
            }
        }

        public void PlayImmediately()
        {
            if (freezed) return;
            if (m_CurrentDisk != null)
            {
                m_CurrentCheckPointIndex = 0;
                m_CurrentDisk.director.Play();
                EnterFlowState(GameFlowEvent.OnGameStart);
            }
        }

        public void StopImmediately()
        {
            if (freezed) return;
            if (m_CurrentDisk != null && BetweenFlowState(currentState, GameFlowEvent.OnGameStart, GameFlowEvent.OnGameEnd))
            {
                if (currentState == GameFlowEvent.OnEnterScoring)
                {
                    EnterFlowState(GameFlowEvent.OnEndScoring);
                }
                m_CurrentDisk.director.Stop();
                EnterFlowState(GameFlowEvent.OnGameEnd);
            }
        }

        public void OnCheckPointStart()
        {
            if (BetweenFlowState(currentState, GameFlowEvent.OnGameStart, GameFlowEvent.OnGameEnd))
            {
                EnterFlowState(GameFlowEvent.OnEnterScoring);
                m_CurrentCheckPointScore = 0;

                Debug.Log("Start");
            }
        }

        public void OnCheckPointEnd()
        {
            if (BetweenFlowState(currentState, GameFlowEvent.OnGameStart, GameFlowEvent.OnGameEnd))
            {
                EnterFlowState(GameFlowEvent.OnEndScoring);

                m_TotalScore += m_CurrentCheckPointScore;
                m_CurrentCheckPointIndex++;

                Debug.Log("End");
            }
        }

        public void Update()
        {
            if (BetweenFlowState(m_CurrentState, GameFlowEvent.OnGameStart, GameFlowEvent.OnGameEnd))
            {
                m_CurrentTime = m_CurrentDisk.director.time;
                if (m_CurrentDisk.director.state == PlayState.Paused)
                {
                    EnterFlowState(GameFlowEvent.OnGameEnd);
                }

                var checkPoint = m_CurrentDisk.danceMasks[m_CurrentCheckPointIndex].GetComponent<DanceCheckPoint>();
                var referenceLandmarks = checkPoint.referenceLandmarks;
                var playerLandmarks = m_InputProvider.GetLandmarkPoints();
                var rawScore = m_ScoringMechanism.GetRawScore(referenceLandmarks, playerLandmarks, checkPoint.weights);
                var rank = GetRank(rawScore);
                var thisFrameScore = m_GradeConfig.definitions[rank].score;
                m_CurrentCheckPointScore = Mathf.Max(thisFrameScore, m_CurrentCheckPointScore);
            }
        }

        private GradeConfig m_GradeConfig;
        private IInputPostureProvider m_InputProvider;
        private IScoringMechanism m_ScoringMechanism;
        private float m_TotalScore = 0;
        private DanceGameDiskData m_CurrentDisk;
        private float m_CurrentCheckPointScore = 0;
        private int m_CurrentCheckPointIndex = 0;
        private double m_CurrentTime = 0;
        private GameFlowEvent m_CurrentState;

        #region INTERNAL
        void EnterFlowState(GameFlowEvent flowEvent)
        {
            if (GameEventNotifier != null)
            {
                GameEventNotifier(this, flowEvent);
            }
            m_CurrentState = flowEvent;
        }

        /// <summary>
        /// Include A, Not Include B
        /// </summary>
        /// <returns></returns>
        bool BetweenFlowState(GameFlowEvent target, GameFlowEvent A, GameFlowEvent B)
        {
            return (int)target >= (int)A && (int)target < (int)B;
        }

        int GetRank(float grade)
        {
            var defs = m_GradeConfig.definitions;
            for (int i = 0; i < defs.Count; i++)
            {
                var d = defs[i];
                if (grade >= d.gradeRangeMin && grade < d.gradeRangeMax)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion
    }
}