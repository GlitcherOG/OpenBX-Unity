using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddonListingScript : MonoBehaviour
{
    public int Index;
    public AddonMenu menu;
    public Image Icon;
    public TMP_Text label;
    public Toggle toggle;

    bool DisablePush;
    public void UpdateData(AddonLoading.AddonData addonData, int AddonIndex)
    {
        DisablePush = true;
        Rect rect = new Rect(0, 0, addonData.icon.width, addonData.icon.height);
        Icon.sprite = Sprite.Create(addonData.icon, rect, new Vector2(0.5f, 0.5f));
        label.text = addonData.Name;
        toggle.isOn = addonData.Load;
        Index = AddonIndex;
        DisablePush = false;
    }
    
    public void SendToggle(bool Value)
    {
        if (!DisablePush)
        {
            menu.ToggleAddon(Index, Value);
        }
    }
}
