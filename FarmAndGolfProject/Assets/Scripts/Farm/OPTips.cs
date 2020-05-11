using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OPTips : MonoBehaviour
{
    public Sprite[] images;
    public GameObject left;
    public GameObject right;

    [SerializeField] int page =0;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        image.sprite = images[page];
        left.SetActive(false);
        if (images.Length == 1)
        { right.SetActive(false); left.SetActive(false); }
    }

    public void Left()
    {
        if (page > 0)
            page--;
        if(page == 0)
            left.SetActive(false);
        if (images.Length != 1)
        { right.SetActive(true); }
        image.sprite = images[page];
    }
    public void Right()
    {
        if (page < images.Length-1)
            page++;
        if (page == images.Length - 1)
            right.SetActive(false);
        if (images.Length != 1)
        { left.SetActive(true); }
        image.sprite = images[page];
    }
}
