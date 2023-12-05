using UnityEngine;
using UnityEngine.UI;

public class PlayerNameManager : MonoBehaviour
{
    public GameObject obj;
    private static PlayerNameManager instance;
    public static PlayerNameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerNameManager>();
            return instance;
        }
    }

    public string playerName;
    public InputField playerNameInputField;
 public Button saveButton; // Inspector에서 직접 연결

    void Start()
    {
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SavePlayerName);
        }
        else
        {
            Debug.LogError("saveButton이 할당되지 않았습니다.");
        }
    }


    void SavePlayerName()
    {
        playerName = playerNameInputField.text;
        obj= GameObject.Find("Phoenix");
        obj.GetComponent<Phoenix>().playerName2 = playerName;
        Debug.Log("플레이어 이름 저장됨: " + playerName);
    }
}
