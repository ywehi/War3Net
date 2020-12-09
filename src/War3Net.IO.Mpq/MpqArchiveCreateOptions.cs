﻿// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveCreateOptions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Mpq
{
    public sealed class MpqArchiveCreateOptions
    {
        /// <summary>
        /// Default value for an <see cref="MpqArchive"/>'s blocksize.
        /// </summary>
        public const ushort DefaultBlockSize = 3;

        public MpqArchiveCreateOptions()
        {
            BlockSize = DefaultBlockSize;
            HashTableSize = null;
            WriteArchiveFirst = true;
            AttributesFlags = AttributesFlags.Crc32 | AttributesFlags.DateTime;
            AttributesCreateMode = MpqFileCreateMode.Overwrite;
            ListFileCreateMode = MpqFileCreateMode.Overwrite;
        }

        /// <summary>
        /// The size of blocks in compressed files, which is used to enable seeking.
        /// </summary>
        public ushort BlockSize { get; init; }

        /// <summary>
        /// The desired size of the <see cref="BlockTable"/>. Larger size decreases the likelihood of hash collisions.
        /// </summary>
        public ushort? HashTableSize { get; init; }

        /// <summary>
        /// If <see langword="true"/>, the archive files will be positioned directly after the header. Otherwise, the hashtable and blocktable will come first.
        /// </summary>
        public bool WriteArchiveFirst { get; init; }

        public AttributesFlags AttributesFlags { get; init; }

        public MpqFileCreateMode AttributesCreateMode { get; init; }

        public MpqFileCreateMode ListFileCreateMode { get; init; }
    }
}