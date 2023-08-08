using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float _xRotation;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public void ProcessLook(Vector2 input)
    {
        RotateUpAndDown(input.y); 
        RotateLeftAndRight(input.x); 
    }

    private void RotateUpAndDown(float mouseY)
    {
        _xRotation -= mouseY * Time.deltaTime * ySensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
        cam.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
    }
    
    private void RotateLeftAndRight(float mouseX)
    {
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime * xSensitivity));
    }
}