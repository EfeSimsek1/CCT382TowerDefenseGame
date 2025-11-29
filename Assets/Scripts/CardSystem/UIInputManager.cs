using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIInputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactable;
    [SerializeField] private LayerMask groundLayer;
    private Interactable currentHoveredObject;
    private Interactable ground;
    public static Vector3 groundPos;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        #region Non-ground interaction

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactable) && hit.collider.gameObject.GetComponent<Interactable>() != null)
        {
            Interactable prevSelected = currentHoveredObject;
            Interactable newSelected = hit.collider.gameObject.GetComponent<Interactable>();

            if (currentHoveredObject == null) //Mouse entered an object
            {
                currentHoveredObject = newSelected;
                currentHoveredObject.OnMouseEnterObj();
            }
            else if (newSelected != currentHoveredObject) //Mouse exited an object and entered a new one
            {
                prevSelected.OnMouseExitObj();
                currentHoveredObject = newSelected;
                newSelected.OnMouseEnterObj();
            }
            else //Mouse is still on the same object
            {
                currentHoveredObject.OnMouseHoverObj();
            }
        }
        else if(currentHoveredObject != null) // Mouse exited an object
        {
            Interactable prevSelected = currentHoveredObject.GetComponent<Interactable>();
            prevSelected.OnMouseExitObj();
            currentHoveredObject = null;
        }

        if (Input.GetMouseButtonDown(0) && currentHoveredObject)
        {
            currentHoveredObject.OnMouseDownObj();
        }
        else if (Input.GetMouseButtonUp(0) && currentHoveredObject)
        {
            currentHoveredObject.OnMouseUpObj();
        }

        #endregion

        #region Ground interaction

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer) && hit.collider.gameObject.GetComponent<Interactable>() != null)
        {
            ground = hit.collider.gameObject.GetComponent<Interactable>();
            ground.OnMouseHoverObj();
            groundPos = hit.point;
        }
        else if(ground != null)
        {
            ground.OnMouseExitObj();
            ground = null;
        }

        #endregion

        if (Input.GetMouseButtonDown(0) && ground)
        {
            ground.OnMouseDownObj();
        }
        else if (Input.GetMouseButtonUp(0) && ground)
        {
            ground.OnMouseUpObj();
        }

    }
}
