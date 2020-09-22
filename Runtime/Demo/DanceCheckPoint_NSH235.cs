using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Fuxi.DanceGameKit
{
    public class DanceCheckPoint_NSH235 : DanceCheckPoint
    {
        public void Awake()
        {
            uint[] startIndice = new uint[] { 1, 1, 5, 6, 1, 2, 3, 1, 11, 12, 1, 8, 9 };
            uint[] endIndice = new uint[] { 0, 5, 6, 7, 2, 3, 4, 11, 12, 13, 8, 9, 10 };

            weights = new float[13]
            {
            Weight_Head_2_Neck,
            Weight_Neck_2_Right_Shoulder,
            Weight_Right_Shoulder_2_Right_Elbow,
            Weight_Right_Elbow_2_Right_Hand,
            Weight_Neck_2_Left_Shoulder,
            Weight_Left_Shoulder_Left_Elbow,
            Weight_Left_Elbow_2_Left_Hand,
            Weight_Neck_2_Right_Hip,
            Weight_Right_Hip_2_Right_Knee,
            Weight_Right_Knee_2_Right_Foot,
            Weight_Neck_2_Left_Hip,
            Weight_Left_Hip_2_Left_Knee,
            Weight_Left_Knee_2_Left_Foot,
            };

            referenceLandmarks = new Vector3[14];
            var spine = transform.Find("Bone001/Bip001/Bip001 Pelvis/Bip001 Spine");

            referenceLandmarks[0] = spine.GetChild(2).GetChild(0).GetChild(0).GetChild(0).position;
            referenceLandmarks[1] = spine.GetChild(2).GetChild(0).position;
            referenceLandmarks[2] = spine.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).position;
            referenceLandmarks[3] = spine.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).position;
            referenceLandmarks[4] = spine.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).position;
            referenceLandmarks[5] = spine.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).position;
            referenceLandmarks[6] = spine.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).position;
            referenceLandmarks[7] = spine.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).position;
            referenceLandmarks[8] = spine.GetChild(0).position;
            referenceLandmarks[9] = spine.GetChild(0).GetChild(0).position;
            referenceLandmarks[10] = spine.GetChild(0).GetChild(0).GetChild(0).position;
            referenceLandmarks[11] = spine.GetChild(1).position;
            referenceLandmarks[12] = spine.GetChild(1).GetChild(0).position;
            referenceLandmarks[13] = spine.GetChild(1).GetChild(0).GetChild(0).position;

            for (int i = 0; i < referenceLandmarks.Length; i++)
            {
                referenceLandmarks[i] = transform.InverseTransformPoint(referenceLandmarks[i]);
            }

            if (!isUsedWeight)
            {
                for (int i = 0; i < weights.Length; i++)
                {
                    weights[i] = 1.0f / weights.Length;
                }
            }
        }
    }
}