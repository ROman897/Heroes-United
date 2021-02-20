using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private int next_level = 0;

    private static LevelManager instance;

    private bool in_game = false;

    private int enemies_count;

    private Dictionary<Vector2Int, BuyableUnit> heroes = new Dictionary<Vector2Int, BuyableUnit>();

    [SerializeField]
    private int starting_money;

    private int money;

    [SerializeField]
    private int[] money_for_level;

    public static LevelManager singleton() {
        return instance;
    }

    private void on_scene_loaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "MainMenu") {
            next_level = 0;
            money = starting_money;
            heroes = new Dictionary<Vector2Int, BuyableUnit>();
            GameObject.Find("StartGameButton").GetComponent<Button>().onClick.AddListener(start_game);
        }

        if (scene.name.StartsWith("Level")) {
            set_up_game();
        }
    }

    public bool try_spend_money(int amount) {
        if (money < amount) {
            return false;
        }
        money -= amount;
        return true;
    }

    public int get_money() {
        return money;
    }

    private void set_up_game() {
        GameObject.FindObjectOfType<PlayerController>().spawn_heroes(heroes);
    }

    private void start_game() {
        load_shop();
    }

    public void level_won(Dictionary<Vector2Int, BuyableUnit> current_heroes) {
        money += money_for_level[next_level - 1];
        heroes = current_heroes;
        load_shop();
    }

    private void load_shop() {
        SceneManager.LoadScene("UnitShop");
    }

    public void load_main_menu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void play_next_level(Dictionary<Vector2Int, BuyableUnit> formation) {
        heroes = formation;
        SceneManager.LoadScene("Level" + next_level);
        ++next_level;
    }

    public Dictionary<Vector2Int, BuyableUnit> get_remaining_heroes() {
        return heroes;
    }

    void Awake() {
        DontDestroyOnLoad(gameObject);
        instance = this;
        SceneManager.LoadScene("MainMenu");
        SceneManager.sceneLoaded += on_scene_loaded;
    }
}
