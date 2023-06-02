using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool ListContains<T>(List<Useable> utils) where T : Useable
    {
        foreach (Useable util in utils)
        {
            if (util is T)
            {
                return true;
            }
        }
        return false;
    }
}
