using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Fuxi.DanceGameKit
{
    [Serializable]
    public abstract class EventTrackClip : PlayableAsset, ITimelineClipAsset
    {
        public bool _ExecuteInEditMode = false;
        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        protected IExposedPropertyTable _Resolver;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<EventTrackBehaviour>.Create(graph);
            _Resolver = graph.GetResolver();
            EventTrackBehaviour clone = playable.GetBehaviour();
            clone._EventClip = this;
            return playable;
        }

        public virtual void UpdateFrame(Playable playable, TimelineClip clip, double lastTime, double time)
        {

        }

        public void GoSendEvent(GameObject Go, string Event)
        {
            if (Go != null)
            {
                if (!string.IsNullOrEmpty(Event))
                {
                    Go.SendMessage(Event);
                }
            }
        }
    }
}