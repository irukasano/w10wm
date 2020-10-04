using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace eventHook.EventMap
{
    class EventMapConverter
    {
        /// <summary>
        /// Convert int eventcode to EventName.
        /// </summary>
        /// <param name="eventcode">int</param>
        /// <returns></returns>
        public static EventName CodeToName(int eventcode)
        {
            if (Enum.IsDefined(typeof(EventName), eventcode))
                return (EventName)eventcode;

            return EventName.UNKNOWN;
        }

        /// <summary>
        /// Convert EventName to int eventcode.
        /// </summary>
        /// <param name="key">EventName</param>
        /// <returns></returns>
        public static int NameToCode(EventName key)
        {
            return (int)key;
        }
    }
}


