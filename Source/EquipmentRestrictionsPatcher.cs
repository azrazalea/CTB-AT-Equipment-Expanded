using System.Collections.Generic;
using System.Xml;
using Verse;

namespace CTB_AT_Equipment
{

	public class EquipmentRestrictionsPatcher : PatchOperation
	{
		public List<ItemLevelRule> itemLevelRules;


		protected override bool ApplyWorker(XmlDocument xml)
		{
			foreach (ItemLevelRule item in itemLevelRules)
			{
				EquipmentRestrictions.itemList.SetOrAdd(item.thingDef, item);
			}
			return true;
		}
	}
}
