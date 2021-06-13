using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    float money = 20f;
    float score = 1.5f;

    public void Pay(float payment)
    {
        money += payment;
    }

    public void Rate(float percent_satisfied)
    {
        if (percent_satisfied <= 0.2f)
            score -= 1f;
        else if (percent_satisfied <= 0.4f)
            score -= 0.5f;
        else if (percent_satisfied >= 0.6f)
            score += 0.5f;
        else if (percent_satisfied >= 0.8f)
            score += 1f;
    }
}
