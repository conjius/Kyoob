//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class PlayerScript : MonoBehaviour {
//    [Range(1, 30)] public int AnimationLength; // animation length in frames
//    [Range(0.0f, 1.0f)] public float SquashAmount;
//    [Range(0.0f, 5000.0f)] public float FullJumpForce;
//    [Range(9.81f, 30.0f)] public float NormalGravity;
//    [Range(9.81f, 50.0f)] public float FallGravity;
//
//
//    private enum State {
//        Idle,
//        Loaded,
//        MidAir
//    }
//
//    private bool _animating;
//    private Rigidbody _rb;
//    private State _state;
//    private int _frameNum;
//
//    // Use this for initialization
//    private void Start() {
//        _rb = GetComponent<Rigidbody>();
//        _state = State.Idle;
//        _frameNum = 1;
//        _animating = false;
//    }
//
//    private void ZeroScale() {
//        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
//    }
//
//
//    private bool GetKeyboardInput() {
//        if (Input.GetKey(KeyCode.Space)) {
//            _state = State.Loaded;
//            return true;
//        }
//
//        if (Input.GetKeyUp(KeyCode.Space)) {
//            _animating = true;
//            _state = State.MidAir;
//            _frameNum = 1;
//            return true;
//        }
//
//        _state = State.Idle;
//        return false;
//    }
//
//    private void GetUserTouch() {
//        if (Input.touchCount > 0) {
//            var touch = Input.GetTouch(0);
//            switch (touch.phase) {
//                case TouchPhase.Moved:
//                case TouchPhase.Stationary:
//                case TouchPhase.Began:
//                    _state = State.Loaded;
//                    _frameNum = 1;
//                    ZeroScale();
//                    break;
//                case TouchPhase.Canceled:
//                case TouchPhase.Ended:
//                    _state = State.MidAir;
//                    ZeroScale();
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//        }
//
//        _state = State.Idle;
//    }
//
//    private void GetInput() {
//        if (!GetKeyboardInput()) {
//            GetUserTouch();
//        }
//    }
//    
//    private void GravityTweak() {
//        Physics.gravity = _rb.velocity.y < -0.1
//            ? new Vector3(0.0f, -FallGravity, 0.0f)
//            : new Vector3(0.0f, -NormalGravity, 0.0f);
//    }
//
//    private void AnimateAndApplyForce() {
//        GravityTweak();
//        switch (_state) {
//            case State.Idle:
//                break;
//            case State.Loaded:
//                if (!_animating) break;
//                if (_frameNum % AnimationLength == 0) {
//                    _animating = false;
//                    break;
//                }
//                transform.localScale = new Vector3(
//                    1.0f + SquashAmount * ((float) _frameNum / AnimationLength),
//                    1.0f - SquashAmount * ((float) _frameNum / AnimationLength),
//                    transform.localScale.z);
//                _frameNum++;
//                break;
//
//            case State.MidAir:
//                if (!_animating) break;
//                if (_frameNum == 1)
//                    _rb.AddForce(Vector3.up * FullJumpForce);
//                if (_frameNum % AnimationLength == 0) {
//                    _animating = false;
//                    break;
//                }
//                transform.localScale = new Vector3(
//                    1.0f - SquashAmount * ((float) _frameNum / AnimationLength),
//                    1.0f + SquashAmount * ((float) _frameNum / AnimationLength),
//                    transform.localScale.z);
//                _frameNum++;
//                break;
//            default:
//                throw new ArgumentOutOfRangeException();
//        }
//    }
//
//    // Update is called once per frame
//    private void Update() {
//        GetInput();
//        AnimateAndApplyForce();
//    }
//
//    private void OnCollisionEnter(Collision other) {
//        GravityTweak();
//    }
//}