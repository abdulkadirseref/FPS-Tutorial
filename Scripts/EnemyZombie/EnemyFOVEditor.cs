using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFOV))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyFOV fov = (EnemyFOV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.raycast.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.raycast.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.raycast.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.raycast.transform.position, fov.raycast.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.raycast.transform.position, fov.raycast.transform.position + viewAngle02 * fov.radius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.raycast.transform.position, fov.playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}