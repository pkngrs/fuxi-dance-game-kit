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
    public class DanceGameDisk : MonoBehaviour
    {
        public float scrollSpeed = 5;
    }

    public class DanceGameDiskData : JsonIO<DanceGameDiskData>
    {
        public string displayName;
        public string diskPrefabAddressable;

        private bool m_IsMounted;
        private GameObject m_DiskInstance;
        private PlayableDirector m_Director;
        private List<GameObject> m_DanceMasks;
        private double m_StartTime;
        private double m_EndTime;

        public GameObject diskInstance
        {
            get { return m_IsMounted ? m_DiskInstance : null; }
        }

        public PlayableDirector director
        {
            get { return m_IsMounted ? m_Director : null; }
        }

        public GameObject[] danceMasks
        {
            get { return m_IsMounted ? m_DanceMasks.ToArray() : null; }
        }

        public double startTime
        {
            get { return m_IsMounted ? m_StartTime : -1d; }
        }

        public double endTime
        {
            get { return m_IsMounted ? m_EndTime : -1d; }
        }

        public void Mount()
        {
            var request = Addressables.InstantiateAsync(diskPrefabAddressable);
            request.Completed += (r) =>
            {
                m_DiskInstance = r.Result;
                m_Director = m_DiskInstance.GetComponent<PlayableDirector>();

                TimeEventTrack timeTrack = null;
                var timeline = m_Director.playableAsset as TimelineAsset;
                var tracks = timeline.GetRootTracks();
                foreach (var t in tracks)
                {
                    if (t.GetType() == typeof(TimeEventTrack))
                    {
                        timeTrack = t as TimeEventTrack;
                    }
                }

                var clips = timeTrack.GetClips();
                foreach (var c in clips)
                {
                    m_StartTime = c.start;
                    m_EndTime = c.end;
                }

                Transform MaskRoot = diskInstance.transform.Find("MaskRoot");
                m_DanceMasks = new List<GameObject>();
                for (int i = 0; i < MaskRoot.childCount; i++)
                {
                    var mask = MaskRoot.GetChild(i).gameObject;
                    m_DanceMasks.Add(mask);
                }

                m_IsMounted = true;
            };
        }
        public void Unmount()
        {
            if (diskInstance != null)
            {
                GameObject.DestroyImmediate(diskInstance);
            }
        }
    }
}