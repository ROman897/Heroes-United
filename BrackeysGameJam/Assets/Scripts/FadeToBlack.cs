using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    private Image black_rect_image;
    private Text victory_text;
    private Text defeat_text;

    private Text fading_text;

    public void fade(float fade_time, bool victory) {
        black_rect_image = transform.Find("FadeRectangle").GetComponent<Image>();

        if (victory) {
            fading_text = transform.Find("VictoryText").GetComponent<Text>();
        } else {
            fading_text = transform.Find("DefeatText").GetComponent<Text>();
        }

        StartCoroutine(enum_fade(fade_time));
    }

    private IEnumerator enum_fade(float fade_time) {
        while (black_rect_image.color.a < 1) {
            Color old_color = black_rect_image.color;
            float alpha = old_color.a + Time.deltaTime / fade_time;
            black_rect_image.color = new Color(old_color.r, old_color.g, old_color.b, alpha); 
            fading_text.color = new Color(fading_text.color.r, fading_text.color.g, fading_text.color.b, alpha);
            yield return null;
        }
    }
}
