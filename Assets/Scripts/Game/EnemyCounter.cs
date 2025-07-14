using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCounter : MonoBehaviour
{
    // script to manage the number of enemies and call coresponding code that relies on the number
    private int enemies = 0; // count of enmies
    private TextMeshProUGUI text;
    [SerializeField] GameObject scene;
    StateManager sm;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        sm = scene.GetComponent<StateManager>();
    }

    public void AddEnemy() {
        ++enemies;
        if(!text) {text = GetComponent<TextMeshProUGUI>();}
        text.text = $"enemies={enemies};";
    }

    public void RemoveEnemy() {
        --enemies;
        if(!text) {text = GetComponent<TextMeshProUGUI>();}
        if(enemies == 0) {sm.Win();}
        text.text = $"enemies={enemies};";
    }
}
