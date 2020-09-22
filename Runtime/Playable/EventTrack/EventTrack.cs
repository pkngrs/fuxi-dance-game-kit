using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Fuxi.DanceGameKit
{
    [TrackColor(0.855f, 0.8623f, 0.87f)]
    [TrackClipType(typeof(EventTrackClip))]
    public abstract class EventTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            ScriptPlayable<EventTrackMixerBehaviour> result = ScriptPlayable<EventTrackMixerBehaviour>.Create(graph, inputCount);
            result.GetBehaviour()._clips = GetClips() as TimelineClip[];
            return result;
        }
    }
}