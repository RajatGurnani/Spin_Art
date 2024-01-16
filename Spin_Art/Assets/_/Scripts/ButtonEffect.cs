using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour
{
    public float duration = 0f;
    public float pressDuration;
    public Image pressedImage;
    Sequence sequence;
    public bool setOnAwake = true;
    public bool oneShot = true;
    public UnityEvent OnPress;

    private void Awake()
    {
        pressedImage.transform.localScale = Vector3.zero;
        sequence = DOTween.Sequence()
        .OnStart(() =>
        {
            pressedImage.transform.localScale = Vector3.zero;
        })
        .Append(pressedImage.transform.DOScale(Vector3.one, pressDuration))
        .OnComplete(() =>
        {
            pressedImage.transform.localScale = Vector3.zero;
        })
        .SetUpdate(UpdateType.Normal, true)
        .SetAutoKill(false)
        .OnKill(() =>
        {
            pressedImage.transform.localScale = Vector3.zero;
        })
        .Pause();

        if (setOnAwake)
        {
            GetComponent<Button>().onClick.AddListener(ButtonPressed);
        }
    }

    public void ButtonPressed()
    {
        sequence.Complete();
        sequence.Restart();
    }

    public void OnPressEnter() 
    {

    }

    public void OnPressed()
    {

    }

    public void OnPressExit()
    {

    }
}
