using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EventManager
{
    private static EventManager instance;

    public static EventManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new EventManager();
            }

            return instance;
        }
    }


    



}
