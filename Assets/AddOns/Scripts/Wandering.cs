using System;
using UnityEngine;

public enum State
{
    Wandering,
    Attention,
    Pursuit,
    Stay
}

public class Wandering : MonoBehaviour
{
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] public Transform _player;
    
    //два типа блуждания - по кругу или возвращаясь обратно по пройденному пути
    public enum WonderingType
    {
        Cycle,
        MoveBack
    }

    public float WanderingSpeed = 2;
    public float PursuitSpeed = 5;
    public float RotateSpeed = .3f;
    public bool IsWondering = true;

    public WonderingType CurrentWonderingType;
    public State CurrentState = State.Wandering;

    public float TimeInPoint = 0;

    private const float Eps = 0.2f;

    private Transform _currentWayPoint;
    private int _currentPointIndex;
    private Vector3 _targetDirection;

    //для типа MoveBack - дыигаемся вперед или назад по заданному пути
    private bool _forward = true;
    private bool _back;

    private bool _checkPoint;
    private float _currentPointTime;

    private void Awake() =>
        _currentWayPoint = _wayPoints[_currentPointIndex];

    public void SetNewState(State state)
    {
        CurrentState = state;
    }
    
    private void FixedUpdate()
    {
        switch (CurrentState)
        {
            case State.Wandering:
                if (!IsWondering)
                    return;

                _currentWayPoint = _wayPoints[_currentPointIndex];

                //постоять заданное время в точке, до которой добрались
                if (CheckPoint()) return;
                //повернуться в сторону цели
                LookAtTheTarget();
                //двигаться
                Move();

                if (_targetDirection.magnitude < Eps)
                    Wander();
                
                break;
            case State.Attention:
                break;
            case State.Pursuit:
                break;
            case State.Stay:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool CheckPoint()
    {
        if (!_checkPoint)
            return false;

        if (_currentPointTime > 0)
        {
            _currentPointTime -= Time.deltaTime;
            return true;
        }

        _checkPoint = false;
        return false;
    }

    private void LookAtTheTarget()
    {
        _targetDirection = _currentWayPoint.position - transform.position;
        _targetDirection.y = 0;
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_targetDirection),
                Time.time * RotateSpeed);
    }

    private void Move()
    {
        var speed = CurrentState == State.Wandering ? WanderingSpeed : PursuitSpeed;
        transform.position += transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;
    }

    private void Wander()
    {
        switch (CurrentWonderingType)
        {
            case WonderingType.Cycle:
                MakeCycle();
                break;
            case WonderingType.MoveBack:
                MakeMoveBack();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _currentPointTime = TimeInPoint;
        _checkPoint = true;
    }

    private void MakeMoveBack()
    {
        if (_forward)
        {
            if (_currentPointIndex < _wayPoints.Length - 1)
                _currentPointIndex++;
            else
            {
                _forward = false;
                _back = true;
                _currentPointIndex--;
            }
        }
        else if (_back)
        {
            if (_currentPointIndex > 0)
                _currentPointIndex--;
            else
            {
                _forward = true;
                _back = false;
                _currentPointIndex++;
            }
        }
    }

    private void MakeCycle()
    {
        if (_currentPointIndex < _wayPoints.Length - 1)
            _currentPointIndex++;
        else
            _currentPointIndex = 0;
    }
}