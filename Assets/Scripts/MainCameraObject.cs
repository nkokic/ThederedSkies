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
    float speed, reach;

    [SerializeField]
    Vector2 cursorPosition;

    Func<Vector2> CursorSetMethod;

    [SerializeField]
    float maxShake, minShake = 0.1f, accumulatedShake = 0, shakeDamping = 0.1f;

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
        {
            UpdateLocation();
        }
        else
        {
            FreeLook();
        }

        ScreenShake();
    }

    void UpdateLocation()
    {
        Vector3 locationT = Vector2.Lerp(
            Camera.main.transform.position,
            (Vector2)targetTransform.position + cursorPosition * reach * Camera.main.orthographicSize,
            speed * Time.deltaTime
            );
        Camera.main.transform.position = locationT + new Vector3(0,0, Camera.main.transform.position.z);
    }

    void FreeLook()
    {
        Camera.main.transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * speed * Time.deltaTime;
    }

    void ScreenShake()
    {
        if (accumulatedShake > minShake)
            Camera.main.transform.position += new Vector3(UnityEngine.Random.value * 2f - 1f, UnityEngine.Random.value * 2f - 1f) * accumulatedShake;

        accumulatedShake = Mathf.MoveTowards(accumulatedShake, 0, shakeDamping * Time.deltaTime);
    }

    static public void AddShake(float impulse)
    {
        singletonCamera.accumulatedShake = Mathf.MoveTowards(singletonCamera.accumulatedShake, singletonCamera.maxShake, impulse);
    }

    static public void SetTargetObject(Transform transform)
    {
        singletonCamera.targetTransform = transform;
    }

    void ChooseCursor()
    {
        if (JoystickPosition().magnitude > 0.1f)
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
