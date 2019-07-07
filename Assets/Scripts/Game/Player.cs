using UnityEngine;


public class Player : MonoBehaviour
{
    PlayerAnimator animator;
    ElementController elementController;
    public Element element = Element.water;
    public Score score;

    private void Awake()
    {
        animator = GetComponent<PlayerAnimator>();
        elementController = GetComponent<ElementController>();
    }

    public void ResetState()
    {
        animator.ResetAnimations();
    }

    #region Element

    public void ChangeElement()
    {
        SetElement(GetNextElement(element));
    }

    public void SetElement(Element newElement, bool animated = true)
    {
        element = newElement;
        elementController.SetElement(newElement);
        if (animated) {
            animator.Spin();
        }
    }

    public Element GetNextElement(Element currentElement)
    {
        switch (currentElement) {
            case Element.earth:
                return Element.water;
            case Element.water:
                return Element.earth;
            default:
                return Element.water;
        }
    }

    #endregion
}
