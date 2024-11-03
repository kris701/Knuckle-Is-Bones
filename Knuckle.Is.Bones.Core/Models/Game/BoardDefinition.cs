﻿using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game
{
    public class BoardDefinition : IUnlockable, IGenericClonable<BoardDefinition>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ColumnDefinition> Columns { get; set; }
        public int RequiredPoints { get; set; }

        [JsonConstructor]
        public BoardDefinition(Guid iD, string name, string description, List<ColumnDefinition> columns, int requiredPoints)
        {
            ID = iD;
            Name = name;
            Description = description;
            Columns = columns;
            RequiredPoints = requiredPoints;
        }

        public BoardDefinition(BoardDefinition other)
        {
            ID = other.ID;
            Name = other.Name;
            Description = other.Description;
            Columns = new List<ColumnDefinition>(other.Columns.Count);
            foreach (var col in other.Columns)
                Columns.Add(new ColumnDefinition(col));
            RequiredPoints = other.RequiredPoints;
        }

        public int GetValue()
        {
            int value = 0;
            foreach (var column in Columns)
                value += column.GetValue();
            return value;
        }

        public bool IsFull()
        {
            foreach (var column in Columns)
                if (!column.IsFull())
                    return false;
            return true;
        }

        public BoardDefinition Clone() => new BoardDefinition(this);
    }
}