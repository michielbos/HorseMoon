using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon
{
	public enum Location { Farm, House, Forest }

	public class LocationController : SingletonMonoBehaviour<LocationController>
	{
		public AudioClip farmMusic;
		public AudioClip forestMusic;
		public AudioClip nightMusic;

		public float dayMusicStartHour = 6f;
		public float nightMusicStartHour = 19f;

		public AudioClip CurrentMusic {
			get {
				if (location == Location.Forest)
					return forestMusic;
				else if (TimeController.Instance.WorldTimeHours < 6f || TimeController.Instance.WorldTimeHours >= nightMusicStartHour)
					return nightMusic;
				return farmMusic;
			}
		}

		public Location Location {
			get => location;
			set {
				location = value;
				PlayCurrentMusic();
				locationChanged?.Invoke(value);
			}
		}
		private Location location;

		public delegate void LocationEvent(Location newLocation);
		public LocationEvent locationChanged;

		private MusicPlayer musicPlayer;

		private void Start()
		{
			musicPlayer = FindObjectOfType<MusicPlayer>();
			PlayCurrentMusic();
		}

		public void PlayCurrentMusic() {
			musicPlayer.PlaySong(CurrentMusic);
		}
	}
}
