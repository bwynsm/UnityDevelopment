using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class DamageData 
{
	Dictionary<DAMAGE_TYPE, Dictionary<ARMOR_TYPE, Damage>> damageData;


	public DamageData()
	{
		damageData = new Dictionary<DAMAGE_TYPE, Dictionary<ARMOR_TYPE, Damage>> ();

		foreach (DAMAGE_TYPE damageType in Enum.GetValues(typeof(DAMAGE_TYPE)))
		{
			// create a new dictionary for each item
			damageData.Add (damageType, new Dictionary<ARMOR_TYPE, Damage> ());


			foreach (ARMOR_TYPE armorType in Enum.GetValues(typeof(ARMOR_TYPE)))
			{
				// create a new dictionary key for our armor type
				damageData[damageType].Add(armorType, new Damage());
				Damage damageVal = damageData [damageType] [armorType];

				switch (armorType)
				{
				case ARMOR_TYPE.HEAVY:

					switch (damageType)
					{

					// hit chance is 1, damage is very low
					case DAMAGE_TYPE.DAGGER:
						damageVal.hitChance = 1.0f; 
						damageVal.critChance = 0.01f; 
						damageVal.damage = 0.3f; 

						break;

						// there is always a chance for a heavy attack to miss
					case DAMAGE_TYPE.HEAVY: 
						damageVal.hitChance = 0.9f;
						damageVal.critChance = 0.10f; 
						damageVal.damage = 1.2f; 
						break;

						// natural hits heavy pretty easily
					case DAMAGE_TYPE.NATURAL:
						damageVal.hitChance = 0.95f;
						damageVal.critChance = 0.08f; 
						damageVal.damage = 0.85f;
						break;

						// this should have a high chance of hitting, but a javelin might still miss
					case DAMAGE_TYPE.PIERCING:
						damageVal.hitChance = 0.95f;
						damageVal.critChance = 0.08f; 
						damageVal.damage = 0.9f;
						break;

						// siege will always have a decent chance of missing, but with high damage
					case DAMAGE_TYPE.SIEGE:
						damageVal.hitChance = 0.65f;
						damageVal.critChance = 0.33f; 
						damageVal.damage = 1.3f;
						break;

						// spells are similar accuracy to natural
					case DAMAGE_TYPE.SPELL:
						damageVal.hitChance = 0.95f;
						damageVal.critChance = 0.15f; 
						damageVal.damage = 0.90f;
						break;

						// hard to miss with a sword when someone is moving so slowly
					case DAMAGE_TYPE.SWORD:
						damageVal.hitChance = 0.98f;
						damageVal.critChance = 0.01f;
						damageVal.damage = 0.75f;
						break;

					default:
						break;

					}

					break;


					// BARRIER ARMOR is MAGICAL ARMOR. It doesn't dodge very much, because we're basically
					// wearing cloth. But perhaps we can add a little evasion against natural and spells
				case ARMOR_TYPE.BARRIER:
					switch (damageType)
					{

					// hit chance is 1, damage is very low
					case DAMAGE_TYPE.DAGGER:
						damageVal.hitChance = 1.0f; 
						damageVal.critChance = 0.3f;
						damageVal.damage = 0.95f;
						break;

						// there is always a chance for a heavy attack to miss
						// mages are not lithe.. but even a mage can dodge a heavy attack sometimes
					case DAMAGE_TYPE.HEAVY: 
						damageVal.hitChance = 0.85f;
						damageVal.critChance = 0.25f; 
						damageVal.damage = 1.25f;
						break;

						// natural is not good against mages
					case DAMAGE_TYPE.NATURAL:
						damageVal.hitChance = 0.65f;
						damageVal.critChance = 0.15f; 
						damageVal.damage = 0.70f;

						break;

						// mages are very susceptible to projectiles
					case DAMAGE_TYPE.PIERCING:
						damageVal.hitChance = 0.95f;
						damageVal.critChance = 0.10f;
						damageVal.damage = 1.0f;
						break;

						// siege will always have a decent chance of missing, but with high damage
					case DAMAGE_TYPE.SIEGE:
						damageVal.hitChance = 0.45f;
						damageVal.critChance = 0.5f;
						damageVal.damage = 1.30f;
						break;

						// spells are similar accuracy to natural
					case DAMAGE_TYPE.SPELL:
						damageVal.hitChance = 0.85f;
						damageVal.critChance = 0.05f; 
						damageVal.damage = 0.65f;
						break;

						// hard to miss with a sword when someone is moving so slowly
					case DAMAGE_TYPE.SWORD:
						damageVal.hitChance = 1.0f;
						damageVal.critChance = 0.1f; 
						damageVal.damage = 1.0f;
						break;

					default:
						break;

					}


					break;
				case ARMOR_TYPE.LIGHT:

					switch (damageType)
					{

					// hit chance is high with a dagger, but a thief can evade a lot
					case DAMAGE_TYPE.DAGGER:
						damageVal.hitChance = 0.9f; 
						damageVal.critChance = 0.25f; 
						damageVal.damage = 0.90f;
						break;

						// there is always a chance for a heavy attack to miss
						// thief types are agile, and will dodge these attacks
					case DAMAGE_TYPE.HEAVY: 
						damageVal.hitChance = 0.45f;
						damageVal.critChance = 0.25f; 
						damageVal.damage = 1.25f;
						break;

						// dragons are good against thieves in general
					case DAMAGE_TYPE.NATURAL:
						damageVal.hitChance = 0.85f;
						damageVal.critChance = 0.15f; 
						damageVal.damage = 1.2f;
						break;

						// thieves are susceptible to projectiles a bit, but not as much as mages
					case DAMAGE_TYPE.PIERCING:
						damageVal.hitChance = 0.70f;
						damageVal.critChance = 0.15f; 
						damageVal.damage = 1.0f;
						break;

						// siege will always have a decent chance of missing, but with high damage
					case DAMAGE_TYPE.SIEGE:
						damageVal.hitChance = 0.3f;
						damageVal.critChance = 0.4f; 
						damageVal.damage = 1.35f;
						break;

						// spells are similar accuracy to natural
					case DAMAGE_TYPE.SPELL:
						damageVal.hitChance = 0.75f;
						damageVal.critChance = 0.1f; 
						damageVal.damage = 1.0f;
						break;

						// hard to miss with a sword when someone is moving so slowly
					case DAMAGE_TYPE.SWORD:
						damageVal.hitChance = 0.80f;
						damageVal.critChance = 0.15f; 
						damageVal.damage = 1.0f;
						break;

					default:
						break;

					}

					break;


				case ARMOR_TYPE.MEDIUM:

					switch (damageType)
					{

					// hit chance is 1, damage is very low
					case DAMAGE_TYPE.DAGGER:
						damageVal.hitChance = 1.0f; 
						damageVal.critChance = 0.15f;
						damageVal.damage = 0.85f;
						break;

						// there is always a chance for a heavy attack to miss
						// mages are not lithe.. but even a mage can dodge a heavy attack sometimes
					case DAMAGE_TYPE.HEAVY: 
						damageVal.hitChance = 0.80f;
						damageVal.critChance = 0.20f; 
						damageVal.damage = 1.15f;
						break;

						// natural is not good against mages
					case DAMAGE_TYPE.NATURAL:
						damageVal.hitChance = 0.70f;
						damageVal.critChance = 0.15f; 
						damageVal.damage = 1.0f;
						break;

						// mages are very susceptible to projectiles
					case DAMAGE_TYPE.PIERCING:
						damageVal.hitChance = 0.90f;
						damageVal.critChance = 0.12f; 
						damageVal.damage = 0.9f;
						break;

						// siege will always have a decent chance of missing, but with high damage
					case DAMAGE_TYPE.SIEGE:
						damageVal.hitChance = 0.5f;
						damageVal.critChance = 0.4f;
						damageVal.damage = 1.25f;
						break;

						// spells are similar accuracy to natural
					case DAMAGE_TYPE.SPELL:
						damageVal.hitChance = 0.85f;
						damageVal.critChance = 0.1f; 
						damageVal.damage = 1.0f;
						break;

						// hard to miss with a sword when someone is moving so slowly
					case DAMAGE_TYPE.SWORD:
						damageVal.hitChance = 1.0f;
						damageVal.critChance = 0.12f; 
						damageVal.damage = 1.0f;
						break;

					default:
						break;

					}
					break;
				default:
					break;

				}


				damageData [damageType] [armorType] = damageVal;

			} // END LOOP OVER ARMOR TYPE

		} // END LOOP OVER DAMAGE TYPE

	} // END DamageData constructor



	public DamageData (TextAsset file)
	{
		// open and read file in
		string[] lines = file.text.Split('\n');

		// for the population phase, we'll store a list of our header column
		// so that we can use that for our dictionary
		List<string> headerColumn = new List<string>();


		int rowIndex = 0;

		foreach (var line in lines)
		{
			

			// split the line
			string[] columns = line.Split(',');
			int columnIndex = 0;

			// walk over columns
			foreach (var column in columns)
			{
				// split our column into its 3 values if that is appropriate (ie we are not in column 0 and we are not in row 0)
				if (columnIndex != 0 && rowIndex != 0)
				{
					Debug.Log ("ROW > 0, COLUMN > 0 : " + column);
				}
				// if we are in here, we are in our first row, but we can ignore our first column
				else if (columnIndex != 0 && rowIndex == 0)
				{
					Debug.Log ("ROW == 0, COLUMN > 0 : " + column);
				}
				// otherwise, if we are not in row 0, and we have a column index of 0... get our column name
				else if (columnIndex == 0 && rowIndex != 0)
				{
					Debug.Log ("ROW > 0, COLUMN == 0 : " + column);
				}


				// update our column number
				columnIndex++;
			}

			// update our line number
			rowIndex++;
		}

	}




	/// <summary>
	/// Damage struct class.
	/// </summary>
	public struct Damage
	{

		public float damage;
		public float hitChance;
		public float critChance;
	};


	/// <summary>
	/// Calculates the hit chance for the player
	/// </summary>
	public float calculateHitChance(ARMOR_TYPE armor, DAMAGE_TYPE damage)
	{

		Debug.Log (damageData [damage] [armor].critChance + " " + damageData [damage] [armor].damage + " " + damageData [damage] [armor].hitChance);


		return damageData [damage] [armor].hitChance;
	}


	/// <summary>
	/// Calculates the crit chance.
	/// </summary>
	/// <returns>The crit chance.</returns>
	/// <param name="armor">Armor.</param>
	/// <param name="damage">Damage.</param>
	public float calculateCritChance(ARMOR_TYPE armor, DAMAGE_TYPE damage)
	{
		return damageData [damage] [armor].critChance; 
	}




	/// <summary>
	/// Calculates the damage dealt.
	/// </summary>
	/// <returns>The damage.</returns>
	/// <param name="armor">Armor.</param>
	/// <param name="damage">Damage.</param>
	/// <param name="baseDamage">Base damage.</param>
	public float calculateDamage(ARMOR_TYPE armor, DAMAGE_TYPE damage, int baseDamage)
	{
		int damageCalculation = Mathf.RoundToInt(baseDamage * damageData [damage] [armor].damage);

		return damageCalculation;
	}

}
