using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuxi.DanceGameKit
{
    public class DanceCheckPoint : MonoBehaviour
    {
        public float Weight_Head_2_Neck;
        public float Weight_Neck_2_Right_Shoulder;
        public float Weight_Right_Shoulder_2_Right_Elbow;
        public float Weight_Right_Elbow_2_Right_Hand;
        public float Weight_Neck_2_Left_Shoulder;
        public float Weight_Left_Shoulder_Left_Elbow;
        public float Weight_Left_Elbow_2_Left_Hand;
        public float Weight_Neck_2_Right_Hip;
        public float Weight_Right_Hip_2_Right_Knee;
        public float Weight_Right_Knee_2_Right_Foot;
        public float Weight_Neck_2_Left_Hip;
        public float Weight_Left_Hip_2_Left_Knee;
        public float Weight_Left_Knee_2_Left_Foot;

        public Vector3[] referenceLandmarks;
        public float[] weights;
        public bool isUsedWeight;
    }
}