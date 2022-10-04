#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/24
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;
using Mojiex.AnimateUI;

namespace Mojiex.AnimateUIEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AnimateImage))]
    public class Editor_AnimateImage : ImageEditor
    {
	    private SerializedProperty Type;
        private SerializedProperty TargetPos;
        private SerializedProperty AnimateTime;

        protected override void OnEnable()
        {
            base.OnEnable();
            AnimateTime = serializedObject.FindProperty("AnimateTime");
            TargetPos = serializedObject.FindProperty("m_targetPos");
            Type = serializedObject.FindProperty("m_Type");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            if(Type.enumValueIndex == 3)
            {
                EditorGUILayout.PropertyField(AnimateTime);
                EditorGUILayout.PropertyField(TargetPos);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}