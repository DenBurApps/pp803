using System;
using UnityEngine;

namespace Pole
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        private const string JumpConditionName = "IsJumping";
        private const string FlyConditionName = "IsFlying";

        private readonly int _isJumping = Animator.StringToHash(JumpConditionName);
        private readonly int _isFlying = Animator.StringToHash(FlyConditionName);
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetJumping()
        {
            _animator.SetBool(_isJumping, true);
        }

        public void SetFlying()
        {
            _animator.SetBool(_isJumping, false);
            _animator.SetBool(_isFlying, true);
        }

        public void SetRunning()
        {
            _animator.SetBool(_isJumping, false);
            _animator.SetBool(_isFlying, false);
        }
    }
}
