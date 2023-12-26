using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager :MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = -1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
