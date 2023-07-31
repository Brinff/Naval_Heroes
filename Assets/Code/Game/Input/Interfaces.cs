using System.Collections;
using UnityEngine;

public interface IInputListener
{

}

public interface IInput
{

}

public interface IInput<T> : IInput where T : IInputListener
{
    void SetListener(T listener);
}

public interface IInputCameraAxisListener : IInputListener
{
    void OnInputCameraAxis(Vector2 axis);
}

public interface IInputMovementAxisListener : IInputListener
{
    void OnInputMovementAxis(float rudder, float throttle);
}

public interface IInputZoomListener : IInputListener
{
    void OnInputZoomToggle(bool isActive);
    void OnInputZoomFactor(float value);
}

public interface IInputFireListener : IInputListener
{
    void OnInputFire(bool isFire);
}