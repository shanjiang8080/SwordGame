using UnityEngine;
using UnityEngine.InputSystem;

public class PivotMovement : MonoBehaviour
{
    // need reference to Camera and Player location for mouse control scheme
    public Transform player;
    public new Camera camera;
    public SwordController controller;

    InputAction pivotAction;
    InputAction throwAction;
    private Vector2 angle;

    public bool isThrown => controller.isThrown;
    public bool isRetracting => controller.isRetracting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pivotAction = InputSystem.actions.FindAction("Pivot");
        throwAction = InputSystem.actions.FindAction("Throw");
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            // using a gamepad
            angle = pivotAction.ReadValue<Vector2>().normalized;

        }
        else if (Mouse.current != null)
        {
            var worldposition = camera.ScreenToWorldPoint(pivotAction.ReadValue<Vector2>());
            worldposition.z = 0f;
            angle = (worldposition - player.transform.position).normalized;
        }

        // read throw/recall button
        if (throwAction.WasPressedThisFrame())
        {
            if (!isThrown && !isRetracting)
            {
                Debug.Log("throw it!");
                // throw has happened. throw the thing
                // unparent the thing
                transform.parent = null;
                controller.Throw();
            } else if (!isRetracting)
            {
                Debug.Log("recall it!");
                // recall has happened...
                controller.Recall();
            }
        }
    }
    void FixedUpdate()
    {
        if (!isThrown)
        {
            controller.Move(angle);
        }
    }
}
