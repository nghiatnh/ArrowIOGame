using System;
using System.Collections.Generic;
using UnityEngine;

public class GameConstant{
    public const float INIT_ATK = 12;
    public static readonly int[] MAX_EXP_LEVEL = {0, 20, 35, 60, 100, 150, 180, 220, 300, 350, 450, 550, 700, 900, 1150, 1400, 1600, 2000, 2450};
    public static readonly Array LIST_SKILL = Enum.GetValues(typeof(SKILLS));

    private const float CRIT_AVERAGE = 0.25f;

    public static float CritAverage(int countCrit){
        if (countCrit == 1) return 100 * CRIT_AVERAGE;
        float CA = CritAverage(countCrit - 1);
        return CA + (100 - CA) * CRIT_AVERAGE;
    }

    public static int CompareCharacterPower(CharacterInfo c1, CharacterInfo c2){
        return -Math.Sign(c1.powerPoint - c2.powerPoint);
    }

    public static string[] ENEMY_NAME = new string[]{"Lionel Messi", "Cristiano Ronaldo", "Xavi", "Andres Iniesta", "Zlatan Ibrahimovic", "Radamel Falcao", "Robin van Persie", "Andrea Pirlo", "Yaya Toure", "Edinson Cavani", "Sergio Aguero", "Iker Casillas", "Neymar", "Sergio Busquets", "Xabi Alonso", "Thiago Silva", "Mesut Ozil", "David Silva", "Bastian Schweinsteiger", "Gianluigi Buffon", "Luis Suarez", "Sergio Ramos", "Vincent Kompany", "Gerard Pique", "Philipp Lahm", "Willian", "Marco Reus", "Franck Ribery", "Manuel Neuer", "Ashley Cole", "Wayne Rooney", "Juan Mata", "Thomas Muller", "Mario Götze", "Karim Benzema", "Cesc Fabregas", "Oscar", "Fernandinho", "Javier Mascherano", "Gareth Bale", "Javier Zanetti", "Daniele De Rossi", "Dani Alves", "Petr Cech", "Mats Hummels", "Carles Puyol", "Angel Di Maria", "Carlos Tevez", "Didier Drogba", "Giorgio Chiellini", "Marcelo", "Stephan El Shaarawy", "Toni Kroos", "Samuel Eto’o", "Jordi Alba", "Mario Gomez", "Arturo Vidal", "Eden Hazard", "James Rodriguez", "Marouane Fellaini", "Ramires", "David Villa", "Klaas Jan Huntelaar", "Nemanja Vidic", "Joe Hart", "Arjen Robben", "Mario Balotelli", "Mathieu Valbuena", "Pierre-Emerick Aubameyang", "Robert Lewandowski", "Hernanes", "Pedro", "Santi Cazorla", "Christian Eriksen", "Ezequiel Lavezzi", "Joao Moutinho", "Mario Mandžukić", "Patrice Evra", "David Luiz", "Luka Modric", "Victor Wanyama", "Mapou Yanga-M'Biwa", "Hulk", "Darijo Srna", "Emmanuel Mayuka", "John Terry", "Kwadwo Asamoah", "Leonardo Bonucci", "Javier Pastore", "Henrikh Mkhitaryan", "Moussa Dembele", "Hatem Ben Arfa", "Samir Nasri", "Shinji Kagawa", "Wesley Sneijder", "Pepe", "Marek Hamsik", "Javi Martinez", "Diego Forlan", "Paulinho" };
}

public enum SKILLS{
    MAGNET,
    RANGE_UP,
    CRIT,
    BLOOD_SUCKING,
    ALL_STATUS_UP,
    THROUGH_WALL,
    MORE_EXP,
    ATK_UP,
    ATK_UP_HP_DOWN,
    DEF_UP,
    HP_UP,
    SPEED_UP,
    BULLET_UP,
    KILL_HP_UP,
    BULLET_THROUGH_WALL,
    MAKE_SPIKE,
    PET_NINJA,
}

public enum STATUS
{
    CRIT,
    BLOOD_SUCKING,
    KILL_HP_UP,
    BULLET_THROUGH_WALL,
    BULLET_UP,
    MAKE_SPIKE,
    MORE_EXP,
    THROUGH_WALL,
}
