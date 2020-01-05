﻿using NtCore.Clients;
using NtCore.Game.Battle;
using NtCore.Game.Entities;

namespace NtCore.Events.Battle
{
    public class EntityDamageEvent : Event
    {
        public LivingEntity Target { get; }
        public LivingEntity Caster { get; }
        public int Damage { get; }


        public EntityDamageEvent(IClient client, LivingEntity target, LivingEntity caster, int damage) : base(client)
        {
            Target = target;
            Caster = caster;
            Damage = damage;
        }
    }
}