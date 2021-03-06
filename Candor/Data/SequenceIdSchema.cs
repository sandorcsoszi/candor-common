﻿using System;

namespace Candor.Data
{
    /// <summary>
    /// Defines the schema for a sequence Id that supports an Id with any character set.
    /// This is not limited to simple numeric sequences.
    /// </summary>
    public class SequenceIdSchema
    {
        private LexicalCharacterSetType _characterSet = LexicalCharacterSetType.Numeric;
        private int _rangeSize = 100;

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public String TableName { get; set; }
        /// <summary>
        /// Gets or sets the character set type used to generate the next sequence Id from a given value.
        /// </summary>
        public LexicalCharacterSetType CharacterSetType
        {
            get { return _characterSet; }
            set { _characterSet = value; }
        }
        /// <summary>
        /// Gets or sets the seed value the first id should be incremented from (not that of the first value).
        /// </summary>
        public String SeedValue { get; set; }
        /// <summary>
        /// Gets or sets the number of Ids to reserve from the store per node.
        /// </summary>
        /// <remarks>
        /// This should at least be the number of sequences you expect to generate per node within a minute.
        /// </remarks>
        public Int32 RangeSize
        {
            get { return _rangeSize; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", value, "The range size must be 1 or greater.  It should at least be the number of sequences you expect to generate per node within a minute.");
                _rangeSize = value;
            }
        }
    }
}
