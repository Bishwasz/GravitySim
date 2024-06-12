using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHovered = false;

    // This method is called when the pointer enters the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        //Debug.Log("Pointer Entered");
    }

    // This method is called when the pointer exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    // Method to check if the button is currently hovered
    public bool IsButtonHovered()
    {
        return isHovered;
    }
}
