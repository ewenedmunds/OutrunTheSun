using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    [System.Serializable]
    public class UpgradeInfo
    {
        public string name;
        public float bloodLevelHeight;
    }

    public UpgradeInfo[] infos;
    public float bloodSpeed;

    // Start is called before the first frame update
    void Start()
    {
        DrawBloods(true);
    }

    // Update is called once per frame
    void Update()
    {
        DrawBloods();
    }

    public void DrawBloods(bool instant=false)
    {
        DataStore data = GameObject.FindGameObjectWithTag("DataStore").GetComponent<DataStore>();

        //Go through each upgrade
        for (int i = 0; i < infos.Length; i++)
        {
            //If the upgrade is unlocked, rise the blood level to its correct height for that upgrade
            if (data.upgrades.Contains(infos[i].name)){
                SpriteRenderer sr = transform.GetChild(i).GetComponent<SpriteRenderer>();
                sr.size = new Vector2(sr.size.x, Mathf.Lerp(sr.size.y, infos[i].bloodLevelHeight, bloodSpeed * Time.deltaTime));

                //Or if this when the scene is first loaded, instantly rise the blood
                if (instant)
                {
                    sr.size = new Vector2(sr.size.x, infos[i].bloodLevelHeight);
                }
            }
        }
    }
}
