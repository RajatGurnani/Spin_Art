using UnityEngine;

public class Table : MonoBehaviour
{
    public float rotateSpeed = 1f;

    private void Update()
    {
        transform.Rotate(Vector3.forward, 360 * rotateSpeed * Time.deltaTime, Space.Self);
    }
}
