using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropDownHandler : MonoBehaviour
{
    // Reference to the Dropdown component
    public TMP_Dropdown dropdown;

    void Start()
    {
        // Ensure the dropdown is assigned
        if (dropdown != null)
        {
            // Add a listener to handle changes
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
    }

    // Function called when the dropdown value changes
    void OnDropdownValueChanged(int index)
    {
        SceneManagerETW.Instance.SetDifficulty(index);
    }

    void OnDestroy()
    {
        // Clean up the listener when the object is destroyed
        if (dropdown != null)
        {
            dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }
    }
}
