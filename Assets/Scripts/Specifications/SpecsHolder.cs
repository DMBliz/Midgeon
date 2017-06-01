﻿using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SpecsHolder : MonoBehaviour
{
	public event Action<BaseModificator> OnAddModificator;
	public event Action<BaseModificator> OnRemoveModificator;
	private List<BaseStat> stats = new List<BaseStat>();
	private List<BaseAttribute> attributes = new List<BaseAttribute>();

	public void AddSpec<T>(T spec)
	{
		if (typeof(T) == typeof(BaseStat))
		{
			stats.Add(spec as BaseStat);
		}

		if (typeof(T) == typeof(BaseAttribute))
		{
			attributes.Add(spec as BaseAttribute);
		}

		UpdateEvents();
	}

	public void UpdateEvents()
	{
		foreach (BaseStat stat in stats)
		{
			stat.OnAddModificator -= OnAddModificator;
			stat.OnRemoveModificator -= OnRemoveModificator;

			stat.OnAddModificator += OnAddModificator;
			stat.OnRemoveModificator += OnRemoveModificator;
		}

		foreach (BaseAttribute stat in attributes)
		{
			stat.OnAddModificator -= OnAddModificator;
			stat.OnRemoveModificator -= OnRemoveModificator;

			stat.OnAddModificator += OnAddModificator;
			stat.OnRemoveModificator += OnRemoveModificator;
		}
	}

	public void AddEffects(List<Effect> effects)
	{
		foreach (Effect effect in effects)
		{
			WeaponEffect temp = effect as WeaponEffect;
			if (temp != null)
			{
				int chance = Random.Range(0, 100);
				if(temp.chance>chance)
					AddModificator(temp.statName, temp.modificator);
			}
			else
			{
				AddModificator(effect.statName, effect.modificator);
			}
		}
	}

	public void AddEffects(List<WeaponEffect> effects)
	{
		foreach (WeaponEffect effect in effects)
		{
			int chance = Random.Range(0, 100);
			if (effect.chance > chance)
				AddModificator(effect.statName, effect.modificator);
		}
	}

	public void Remove(string name)
	{
		foreach (BaseStat stat in stats)
		{
			if (stat.StatName == name)
			{
				stats.Remove(stat);
				return;
			}
		}
		foreach (BaseAttribute attribute in attributes)
		{
			if (attribute.Name == name)
			{
				attributes.Remove(attribute);
				return;
			}
		}
	}

	public void AddModificator(string name, BaseModificator modificator)
	{
		BaseStat stat = GetStat<BaseStat>(name);
		if (stat == null)
		{
			BaseAttribute attribute = GetStat<BaseAttribute>(name);
			if (attribute == null)
			{
				return;
			}
			attribute.AddModificator(modificator);
			return;
		}
		stat.AddModificator(modificator);
	}

	public void RemoveModificator(BaseModificator modificator)
	{
		
	}

	public T GetStat<T>(string name) where T : class
	{
		if (typeof(T) == typeof(BaseStat))
		{
			foreach (BaseStat stat in stats)
			{
				if (stat.StatName == name)
					return stat as T;
			}
		}
		else if (typeof(T) == typeof(BaseAttribute))
		{
			foreach (BaseAttribute attribute in attributes)
			{
				if (attribute.Name == name)
					return attribute as T;
			}
		}
		return null;
	}
}