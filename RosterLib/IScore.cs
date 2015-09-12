using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace RosterLib
{
    public interface IScore
    {
        string Name { get; }
        string ScoreType { get; set; }

        string PlayerId1 { get; set; }

        string Error { get; set; }

        bool IsValid();


        void Dump();
        void Load(DataRow dr);
    }
}
