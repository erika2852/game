using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToChangeScene : MonoBehaviour
{
    // 클릭 시 이동할 Scene의 이름
    public string targetSceneName;

    private void OnMouseDown()
    {
        // targetSceneName에 지정된 Scene으로 이동
        SceneManager.LoadScene(targetSceneName);
    }
}
