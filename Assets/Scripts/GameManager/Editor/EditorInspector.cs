using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FrameGizmos))]
public class EditorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        FrameGizmos test = (FrameGizmos)target;

        test.Awake();
        test.OnDrawGizmos();

    }
}
