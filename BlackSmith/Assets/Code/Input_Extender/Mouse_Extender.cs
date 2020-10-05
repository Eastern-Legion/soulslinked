using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputExtender
{

    public static class Mouse_Extender
    {

        private static float timer;

        private static float lastTimer;
        private static float currentTimer;
        private static int clicks = 0;



        public static bool IsLongClick(int _mouseButton)
        {
            if (Input.GetMouseButton(_mouseButton))
            {
                timer += Time.deltaTime;

                if (timer >= 0.5f)
                {
                    clicks = 0;
                    timer = 0;
                    return true;
                }
            }

            if (Input.GetMouseButtonUp(_mouseButton))
            {
                timer = 0;
            }


            return false;
        }

        public static bool IsDoubleClick(int _mouseButton)
        {
            if (Input.GetMouseButtonDown(_mouseButton))
            {
                clicks += 1;

                if(clicks == 1)
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
