using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindUI : MonoBehaviour
{
    public BallMove _ballMove;
    
    public Image windImage;

    public Text windText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        windImage.transform.rotation = Quaternion.Euler(0 , 0 , -90);
        windText.text = "风向(" + _ballMove.windDirection.x + ",  " + _ballMove.windDirection.y + ",  " + _ballMove.windDirection.z + ")";
    }
}
