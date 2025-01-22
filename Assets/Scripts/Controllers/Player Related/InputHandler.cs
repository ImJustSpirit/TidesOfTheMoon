using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public InputActionAsset controls;
    public PlayerController shipController;

    void OnEnable()
    {
        controls.Enable();
        //controls.Ship.Move.performed += shipController.OnMove;
        //controls.Ship.Move.canceled += shipController.OnMove;
    }

    void OnDisable()
    {
        //controls.Ship.Move.performed -= shipController.OnMove;
        //controls.Ship.Move.canceled -= shipController.OnMove;
        controls.Disable();
    }
}