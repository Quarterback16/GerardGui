using RosterLib;
using System.Collections.Generic;
using System.IO;

//|| ** When** || ** MU**   || ** Proj** || ** Spr** || ** Act** || ** ASpr** ||||  ** QB**             ||||  ** RB**            ||||  ** W1**           ||||  ** W2**          |||| ** TE**            ||||  ** PK**         ||  ** DEF**  ||  ** Notes**  ||
//||  Th8     || AF       ||      22  ||  +4     ||         ||          ||  M Ryan(18)  ||      || D Freeman(6) ||     || J Jones(14)   ||   || M Sanu(12)    ||    || A Hooper(5)  ||  || M Bryant(5)  ||  ||           ||  ||
//||          || PE       ||      26  ||         ||         ||          ||  N Foles(18) ||      || J Ajayi(15)  ||     || N Agholor(15) ||   || M Wallace(12) ||    || Z Ertz(5)    ||  || J Elliot(9)  ||  ||           ||  ||

namespace Butler.Models
{
    public class WeekPage
    {
        public NFLWeek Week { get; set; }
        List<NFLGame> Games { get; set; }
        public string OutputFile { get; set; }
        public WikiTable WikiTable { get; set; }

        public WeekPage(NFLWeek week)
        {
            Week = week;
            OutputFile = $"output\\Projections\\{week}-wiki.txt";
            WikiTable = new WikiTable(week.WeekNo);
        }

        public string RenderToWiki()
        {
            GenerateRows();
            SaveFile();
            return OutputFile;
        }

        private void GenerateRows()
        {
            foreach (NFLGame game in Week.GameList())
            {
                AddGameToPage(game);
            }
        }

        private void AddGameToPage(NFLGame game)
        {
            WikiTable.AddGame(game);
        }

        private void SaveFile()
        {
            Utility.EnsureDirectory(OutputFile);

            using (var sw = new StreamWriter(
                path: OutputFile,
                append: false))
            {
                WikiTable.GenerateRows();
                foreach (var row in WikiTable.Rows)
                {
                    sw.WriteLine(row);
                }
                sw.Close();
            }
        }
    }
}
