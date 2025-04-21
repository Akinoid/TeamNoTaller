using UnityEngine;
using System.Collections;

public class WarningArea : MonoBehaviour
{
    public Renderer areaRenderer;
    public float minFlashSpeed = 0.2f;
    public float maxFlashSpeed = 0.05f;

    public IEnumerator StartWarning(float duration, float waitBeforeExplosion)
    {
        float timer = 0f;
        float flashSpeed = minFlashSpeed;

        if (areaRenderer == null)
            areaRenderer = GetComponent<Renderer>();

        while (timer < duration)
        {
            areaRenderer.enabled = !areaRenderer.enabled;
            yield return new WaitForSeconds(flashSpeed);
            timer += flashSpeed;

            // Aumentar la velocidad de parpadeo con el tiempo
            float t = timer / duration;
            flashSpeed = Mathf.Lerp(minFlashSpeed, maxFlashSpeed, t);
        }

        areaRenderer.enabled = true;
        yield return new WaitForSeconds(waitBeforeExplosion);
        Destroy(gameObject); // Quita el área antes de la explosión
    }
}
