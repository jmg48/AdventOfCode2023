using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Dumpify;
using Microsoft.VisualBasic;
using NUnit.Framework.Internal.Commands;

namespace AdventOfCode2023
{
    public record Card(char C) : IComparable<Card>
    {
        private int Strength => C switch
        {
            '2' => 0,
            '3' => 1,
            '4' => 2,
            '5' => 3,
            '6' => 4,
            '7' => 5,
            '8' => 6,
            '9' => 7,
            'T' => 8,
            'J' => 9,
            'Q' => 10,
            'K' => 11,
            'A' => 12,
            _ => throw new ArgumentOutOfRangeException()
        };

        public int CompareTo(Card? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return this.Strength.CompareTo(other.Strength);
        }

        public override string ToString() => C.ToString();
    }

    public record Hand(List<Card> Cards) : IComparable<Hand>
    {
        int Type
        {
            get
            {
                var groupSizes = Cards.GroupBy(c => c).Select(group => group.Count()).OrderByDescending(i => i)
                    .ToList();
                switch (groupSizes[0])
                {
                    case 5:
                        return 6;
                    case 4:
                        return 5;
                    case 3:
                        return groupSizes[1] switch
                        {
                            2 => 4,
                            _ => 3
                        };
                    case 2:
                        return groupSizes[1] switch
                        {
                            2 => 2,
                            _ => 1
                        };
                    default:
                        return 0;
                }
            }
        }

        public int CompareTo(Hand? other)
        {
            var compareType = this.Type.CompareTo(other.Type);
            if (compareType != 0)
            {
                return compareType;
            }

            for (var i = 0; i < 5; i++)
            {
                var compareCard = this.Cards[i].CompareTo(other.Cards[i]);
                if (compareCard != 0)
                {
                    return compareCard;
                }
            }

            return 0;
        }

        public override string ToString() => string.Concat(Cards.Select(c => c.ToString()));
    }

    public class Day07
    {
        [Test]
        public void Part1()
        {
            var input = File.ReadAllLines("Input7.txt");

            var hands = input.Select(line =>
            {
                var split = line.Split(' ');
                var hand = new Hand(split[0].Select(c => new Card(c)).ToList());
                return (hand, bid: int.Parse(split[1]));
            }).ToList();

            var ordered = hands.OrderBy(x => x.hand).ToList();

            foreach (var hand in ordered)
            {
                Console.WriteLine(hand);
            }

            var result = ordered.Select((x, i) => x.bid * (i + 1)).Sum();

            Console.WriteLine(result);
        }
    }
}