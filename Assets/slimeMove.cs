using System.Collections;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    public CameraShake cameraShake;
    private HealthBarHUDTester healthBarHUDTester;
    public Animator slimeAnimator;
    private Speed speedScript;  // Reference to the Speed script
    public float attackInterval = 3.0f;
    public float waitAfterArrival = 1.0f;

    public Sprite targetSprite;
    public Sprite newTargetSprite;

    private float targetZ;
    private bool isMoving = true;
    private bool isAttacking = false;
    private bool spriteShown = false;
    private GameObject targetObject;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        cameraShake = GameObject.Find("Cameras").GetComponent<CameraShake>();
        healthBarHUDTester = FindObjectOfType<HealthBarHUDTester>();
        speedScript = GameObject.Find("GameObject").GetComponent<Speed>();  // Replace "YourGameObjectName" with the actual name
        SetNewTargetZ();
        CreateTargetSprite();
    }

    void Update()
    {
        float distanceToTarget = Mathf.Abs(transform.position.z - targetZ);

        if (distanceToTarget > 0.1f && !isAttacking)
        {
            MoveTowardsTarget();
        }
        else if (distanceToTarget <= 0.1f && !isAttacking)
        {
            if (!spriteShown)
            {
                ShowTargetSprite();
            }
            StartCoroutine(AttackRoutine());
        }
        else
        {
            if (!isAttacking)
            {
                MoveTowardsTarget();
            }
        }
    }

    void MoveTowardsTarget()
    {
        if (!isAttacking)
        {
            isMoving = true;
            Vector3 direction = new Vector3(0, 0, targetZ) - new Vector3(0, 0, transform.position.z);
            direction.Normalize();
            transform.Translate(direction * speedScript.GetSpeed() * Time.deltaTime, Space.World);

            if (Mathf.Abs(transform.position.z - targetZ) < 0.1f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, targetZ);
                StartCoroutine(WaitAfterArrival());
            }

            if (!isAttacking && Mathf.Abs(transform.position.z - targetZ) > 0.1f)
            {
                slimeAnimator.SetTrigger("Walk");
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        isMoving = false;
        isAttacking = true;
        spriteShown = false;

        transform.LookAt(Camera.main.transform.position);
        yield return new WaitForSeconds(attackInterval);
        slimeAnimator.SetTrigger("Attack");
        cameraShake.ShakeCamera();
        healthBarHUDTester.Hurt(1);
        isAttacking = false;
    }

    IEnumerator WaitAfterArrival()
    {
        yield return new WaitForSeconds(waitAfterArrival - 1.0f);
        ChangeSpriteToNew();

        yield return new WaitForSeconds(1.0f);

        if (!isAttacking)
        {
            slimeAnimator.SetTrigger("Walk");
        }
    }

    void CreateTargetSprite()
    {
        if (targetObject == null)
        {
            targetObject = new GameObject("TargetObject");
            targetObject.transform.parent = transform;
            targetObject.transform.localPosition = new Vector3(0, 0.5f, -0.1f);
            targetObject.transform.localRotation = Quaternion.identity;

            Vector3 localPosition = targetObject.transform.localPosition;
            localPosition.z += 1.0f;
            targetObject.transform.localPosition = localPosition;

            spriteRenderer = targetObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = targetSprite;
            spriteRenderer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            spriteRenderer.enabled = false;
        }
    }

    void ShowTargetSprite()
    {
        if (spriteShown) return;

        CreateTargetSprite();
        spriteShown = true;
        spriteRenderer.enabled = true;
    }

    void ChangeSpriteToNew()
    {
        if (!spriteShown)
        {
            ChangeSprite(newTargetSprite);
            spriteShown = true;
        }
    }

    void ChangeSprite(Sprite newSprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }

    void SetNewTargetZ()
    {
        targetZ = Random.Range(5f, 7f);
    }
}
