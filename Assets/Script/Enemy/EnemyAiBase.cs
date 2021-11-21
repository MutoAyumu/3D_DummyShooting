using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiBase : MonoBehaviour
{
    [SerializeField, Tooltip("行動パターン")] private EnemyAiState aiState;

    public EnemyAiState AiState { get => aiState; set => aiState = value; }

    public　enum EnemyAiState
    {
        Wait,       //一旦停止
        Move,       //行動
        Attack,     //攻撃
        Idle,       //停止
    }
    /// <summary>基本処理</summary>
    public virtual void MainRoutine()
    {

    }
    /// <summary></summary>
    public virtual void UpdateAI()
    {
        switch (AiState)
        {
            case EnemyAiState.Wait:
                Wait();
                break;
            case EnemyAiState.Move:
                Move();
                break;
            case EnemyAiState.Attack:
                Attack();
                break;
            case EnemyAiState.Idle:
                Idle();
                break;
        }
    }

    /// <summary>一時停止の処理</summary>
    public virtual void Wait()
    {

    }
    /// <summary>動作の処理</summary>
    public virtual void Move()
    {

    }
    /// <summary>攻撃の処理</summary>
    public virtual void Attack()
    {

    }
    /// <summary>待機処理</summary>
    public virtual void Idle()
    {

    }
}