using System.Collections;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    public Animator slimeAnimator;
    public float speed = 1.0f;
    public float attackInterval = 3.0f;
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
}

void MoveTowardsTarget()
{
    if (!isAttacking) // Check if not attacking
    {
        isMoving = true;
        Vector3 direction = new Vector3(0, 0, targetZ) - new Vector3(0, 0, transform.position.z);
        direction.Normalize();
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Play "walk" animation only when not attacking
        if (!isAttacking)
        {
            slimeAnimator.SetTrigger("Walk");
        }
    }
}


 IEnumerator AttackRoutine()
{
    isMoving = false;
    isAttacking = true;
    spriteShown = false; // 공격 시작 시 스프라이트 상태 초기화

   
    transform.LookAt(Camera.main.transform.position);
    ChangeSpriteToNew();
    yield return new WaitForSeconds(attackInterval); // 공격 간격만큼 대기
    slimeAnimator.SetTrigger("Attack");
     ChangeSpriteToDefault();
    yield return new WaitForSeconds(attackInterval); // 공격 간격만큼 대기
    isAttacking = false;
   
    MoveTowardsTarget();
}

    void CreateTargetSprite()
    {
        if (targetObject == null)
        {
            targetObject = new GameObject("TargetObject");
            targetObject.transform.parent = transform;
            targetObject.transform.localPosition = new Vector3(0, 0.5f, -0.5f);
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
            Debug.Log("changenew");
            spriteShown = true;
        }
    }

    void ChangeSpriteToDefault()
    {
        if (spriteShown)
        {
            
            ChangeSprite(targetSprite);
            Debug.Log("change");
            spriteShown = false;
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
