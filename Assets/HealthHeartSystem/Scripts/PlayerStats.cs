/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using UnityEngine;
using UnityEngine.SceneManagement;  // Import the SceneManager class

public class PlayerStats : MonoBehaviour
{
    public GameObject obj;
public GameObject obj2;

public GameObject obj3;
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;
    public string playerName2;
    #region Singleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerStats>();
            return instance;
        }
    }
    #endregion

    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float maxTotalHealth;

    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }

    public void Heal(float health)
    {
        this.health += health;
        ClampHealth();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        ClampHealth();

        // Check if health is zero or below, and transition to a specified scene
        if (health <= 0)
        {
            obj2= GameObject.Find("Phoenix");
            obj= GameObject.Find("StartProgram");
            obj3= GameObject.Find("GameManager");
            obj.GetComponent<StartProgram>().CreateCharacter(obj2.GetComponent<Phoenix>().playerName2,obj3.GetComponent<RandomSpawn>().score);
            SceneManager.LoadScene("gameover");
        }
    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }
    }

    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }
}
