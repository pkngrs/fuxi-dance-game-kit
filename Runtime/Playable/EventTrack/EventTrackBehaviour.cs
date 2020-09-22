using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Fuxi.DanceGameKit
{
    [Serializable]
    public class EventTrackBehaviour : PlayableBehaviour
    {
        public EventTrackClip _EventClip;
        public IExposedPropertyTable _Resolver;
        double _lastTime = -1;

        public void Update(Playable playable, TimelineClip clip, double time)
        {
            if (_EventClip._ExecuteInEditMode || Application.isPlaying)
            {
                _EventClip.UpdateFrame(playable, clip, _lastTime, time);
            }
            _lastTime = time;
        }
    }
}