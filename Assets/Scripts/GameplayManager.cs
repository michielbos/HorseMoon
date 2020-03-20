using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon
{
	public class GameplayManager : SingletonMonoBehaviour<GameplayManager>
	{
		public bool AllowGameplay {
			get { return allowGameplay; }
			set {
				TimeController.Instance.runWorldTime = value;
				Player.Instance.LockControls = !value;
				if (value)
					CharacterControl.UndoLock();
				else
					CharacterControl.LockAll();
				
				allowGameplay = value;
			}
		}
		private bool allowGameplay;
	}
}
