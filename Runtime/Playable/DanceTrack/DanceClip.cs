using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.Timeline;

namespace Fuxi.DanceGameKit
{
    public class DanceClip : EventTrackClip
    {
        public float Weight_Head_2_Neck = 1;
        public float Weight_Neck_2_Right_Shoulder = 1;
        public float Weight_Right_Shoulder_2_Right_Elbow = 1;
        public float Weight_Right_Elbow_2_Right_Hand = 1;
        public float Weight_Neck_2_Left_Shoulder = 1;
        public float Weight_Left_Shoulder_Left_Elbow = 1;
        public float Weight_Left_Elbow_2_Left_Hand = 1;
        public float Weight_Neck_2_Right_Hip = 1;
        public float Weight_Right_Hip_2_Right_Knee = 1;
        public float Weight_Right_Knee_2_Right_Foot = 1;
        public float Weight_Neck_2_Left_Hip = 1;
        public float Weight_Left_Hip_2_Left_Knee = 1;
        public float Weight_Left_Knee_2_Left_Foot = 1;

        public override void UpdateFrame(Playable playable, TimelineClip clip, double lastTime, double time)
        {
            var start = clip.start;
            var end = clip.end;

            if (lastTime < start && time >= start || lastTime > time && time == start)
            {
                var target = DanceGameManager.Instance.gameObject;
                GoSendEvent(target, "OnDanceClipStart");
            }
            if (lastTime < end && time >= end || lastTime > time && time == end)
            {
                var target = DanceGameManager.Instance.gameObject;
                GoSendEvent(target, "OnDanceClipEnd");
            }
        }
    }
}