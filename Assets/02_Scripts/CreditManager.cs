using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditManager : MonoBehaviour
{
    GameObject golds;
    GameObject gems;
    GameObject crystals;
    public GameObject crystalPopup;
    private void Start()
    {
        golds = transform.Find("Golds").gameObject;
        gems = transform.Find("Gems").gameObject;
        crystals = transform.Find("Crystals").gameObject;
        crystalPopup.SetActive(false);
        crystals.GetComponent<Button>().onClick.AddListener(() => OnCrystalBtnClicked());
    }
    public void SetGold()
    {
        if (golds == null)
            golds = transform.Find("Golds").gameObject;
        golds.GetComponentInChildren<Text>().text = GameManager.Instance.Gold.ToString();
    }
    public void SetGem()
    {
        if (gems == null)
            gems = transform.Find("Gems").gameObject;
        gems.GetComponentInChildren<Text>().text = GameManager.Instance.Gem.ToString();
    }

    public void SetCrystal()
    {
        if (crystals == null)
            crystals = transform.Find("Crystals").gameObject;
        crystals.GetComponentInChildren<Text>().text = GameManager.Instance.Crystals.ToString();
    }

    private void OnCrystalBtnClicked()
    {
        if (GameManager.Instance.Crystals < 1)
            return;
        crystalPopup.SetActive(true);
    }
}
