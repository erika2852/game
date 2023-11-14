using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject rangeObject;
    public GameObject capsul;
    public float spawnInterval = 1f;
    public int maxCount = 10;

    private BoxCollider rangeCollider;
    private int currentCount = 0;

    public List<Vector3> spawnedPositions = new List<Vector3>();
    public float minimumDistance = 2.0f;

    private void Awake()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
    }

    private void Start()
    {
        StartCoroutine(RandomRespawn_Coroutine());
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        while (currentCount < maxCount)
        {
            yield return new WaitForSeconds(spawnInterval);

            GameObject instantCapsul = Instantiate(capsul, Return_RandomPosition(), Quaternion.identity);
            currentCount++;
        }
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 newPosition;
        int safetyNet = 100;

        do
        {
            Vector3 originPosition = rangeObject.transform.position;
            float range_X = rangeCollider.bounds.size.x;
            float range_Z = rangeCollider.bounds.size.z;

            range_X = Random.Range((range_X / 2) * -1, range_X / 2);
            range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);

            newPosition = originPosition + new Vector3(range_X, 0f, range_Z);

            safetyNet--;
            if (safetyNet <= 0)
            {
                Debug.LogError("Too many attempts to find a new spawn position.");
                return newPosition;
            }
        }
        while (IsPositionTooClose(newPosition));

        spawnedPositions.Add(newPosition);
        return newPosition;
    }

    bool IsPositionTooClose(Vector3 position)
    {
        foreach (var spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minimumDistance)
            {
                return true;
            }
        }
        return false;
    }
}
