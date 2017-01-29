using System;

namespace MoodsicApp
{
    struct DetectedResult : IComparable<DetectedResult>
    {
        public EmotionEnum emotion;
        public float value;

        public DetectedResult(EmotionEnum e, float v)
        {
            emotion = e;
            value = v;
        }

        public Mood toMood()
        {
            switch (emotion)
            {
                case EmotionEnum.ANGER:
                    return Mood.AGRESSIVE;
                case EmotionEnum.CONTEMPT:
                case EmotionEnum.DISGUST:
                case EmotionEnum.FEAR:
                    return Mood.PEACEFUL;
                case EmotionEnum.HAPPINESS:
                    return Mood.LIVELY;
                case EmotionEnum.NEUTRAL:
                    return Mood.PEACEFUL;
                case EmotionEnum.SADNESS:
                    return Mood.SENTIMENTAL;
                case EmotionEnum.SURPRISE:
                    return Mood.STIRRING;

            }
            return Mood.PEACEFUL;
        }

        public int CompareTo(DetectedResult other)
        {
            if (this.value < other.value) return -1;
            if (this.value == other.value) return 0;
            return 1;
        }
    }

    enum EmotionEnum
    {
        NONE = 0,
        ANGER = 1,
        CONTEMPT = 2,
        DISGUST = 3,
        FEAR = 4,
        HAPPINESS = 5,
        NEUTRAL = 6,
        SADNESS = 7,
        SURPRISE = 8,
    }

    enum Mood
    {
        AGRESSIVE = 42958,
        LIVELY = 65332,
        PEACEFUL = 65322,
        SENTIMENTAL = 65324,
        STIRRING = 65331
    }
}
