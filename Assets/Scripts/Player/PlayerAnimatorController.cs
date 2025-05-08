using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        [SerializeField] private GameObject foxFront;
        [SerializeField] private GameObject foxSide;
        [SerializeField] private GameObject foxBack;
        
        private Animator _animFront, _animSide, _animBack;
        private GameObject _activeModel;
        private Animator _activeAnim;
        
        private SpriteRenderer[] _allRenderers;
        
        private Dictionary<ModelType, (GameObject model,Animator animator)> _modelMap;


        private void Awake()
        {
            _animFront = foxFront.GetComponent<Animator>();
            _animSide = foxSide.GetComponent<Animator>();
            _animBack = foxBack.GetComponent<Animator>();

            _modelMap = new Dictionary<ModelType, (GameObject, Animator)>()
            {
                {ModelType.Front,(foxFront,_animFront)},
                {ModelType.Back, (foxBack, _animBack)},
                { ModelType.Side , (foxSide,_animSide)}
            };
            
            ActivateModel(ModelType.Front);
            
            _allRenderers = foxFront.GetComponentsInChildren<SpriteRenderer>(true)
                .Concat(foxBack.GetComponentsInChildren<SpriteRenderer>(true))
                .Concat(foxSide.GetComponentsInChildren<SpriteRenderer>(true))
                .ToArray();

        }

        public void SetWalking(bool isWalking)
        {
            _activeAnim.SetBool(IsWalking, isWalking);
        }

        public void SetSideDirectionRight(bool isRight)
        {
            foxSide.transform.localScale = new Vector3(isRight ? 0.4f : -0.4f, 0.4f, 0.4f);
        }
        
        public void SetTransparency(float alpha)
        {
            foreach (var sr in _allRenderers)
            {
                var color = sr.color;
                color.a = alpha;
                sr.color = color;
            }
        }

        public void ActivateModel(ModelType modelType)
        {
            var (model,animator) = _modelMap[modelType];
            ActivateModelHelper(model, animator);
        }
        
        private void ActivateModelHelper(GameObject model, Animator anim)
        {
            
            if (_activeModel == model) return;

            if (_activeModel != null) _activeModel.SetActive(false);
            model.SetActive(true);
            _activeModel = model;
            _activeAnim = anim;
        }
        
        
        
    }

    public enum ModelType
    {
        Front,
        Side,
        Back
    }
}