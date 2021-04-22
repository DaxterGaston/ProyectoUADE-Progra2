using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ViewCone))]
public class ViewConeEditor : Editor
{
    private void OnSceneGUI()
    {
        ViewCone cv = (ViewCone)target;
        Handles.color = Color.white;
        //Dibujo lo que seria el circulo dentro del cual puedo captar las colisiones y ver al jugador.
        Vector3 normal = Vector3.forward;
        //normal -= new Vector3(normal.x, normal.y, normal.z - 10);
        Handles.DrawWireArc(cv.transform.position, normal, Vector3.up, 360, cv.Radius);

        DibujarLineasCono(cv);

        //Normal que necesito en 2D
        Debug.DrawRay(cv.transform.position, normal, Color.red);
        Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        Debug.DrawRay(cv.transform.position, mouse - cv.transform.position, Color.magenta);
    }

    private void DibujarLineasCono(ViewCone cv)
    {
        //Vectores limites del cono
        Vector3 vectorAnguloA = cv.DirectionFromAngle(-cv.Angle / 2);
        Vector3 vectorAnguloB = cv.DirectionFromAngle(cv.Angle / 2);
        Handles.DrawLine(cv.transform.position, cv.transform.position + vectorAnguloA * cv.Radius);
        Handles.DrawLine(cv.transform.position, cv.transform.position + vectorAnguloB * cv.Radius);
    }
}