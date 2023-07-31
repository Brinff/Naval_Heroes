using System.Collections;
using UnityEngine;

namespace Game
{
    public enum SpeedUnit { KNOTS_PER_H, KM_PER_H, M_PER_H, M_PER_S }
    public static class SpeedCovertor
    {
        public static float Convert(float value, SpeedUnit from, SpeedUnit to)
        {
            float ms = ConvertMetersFrom(value, from);
            return ConvertMetersTo(ms, to);
        }

        private static float ConvertMetersFrom(float value, SpeedUnit from)
        {
            switch (from)
            {
                case SpeedUnit.KNOTS_PER_H: return value * 0.514444f;
                case SpeedUnit.KM_PER_H: return value * 0.277778f;
                case SpeedUnit.M_PER_H: return value * 0.000277778f;
                case SpeedUnit.M_PER_S: return value;
            }
            return value;
        }

        private static float ConvertMetersTo(float value, SpeedUnit to)
        {
            switch (to)
            {
                case SpeedUnit.KNOTS_PER_H: return value * 1.94384f;
                case SpeedUnit.KM_PER_H: return value * 3.6f;
                case SpeedUnit.M_PER_H: return value * 3600f;
                case SpeedUnit.M_PER_S: return value;
            }
            return value;
        }
    }
}