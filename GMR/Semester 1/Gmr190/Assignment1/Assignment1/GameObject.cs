using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment1
{
    class GameObject
    {
        string _ObjectName;
        int _MaxDuration;
        int _CurrentDuration;

        public GameObject(string objectName, int maxDuration, int currentDuration)
        {
            _ObjectName = objectName;
            _MaxDuration = maxDuration;
            SetCurrentDuration(currentDuration);
        }

        public string GetObjectName()
        {
            return _ObjectName;
        }

        public int GetMaxDuration()
        {
            return _MaxDuration;
        }

        public int GetCurrentDuration()
        {
            return _CurrentDuration;
        }

        public void SetCurrentDuration(int curDuration)
        {
            _CurrentDuration = curDuration;
        }

        public void Repair()
        {
            SetCurrentDuration(_MaxDuration);
        }

        public void TakeDamage(int hitPoints)
        {
            SetCurrentDuration(GetCurrentDuration() - hitPoints);
        }
    }
}
