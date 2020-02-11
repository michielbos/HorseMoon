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

        public int Day {
            get => day;
            set {
                day = value;
                StoryProgress.Instance.Set("Day", value);
            }
        }
        private int day = 1;

        public DayOfWeek WeekDay => (DayOfWeek) ((Day - 1) % 7);
        
        public enum DayOfWeek {
            Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
        }

        public float WorldTimeSeconds {
            get => worldTime;
            set {
                float previousTime = worldTime;
                worldTime = value;
                UpdateSun();
                if ((previousTime / HOUR < LocationController.Instance.dayMusicStartHour && value / HOUR >= LocationController.Instance.dayMusicStartHour)
                || (previousTime / HOUR < LocationController.Instance.nightMusicStartHour && value / HOUR >= LocationController.Instance.nightMusicStartHour))
                    LocationController.Instance.PlayCurrentMusic();
            }
        }
        private float worldTime;

        public float WorldTimeMinutes {
            get => worldTime / MINUTE;
            set { WorldTimeSeconds = value * MINUTE; }
        }

        public float WorldTimeHours {
            get => worldTime / HOUR;
            set { WorldTimeSeconds = value * HOUR; }
        }

        public System.Action DayPassed;

        public Light2D sunlight;

        public Color nightColor;
        public Color sunriseColor;
        public Color dayColor;
        public Color sunsetColor;
        public Color forestColorMultiplier;

        public Gradient MorningSunlightRamp;
        public Gradient EveningSunlightRamp;

        private void Start() {
            WorldTimeSeconds = HOUR * 6f;
        }

        private void Update()
        {
            // DEBUG -->
            if (Input.GetKey(KeyCode.BackQuote))
            {
                if (Input.GetKey(KeyCode.Minus))
                    WorldTimeSeconds -= Time.deltaTime * 9000f;
                else if (Input.GetKey(KeyCode.Equals))
                    WorldTimeSeconds += Time.deltaTime * 9000f;
            }
            
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

            StoryProgress.Instance.OnDayPassed();
            WorldTimeSeconds = HOUR * 6f;
            Day++;
            LocationController.Instance.PlayCurrentMusic();

            DayPassed?.Invoke();
        }

        private void UpdateSun()
        {
            // CONCEPT TEST, this should be replaced with a better method. -->
            if (sunlight == null || Camera.main == null)
                return;            

            Color multiplierColor = GetIsInForest(Camera.main.transform.position) ? forestColorMultiplier : Color.white;
            if(WorldTimeHours < 6f)
            {
                sunlight.color = MorningSunlightRamp.Evaluate(0f) * multiplierColor;
            }
            else if(WorldTimeHours < 10f)
            {
                sunlight.color = MorningSunlightRamp.Evaluate((WorldTimeHours - 6f) / 2f) * multiplierColor;
            }
            else if(WorldTimeHours < 17f)
            {
                sunlight.color = MorningSunlightRamp.Evaluate(1f) * multiplierColor;
            }
            else if(WorldTimeHours < 19f)
            {
                sunlight.color = EveningSunlightRamp.Evaluate((WorldTimeHours -17f) / 2f) * multiplierColor;
            }
            else
            {
                sunlight.color = EveningSunlightRamp.Evaluate(1f) * multiplierColor;
            }
        }

        private bool GetIsInForest(Vector3 pos) {
            return pos.x < -150f;
        }
    }

}