using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class herbtext : MonoBehaviour {
    public Text myText;
 
 
    void Update () {
        myText.text = GameObject.FindGameObjectsWithTag("herbivore").Length.ToString();
    }
}
 
