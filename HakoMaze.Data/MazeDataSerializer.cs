using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace HakoMaze.Data
{
    [Serializable]
    public class MazeDataSerializer
    {
        public MazeFrameData FrameData { get; set; }
        public MazeContentData ContentData { get; set; }

        // Serialize 用のデフォルトコンストラクタ
        public MazeDataSerializer()
        {
        }

        public MazeDataSerializer( MazeFrameData frameData, MazeContentData contentData )
        {
            this.FrameData = frameData;
            this.ContentData = contentData;
        }

        public void Save( string fileName )
        {
            using (var writer = new StreamWriter( fileName, false, Encoding.UTF8 )) {
                var serializer = new XmlSerializer( typeof( MazeDataSerializer ) );
                serializer.Serialize( writer, this );
            }
        }

        public bool Load( string fileName )
        {
            using (var reader = new StreamReader( fileName, Encoding.UTF8 )) {
                var serializer = new XmlSerializer( typeof( MazeDataSerializer ) );
                try {
                    var ob = serializer.Deserialize( reader );
                    if (ob is MazeDataSerializer data) {
                        FrameData.Load( data.FrameData );
                        ContentData.Load( data.ContentData );
                    }
                } catch {
                    return false;
                }
            }

            return true;
        }
    }
}
