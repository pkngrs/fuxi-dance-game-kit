using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Fuxi.DanceGameKit
{
    public class EventTrackMixerBehaviour : PlayableBehaviour
    {
        public TimelineClip[] _clips;

        // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            int inputCount = playable.GetInputCount();
            double time = playable.GetGraph().GetRootPlayable(0).GetTime();
            for (int i = 0; i < inputCount; i++)
            {
                ScriptPlayable<EventTrackBehaviour> inputPlayable = (ScriptPlayable<EventTrackBehaviour>)playable.GetInput(i);
                EventTrackBehaviour input = inputPlayable.GetBehaviour();
                input.Update(playable, _clips[i], time);
            }
        }
    }
}