// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Diagnostics;
// using UnityEngine;
//
// public class EnemyBehaviour : MonoBehaviour
// {
//     #region Miembros Privados
//     
//     private Animator _animator;
//     private Vector3 _scale;
//     private bool _walking;
//     private bool _lookingRight;
//     
//     #endregion
//     
//     void Start()
//     {
//         _animator = GetComponent<Animator>();
//     }
//
//     void Update()
//     {
//         _walking = true;
//         if (transform.position.x <= _collider.transform.position.x)
//         {
//             _lookingRight = true;
//         }
//         else
//             _lookingRight = false;
//
//         _walking = false;
//         if (transform.position.x <= _collider.transform.position.x)
//         {
//             _lookingRight = true;
//         }
//         else
//             _lookingRight = false;
//         UpdateAnimations();
//     }
//      
//     //SpawnController.Instance.StoreEnemy(this); para guardarlo en el pool
//
//     private void UpdateAnimations()
//     {
//         print(_walking);
//         _animator.SetBool("Walking", _walking);
//         if (!_lookingRight) _scale.x = -3;
//         else _scale.x = 3;
//         transform.localScale = _scale;
//     }
// }
