using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rating { ONE, TWO, THREE, FOUR, FIVE };

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    float money = 20f;
    float score = 1.5f;

    public void Pay(float payment)
    {
        money += payment;
    }

    public void Rate(Rating rating)
    {
        switch (rating)
        {
            case Rating.ONE:
                score -= 1f;
                break;
            case Rating.TWO:
                score -= 0.5f;
                break;
            case Rating.THREE:
                break;
            case Rating.FOUR:
                score += 0.5f;
                break;
            case Rating.FIVE:
                score += 1f;
                break;
            default:
                break;
        }
    }
}
