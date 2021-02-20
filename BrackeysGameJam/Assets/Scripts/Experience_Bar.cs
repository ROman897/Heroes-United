using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//[ExecuteInEditMode()]
public class Experience_Bar : MonoBehaviour
{
    public static Experience_Bar instace;

    public int max_experience;
    public int current_experience;
    public int min_experience;
    private Image fillamount_image;
    private TextMeshProUGUI current_xp_text;
    private TextMeshProUGUI next_level_xp_text;

    public float ToLVL1;
    public GameObject ExperiencePX;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        instace = this;
        fillamount_image = transform.GetChild(0).gameObject.GetComponent<Image>();
        current_xp_text = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        next_level_xp_text = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float currentoffset = (float)current_experience - (float)min_experience;
        float maxOffset = (float)max_experience - (float)min_experience;
        float fillAmount = currentoffset / maxOffset;
        fillamount_image.fillAmount = fillAmount;
        current_xp_text.SetText("XP : " + current_experience);
        next_level_xp_text.SetText("NEXT LEVEL IN " + max_experience);
        CheckLevel();


    }

    public void SetExperience(float _amount)
    {
        current_experience = (int)_amount;
    }

    public void AddExperience(float _amount)
    {
        current_experience += (int)_amount;
    }

    public void CheckLevel()
    {
        if (current_experience >= max_experience)
        {
            max_experience = 300;
            min_experience = current_experience;
            GameObject player_party = GameObject.Find("PlayerParty");
            //GameObject go = Instantiate(ExperiencePX, player_party.transform.position, Quaternion.identity);
            
            //Debug.Log(transform.childCount);
            float i = 0;
            foreach (Transform child in player_party.transform)
            {
                i += 1;
            Instantiate(ExperiencePX, child.transform.position, Quaternion.identity);
            }
            //Debug.Log(transform.childCount);
            
            //make some particle
        }
    }
}
