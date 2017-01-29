namespace MoodsicApp
{
    struct DetectedResult
    {
        public Emotion emotion;
        public float value;

        public DetectedResult(Emotion e, float v)
        {
            emotion = e;
            value = v;
        }

        public static bool operator <(DetectedResult res1, DetectedResult res2)
        {
            return res1.value < res2.value;
        }
        
        public static bool operator >(DetectedResult res1, DetectedResult res2)
        {
            return res1.value > res2.value;
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
