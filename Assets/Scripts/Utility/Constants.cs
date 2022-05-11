using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const int PASSWORD_MIN_LENGTH = 4;

    public const int PICKUP_LAYER_INDEX = 20;
    public const int RANGED_SPEAR_LAYER_INDEX = 22;

    public const float VOLUME_RECALCULATION_COEF = 100.0f;
    public const float VOLUME_DEFAULT_VALUE = 1;

    public const float RESPAWN_SCREEN_FADE_TIME = 1.5f;
    public const int HIT_COLLIDER_LAYER_INDEX = 24;

    public const string HEALTH_ATTRIBUTE_NAME = "Health";
    public const float MAX_HEALTH_MULTIPLIER = 1.5f;
    public const float CRIT_DAMAGE_CHANCE = 15;
    public const int NON_CRITICAL_ATTACKS_LIMIT = 4;
    public const float CRITICAL_DAMAGE_MULTIPLIER = 1.5f;
    public const float DODGE_CHANCE = 15;
    public const int NOT_DODGED_ATTACKS_LIMIT = 4;
    public const int CHARACTER_SPEED_VALUE = 2;
}
