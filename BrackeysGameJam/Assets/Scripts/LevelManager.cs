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

    private Dictionary<Vector2Int, BuyableUnit> heroes;

    public static LevelManager singleton() {
        return instance;
    }

    private void on_scene_loaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "MainMenu") {
            next_level = 0;
            GameObject.Find("StartGameButton").GetComponent<Button>().onClick.AddListener(start_game);
        }

        if (scene.name.StartsWith("Level")) {
            set_up_game();
        }
    }

    private void set_up_game() {
        GameObject.FindObjectOfType<PlayerController>().spawn_heroes(heroes);
    }

    private void start_game() {
        load_shop(new Dictionary<Vector2Int, BuyableUnit>());
    }

    public void load_shop(Dictionary<Vector2Int, BuyableUnit> current_heroes) {
        heroes = current_heroes;
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
