using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTips : MonoBehaviour
{
    public Sprite[] emotionSp;//各生长阶段心情的贴图
    SpriteRenderer this_sp;//现有的贴图
    [SerializeField] Plant plant;
    // Start is called before the first frame update
    void Start()
    {
        this_sp = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        this_sp.sprite = emotionSp[plant.Stage];
    }
}
