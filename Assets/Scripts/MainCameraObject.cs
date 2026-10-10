using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainCameraObject : MonoBehaviour
{
    public static MainCameraObject singletonCamera = null;

    [SerializeField]
    Transform targetTransform;

    [SerializeField]
    ScreenShake screenShake;

    public ScreenShake ScreenShake => screenShake;

    [SerializeField]
    float speed, reach;

    [SerializeField]
    Vector2 cursorPosition;

    Func<Vector2> CursorSetMethod;

    //Camera camera = Camera.main;

    private void Awake()
    {
        singletonCamera = this;
        CursorSetMethod = MousePosition;

    }
    void Update()
    {
        ChooseCursor();
        cursorPosition = CursorSetMethod();

        if(targetTransform)
            UpdateLocation();
    }

    void UpdateLocation()
    {
        Vector3 locationT = Vector2.Lerp(
            Camera.main.transform.position,
            (Vector2)targetTransform.position + cursorPosition * reach * Camera.main.orthographicSize,
            speed * Time.deltaTime
            );
        Camera.main.transform.position = locationT + new Vector3(0,0, Camera.main.transform.position.z) 
            + (Vector3)ScreenShake.UpdateState(Time.deltaTime);
    }

    static public void SetTargetObject(Transform transform)
    {
        singletonCamera.targetTransform = transform;
    }

    void ChooseCursor()
    {
        if (JoystickPosition().magnitude > 0.01f)
            CursorSetMethod = JoystickPosition;
        if(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.1f)
            CursorSetMethod = MousePosition;
        
    }

    Vector2 MousePosition()
    {
        return new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height) * 2 - Vector2.one;
    }

    Vector2 JoystickPosition()
    {
        return new Vector2(Input.GetAxis("Joystick X"), Input.GetAxis("Joystick Y"));
    }
}
