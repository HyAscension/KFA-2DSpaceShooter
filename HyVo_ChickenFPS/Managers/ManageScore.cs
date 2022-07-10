using HyVo_ChickenFPS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HyVo_ChickenFPS.Managers
{
    public class ManageScore
    {
        private static string fileName = "scores.xml";
        public List<Score> HighScores { get; private set; }
        public List<Score> Scores { get; private set; }

        public ManageScore() : this(new List<Score>())
        {
        }

        public ManageScore(List<Score> scores)
        {
            Scores = scores;
            UpdateHighscores();
        }

        public void Add(Score score)
        {
            Scores.Add(score);
            Scores = Scores.OrderByDescending(s => s.Value).ToList();
            UpdateHighscores();
        }

        public static ManageScore Load()
        {
            if (!File.Exists(fileName))
            {
                return new ManageScore();
            }
            using (StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
                var scores = (List<Score>)serializer.Deserialize(reader);
                return new ManageScore(scores);
            }
        }

        public void UpdateHighscores() => HighScores = Scores.Take(10).ToList();

        public static void Save(ManageScore scoreManager)
        {
            using (StreamWriter reader = new StreamWriter(new FileStream(fileName, FileMode.Create)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
                serializer.Serialize(reader, scoreManager.Scores);
            }
        }
    }
}
