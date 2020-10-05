using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputExtender
{

    public static class Key_Extender
    {
        private static float timer;

        private static float lastTimer;
        private static float currentTimer;
        private static int clicks = 0;



        public static bool IsLongKeyPress(KeyCode _key)
        {
            if (Input.GetKey(_key))
            {
                timer += Time.deltaTime;

                if (timer >= 0.5f)
                {
                    clicks = 0;
                    timer = 0;
                    return true;
                }
            }

            if (Input.GetKeyUp(_key))
            {
                timer = 0;
            }


            return false;
        }

        public static bool IsDoublekeyPress(KeyCode _key)
        {
            if (Input.GetKeyDown(_key))
            {
                clicks += 1;

                if (clicks == 1)
                {
                    lastTimer = Time.unscaledTime;
                }

                if (clicks >= 2)
                {
                    currentTimer = Time.unscaledTime;

                    float differance = currentTimer - lastTimer;

                    if (differance <= 0.2f)
                    {
                        clicks = 0;
                        return true;
                    }
                    else
                    {
                        clicks = 0;
                    }
                }

            }

            return false;
        }
    }

}
