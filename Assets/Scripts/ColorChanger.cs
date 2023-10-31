using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public enum PresetColor
    {
        None,
        Red,
        Green,
        Blue
    }
    public Color colour;

    public PresetColor selectedColor;

    private void OnValidate()
    {
        // Change the object's color in the Editor when the 'selectedColor' value changes
        switch (selectedColor)
        {
            case PresetColor.None:
                colour = Color.white;
                    break;
            case PresetColor.Red:
                colour = Color.red;
                break;
            case PresetColor.Green:
                colour = Color.green;
                break;
            case PresetColor.Blue:
                colour = Color.blue;
                break;
            default:
                break;
        }
        GetComponent<Renderer>().material.color = colour;
    }
}
