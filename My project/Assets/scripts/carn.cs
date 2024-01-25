using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class carntext : MonoBehaviour {
    public Text myText;
 
 
    void Update () {
        myText.text = GameObject.FindGameObjectsWithTag("carnivore").Length.ToString();
    }
}
 
