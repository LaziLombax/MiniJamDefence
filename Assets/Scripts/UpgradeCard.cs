using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    public string weapon;
    public TextMeshProUGUI title;
    public TextMeshProUGUI upgradeName;
    public Image upgradeIconRenderer;
    public Button confirmUpgrade;

    public void UpdateUpgradeCard(UpgradeMethod upgradeMethod, UpgradeInfo infoToUse)
    {
        upgradeIconRenderer.sprite = infoToUse.icon;
        upgradeName.text = infoToUse.info;
        title.text = weapon;
        confirmUpgrade.onClick.RemoveAllListeners();
        confirmUpgrade.onClick.AddListener(GameManager.Instance.UIHandler.ConfirmFade);
        confirmUpgrade.onClick.AddListener(delegate { GameManager.Instance.Upgrade(upgradeMethod);});
    }
}
