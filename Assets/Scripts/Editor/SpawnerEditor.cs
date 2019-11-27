using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(EnemySpawner))]
public class SpawnerEditor : Editor
{

    static BoxBoundsHandle s_BoxBoundsHandle = new BoxBoundsHandle();
    static Color s_EnabledColor = Color.green + Color.grey;
    void OnSceneGUI()
    {
        EnemySpawner spawner = (EnemySpawner)target;

        if (!spawner.enabled)
            return;

        Matrix4x4 handleMatrix = spawner.transform.localToWorldMatrix;
        handleMatrix.SetRow(0, Vector4.Scale(handleMatrix.GetRow(0), new Vector4(1f, 1f, 0f, 1f)));
        handleMatrix.SetRow(1, Vector4.Scale(handleMatrix.GetRow(1), new Vector4(1f, 1f, 0f, 1f)));
        handleMatrix.SetRow(2, new Vector4(0f, 0f, 1f, spawner.transform.position.z));
        using (new Handles.DrawingScope(handleMatrix))
        {
            s_BoxBoundsHandle.center = spawner.offset;
            s_BoxBoundsHandle.size = spawner.size;

            s_BoxBoundsHandle.SetColor(s_EnabledColor);
            EditorGUI.BeginChangeCheck();
            s_BoxBoundsHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spawner, "Modify Spawner");

                spawner.size = s_BoxBoundsHandle.size;
                spawner.offset = s_BoxBoundsHandle.center;
            }
        }
    }
}
