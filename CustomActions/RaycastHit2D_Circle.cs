using Unity.VisualScripting;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Custom")]
	public class RaycastHit2D_Circle : FsmStateAction
	{

		public FsmOwnerDefault owner;
		public FsmGameObject target;

		public FsmFloat radius;
		[UIHint(UIHint.LayerMask)] public FsmInt layermaskfsm;

		public FsmEvent detectEvent;

		private GameObject gameObject;
		private float tempRadius;
		LayerMask layerMask;

		public override void OnEnter()
		{
			tempRadius = radius.Value;
			gameObject = Fsm.GetOwnerDefaultTarget(owner);
			layerMask = layermaskfsm.Value;
		}

		public override void OnUpdate()
		{
			RaycastHit2D hit = Physics2D.CircleCast(
			gameObject.transform.position, tempRadius, Vector2.zero, Mathf.Infinity, layerMask);

			if (!hit) return;

			target = hit.collider.gameObject;

			if (detectEvent.IsUnityNull()) Finish();
			Fsm.Event(detectEvent);
		}


		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(gameObject.transform.position, tempRadius);
		}
	}

}
