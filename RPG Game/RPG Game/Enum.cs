using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game
{
    enum MovementsForPlayer
    {
        IdleUp,
        IdleDown,
        IdleRight,
        IdleLeft,
        None,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        SwingUp,
        SwingDown,
        SwingLeft,
        SwingRight
    };
    enum RepeatRotation
    {
        DoNothing,
        LoopRotation,
        Stop
    };

    public enum ScreenStates
    {
        StartingScreen,
        Portal1Screen
    };

    enum OriginType
    {
        Zero,
        Center
    };
    enum EnemyMovements
    { 
        MoveRight,
        MoveLeft,
        MoveUp,
        MoveDown,
        SwingUp,
        SwingDown,
        SwingLeft,
        SwingRight,
        IdleRight,
        IdleLeft,
        IdleUp,
        IdleDown,
        None
    };
    
    enum StatesForFindingAttackMovement
    {
        SwitchToAttack,
        StartTimers,
        SwitchToIdle
    };
    enum HumanMovementToPlacesStates
    {
        FollowingSquarePath,
        FollowingPlayersMovements,
        AttackingPlayer,
        FigureWhichStateToGoTo
    }
    enum HumanAttackingPlayerStates
    {
        FindPredictiveDistance,
        IsTargetReached,
        SetIdle,
        SetNewMovement
    }
}
