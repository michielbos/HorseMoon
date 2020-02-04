using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using HorseMoon.Speech;

namespace HorseMoon {

    public class TimeController : SingletonMonoBehaviour<TimeController>
    {
        private const float MINUTE = 60f;
        private const float HOUR = 3600f;

        public bool runWorldTime = true;

        public float WorldTimeScale;

        public int day = 1;

        public DayOfWeek WeekDay => (DayOfWeek) ((day - 1) % 7);
        
        public enum DayOfWeek {
            Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
        }

        public float WorldTimeSeconds {
            get => worldTime;
            set {
                worldTime = value;
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

        public System.Action DayPassed;

        public Light2D sunlight;

        public Color nightColor;
        public Color sunriseColor;
        public Color dayColor;
        public Color sunsetColor;
        public Color forestColorMultiplier;

        private void Start()
        {
            WorldTimeSeconds = HOUR * 9f;
        }

        private void Update()
        {
            // DEBUG -->
            if (Input.GetKey(KeyCode.Minus))
                WorldTimeSeconds -= Time.deltaTime * 9000f;
            else if (Input.GetKey(KeyCode.Equals))
                WorldTimeSeconds += Time.deltaTime * 9000f;

            // TEMP -->
            if (sunlight == null)
                sunlight = FindObjectOfType<Light2D>();

            if (runWorldTime)
            {
                // Force the next day when the time rolls to 6 AM. -->
                if (WorldTimeSeconds >= HOUR * 24f) {
                    Player.Instance.PassOut();
                } else
                    WorldTimeSeconds += Time.deltaTime * WorldTimeScale;
            }
        }

        public void NextDay() {
            CropManager.Instance.OnDayPassed();
            TilemapManager.Instance.OnDayPassed();
            ScoreManager.Instance.OnDayPassed();
            VisitFarmEvent vfe = FindObjectOfType<VisitFarmEvent>();
            if (vfe.enabled)
                vfe.Finish();
            SpeechUI.Instance.Behavior.variableStorage.SetValue("$TTSpokeToday", false);
            SpeechUI.Instance.Behavior.variableStorage.SetValue("$passedOutToday", false);
            WorldTimeSeconds = HOUR * 6f;
            day++;
            DayPassed?.Invoke();
        }

        private void UpdateSun()
        {
            // CONCEPT TEST, this should be replaced with a better method. -->
            if (sunlight == null || Camera.main == null)
                return;

            Color multiplierColor = GetIsInForest(Camera.main.transform.position) ? forestColorMultiplier : Color.white;

            if (WorldTimeSeconds < HOUR * 5f) // 00:00 - 05:00 > Night
            {
                sunlight.color = nightColor * multiplierColor;
            }
            else if (WorldTimeSeconds < HOUR * 6f) // 05:00 - 06:00 > Night To Sunrise
            {
                sunlight.color = Color.Lerp(nightColor, sunriseColor, (WorldTimeSeconds - HOUR * 5f) / HOUR) * multiplierColor;
            }
            else if (WorldTimeSeconds < HOUR * 7f) // 06:00 - 07:00 > Sunrise to Day
            {
                sunlight.color = Color.Lerp(sunriseColor, dayColor, (WorldTimeSeconds - HOUR * 6f) / HOUR) * multiplierColor;
            }
            else if (WorldTimeSeconds < HOUR * 18f) // 07:00 - 18:00 > Day
            {
                sunlight.color = dayColor * multiplierColor;
            }
            else if (WorldTimeSeconds < HOUR * 19f) // 18:00 - 19:00 > Day To Sunset
            {
                sunlight.color = Color.Lerp(dayColor, sunsetColor, (WorldTimeSeconds - HOUR * 18f) / HOUR) * multiplierColor;
            }
            else if (WorldTimeSeconds < HOUR * 20f + MINUTE * 30f) // 19:00 - 20:30 > Sunset to Night
            {
                sunlight.color = Color.Lerp(sunsetColor, nightColor, (WorldTimeSeconds - HOUR * 19f) / (HOUR + MINUTE * 30f)) * multiplierColor;
            }
            else // 20:00 - 24:00 > Night
            {
                sunlight.color = nightColor * multiplierColor;
            }
        }

        private bool GetIsInForest(Vector3 pos) {
            return pos.x < -150f;
        }
    }

}