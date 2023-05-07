using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddonMenu : MonoBehaviour
{
    public GameObject UIPrefab;
    public GameObject ViewPort;
    public bool Changed;

    public List<AddonListingScript> addonListingScripts = new List<AddonListingScript>();
    public void GenerateUI()
    {
        Changed = false;
        for (int i = 0; i < AddonLoading.instance.addonDatas.Count; i++)
        {
            var Temp = Instantiate(UIPrefab, ViewPort.transform);
            AddonListingScript addon = Temp.GetComponent<AddonListingScript>();
            addon.UpdateData(AddonLoading.instance.addonDatas[i], i);
            addon.menu = this;

            addonListingScripts.Add(addon);
        }
    }

    public void ToggleAddon(int Index, bool Toggle)
    {
        var Temp = AddonLoading.instance.addonDatas[Index];
        Changed = true;
        Temp.Load = Toggle; 
        AddonLoading.instance.addonDatas[Index] = Temp;
    }

    public void DestroyUI()
    {
        for (int i = 0; i < addonListingScripts.Count; i++)
        {
            Destroy(addonListingScripts[i].gameObject);
        }
        addonListingScripts.Clear();
        addonListingScripts = new List<AddonListingScript>();
    }
}
