using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool ListContains<T>(List<IUseable> utils) where T : IUseable
    {
        foreach (IUseable util in utils)
        {
            if (util is SpaceshipECM)
            {
                return true;
            }
        }
        return false;
    }
}
