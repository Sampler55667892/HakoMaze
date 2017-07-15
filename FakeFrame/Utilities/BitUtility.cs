namespace FakeFrame
{
    public static class BitUtility
    {
        public static bool[] ToBitSequence( ulong value )
        {
            var bits = new bool[ 64 ];
            var index = 0;
 
            while (value > 0)  {
                var mod = value % 2;
                bits[ index++ ] = mod == 1;
                value -= mod;
                value /= 2;
            }
 
            return bits;
        }

        public static string ToString( bool[] bits )
        {
            var display = string.Empty;

            for (var i = 0; i < bits.Length; ++i)
                display += bits[ i ] ? "1" : "0";

            return display;
        }

        #region _TEST_

        static void _TestToBitSequence()
        {
            System.Diagnostics.Debug.Assert( _TestBits( "00000", BitUtility.ToBitSequence( 0 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "10000", BitUtility.ToBitSequence( 1 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "01000", BitUtility.ToBitSequence( 2 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "11000", BitUtility.ToBitSequence( 3 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "00100", BitUtility.ToBitSequence( 4 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "10100", BitUtility.ToBitSequence( 5 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "01100", BitUtility.ToBitSequence( 6 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "11100", BitUtility.ToBitSequence( 7 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "00010", BitUtility.ToBitSequence( 8 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "10010", BitUtility.ToBitSequence( 9 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "01010", BitUtility.ToBitSequence( 10 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "11010", BitUtility.ToBitSequence( 11 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "00110", BitUtility.ToBitSequence( 12 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "10110", BitUtility.ToBitSequence( 13 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "01110", BitUtility.ToBitSequence( 14 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "11110", BitUtility.ToBitSequence( 15 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "00001", BitUtility.ToBitSequence( 16 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "10001", BitUtility.ToBitSequence( 17 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "01001", BitUtility.ToBitSequence( 18 ) ) );
            System.Diagnostics.Debug.Assert( _TestBits( "11001", BitUtility.ToBitSequence( 19 ) ) );
        }

        static bool _TestBits( string expected, bool[] actual )
        {
            if (expected.Length != actual.Length)
                return false;

            var charArray = expected.ToCharArray();

            for (var i = 0; i < actual.Length; ++i) {
                if (string.Compare( charArray[ i ].ToString(), actual[ i ] ? "1" : "0" ) != 0)
                    return false;
            }

            return true;
        }

        #endregion  // _TEST_
    }
}
