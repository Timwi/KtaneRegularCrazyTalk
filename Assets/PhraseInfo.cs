namespace RegularCrazyTalk
{
    /// <summary>Represents a piece of text that can be inserted in a phrase (the stuff in square brackets in the manual).</summary>
    sealed class PhraseArgPair
    {
        /// <summary>The text to insert in the phrase.</summary>
        public string Insert { get; private set; }
        /// <summary>Contains the necessary information to obtain the digit/hold/release values for this insert.</summary>
        public object Value { get; private set; }

        /// <summary>Constructor.</summary>
        public PhraseArgPair(string insert, object value)
        {
            Insert = insert;
            Value = value;
        }
    }

    /// <summary>Represents a phrase with all square brackets filled in, but before embellishment.</summary>
    sealed class PotentialPhraseAction
    {
        /// <summary>Full phrase.</summary>
        public string Phrase { get; private set; }
        /// <summary>Values of the three columns as in the manual (left to right).</summary>
        public int[] ColValues { get; private set; }

        /// <summary>Constructor.</summary>
        public PotentialPhraseAction(string phrase, int[] colValues)
        {
            Phrase = phrase;
            ColValues = colValues;
        }
    }

    /// <summary>Represents a phrase with all square brackets filled in and after embellishment.</summary>
    sealed class PhraseAction
    {
        /// <summary>Full phrase.</summary>
        public string Phrase { get; private set; }
        /// <summary>
        ///     Digit to show in the 7-segment display.</summary>
        /// <remarks>
        ///     This is the correct digit for the phrase ONLY if this is the correct phrase.</remarks>
        public int ShownDigit { get; set; }
        /// <summary>Digit the phrase would have in the manual.</summary>
        public int ExpectedDigit { get; set; }
        /// <summary>Digit when to hold down the screen.</summary>
        public int Hold { get; private set; }
        /// <summary>Digit when to release the screen.</summary>
        public int Release { get; private set; }

        /// <summary>Constructor.</summary>
        public PhraseAction(string phrase, int digit, int hold, int release)
        {
            Phrase = phrase;
            ShownDigit = digit;
            ExpectedDigit = digit;
            Hold = hold;
            Release = release;
        }
    }
}
