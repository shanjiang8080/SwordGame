using UnityEngine;
using UnityEngine.InputSystem;

public class PivotController : MonoBehaviour
{
    // need reference to Camera and Player location for mouse control scheme
    public Transform player;
    public new Camera camera;

    InputAction pivotAction;
    private Vector2 angle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pivotAction = InputSystem.actions.FindAction("Pivot");
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
    }
    void FixedUpdate()
    {
        //Debug.Log(angle);
    }
}
