using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Fuxi.DanceGameKit
{

    public class DanceDiskCreator : EditorWindow
    {
        static void CreateDanceDisk()
        {
            TimelineAsset timeline = new TimelineAsset();
            timeline.CreateTrack<AnimationTrack>();
            timeline.CreateTrack<DanceTrack>();
            timeline.CreateTrack<AudioTrack>();
            AssetDatabase.CreateAsset(timeline, "Assets/Temp.playable");
        }

        [MenuItem("Assets/Create/Dance Disk", priority = -1)]
        static void ShowWindow()
        {
            var wnd = EditorWindow.CreateInstance<DanceDiskCreator>();
            wnd.Show();
        }

        private AnimationClip m_animationClip;
        private AudioClip m_audioClip;
        private GameObject m_targetDancer;
        private string m_name;

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("舞蹈名称");
            m_name = GUILayout.TextField(m_name);
            GUILayout.EndHorizontal();
            m_targetDancer = (GameObject)EditorGUILayout.ObjectField("舞者模型", m_targetDancer, typeof(GameObject), false);
            m_animationClip = (AnimationClip)EditorGUILayout.ObjectField("舞蹈动画", m_animationClip, typeof(AnimationClip), false);
            m_audioClip = (AudioClip)EditorGUILayout.ObjectField("舞蹈音乐", m_audioClip, typeof(AudioClip), false);
            if (GUILayout.Button("生成"))
            {
                AssetDatabase.CreateFolder(Path.Combine("Assets", "Builds"), m_name);

                AssetDatabase.CreateFolder(Path.Combine("Assets", "Builds", m_name), "Animations");
                AssetDatabase.CreateFolder(Path.Combine("Assets", "Builds", m_name), "Audios");
                AssetDatabase.CreateFolder(Path.Combine("Assets", "Builds", m_name), "Prefabs");

                var p1 = Copy(m_animationClip, "Animations");
                var p2 = Copy(m_audioClip, "Audios");

                TimelineAsset timeline = new TimelineAsset();
                var animTrack = timeline.CreateTrack<AnimationTrack>();
                var audioTrack = timeline.CreateTrack<AudioTrack>();
                var danceTrack = timeline.CreateTrack<DanceTrack>();

                var animClip = timeline.GetRootTrack(0).CreateClip<AnimationPlayableAsset>();
                var anim = AssetDatabase.LoadAssetAtPath<AnimationClip>(p1);
                (animClip.asset as AnimationPlayableAsset).clip = anim;
                animClip.start = 0;
                animClip.duration = anim.length;

                var audioClip = timeline.GetRootTrack(1).CreateClip<AudioPlayableAsset>();
                var audio = AssetDatabase.LoadAssetAtPath<AudioClip>(p2);
                (audioClip.asset as AudioPlayableAsset).clip = audio;
                audioClip.start = 0;
                audioClip.duration = audio.length;

                GameObject root = new GameObject(m_name);
                root.transform.position = Vector3.zero;
                root.AddComponent<DanceGameDisk>();

                GameObject reference = GameObject.Instantiate<GameObject>(m_targetDancer);
                reference.name = "Reference";
                reference.transform.SetParent(root.transform);
                reference.transform.localPosition = Vector3.zero;
                reference.transform.localRotation = Quaternion.identity;
                reference.transform.localScale = Vector3.one;

                GameObject masks = new GameObject("MaskRoot");
                masks.transform.SetParent(root.transform);
                masks.transform.localPosition = Vector3.zero;
                masks.transform.localRotation = Quaternion.identity;
                masks.transform.localScale = Vector3.one;

                AssetDatabase.CreateAsset(timeline, Path.Combine("Assets", "Builds", m_name, "Animations", m_name) + ".playable");
                var dir = root.AddComponent<PlayableDirector>();
                dir.playableAsset = timeline;
                dir.SetGenericBinding(animTrack, reference);
            }
        }

        private string Copy(Object target, string subFolder)
        {
            var ap = AssetDatabase.GetAssetPath(target);
            var ext = Path.GetExtension(ap);
            var tp = Path.Combine("Assets", "Builds", m_name, subFolder, m_name) + ext;
            AssetDatabase.CopyAsset(ap, tp);
            return tp;
        }
    }
}