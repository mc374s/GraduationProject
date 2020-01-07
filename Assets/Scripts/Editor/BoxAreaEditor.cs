using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(BoxArea))]
public class BoxAreaEditor : Editor
{

    static BoxBoundsHandle boxBoundsHandle = new BoxBoundsHandle();
    static Color enabledColor = Color.green + Color.grey;
    void OnSceneGUI()
    {
        BoxArea boxArea = (BoxArea)target;

        if (!boxArea.enabled)
            return;

        Matrix4x4 handleMatrix = boxArea.transform.localToWorldMatrix;
        handleMatrix.SetRow(0, Vector4.Scale(handleMatrix.GetRow(0), new Vector4(1f, 1f, 0f, 1f)));
        handleMatrix.SetRow(1, Vector4.Scale(handleMatrix.GetRow(1), new Vector4(1f, 1f, 0f, 1f)));
        handleMatrix.SetRow(2, new Vector4(0f, 0f, 1f, boxArea.transform.position.z));
        using (new Handles.DrawingScope(handleMatrix))
        {
            boxBoundsHandle.center = boxArea.offset;
            boxBoundsHandle.size = boxArea.size;

            boxBoundsHandle.SetColor(enabledColor);
            EditorGUI.BeginChangeCheck();
            boxBoundsHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(boxArea, "Modify Spawner");

                boxArea.size = boxBoundsHandle.size;
                boxArea.offset = boxBoundsHandle.center;
            }
        }
    }
}
