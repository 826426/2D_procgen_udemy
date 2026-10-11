using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    void Update()
    {
        ammoText.text = Ammunition.ammoCount.ToString();
    }
}
