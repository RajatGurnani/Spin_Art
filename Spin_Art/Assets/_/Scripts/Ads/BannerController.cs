using UnityEngine;

public class BannerController : MonoBehaviour
{
    private void OnEnable()
    {
        AdmobController.Instance.ShowBanner();
    }

    private void OnDisable()
    {
        AdmobController.Instance.HideBanner();
    }
}
