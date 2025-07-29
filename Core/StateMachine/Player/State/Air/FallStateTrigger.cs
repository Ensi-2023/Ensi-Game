using System.Collections;
using UnityEngine;

public class FallStateTrigger : MonoBehaviour
{
    /*
    private Coroutine checkFallCoroutine;
    private PlayerStateMashine playerStateMachine;
    
    private IState groundChecker;
    public void Initialize(PlayerStateMashine stateMachine, IState groundChecker)
    {
        this.playerStateMachine = stateMachine;
        this.groundChecker = groundChecker;
    }
    
    public void CheckAndStartFallRoutine()
    {
        if (playerStateMachine == null || groundChecker == null)
            return;

        if (groundChecker.IsGroundedCheck())
        {
            // Если на земле — останавливаем корутину, если она запущена
            if (checkFallCoroutine != null)
            {
                StopCoroutine(checkFallCoroutine);
                checkFallCoroutine = null;
            }
        }
        else
        {
            // Не на земле — начинаем проверку падения, если ещё не начата
            if (checkFallCoroutine == null)
            {
                checkFallCoroutine = StartCoroutine(CheckFallDelay());
            }
        }
    }

    private IEnumerator CheckFallDelay()
    {
        // Ждём заданное время (например, 0.25 секунды)
        yield return new WaitForSeconds(0.25f);

        // Проверяем снова, всё ли ещё не на земле
        if (!groundChecker.IsGroundedCheck())
        {
            playerStateMachine.Change(playerStateMachine.FallState);
        }

        checkFallCoroutine = null;
    }*/
    
}
