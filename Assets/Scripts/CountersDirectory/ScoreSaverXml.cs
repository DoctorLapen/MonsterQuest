
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace MonsterQuest
{
    public class ScoreSaverXml : IScoreSaver
    {
       
        private XmlSerializer _xmlSerializer;
        private string _path;
        public ScoreSaverXml(string fileName)
        {
            _xmlSerializer = new XmlSerializer(typeof(ScoreAmount));
            _path = Path.Combine(Application.persistentDataPath, fileName);
        }

        public void Save(ScoreAmount score)
        {
            using (FileStream fs = new FileStream( _path , FileMode.OpenOrCreate))
            {
                _xmlSerializer.Serialize(fs, score);
                
            }
        }
        public ScoreAmount Load()
        {
            
            if (File.Exists(_path))
            {
                ScoreAmount scoreAmount;
                using (FileStream fs = new FileStream(_path, FileMode.OpenOrCreate))
                {
                    scoreAmount = (ScoreAmount) _xmlSerializer.Deserialize(fs);

                }

                return scoreAmount;
            }
            else
            {
                return  new ScoreAmount();
            }
           
        }
    }
}