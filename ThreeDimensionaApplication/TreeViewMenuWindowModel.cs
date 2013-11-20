using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeDimensionaApplication
{
    /// <summary>
    /// 这个国家队是某个地方的队
    /// </summary>
    public class Team
    {
        public Team(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    /// <summary>
    /// 这个国家队的某个地方的队集合
    /// </summary>
    public class TeamList:List<Team>
    {
        public TeamList():base()
        {

        }
    }

    /// <summary>
    /// 这个国家队来自哪里
    /// </summary>
    public class Division
    {
        public string Name { get; set; }

        public TeamList Teams { get; set; }

        public Division(string name)
        {
            Name = name;
            Teams = new TeamList();
        }

    }

    /// <summary>
    /// 这个国家队的集合
    /// </summary>
    public class DivisionList:List<Division>
    {
        public DivisionList():base()
        {

        }
    }

    /// <summary>
    /// 哪个国家的队
    /// </summary>
    public class League
    {
        public string Name { get; set; }
        public DivisionList Divisions { get; set; }

        public League(string name)
        {
            Name = name;
            Divisions = new DivisionList();
        }

    }

    /// <summary>
    /// 所有队及类型
    /// </summary>
    public class LeagueList:List<League>
    {
        public LeagueList():base()
        {
            League l;
            Division d;

            l = new League("中国队");
            Add(l);
            d = new Division("中国北方队");
            l.Divisions.Add(d);
            d.Teams.Add(new Team("中国北方1号队"));
            d.Teams.Add(new Team("中国北方2号队"));
            d.Teams.Add(new Team("中国北方3号队"));
            d.Teams.Add(new Team("中国北方4号队"));
            d.Teams.Add(new Team("中国北方5号队"));
            d = new Division("中国南方队");
            l.Divisions.Add(d);
            d.Teams.Add(new Team("中国南方1号队"));
            d.Teams.Add(new Team("中国南方2号队"));
            d.Teams.Add(new Team("中国南方3号队"));
            d.Teams.Add(new Team("中国南方4号队"));
            d.Teams.Add(new Team("中国南方5号队"));

            l = new League("美国队");
            Add(l);
            d = new Division("美国北方队");
            l.Divisions.Add(d);
            d.Teams.Add(new Team("美国北方1号队"));
            d.Teams.Add(new Team("美国北方2号队"));
            d.Teams.Add(new Team("美国北方3号队"));
            d.Teams.Add(new Team("美国北方4号队"));
            d.Teams.Add(new Team("美国北方5号队"));
            d = new Division("美国南方队");
            l.Divisions.Add(d);
            d.Teams.Add(new Team("美国南方1号队"));
            d.Teams.Add(new Team("美国南方2号队"));
            d.Teams.Add(new Team("美国南方3号队"));
            d.Teams.Add(new Team("美国南方4号队"));
            d.Teams.Add(new Team("美国南方5号队"));

        }
    }

}
