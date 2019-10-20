using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CVariableExclusivity : MonoBehaviour
{
    public string CVariable;
    public int check = 1;
    public bool setActive = true;
    // Update is called once per frame
    void Update()
    {
        if(SaveData.currentSave.getCVariable(CVariable) == check) {
            this.gameObject.SetActive(setActive);
        } else {
            this.gameObject.SetActive(!setActive);
        }
    }
}
