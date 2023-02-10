using mazeGame.GameElements;
using System;
using System.Collections.Generic;
/*using System.Linq;
using System.Text;
using System.Threading.Tasks;*/
using Microsoft.Xna.Framework;
/*using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;*/

namespace mazeGame.Menu
{

    internal class HighScore : IComparable<HighScore>
    {
        internal int score;
        internal TimeSpan time;

        internal HighScore(int score, TimeSpan time)
        {
            this.score = score;
            this.time = time;
        }

        public int CompareTo(HighScore other)
        {
            if (this.score > other.score)
                return -1;
            else if(this.score < other.score)
                return 1;
            else
            {
                //scores are the same; we'll compare the times
                if (this.time < other.time)
                    return -1;
                else if (this.time > other.time)
                    return 1;
                else
                    return 0;
            }
        }//END CompareTo()
    }

    internal class HighScores
    {
        List<HighScore> highScores;        

        internal HighScores()
        {
            highScores = new();
            for (int i = 0; i < 5; i++)
            {
                HighScore aScore = new(0, new TimeSpan(100000000 * (i+1)));
                highScores.Add(aScore);
            }
        }

        internal void SetupHighScores(List<HighScoresElement> highScoresElements)
        {
            HighScoresElement el = new("High scores:", new Vector2(100, 100), Color.Black);
            highScoresElements.Add(el);
            for(int i = 0; i < 5; i++)
            {
                TimeSpan time = highScores[i].time;
                el = new($"{highScores[i].score} -- {time.ToString()}", new Vector2(100, 150 + 50*i), Color.Black);
                                                  /*{time.Minutes}:{time.Seconds}:{time.Milliseconds}*/
                highScoresElements.Add(el);
            }
        }

        internal void AddIfItBelongs(HighScore score)
        {
            highScores.Add(score);
            highScores.Sort();
            while (highScores.Count > 5)
                highScores.RemoveAt(5);
        }
    }
}
