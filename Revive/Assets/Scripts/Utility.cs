using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    public static float Map(float value, float low1, float high1, float low2, float high2) {
        float mappedValue = low2 + (high2 - low2) * (value - low1) / (high1 - low1);
        if (float.IsNaN(mappedValue))
            return 0;
        else
            return mappedValue;
    }
}
