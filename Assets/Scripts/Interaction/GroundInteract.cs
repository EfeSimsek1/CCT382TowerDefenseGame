using UnityEngine;

public class GroundInteract : Interactable
{
    [SerializeField] private GameObject abilityRangeIndicator;

    void Start()
    {
        abilityRangeIndicator.SetActive(false);
    }

    public override void OnMouseHoverObj()
    {
        base.OnMouseHoverObj();

        abilityRangeIndicator.transform.position = UIInputManager.groundPos + Vector3.up * (abilityRangeIndicator.transform.lossyScale.y/2);

        Card heldCard = CardInteractionManager.HeldCard;

        if (CardInteractionManager.IsCardHeld() && heldCard.cardType == Card.CardType.Ability && ((AbilityCard)heldCard).abilityType == Card.AbilityType.AOE)
        {
            abilityRangeIndicator.SetActive(true);
            abilityRangeIndicator.transform.localScale = new Vector3(((AOEAbilityCard)heldCard).radius, abilityRangeIndicator.transform.localScale.y, ((AOEAbilityCard)heldCard).radius);
        }
        else if(!CardInteractionManager.IsCardHeld())
        {
            abilityRangeIndicator.SetActive(false);
        }

        Card lastHeldCard = CardInteractionManager.LastHeldCard;

        if (CardInteractionManager.cardReleasedTrigger && lastHeldCard.cardType == Card.CardType.Ability && ((AbilityCard)lastHeldCard).abilityType == Card.AbilityType.AOE)
        {
            ((AbilityCard)lastHeldCard).Activate();
            CardInteractionManager.PlayCard(lastHeldCard);
            abilityRangeIndicator.SetActive(false);
            CardInteractionManager.cardReleasedTrigger = false;
        }
    }

    public override void OnMouseExitObj()
    {
        base.OnMouseExitObj();

        abilityRangeIndicator.SetActive(false);
    }
}
