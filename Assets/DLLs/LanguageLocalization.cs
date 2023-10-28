using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public enum Language
{
    English,
    French,
    Portuguese
}
public class LanguageLocalization : MonoBehaviour
{
    Language selectedLanguage = Language.English;

    // Import the functions from the DLL
    [DllImport("LanguageTranslation")] 
    private static extern void InitializeTranslations();

    [DllImport("LanguageTranslation")]
    private static extern IntPtr Translate(string key, int language);

    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private TMP_Dropdown languageDropdown;
    private string[] defaults;

    void Start()
    {
        defaults = new string[texts.Length-1];
        for (var i = 0; i < texts.Length-1; i++)
        {
            defaults[i] = texts[i].text.Trim();
        }

        // Initialize translations when the game starts
        InitializeTranslations();
        int n = PlayerPrefs.GetInt("Language");
        selectedLanguage = (Language)n;
        languageDropdown.SetValueWithoutNotify(n);
        UpdateAllTexts();
    }

    public void UpdateSelectedLang()
    {
        PlayerPrefs.SetInt("Language", (int)selectedLanguage);
        selectedLanguage = (Language) languageDropdown.value;
        UpdateAllTexts();
    }

    private void UpdateAllTexts()
    {
        
        int v = (int) selectedLanguage;
        
        for (var index = 0; index < texts.Length-1; index++)
        {
            print(defaults[index] + " --> " + selectedLanguage);
            texts[index].text = Marshal.PtrToStringAnsi(Translate(defaults[index], v));
        }
        
        //Cheesy, but I'm tired and don't want to make a conversion system because dlls are really hard to work with
        texts[^1].text = Marshal.PtrToStringAnsi(Translate(languageDropdown.options[languageDropdown.value].text, v));
    }



}
