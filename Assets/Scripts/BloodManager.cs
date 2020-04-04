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

        for (int i = 0; i < infos.Length; i++)
        {
            if (data.upgrades.Contains(infos[i].name)){
                SpriteRenderer sr = transform.GetChild(i).GetComponent<SpriteRenderer>();
                sr.size = new Vector2(sr.size.x, Mathf.Lerp(sr.size.y, infos[i].bloodLevelHeight, bloodSpeed * Time.deltaTime));

                if (instant)
                {
                    sr.size = new Vector2(sr.size.x, infos[i].bloodLevelHeight);
                }
            }
        }
    }
}
