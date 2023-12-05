using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform mainCameraTransform;
    private Vector3 originalPosition;

    private float shakeIntensity = 0.1f;
    private float shakeDuration = 0.5f;

    // 피격 시 재생할 사운드
    public AudioClip hitSound;
    private AudioSource audioSource;

    private void Start()
    {
        mainCameraTransform = GameObject.Find("Cameras").transform;

        // AudioSource 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = hitSound;
    }

    public void ShakeCamera()
    {
        originalPosition = mainCameraTransform.position;
        InvokeRepeating("StartShaking", 0f, 0.01f);
        Invoke("StopShaking", shakeDuration);

        // 피격음 재생
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    private void StartShaking()
    {
        float shakeAmountX = Random.Range(-1f, 1f) * shakeIntensity;
        float shakeAmountY = Random.Range(-1f, 1f) * shakeIntensity;

        Vector3 newPosition = originalPosition + new Vector3(shakeAmountX, shakeAmountY, 0f);
        mainCameraTransform.position = newPosition;
    }

    private void StopShaking()
    {
        CancelInvoke("StartShaking");
        mainCameraTransform.position = originalPosition;
    }
}
