﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventHook.EventMap
{
    public enum EventName
    {
        UNKNOWN = 0x00000000,

        EVENT_MIN = 0x00000001,
        EVENT_SYSTEM_SOUND = 0x00000001,
        EVENT_SYSTEM_ALERT = 0x00000002,
        EVENT_SYSTEM_FOREGROUND = 0x00000003,
        EVENT_SYSTEM_MENUSTART = 0x00000004,
        EVENT_SYSTEM_MENUEND = 0x00000005,
        EVENT_SYSTEM_MENUPOPUPSTART = 0x00000006,
        EVENT_SYSTEM_MENUPOPUPEND = 0x00000007,
        EVENT_SYSTEM_CAPTURESTART = 0x00000008,
        EVENT_SYSTEM_CAPTUREEND = 0x00000009,
        EVENT_SYSTEM_MOVESIZESTART = 0x0000000a,
        EVENT_SYSTEM_MOVESIZEEND = 0x0000000b,
        EVENT_SYSTEM_CONTEXTHELPSTART = 0x0000000c,
        EVENT_SYSTEM_CONTEXTHELPEND = 0x0000000d,
        EVENT_SYSTEM_DRAGDROPSTART = 0x0000000e,
        EVENT_SYSTEM_DRAGDROPEND = 0x0000000f,
        EVENT_SYSTEM_DIALOGSTART = 0x00000010,
        EVENT_SYSTEM_DIALOGEND = 0x00000011,
        EVENT_SYSTEM_SCROLLINGSTART = 0x00000012,
        EVENT_SYSTEM_SCROLLINGEND = 0x00000013,
        EVENT_SYSTEM_SWITCHSTART = 0x00000014,
        EVENT_SYSTEM_SWITCHEND = 0x00000015,
        EVENT_SYSTEM_MINIMIZESTART = 0x00000016,
        EVENT_SYSTEM_MINIMIZEEND = 0x00000017,

        EVENT_OBJECT_CREATE = 0x00008000,
        EVENT_OBJECT_DESTROY = 0x00008001,
        EVENT_OBJECT_SHOW = 0x00008002,
        EVENT_OBJECT_HIDE = 0x00008003,
        EVENT_OBJECT_REORDER = 0x00008004,
        EVENT_OBJECT_FOCUS = 0x00008005,
        EVENT_OBJECT_SELECTION = 0x00008006,
        EVENT_OBJECT_SELECTIONADD = 0x00008007,
        EVENT_OBJECT_SELECTIONREMOVE = 0x00008008,
        EVENT_OBJECT_SELECTIONWITHIN = 0x00008009,
        EVENT_OBJECT_STATECHANGE = 0x0000800a,
        EVENT_OBJECT_LOCATIONCHANGE = 0x0000800b,
        EVENT_OBJECT_NAMECHANGE = 0x0000800c,
        EVENT_OBJECT_DESCRIPTIONCHANGE = 0x0000800d,
        EVENT_OBJECT_VALUECHANGE = 0x0000800e,
        EVENT_OBJECT_PARENTCHANGE = 0x0000800f,
        EVENT_OBJECT_HELPCHANGE = 0x00008010,
        EVENT_OBJECT_DEFACTIONCHANGE = 0x00008011,
        EVENT_OBJECT_ACCELERATORCHANGE = 0x00008012,
        EVENT_OBJECT_INVOKED = 0x00008013,
        EVENT_OBJECT_CLOAKED = 0x00008017,
        EVENT_OBJECT_UNCLOAKED = 0x00008018,
        EVENT_OBJECT_DRAGSTART = 0x00008021,
        EVENT_OBJECT_DRAGCANCEL = 0x00008022,
        EVENT_OBJECT_DRAGCOMPLETE = 0x00008023,
        EVENT_OBJECT_DRAGENTER = 0x00008024,
        EVENT_OBJECT_DRAGLEAVE = 0x00008025,
        EVENT_OBJECT_DRAGDROPPED = 0x00008026,


        // XP or lator
        EVENT_CONSOLE_CARET = 0x00004001,
        CONSOLE_CARET_SELECTION = 0x00000001,
        CONSOLE_CARET_VISIBLE = 0x00000002,
        EVENT_CONSOLE_UPDATE_REGION = 0x00004002,
        EVENT_CONSOLE_UPDATE_SIMPLE = 0x00004003,
        EVENT_CONSOLE_UPDATE_SCROLL = 0x00004004,
        EVENT_CONSOLE_LAYOUT = 0x00004005,
        EVENT_CONSOLE_START_APPLICATION = 0x00004006,
        CONSOLE_APPLICATION_16BIT = 0x00000001,
        EVENT_CONSOLE_END_APPLICATION = 0x00004007,

        EVENT_MAX = 0x7fffffff,

        // Vista or lator
        EVENT_SYSTEM_DESKTOPSWITCH = 0x00000020,
        //EVENT_OBJECT_INVOKED = 0x00008013,
        EVENT_OBJECT_TEXTSELECTIONCHANGED = 0x00008014,
        EVENT_OBJECT_CONTENTSCROLLED = 0x00008015,

    }
}

