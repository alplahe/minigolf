using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

// How to search where a broadcast is invoked:
// 1. Ctrl + Shift + F: search in all the project
// 2. Copy-paste this line

// Messenger\.Broadcast(<[^<>]+>)?\(

// 3. Continue that line with the Broadcast you want to search
// Example: Messenger\.Broadcast(<[^<>]+>)?\(BroadcastName.Room.OnRoomFinished

// 4. CHECK THE "Use regular expressions" ("Usar expresiones regulares") CHECKBOX!!!

// This Regular expression allows you to find usages of the broadcast with different signature
// In other words, it find the broadcast usages no matter the <,,> part, it will return the ones 
// doesnt ask for type, the ones that use 1 type, 2 types, ...

// Note: the dots used in "BroadcastName.Room.OnRoomFinished" should be written as special characters:
// "BroadcastName\.Room\.OnRoomFinished" for a correct search, but it also works the other way.

namespace Minigolf
{
  [System.Serializable]
  public class BroadcastNameType
  {
    public string name;

    public string prefix;
    public string suffix;

    public override string ToString()
    {
      return name;
    }
  }


  public class BroadcastName
  {
    public class Screen
    {
      public static string prefix = "Screen_";

      public static string OnBeingTouched = prefix + "OnBeingTouched";
      public static string OnTouchedDown = prefix + "OnTouchedDown";
      public static string OnTouchedReleased = prefix + "OnTouchedReleased";
    }

    public class Ball
    {
      public static string prefix = "Ball_";

      public static string OnBallInHole = prefix + "OnBallInHole";
    }
  }
}