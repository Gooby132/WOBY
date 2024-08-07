﻿namespace Woby.Core.CommonLanguage.Signaling.Linguistics
{
    public class LanguageOrder
    {
        public string Language { get; }
        public float? Order { get; }

        public LanguageOrder(string language, float? order)
        {
            Language = language;
            Order = order;
        }
    }
}
