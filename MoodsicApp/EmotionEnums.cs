using System;
using Microsoft.ProjectOxford.Emotion.Contract;

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

    class CalcScores : Scores
    {
        public CalcScores() : base() { }
        public CalcScores(Scores other)
        { 
            Anger = other.Anger;
            Contempt = other.Contempt;
            Disgust = other.Disgust;
            Fear = other.Fear;
            Happiness = other.Happiness;
            Neutral = other.Neutral;
            Sadness = other.Sadness;
            Surprise = other.Surprise;
        }

        public static CalcScores operator*(float value, CalcScores other)
        {
            other.Anger *= value;
            other.Contempt *= value;
            other.Disgust *= value;
            other.Fear *= value;
            other.Happiness *= value;
            other.Neutral *= value;
            other.Sadness *= value;
            other.Surprise *= value;
            return other;
        }

        public static CalcScores operator*(CalcScores other, float value)
        {
            return value*other;
        }

        public static CalcScores operator+(float value, CalcScores other)
        {
            other.Anger += value;
            other.Contempt += value;
            other.Disgust += value;
            other.Fear += value;
            other.Happiness += value;
            other.Neutral += value;
            other.Sadness += value;
            other.Surprise += value;
            return other;
        }

        public static CalcScores operator+(CalcScores other, float value)
        {
            return value + other;
        }

        public static CalcScores operator+(CalcScores c1, CalcScores c2)
        {
            c1.Anger += c2.Anger;
            c1.Contempt += c2.Contempt;
            c1.Disgust += c2.Disgust;
            c1.Fear += c2.Fear;
            c1.Happiness += c2.Happiness;
            c1.Neutral += c2.Neutral;
            c1.Sadness += c2.Sadness;
            c1.Surprise += c2.Surprise;
            return c1;
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
