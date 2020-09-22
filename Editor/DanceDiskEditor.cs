using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Fuxi.DanceGameKit
{
    [CustomEditor(typeof(DanceGameDisk))]
    public class DanceDiskEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var go = (target as DanceGameDisk).gameObject;
            base.OnInspectorGUI();
            if (GUILayout.Button("生成舞蹈剪影模板"))
            {
                GenerateDanceMask(go.GetComponent<PlayableDirector>());
                var path = Path.Combine("Assets", "Builds", go.name, "Prefabs", go.name) + ".prefab";
                if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
                {
                    AssetDatabase.DeleteAsset(path);
                }
                bool status;
                PrefabUtility.SaveAsPrefabAsset(go, path, out status);
                if (status)
                {
                    Debug.Log("保存成功");
                }
                else
                {
                    Debug.LogError("保存失败");
                }
            }
        }

        void GenerateDanceMask(PlayableDirector director)
        {
            DanceTrack danceTrack = null;

            var timeline = director.playableAsset as TimelineAsset;
            var tracks = timeline.GetRootTracks();

            foreach (var t in tracks)
            {
                if (t.GetType() == typeof(DanceTrack))
                {
                    danceTrack = t as DanceTrack;
                }
            }

            var clips = danceTrack.GetClips();

            int i = 0;
            foreach (var c in clips)
            {
                var m = CaptureOneShot(director, c);

                var mask = m.GetComponent<DanceCheckPoint>();

                mask.isUsedWeight = false;

                var time = c.start + (c.end - c.start) / 2;

                m.transform.localPosition = new Vector3((float)time * (target as DanceGameDisk).scrollSpeed, 0, 0);
                i++;
            }
        }

        GameObject CaptureOneShot(PlayableDirector director, TimelineClip clip)
        {
            var time = clip.start + (clip.end - clip.start) / 2;

            director.time = time;
            director.Evaluate();

            var maskRoot = director.gameObject.transform.Find("MaskRoot");
            var reference = director.gameObject.transform.Find("Reference").gameObject;
            var go = GameObject.Instantiate(reference, maskRoot);
            var mask = go.AddComponent<DanceCheckPoint>();

            var d = clip.asset as DanceClip;

            mask.isUsedWeight = false;

            mask.Weight_Head_2_Neck = d.Weight_Head_2_Neck;
            mask.Weight_Neck_2_Right_Shoulder = d.Weight_Neck_2_Right_Shoulder;
            mask.Weight_Right_Shoulder_2_Right_Elbow = d.Weight_Right_Shoulder_2_Right_Elbow;
            mask.Weight_Right_Elbow_2_Right_Hand = d.Weight_Right_Elbow_2_Right_Hand;
            mask.Weight_Neck_2_Left_Shoulder = d.Weight_Neck_2_Left_Shoulder;
            mask.Weight_Left_Shoulder_Left_Elbow = d.Weight_Left_Shoulder_Left_Elbow;
            mask.Weight_Left_Elbow_2_Left_Hand = d.Weight_Left_Elbow_2_Left_Hand;
            mask.Weight_Neck_2_Right_Hip = d.Weight_Neck_2_Right_Hip;
            mask.Weight_Right_Hip_2_Right_Knee = d.Weight_Right_Hip_2_Right_Knee;
            mask.Weight_Right_Knee_2_Right_Foot = d.Weight_Right_Knee_2_Right_Foot;
            mask.Weight_Neck_2_Left_Hip = d.Weight_Neck_2_Left_Hip;
            mask.Weight_Left_Hip_2_Left_Knee = d.Weight_Left_Hip_2_Left_Knee;
            mask.Weight_Left_Knee_2_Left_Foot = d.Weight_Left_Knee_2_Left_Foot;

            DestroyImmediate(go.GetComponent<Animator>());
            Match(go.transform, reference.transform);
            return go;
        }

        void Match(Transform target, Transform reference)
        {
            target.localPosition = reference.localPosition;
            target.localRotation = reference.localRotation;
            target.localScale = reference.localScale;
            for (int i = 0; i < target.childCount; i++)
            {
                Match(target.GetChild(i), reference.GetChild(i));
            }
        }
    }
}