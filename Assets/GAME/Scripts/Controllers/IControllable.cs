using UnityEngine;
/// <summary>
/// Interface for objects that can be controlled by the playercontroller
/// </summary>
public interface IControllable
{
    void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput);
}
