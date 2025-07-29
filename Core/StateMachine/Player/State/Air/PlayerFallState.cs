using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    PlayerFallData fallData;
    private Vector3 playerPositionOnEnter;
    
    float speedCurrent;
    float falldistance;
    public PlayerFallState(PlayerStateMashine statesMashine) : base(statesMashine)
    {
        fallData = PlayerStatesMashine.Player.Data.AirData.FallData;
    }

    public override void EnterState()
    {
        
        base.EnterState();
        
        PlayerStatesMashine.Player.HandStateMashine.MoveState(WeaponStateEnum.MoveState.Falling);
        
        speedCurrent = PlayerStatesMashine.ReusableData.currentMoveSpeed;
        playerPositionOnEnter = PlayerStatesMashine.Player.transform.position;
        PlayerStatesMashine.ReusableData.Falling = true;
        StartAnimation(animationData.FallingParamentHash);
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Update()
    {
        if (PlayerStatesMashine.ReusableData.Grounded)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.LandingState);
            return;
        }

        base.Update();
    }

    public override void ExitState()
    {
        base.ExitState();
        PlayerStatesMashine.ReusableData.Falling = false;
        PlayerStatesMashine.ReusableData.LastFallDistance = falldistance;
        StopAnimation(animationData.FallingParamentHash);
    }

    internal override void PlayerMove()
    {
      
    }


    protected override void OnMove()
    {
        
    }

    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
        
        if (movementData.LayerData.IsGroundLayer(collider.gameObject.layer))
        {
            falldistance = playerPositionOnEnter.y - PlayerStatesMashine.Player.transform.position.y;
        }
        
    }

    /*
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (movementData.LayerData.IsGroundLayer(collision.gameObject.layer))
        {
            float fallDistance = playerPositionOnEnter.y - PlayerStatesMashine.Player.transform.position.y;
            
            if(PlayerStatesMashine.ReusableData.MoveDirection==Vector2.zero)
                PlayerStatesMashine.Change(PlayerStatesMashine.IdleState); // idle
            OnMove();
        }
        
    }*/
}
