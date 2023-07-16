using UnityEngine;
public interface IControllable
{
    void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput);
}
