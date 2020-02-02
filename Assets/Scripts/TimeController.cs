using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace HorseMoon {

    public class TimeController : SingletonMonoBehaviour<TimeController>
    {
        private const float MINUTE = 60f;
        private const float HOUR = 3600f;

        public bool runWorldTime = true;

        public float WorldTimeScale;

        public float WorldTimeSeconds {
            get => worldTime;
            set {
                worldTime = Mathf.Repeat(value, HOUR * 24f);
                Debug.Log($"Seconds: {worldTime}, Minutes: {WorldTimeMinutes}, Hours: {WorldTimeHours}");
                UpdateSun();
            }
        }
        private float worldTime;

        public float WorldTimeMinutes {
            get => worldTime / MINUTE;
            set {
                WorldTimeSeconds = value * MINUTE;
            }
        }

        public float WorldTimeHours {
            get => worldTime / HOUR;
            set {
                WorldTimeSeconds = value * HOUR;
            }
        }

        public Light2D sunlight;

        public Color nightColor;
        public Color sunriseColor;
        public Color dayColor;
        public Color sunsetColor;

        private void Start()
        {
            WorldTimeSeconds = HOUR * 6f;
        }

        private void Update()
        {
            // TEMP -->
            if (sunlight == null)
                sunlight = FindObjectOfType<Light2D>();

            if (runWorldTime)
            {
                // Force the next day when the time rolls to 6 AM. -->
                if (WorldTimeSeconds < HOUR * 6f && WorldTimeSeconds + Time.deltaTime * WorldTimeScale >= HOUR * 6f)
                    NextDay();
                else
                    WorldTimeSeconds += Time.deltaTime * WorldTimeScale;
            }
        }

        public void NextDay() {
            CropManager.Instance.OnDayPassed();
            TilemapManager.Instance.OnDayPassed();
            ScoreManager.Instance.OnDayPassed();
            WorldTimeSeconds = HOUR * 6f;
        }

        private void UpdateSun()
        {
            // CONCEPT TEST, this should be replaced with a better method. -->
            if (sunlight == null)
                return;

            if (WorldTimeSeconds < HOUR * 5f) // 00:00 - 05:00 > Night
            {
                sunlight.color = nightColor;
            }
            else if (WorldTimeSeconds < HOUR * 6f) // 05:00 - 06:00 > Night To Sunrise
            {
                sunlight.color = Color.Lerp(nightColor, sunriseColor, (WorldTimeSeconds - HOUR * 5f) / HOUR);
            }
            else if (WorldTimeSeconds < HOUR * 7f) // 06:00 - 07:00 > Sunrise to Day
            {
                sunlight.color = Color.Lerp(sunriseColor, dayColor, (WorldTimeSeconds - HOUR * 6f) / HOUR);
            }
            else if (WorldTimeSeconds < HOUR * 18f) // 07:00 - 18:00 > Day
            {
                sunlight.color = dayColor;
            }
            else if (WorldTimeSeconds < HOUR * 19f) // 18:00 - 19:00 > Day To Sunset
            {
                sunlight.color = Color.Lerp(dayColor, sunsetColor, (WorldTimeSeconds - HOUR * 18f) / HOUR);
            }
            else if (WorldTimeSeconds < HOUR * 20f + MINUTE * 30f) // 19:00 - 20:30 > Sunset to Night
            {
                sunlight.color = Color.Lerp(sunsetColor, nightColor, (WorldTimeSeconds - HOUR * 19f) / (HOUR + MINUTE * 30f));
            }
            else // 20:00 - 24:00 > Night
            {
                sunlight.color = nightColor;
            }
        }
    }

}