namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Logic)]
    public class CompareObjectLayer : FsmStateAction
    {
        [RequiredField] public FsmGameObject gameObject;

        [RequiredField] [UIHint(UIHint.LayerMask)]
        public FsmInt privateLayerMask;

        private int targetMask;
        public FsmEvent equalEvent;
        public FsmEvent notEqualEvent;


        public override void OnEnter()
        {
            if (gameObject.Value != null)
            {
                int objectLayer = gameObject.Value.layer;

                targetMask = privateLayerMask.Value;
                if ((targetMask & (1 << objectLayer)) != 0)
                {
                    Fsm.Event(equalEvent);
                }
                else
                {
                    Fsm.Event(notEqualEvent);
                }
            }
            else
            {
                Fsm.Event(notEqualEvent);
            }

            Finish();
        }
    }
}
