using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm
{
    public static class StringGuidExtensions
    {
        public static Guid ToGuid(this string input)
        {
            byte[] databytes = Encoding.Default.GetBytes(input);
            Array.Resize(ref databytes, 16);
            return new Guid(databytes);
        }

        public static string FromGuid(this Guid guid)
        {
            byte[] databytes = guid.ToByteArray();
            return Encoding.Default.GetString(databytes);
        }
    }
}
