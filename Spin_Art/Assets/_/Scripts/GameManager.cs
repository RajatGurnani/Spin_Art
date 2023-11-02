using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager :MonoBehaviour
{
    //public override void Awake()
    //{
    //    base.Awake();
    //    name = nameof(GameManager);
    //}

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
