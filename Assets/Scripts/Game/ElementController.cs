using UnityEngine;

// Stateless element setter/changer
public class ElementController : MonoBehaviour
{
    public void SetElement(Element element)
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor", ColorForElement(element));
    }

    Color ColorForElement(Element newElement)
    {
        var configuration = DependencyContainer.Instance.gameConfiguration;
        switch (newElement) {
            case Element.earth:
                return configuration.earthColor;
            case Element.water:
                return configuration.waterColor;
            default:
                return configuration.waterColor;
        }
    }
}
