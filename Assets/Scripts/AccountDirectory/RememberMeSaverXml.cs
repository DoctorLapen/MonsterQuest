using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace MonsterQuest
{
   
        public class RememberMeSaverXml : IRememberMeSaver
        {
       
            private XmlSerializer _xmlSerializer;
            private string _path;
            public RememberMeSaverXml(string fileName)
            {
                _xmlSerializer = new XmlSerializer(typeof(RememberMeInfo));
                _path = Path.Combine(Application.persistentDataPath, fileName);
                Debug.Log(_path);
            }

            public void Save(RememberMeInfo info)
            {
                using (FileStream fs = new FileStream( _path , FileMode.OpenOrCreate))
                {
                    _xmlSerializer.Serialize(fs, info);
                
                }
            }
            public RememberMeInfo Load()
            {
            
                if (File.Exists(_path))
                {
                    RememberMeInfo info;
                    using (FileStream fs = new FileStream(_path, FileMode.OpenOrCreate))
                    {
                        info = (RememberMeInfo) _xmlSerializer.Deserialize(fs);

                    }

                    return info;
                }
                else
                {
                    return  new RememberMeInfo();
                }
           
            }
            public void Delete()
            {
                File.Delete(_path);
            }
        }
    
}