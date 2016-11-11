using System;
using System.Collections.Generic;
using System.IO;

namespace Homework3 {
    class NumberReader : IDisposable {
        private readonly string fname;

        public NumberReader(FileInfo file) { fname = file.FullName; }

        //http://cc.davelozinski.com/c-sharp/fastest-way-to-read-text-files
        public IEnumerable<long> ReadIntegers() {
            foreach (var l in File.ReadLines(fname))
            {
                var value = long.Parse(l);
                yield return value;
            }
        } 

        public void Dispose() {}
    }
}