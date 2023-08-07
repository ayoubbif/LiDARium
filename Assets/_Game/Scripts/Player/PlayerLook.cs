using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float _xRotation;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    private void Awake()
    {
        // Hide and lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ProcessLook(Vector2 input)
    {
        var mouseX = input.x;
        var mouseY = input.y;
        
        // Rotate player to look up and down
        _xRotation -= mouseY * Time.deltaTime * ySensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
        
        // Apply this to camera transform 
        cam.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        
        // Rotate player to look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime * xSensitivity));
    }
}
