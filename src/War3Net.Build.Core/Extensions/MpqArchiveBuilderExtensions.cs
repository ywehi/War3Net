﻿// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveBuilderExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Linq;

using War3Net.Build.Info;
using War3Net.IO.Mpq;

namespace War3Net.Build.Extensions
{
    public static class MpqArchiveBuilderExtensions
    {
        private static readonly ulong CampaignInfoHashedFileName = MpqHash.GetHashedFileName(CampaignInfo.FileName);
        private static readonly ulong MapInfoHashedFileName = MpqHash.GetHashedFileName(MapInfo.FileName);

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, bool leaveOpen = false)
        {
            var mpqFiles = mpqArchiveBuilder.ToArray();
            var campaignInfoFile = mpqFiles.SingleOrDefault(file => file.Name == CampaignInfoHashedFileName);
            if (campaignInfoFile is not null)
            {
                var campaignInfo = CampaignInfo.Parse(campaignInfoFile.MpqStream, true);
                campaignInfoFile.MpqStream.Position = 0;
                mpqArchiveBuilder.SaveWithPreArchiveData(stream, campaignInfo, leaveOpen);
            }
            else
            {
                var mapInfoFile = mpqFiles.SingleOrDefault(file => file.Name == MapInfoHashedFileName);
                if (mapInfoFile is not null)
                {
                    var mapInfo = MapInfo.Parse(mapInfoFile.MpqStream, true);
                    mapInfoFile.MpqStream.Position = 0;
                    mpqArchiveBuilder.SaveWithPreArchiveData(stream, mapInfo, leaveOpen);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to find {CampaignInfo.FileName} or {MapInfo.FileName} file to use as source for pre-archive data.");
                }
            }
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, CampaignInfo campaignInfo, bool leaveOpen = false)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream);
            mpqArchiveBuilder.SaveTo(stream, leaveOpen);
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MapInfo mapInfo, bool leaveOpen = false)
        {
            mapInfo.WriteArchiveHeaderToStream(stream);
            mpqArchiveBuilder.SaveTo(stream, leaveOpen);
        }
    }
}