using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ClickSelector : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactable;
    private Interactable currentHoveredObject;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


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
    }
}
