﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Resin.IO;

namespace Resin.Sys
{
    public static class Util
    {
        public static readonly DateTime BeginningOfTime = new DateTime(2016, 4, 23);

        public static long GetChronologicalFileId()
        {
            var ticks = DateTime.Now.Ticks - BeginningOfTime.Ticks;
            return ticks;
        }

        private static IEnumerable<string> GetIndexFileNames(string directory)
        {
            return Directory.GetFiles(directory, "*.ix");
        }

        public static IEnumerable<string> GetIndexFileNamesInChronologicalOrder(string directory)
        {
            return GetIndexFileNames(directory)
                .Select(f => new {id = long.Parse(new FileInfo(f).Name.Replace(".ix", "")), fileName = f})
                .OrderBy(info => info.id)
                .Select(info => info.fileName);
        }

        public static int GetDocumentCount(string directory)
        {
            return GetIndexFileNamesInChronologicalOrder(directory)
                .Select(IxInfo.Load)
                .Sum(x=>x.DocumentCount);   
        }

        public static int GetDocumentCount(IEnumerable<IxInfo> ixs)
        {
            return ixs.Sum(x => x.DocumentCount); //TODO: this is a bug. Instead, return distinct doc hashes.
        }
    }
}