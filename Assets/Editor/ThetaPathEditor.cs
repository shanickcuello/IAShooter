using Features.Theta_Path;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ThetaPath))]
public class ThetaPathEditor : Editor
{
    ThetaPath theta;
    PathNode[] childrens;
    private void OnEnable()
    {
        theta = (ThetaPath)target;
        childrens = theta.GetComponentsInChildren<PathNode>();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Revinculate"))
        {
            Revinculate();
        }
    }
    private void Revinculate()
    {
        foreach (var node in childrens)
        {
            foreach (var neightbour in node.neightbourds)
            {
                if (neightbour != null)
                {
                    neightbour.neightbourds.Clear();
                    neightbour.GetNeightbors();
                }
            }
        }
        Repaint();
    }
}