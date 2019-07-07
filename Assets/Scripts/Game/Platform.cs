using UnityEngine;


public class Platform : MonoBehaviour
{
    ElementController elementController;

    [HideInInspector]
    public Element element;


    void Awake()
    {
        elementController = GetComponent<ElementController>();
    }
    
    public void SetElement(Element newElement)
    {
        element = newElement;
        elementController.SetElement(newElement);
    }
}
