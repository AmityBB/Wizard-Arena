using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Player player;
    public float xSen;
    public float ySen;

    private float xRotation;
    private float yRotation;
    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }
    void FixedUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.45f, player.transform.position.z);
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSen;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySen;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        player.gameObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
