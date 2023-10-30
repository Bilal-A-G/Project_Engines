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

    public PresetColor selectedColor;

    private void OnValidate()
    {
        // Change the object's color in the Editor when the 'selectedColor' value changes
        switch (selectedColor)
        {
            case PresetColor.None:
                GetComponent<Renderer>().material.color = Color.white;
                    break;
            case PresetColor.Red:
                GetComponent<Renderer>().material.color = Color.red;
                break;
            case PresetColor.Green:
                GetComponent<Renderer>().material.color = Color.green;
                break;
            case PresetColor.Blue:
                GetComponent<Renderer>().material.color = Color.blue;
                break;
            default:
                break;
        }
    }
}
