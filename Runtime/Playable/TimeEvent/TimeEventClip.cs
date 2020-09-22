using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.Timeline;

namespace Fuxi.DanceGameKit
{
    public class TimeEventClip : EventTrackClip
    {
        [Header("Event Receriver")]
        public ExposedReference<GameObject> Target;
        public string Event;

        public UnityAction Action;

        public override void UpdateFrame(Playable playable, TimelineClip clip, double lastTime, double time)
        {
            var start = clip.start;
            var end = clip.end;

            if (lastTime < start && time >= start || lastTime > time && time == start)
            {
                var target = Target.Resolve(_Resolver);
                GoSendEvent(target, Event);
            }
        }
    }
}