using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private RectTransform canvasRectTranform;
    private Vector3 originalScale;
    private int currentState = 0;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    //[SerializeField] private float rotationTime;
    [SerializeField] private float lerpFactor = 0.1f;

    [SerializeField] private int cardPlayDivider = 4;
    [SerializeField] private float cardPlayMultiplier = 1f;
    [SerializeField] private bool needUpdateCardPlayPosition = false;
    [SerializeField] private int playPositionYDivider = 2;
    [SerializeField] private float playPositionYMultiplier = 1f;
    [SerializeField] private int playPositionXDivider = 4;
    [SerializeField] private float playPositionXMultiplier = 1f;
    [SerializeField] private bool needUpdatePlayPosition = false;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null) 
        {
            canvasRectTranform = canvas.GetComponent<RectTransform>();
        }

        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;

        updateCardPlayPostion();
        updatePlayPostion();
    }

    private void Update()
    {
        if (needUpdateCardPlayPosition)
        {
            updateCardPlayPostion();
        }

        if (needUpdatePlayPosition)
        {
            updatePlayPostion();
        }

        switch (currentState) 
        {
            case 1:
                HandleHoverState();
                break;
            case 2:
                HandleDragState();
                if (!Input.GetMouseButton(0))
                {
                    TransitionToState0();
                }
                break;
            case 3:
                HandlePlayState();
                if (!Input.GetMouseButton(0))
                {
                    TransitionToState0();
                }
                break;

        }
    }

    private void TransitionToState0()
    {
        currentState = 0;
        rectTransform.localScale = originalScale; //Reset Scale
        rectTransform.localRotation = originalRotation; //Reset Rotation
        rectTransform.localPosition = originalPosition; //Reset Position
        glowEffect.SetActive(false); //Disable glow effect
        playArrow.SetActive(false); //Disable play arrow
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            //originalPosition = rectTransform.localPosition;
            //originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale;

            currentState = 1;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == 2)
        {
            if (Input.mousePosition.y > cardPlay.y)
            {
                currentState = 3;
                playArrow.SetActive(true);
                rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPosition, lerpFactor);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Update what card you are holding
        CardInteractionManager.HoldCard(GetComponent<CardDisplay>().cardData);

        if (currentState == 1)
        {
            currentState = 2;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // Update what card you are holding(or in this case, NOT holding)
        CardInteractionManager.ReleaseCard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(currentState == 1)
        {
            TransitionToState0();
        }
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }
    private void HandleDragState()
    {
        //Set card rotation to zero
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.position = Vector3.Lerp(rectTransform.position, Input.mousePosition, lerpFactor);
    }

    private void HandlePlayState()
    {
        rectTransform.localPosition = playPosition;
        rectTransform.localRotation = Quaternion.identity;

        if(Input.mousePosition.y < cardPlay.y)
        {
            currentState = 2;
            playArrow.SetActive(false);
        }
    }
    private void updateCardPlayPostion()
    {
        if (cardPlayDivider != 0 && canvasRectTranform != null)
        {
            float segment = cardPlayMultiplier / cardPlayDivider;

            cardPlay.y = canvasRectTranform.rect.height * segment;
        }
    }

    private void updatePlayPostion()
    {
        if (canvasRectTranform != null && playPositionYDivider != 0 && playPositionXDivider != 0)
        {
            float segmentX = playPositionXMultiplier / playPositionXDivider;
            float segmentY = playPositionYMultiplier / playPositionYDivider;

            playPosition.x = canvasRectTranform.rect.width * segmentX;
            playPosition.y = canvasRectTranform.rect.height * segmentY;
        }
    }

    public void SetRestPositionAndRotation(Vector3 position, Vector3 rotation)
    {
        originalPosition = position;
        originalRotation = Quaternion.Euler(rotation);
    }

}
