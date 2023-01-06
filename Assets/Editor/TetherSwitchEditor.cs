using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TetherSwitch))]
public class TetherSwitchEditor : Editor
{
    private SerializedProperty trajectoryTargets;
    private SerializedProperty radius;
    private SerializedProperty angle;

    private TetherSwitch tetherSwitch;

    private void OnEnable()
    {
        trajectoryTargets = serializedObject.FindProperty("trajectoryTargets");
        radius = serializedObject.FindProperty("radius");
        angle = serializedObject.FindProperty("angle");

        tetherSwitch = target as TetherSwitch;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(trajectoryTargets);
        EditorGUILayout.PropertyField(radius);
        EditorGUILayout.PropertyField(angle);
        serializedObject.ApplyModifiedProperties();

        Tether tether = tetherSwitch.GetTether();
        if (tether != null)
        {
            float angleInDegrees = angle.floatValue * Mathf.PI / 180;
            angleInDegrees += tether.transform.rotation.z;
            float x = tether.transform.position.x + (radius.floatValue * Mathf.Cos(angleInDegrees));
            float y = tether.transform.position.y + (radius.floatValue * Mathf.Sin(angleInDegrees));
            tetherSwitch.transform.position = new Vector3(x, y);
        }

        if (GUILayout.Button("Find Targets"))
        {
            trajectoryTargets.arraySize = 0;
            if (tether != null)
            {
                Vector2 pathBetweenTetherAndSwitch = new Vector2(tether.transform.position.x - tetherSwitch.transform.position.x,
                    tether.transform.position.y - tetherSwitch.transform.position.y);
                Vector2 perpVector = Vector2.Perpendicular(pathBetweenTetherAndSwitch);
                foreach (TetherSwitch otherTetherSwitch in FindObjectsOfType<TetherSwitch>())
                {
                    Vector2 pathBetweenSwitches = new Vector2(otherTetherSwitch.transform.position.x - tetherSwitch.transform.position.x,
                        otherTetherSwitch.transform.position.y - tetherSwitch.transform.position.y);
                    if ((Vector2.Angle(perpVector, pathBetweenSwitches) < 1.5 | Vector2.Angle(-1 *perpVector, pathBetweenSwitches) < 1.5) &
                        otherTetherSwitch != tetherSwitch)
                    {
                        trajectoryTargets.arraySize++;
                        SerializedProperty newTarget = trajectoryTargets.GetArrayElementAtIndex(trajectoryTargets.arraySize - 1);
                        newTarget.objectReferenceValue = otherTetherSwitch.gameObject;
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
