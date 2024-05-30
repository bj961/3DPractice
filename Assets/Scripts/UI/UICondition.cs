using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
        //health = transform.Find("Health").GetComponent<Condition>();
        //hunger = transform.Find("Hunger").GetComponent<Condition>();
        //stamina = transform.Find("Stamina").GetComponent<Condition>();
    }
}
