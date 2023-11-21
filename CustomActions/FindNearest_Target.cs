using UnityEngine;
using System.Collections.Generic;


namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Physics2D)]
    public class FindNearest_Target : FsmStateAction
    {
        [RequiredField] public FsmOwnerDefault ownerDefault;

        [RequiredField] [UIHint(UIHint.Variable)]
        public FsmArray enemyInRange;

        [RequiredField] [ArrayEditor(VariableType.Float)]
        public FsmFloat radius;

        [UIHint(UIHint.Layer)] [Tooltip("Pick only from these layers.")]
        public FsmInt[] layerMask;

        [Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
        public FsmBool InvertMask;

        [RequiredField] [ArrayEditor(VariableType.GameObject)]
        public FsmGameObject NearestTarget;


        private Collider2D[] cols;
        private GameObject gameObject;
        private GameObject tempGO;

        /// Update Later: will check has component
        // [RequiredField] [UIHint(UIHint.ScriptComponent)] [Tooltip("The name of the component to check for.")]
        // public FsmString component;
        // private Component aComponent;

        public override void OnEnter()
        {
            gameObject = Fsm.GetOwnerDefaultTarget(ownerDefault);
        }


        public override void OnUpdate()
        {
            cols = Physics2D.OverlapCircleAll(gameObject.transform.position, radius.Value,
                ActionHelpers.LayerArrayToLayerMask(layerMask, InvertMask.Value));
            if (cols.Length < 1)
            {
                NearestTarget.Value = null;
                if (enemyInRange.Values.Length > 0)
                {
                    enemyInRange.Values = new object[0];
                }

                return;
            }

            List<object> gameObjects = new();

            tempGO = cols[0].gameObject;
            float dis = Vector2.Distance(
                tempGO.transform.position, gameObject.transform.position);

            for (int i = 0; i < cols.Length; i++)
            {
                gameObjects.Add(cols[i].gameObject);

                if (Vector2.Distance(
                        cols[i].gameObject.transform.position,
                        gameObject.transform.position) >= dis) continue;

                tempGO = cols[i].gameObject;
            }

            enemyInRange.Values = gameObjects.ToArray();
            NearestTarget.Value = tempGO;

            Finish();
        }
    }
}
