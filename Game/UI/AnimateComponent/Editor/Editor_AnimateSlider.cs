using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using Mojiex.AnimateUI;

namespace Mojiex.AnimateUIEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AnimateSlider))]
    public class Editor_AnimateSlider : SliderEditor
    {
        private SerializedProperty AnimateTime;
        private SerializedProperty EasingMethod;
        private SerializedProperty ValueAnimChangeType;
        private SerializedProperty AnimationPause;
        private SerializedProperty TargetPos;


        protected override void OnEnable()
        {
            base.OnEnable();
            AnimateTime = serializedObject.FindProperty("AnimateTime");
            EasingMethod = serializedObject.FindProperty("EasingMethod");
            ValueAnimChangeType = serializedObject.FindProperty("valueAnim");
            AnimationPause = serializedObject.FindProperty("AnimationPause");
            TargetPos = serializedObject.FindProperty("m_targetPos");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(AnimationPause);
            EditorGUILayout.PropertyField(AnimateTime);
            EditorGUILayout.PropertyField(EasingMethod);
            EditorGUILayout.PropertyField(ValueAnimChangeType);
            EditorGUILayout.PropertyField(TargetPos);
            serializedObject.ApplyModifiedProperties();
        }
    }
}