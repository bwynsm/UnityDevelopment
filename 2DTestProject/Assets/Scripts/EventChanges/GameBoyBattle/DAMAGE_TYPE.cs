// daggers - high chance to hit against all types : low damage on heavy and medium
// sword - pretty high chance to hit on all types : low damage on heavy
// heavy - lower chance to hit on light, medium on medium : good damage on all
// piercing : better damage on heavy, higher chance to miss on medium and light
// siege: high damage against all types - fair chance to miss on everything
// natural: good damage on everything except barrier
// spell: good damage on everything except barrier : can miss against barrier and light
public enum DAMAGE_TYPE
{
	SPELL,
	PIERCING,
	HEAVY,
	SWORD,
	DAGGER, 
	SIEGE, 
	NATURAL
};