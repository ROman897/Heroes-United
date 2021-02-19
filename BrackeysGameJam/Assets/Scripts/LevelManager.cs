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

    public static LevelManager singleton() {
        return instance;
    }

    private void on_scene_loaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "MainMenu") {
            GameObject.Find("StartGameButton").GetComponent<Button>().onClick.AddListener(start_game);
        }

        if (scene.name.StartsWith("Level")) {
            in_game = true;
            set_up_game();
        } else {
            in_game = false;
        }
    }

    private void set_up_game() {
        enemies_count = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void character_died(Character character) {
    }

    private void start_game() {
        next_level = 0;
        play_next_level();
    }

    public void play_next_level() {
        SceneManager.LoadScene("Level" + next_level);
        ++next_level;
    }

    void Awake() {
        DontDestroyOnLoad(gameObject);
        instance = this;
        SceneManager.LoadScene("MainMenu");
        SceneManager.sceneLoaded += on_scene_loaded;
    }
}
