﻿using System;
using System.Linq;
using Moonlight.Core;
using Moonlight.Core.Enums;
using Moonlight.Game.Entities;
using Moonlight.Game.Utility;

namespace Moonlight.Game.Maps
{
    public class Map
    {
        internal Map(int id, string name, byte[] grid)
        {
            Id = id;
            Name = name;
            Grid = grid;
            Width = BitConverter.ToInt16(Grid.Take(2).ToArray(), 0);
            Height = BitConverter.ToInt16(Grid.Skip(2).Take(2).ToArray(), 0);

            Monsters = new SafeObservableDictionary<long, Monster>();
            Npcs = new SafeObservableDictionary<long, Npc>();
            Drops = new SafeObservableDictionary<long, GroundItem>();
            Players = new SafeObservableDictionary<long, Player>();
            Portals = new SafeObservableDictionary<long, Portal>();
        }

        private byte this[int x, int y] => Grid.Skip(4 + y * Width + x).Take(1).FirstOrDefault();

        public int Id { get; }
        public string Name { get; }
        public byte[] Grid { get; }
        public short Width { get; }
        public short Height { get; }

        public SafeObservableDictionary<long, Monster> Monsters { get; }
        public SafeObservableDictionary<long, Npc> Npcs { get; }
        public SafeObservableDictionary<long, Player> Players { get; }
        public SafeObservableDictionary<long, GroundItem> Drops { get; }
        public SafeObservableDictionary<long, Portal> Portals { get; }

        public Entity GetEntity(EntityType entityType, long entityId)
        {
            switch (entityType)
            {
                case EntityType.NPC:
                    return Npcs[entityId];
                case EntityType.MONSTER:
                    return Monsters[entityId];
                case EntityType.PLAYER:
                    return Players[entityId];
                case EntityType.GROUND_ITEM:
                    return Drops[entityId];
                default:
                    throw new InvalidOperationException();
            }
        }

        public T GetEntity<T>(EntityType entityType, long entityId) where T : Entity
        {
            Entity entity = GetEntity(entityType, entityId);
            if (!(entity is T castEntity))
            {
                return default;
            }

            return castEntity;
        }

        public T GetEntity<T>(long entityId) where T : Entity
        {
            EntityType entityType = EntityUtility.GetEntityType<T>();
            return GetEntity<T>(entityType, entityId);
        }

        public Portal GetPortal(int id) => Portals[id];

        internal void AddPortal(Portal portal)
        {
            Portals[portal.Id] = portal;
        }

        internal void AddEntity(Entity entity)
        {
            switch (entity.EntityType)
            {
                case EntityType.NPC:
                    Npcs[entity.Id] = (Npc)entity;
                    break;
                case EntityType.MONSTER:
                    Monsters[entity.Id] = (Monster)entity;
                    break;
                case EntityType.PLAYER:
                    Players[entity.Id] = (Player)entity;
                    break;
                case EntityType.GROUND_ITEM:
                    Drops[entity.Id] = (GroundItem)entity;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            entity.Map = this;
        }

        internal void RemoveEntity(Entity entity)
        {
            RemoveEntity(entity.EntityType, entity.Id);
        }

        internal void RemoveEntity(EntityType entityType, long entityId)
        {
            switch (entityType)
            {
                case EntityType.NPC:
                    Npcs.Remove(entityId);
                    break;
                case EntityType.MONSTER:
                    Monsters.Remove(entityId);
                    break;
                case EntityType.PLAYER:
                    Players.Remove(entityId);
                    break;
                case EntityType.GROUND_ITEM:
                    Drops.Remove(entityId);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public bool IsWalkable(Position position)
        {
            if (position.X > Width || position.X < 0 || position.Y > Height || position.Y < 0)
            {
                return false;
            }

            byte value = this[position.X, position.Y];

            return value == 0 || value == 2 || value >= 16 && value <= 19;
        }
    }
}