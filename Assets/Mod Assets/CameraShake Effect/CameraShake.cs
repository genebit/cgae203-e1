using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Range(0, 0.2f)]
    public float transitionDuration;

    private CinemachineVirtualCamera virtualCamera;
    private float initialOrthoSize = 3.5f;

    private void Start()
    {
        virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();

        // Store the initial orthographic size
        initialOrthoSize = virtualCamera.m_Lens.OrthographicSize;
    }

    public void TriggerShake()
    {
        StartCoroutine(ShakeAndReturn());
    }

    private IEnumerator ShakeAndReturn()
    {
        yield return ShakeCamera(3f); // Shake to 3f
        yield return new WaitForSeconds(transitionDuration);
        yield return ShakeCamera(initialOrthoSize); // Return to initial orthographic size
    }

    private IEnumerator ShakeCamera(float targetOrthoSize)
    {
        float elapsedTime = 0f;
        float startSize = virtualCamera.m_Lens.OrthographicSize;

        // Smoothly transition from the initial ortho size to targetOrthoSize over transitionDuration seconds
        while (elapsedTime < transitionDuration)
        {
            // Calculate the new ortho size using a lerp
            float newOrthoSize = Mathf.Lerp(startSize, targetOrthoSize, elapsedTime / transitionDuration);

            // Update the virtual camera's orthographic size
            virtualCamera.m_Lens.OrthographicSize = newOrthoSize;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the end of the frame
            yield return null;
        }

        // Ensure the final ortho size is set to targetOrthoSize
        virtualCamera.m_Lens.OrthographicSize = targetOrthoSize;
    }
}
