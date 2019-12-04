﻿using System.Collections.Generic;
using NtCore.API.Extensions;
using NtCore.API.Game.Maps;
using NtCore.API.Managers;
using NtCore.Game.Maps;

namespace NtCore.Managers
{
    public class MapManager : IMapManager
    {
        private readonly IDictionary<int, IMap> _maps = new Dictionary<int, IMap>();
        
        public IMap GetMapById(int id)
        {
            IMap map = _maps.GetValueOrDefault(id);
            if (map == null)
            {
                map = new Map(id);
                _maps[id] = map;
            }

            return map;
        }
    }
}