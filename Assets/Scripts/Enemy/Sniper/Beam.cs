using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour
{
    [Tooltip("Tiempo en segundos que el láser permanece completo tras llegar al destino")]
    public float lingerDuration = 0.1f;

    [Tooltip("Velocidad a la que avanza el láser (unidades por segundo)")]
    public float speed = 100f;

    private LineRenderer lr;
    private Vector3 startPoint;
    private Vector3 endPoint;

    public void Initialize(Vector3 start, Vector3 end)
    {
        lr = GetComponent<LineRenderer>();
        if (lr == null)
        {
            Debug.LogError("Beam: no LineRenderer encontrado.");
            Destroy(gameObject);
            return;
        }

        startPoint = start;
        endPoint = end;
        // Prepara el LineRenderer: dos puntos
        lr.positionCount = 2;
        lr.SetPosition(0, startPoint);
        lr.SetPosition(1, startPoint); // inicialmente coincide con el inicio
        StartCoroutine(AnimateBeam());
    }

    private IEnumerator AnimateBeam()
    {
        float totalDistance = Vector3.Distance(startPoint, endPoint);
        if (totalDistance <= 0.01f)
        {
            // Si está muy cerca, salta a final
            lr.SetPosition(1, endPoint);
            yield return new WaitForSeconds(lingerDuration);
            Destroy(gameObject);
            yield break;
        }

        float travelTime = totalDistance / speed;
        float t = 0f;
        // Avanzar extremo final desde startPoint hasta endPoint
        while (t < travelTime)
        {
            float frac = t / travelTime;
            Vector3 currentEnd = Vector3.Lerp(startPoint, endPoint, frac);
            lr.SetPosition(0, startPoint);
            lr.SetPosition(1, currentEnd);
            t += Time.deltaTime;
            yield return null;
        }
        // Al final, asegura que llegue exactamente
        lr.SetPosition(0, startPoint);
        lr.SetPosition(1, endPoint);

        // Mantener el láser visible un breve tiempo
        yield return new WaitForSeconds(lingerDuration);
        Destroy(gameObject);
    }
}
