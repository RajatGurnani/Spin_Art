using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public float swipeStartTime = 0;
    Vector2 startPosition = Vector2.zero;
    public float swipeTimeThreshold = 0.5f;
    public float swipeDistanceThreshold = 60f;

    public GameObject gamePlayMenu;
    public GameObject vCam;

    private void Update()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    Debug.Log("asdfasdf");
        //    return;
        //}
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            swipeStartTime = Time.timeSinceLevelLoad;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 direction = (Vector2)Input.mousePosition - startPosition;

            if ((direction.x > swipeDistanceThreshold) && (Time.timeSinceLevelLoad - swipeStartTime < swipeTimeThreshold))
            {
                ChangePage();
            }
        }
    }

    public void ChangePage()
    {
        FindObjectOfType<PaintBrush>(true).gameObject.SetActive(true);
        gamePlayMenu.SetActive(true);
        gameObject.SetActive(false);
        vCam.SetActive(false);
    }
}
