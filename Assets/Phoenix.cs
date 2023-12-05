using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phoenix : MonoBehaviour
{
    public Phoenix instance = null;
    public string playerName2 = null;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance == null)
        {
            Destroy(gameObject);
        }
        Debug.Log(playerName2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
