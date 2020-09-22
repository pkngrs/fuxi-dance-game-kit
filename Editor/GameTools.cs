#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameTools : MonoBehaviour
{
    static GameObject target;
    static GameObject reference;
    [MenuItem("Tools/Set Bones Target")]
    static void SetTarget()
    {
        target = Selection.activeGameObject;
    }

    [MenuItem("Tools/Set Bones Reference")]
    static void SetReference()
    {
        reference = Selection.activeGameObject;
    }

    [MenuItem("Tools/Assign Bones")]
    static void AssignBones()
    {
        Assign(target.transform, reference.transform);
    }

    static void Assign(Transform target, Transform refer)
    {
        target.localRotation = refer.localRotation;
        for (int i = 0; i < target.childCount; i++)
        {
            Assign(target.GetChild(i), refer.GetChild(i));
        }
    }
}
//    [MenuItem("Tools/Generate Mask %G")]
//    static void GenerateSelectionDanceMask()
//    {
//        GameObject sel = Selection.activeGameObject;
//        var dir = sel.GetComponent<PlayableDirector>();
//        GenerateDanceMask(dir);
//    }

//    [MenuItem("Tools/Read Mask")]
//    static void ReadSelectionDanceMask()
//    {
//        GameObject sel = Selection.activeGameObject;
//        var dir = sel.GetComponent<PlayableDirector>();
//        ReadDanceMask(dir);
//    }

//    static void ReadDanceMask(PlayableDirector director)
//    {
//        DanceTrack danceTrack = null;

//        var timeline = director.playableAsset as TimelineAsset;
//        var tracks = timeline.GetRootTracks();

//        foreach (var t in tracks)
//        {
//            if (t.GetType() == typeof(DanceTrack))
//            {
//                danceTrack = t as DanceTrack;
//            }
//        }

//        var clips = danceTrack.GetClips();
//        var maskRoot = director.gameObject.transform.Find("MaskRoot");

//        int i = 0;
//        foreach (var c in clips)
//        {
//            var d = maskRoot.GetChild(i).GetComponent<DanceCheckPoint>();
//            var mask = c.asset as DanceClip;

//            mask.Weight_Head_2_Neck = d.Weight_Head_2_Neck;
//            mask.Weight_Neck_2_Right_Shoulder = d.Weight_Neck_2_Right_Shoulder;
//            mask.Weight_Right_Shoulder_2_Right_Elbow = d.Weight_Right_Shoulder_2_Right_Elbow;
//            mask.Weight_Right_Elbow_2_Right_Hand = d.Weight_Right_Elbow_2_Right_Hand;
//            mask.Weight_Neck_2_Left_Shoulder = d.Weight_Neck_2_Left_Shoulder;
//            mask.Weight_Left_Shoulder_Left_Elbow = d.Weight_Left_Shoulder_Left_Elbow;
//            mask.Weight_Left_Elbow_2_Left_Hand = d.Weight_Left_Elbow_2_Left_Hand;
//            mask.Weight_Neck_2_Right_Hip = d.Weight_Neck_2_Right_Hip;
//            mask.Weight_Right_Hip_2_Right_Knee = d.Weight_Right_Hip_2_Right_Knee;
//            mask.Weight_Right_Knee_2_Right_Foot = d.Weight_Right_Knee_2_Right_Foot;
//            mask.Weight_Neck_2_Left_Hip = d.Weight_Neck_2_Left_Hip;
//            mask.Weight_Left_Hip_2_Left_Knee = d.Weight_Left_Hip_2_Left_Knee;
//            mask.Weight_Left_Knee_2_Left_Foot = d.Weight_Left_Knee_2_Left_Foot;
//            i++;
//        }
//    }

//    static void GenerateDanceMask(PlayableDirector director)
//    {
//        DanceTrack danceTrack = null;

//        var timeline = director.playableAsset as TimelineAsset;
//        var tracks = timeline.GetRootTracks();

//        foreach (var t in tracks)
//        {
//            if (t.GetType() == typeof(DanceTrack))
//            {
//                danceTrack = t as DanceTrack;
//            }
//        }

//        var clips = danceTrack.GetClips();

//        int i = 0;
//        foreach (var c in clips)
//        {
//            var m = CaptureOneShot(director, c);

//            var mask = m.GetComponent<DanceMask>();

//            mask.isUsedWeight = false;

//            var time = c.start + (c.end - c.start) / 2;
//            m.transform.localPosition = new Vector3((float)time * GameManager.SCROLL_SPEED, 0, 0);
//            i++;
//        }
//    }

//    static GameObject CaptureOneShot(PlayableDirector director, TimelineClip clip)
//    {
//        var time = clip.start + (clip.end - clip.start) / 2;

//        director.time = time;
//        director.Evaluate();

//        var maskRoot = director.gameObject.transform.Find("MaskRoot");
//        var reference = director.gameObject.transform.Find("Reference").gameObject;
//        var go = GameObject.Instantiate(reference, maskRoot);
//        var mask = go.AddComponent<DanceMask>();

//        var d = clip.asset as DanceClip;

//        mask.isUsedWeight = false;

//        mask.Weight_Head_2_Neck = d.Weight_Head_2_Neck;
//        mask.Weight_Neck_2_Right_Shoulder = d.Weight_Neck_2_Right_Shoulder;
//        mask.Weight_Right_Shoulder_2_Right_Elbow = d.Weight_Right_Shoulder_2_Right_Elbow;
//        mask.Weight_Right_Elbow_2_Right_Hand = d.Weight_Right_Elbow_2_Right_Hand;
//        mask.Weight_Neck_2_Left_Shoulder = d.Weight_Neck_2_Left_Shoulder;
//        mask.Weight_Left_Shoulder_Left_Elbow = d.Weight_Left_Shoulder_Left_Elbow;
//        mask.Weight_Left_Elbow_2_Left_Hand = d.Weight_Left_Elbow_2_Left_Hand;
//        mask.Weight_Neck_2_Right_Hip = d.Weight_Neck_2_Right_Hip;
//        mask.Weight_Right_Hip_2_Right_Knee = d.Weight_Right_Hip_2_Right_Knee;
//        mask.Weight_Right_Knee_2_Right_Foot = d.Weight_Right_Knee_2_Right_Foot;
//        mask.Weight_Neck_2_Left_Hip = d.Weight_Neck_2_Left_Hip;
//        mask.Weight_Left_Hip_2_Left_Knee = d.Weight_Left_Hip_2_Left_Knee;
//        mask.Weight_Left_Knee_2_Left_Foot = d.Weight_Left_Knee_2_Left_Foot;

//        DestroyImmediate(go.GetComponent<Animator>());
//        Match(go.transform, reference.transform);
//        return go;
//    }

//    static void Match(Transform target, Transform reference)
//    {
//        target.localPosition = reference.localPosition;
//        target.localRotation = reference.localRotation;
//        target.localScale = reference.localScale;
//        for (int i = 0; i < target.childCount; i++)
//        {
//            Match(target.GetChild(i), reference.GetChild(i));
//        }
//    }

//    static Vector2[] ExtractLandmark(WebCamTexture texture)
//    {
//        throw new System.NotImplementedException();
//    }

//    static float GetScore(Vector2[] targetLandmarks, Vector2[] referenceLandmarks)
//    {
//        throw new System.NotImplementedException();
//    }
//}
#endif