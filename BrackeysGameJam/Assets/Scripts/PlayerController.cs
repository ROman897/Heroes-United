using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    class ControlledCharacter {
        public PlayerCharacterController player_character;
        public Vector2 offset_from_center;
    }

    [SerializeField]
    private float rotation_speed;

    [SerializeField]
    private bool FollowsMousePos;

    [SerializeField]
    private bool Q_and_E_to_rotate;

    private Vector2 centerPoint;

    private List<ControlledCharacter> controlled_characters = new List<ControlledCharacter>();

    private const float hero_formation_distance = 0.5f;

    public void spawn_heroes(Dictionary<Vector2Int, GameObject> heroes) {
        Vector2[] positions = new Vector2[heroes.Count];
        int index = 0;
        foreach (var pos_hero in heroes) {
            positions[index] = new Vector2(pos_hero.Key.x * hero_formation_distance, pos_hero.Key.y * hero_formation_distance);
            ++index;
        }

        Vector2 center = GetCenterPoint(positions);

        index = 0;
        foreach (var pos_hero in heroes) {
            Vector2 hero_formation_pos = new Vector2(pos_hero.Key.x * hero_formation_distance, pos_hero.Key.y * hero_formation_distance);
            Vector2 hero_offset = hero_formation_pos - center;

            GameObject hero_go = GameObject.Instantiate(pos_hero.Value, transform.position + (Vector3)hero_offset, Quaternion.identity, transform);

            ControlledCharacter controlled_character = new ControlledCharacter();
            controlled_character.player_character = hero_go.GetComponent<PlayerCharacterController>();
            controlled_character.offset_from_center = hero_offset;
            controlled_characters.Add(controlled_character);
            ++index;
        }
    }

    Vector2 GetCenterPoint(Vector2[] points)
    {
        if (points.Length == 1)
        {
            return points[0];
        }
        var bounds = new Bounds(points[0], Vector3.zero);
        for (int i = 0; i < points.Length; i++)
        {
            bounds.Encapsulate(points[i]);
        }
        return bounds.center; //returns the center point of the GamObject Array
    }

    void Update()
    {
        // if (FollowsMousePos && Input.GetMouseButton(0)) faceTowardsMouse();
        // if (Q_and_E_to_rotate) Rotation();
        // float y_movement = 0;
        // if (Input.GetKey("w"))
        // {
        //     y_movement = 1;
        // }
        // else
        // {
        //     if (Input.GetKey("s"))
        //     {
        //         y_movement = -1;
        //     }
        // }
        //
        // float x_movement = 0;
        // if (Input.GetKey("d"))
        // {
        //     x_movement = 1;
        //
        // }
        // else
        // {
        //     if (Input.GetKey("a"))
        //     {
        //         x_movement = -1;
        //     }
        // }

        if (Input.GetMouseButtonUp(1)) {
            Vector2 target_pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int size = 0;
            foreach (ControlledCharacter controlled_character in controlled_characters) {
                if (controlled_character == null) {
                    continue;
                }
                ++size;
                controlled_character.player_character.move_command(target_pos + controlled_character.offset_from_center);
            }
            GameObject.FindGameObjectWithTag("Flag").transform.position = target_pos;
        }
    }
        void faceTowardsMouse()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector2 dir = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.up = dir;
        }

        // void Rotation()
        // {
        //     if (Input.GetKey("q"))
        //     {
        //         foreach (var item in children)
        //         {
        //             item.transform.RotateAround(centerPoint, new Vector3(0f, 0f, 1f), rotation_speed * Time.deltaTime);
        //         }
        //         //transform.GetChild(0).gameObject.transform.RotateAround(centerPoint, new Vector3(0f, 0f, 1f), rotation_speed * Time.deltaTime);          //(new Vector3(0, 0, rotation_speed) * Time.deltaTime);
        //     }
        //
        //     if (Input.GetKey("e"))
        //     {
        //         foreach (var item in children)
        //         {
        //             item.transform.RotateAround(centerPoint, new Vector3(0f, 0f, 1f), -rotation_speed * Time.deltaTime);
        //         }
        //         //transform.GetChild(0).gameObject.transform.RotateAround(centerPoint, new Vector3(0f, 0f, 1f), -rotation_speed * Time.deltaTime);
        //     }
        // }
}
