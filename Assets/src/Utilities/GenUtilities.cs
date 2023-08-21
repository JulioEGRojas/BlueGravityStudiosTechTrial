using System;
using System.Collections.Generic;

public static class GenUtilities {

    public static T GetMin<T>(IEnumerable<T> collection, Func<T,float> f) {
        float minValue = float.MaxValue;
        T minimum = default;
        foreach (T t in collection) {
            if (f(t) < minValue) {
                minValue = f(t);
                minimum = t;
            }
        }
        return minimum;
    }
}