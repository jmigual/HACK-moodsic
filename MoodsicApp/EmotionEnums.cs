using System;

namespace MoodsicApp
{
    struct DetectedResult : IComparable<DetectedResult>
    {
        public Emotion emotion;
        public float value;

        public DetectedResult(Emotion e, float v)
        {
            emotion = e;
            value = v;
        }

        public Mood toMood()
        {
            switch (emotion)
            {
                case Emotion.ANGER:
                    return Mood.AGRESSIVE;
                case Emotion.CONTEMPT:
                case Emotion.DISGUST:
                case Emotion.FEAR:
                    return Mood.PEACEFUL;
                case Emotion.HAPPINESS:
                    return Mood.LIVELY;
                case Emotion.NEUTRAL:
                    return Mood.PEACEFUL;
                case Emotion.SADNESS:
                    return Mood.SENTIMENTAL;
                case Emotion.SURPRISE:
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

    enum Emotion
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
