using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditManager : MonoBehaviour
{
    GameObject golds;
    GameObject gems;
    public GameObject crystals;
    public GameObject crystalPopup;
    public Text crystalText;
    private void Awake()
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
        crystalText.text = $"남은 수량: {GameManager.Instance.Crystals}";
    }

    private void OnCrystalBtnClicked()
    {
        if (GameManager.Instance.Crystals < 1)
            return;
        Time.timeScale = 0;
        crystalPopup.SetActive(true);
    }
}
