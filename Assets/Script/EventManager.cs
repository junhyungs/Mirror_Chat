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

    //public Action<dynamic> Action { get; private set; } dynamic -> 런타임에 동적으로 매개변수 타입을 지정한다. 다양한 매개변수 가능.






}
