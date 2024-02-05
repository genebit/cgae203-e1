using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Platformer.Mechanics
{
    [CustomEditor(typeof(Timer))]
    public class TimerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Timer timer = (Timer) target;

            if (GUILayout.Button("Reset Highscore"))
            {
                timer.ResetHighScore();
            }
        }
    }
}