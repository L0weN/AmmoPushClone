using UnityEngine;

/// <summary>
/// This class is used to store settings for the game.
/// </summary>
public static class Settings
{
    public const string DEFAULT_GAME_NAME = "AmmoPushClone";

    public const int DEFAULT_POWERUP_COST = 50;

    public const float DEFAULT_UI_ANIMATION_DURATION = 0.3f;

    public const float DEFAULT_WON_COIN_MULTIPLIER = 100f;
    public const float DEFAULT_LOST_COIN_MULTIPLIER = 50f;

    public const string PLAYER_TAG = "Player";
    public const string BOSS_TAG = "Boss";
    public const string AMMO_TAG = "Ammo";

    #region Animation
    public const string IS_IDLE = "isIdle";
    public const string IS_MOVING = "isMoving";
    public const string IS_ATTACKING = "isAttacking";
    public const string IS_DIED = "isDied";
    public const string IS_HIT = "isHit";

    public static int idleAnimatorHash = Animator.StringToHash(IS_IDLE);
    public static int moveAnimatorHash = Animator.StringToHash(IS_MOVING);
    public static int attackAnimatorHash = Animator.StringToHash(IS_ATTACKING);
    public static int dieAnimatorHash = Animator.StringToHash(IS_DIED);
    public static int hitAnimatorHash = Animator.StringToHash(IS_HIT);
    #endregion
}
