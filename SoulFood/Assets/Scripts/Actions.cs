using UnityEngine;
using System.Collections;

public static class Actions{

    public static void ConsumeSoul(Collider[] collisions, Vector3 position)
    {
        //eatingDelay -= Time.deltaTime;
        Soul closestSoul = null;
        float closestDistance = 2f; //adjust size upon implementation
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].tag == "Soul" && Mathf.Abs((position - collisions[i].transform.position).magnitude) <= closestDistance)
            {
                closestSoul = collisions[i].GetComponent<Soul>();
            }
        }
        if (closestSoul != null)
        {
            //eatSoul stuff here
        };
    }

    public static void EjectSoul()
    {

    }

    public static void Lunge()
    {

    }
}
