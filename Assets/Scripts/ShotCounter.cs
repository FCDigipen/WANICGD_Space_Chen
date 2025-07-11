using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShotCounter : MonoBehaviour
{
    // manage shots
    private int shots = 0;
    private TextMeshProUGUI text;

    public int getShots() {return shots;}

    // Start is called before the first frame update
    void Start(){text = GetComponent<TextMeshProUGUI>();}
    public void UpdateShots(){++shots; text.text = $"shots={shots};";}

}
