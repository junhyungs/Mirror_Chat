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

    //public Action<dynamic> Action { get; private set; } dynamic -> ��Ÿ�ӿ� �������� �Ű����� Ÿ���� �����Ѵ�. �پ��� �Ű����� ����.






}
