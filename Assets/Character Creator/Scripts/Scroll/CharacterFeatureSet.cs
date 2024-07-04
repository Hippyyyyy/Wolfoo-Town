using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace _WolfooShoppingMall
{
	[Serializable]
    public class CharacterFeatureSet
	{
		#region Item
		public int CharacterID;

		public bool IsCreated;

		public AgeSetting Age;

		public int HairID;

		public int FacialHairID;

		public int EyesID;

		public int MouthID;

		public int BlushID;

		public int WrinklesID;

		public int FrecklesID;

		public int ScarID;

		public int HeadID;

		public int EyebrowsID;

		public int NoseID;

		public int EarsID;

		public int TorsoID;

		public int TorsoBackID;

		public int ArmLeftID;

		public int ArmRightID;

		public int LegLeftID;

		public int LegRightID;

		public int ClothingID;

		public int HatID;

		public int MaskID;

		public int AccessoryInHandID;

		public int SkinID;
        #endregion

        #region ColorItem
        public Color SkinColor;

		public Color EyesColor;

		public Color HairColor;

		public Color EyebrowsColor;

		public int SkinColorId;

		public int EyesColorId;

		public int HairColorId;

		public int EyebrowsColorId;
        #endregion
        public int GetFeatureByCategory(CharacterFeatureCategoryEnum category)
		{
			if (category == CharacterFeatureCategoryEnum.Hair)
			{
				return HairID;
			}
			else if (category == CharacterFeatureCategoryEnum.Eyes)
			{
				return EyesID;
			}
			else if (category == CharacterFeatureCategoryEnum.Eyebrows)
			{
				return EyebrowsID;
			}
			else if (category == CharacterFeatureCategoryEnum.Nose)
			{
				return NoseID;
			}
			else if (category == CharacterFeatureCategoryEnum.Mouth)
			{
				return MouthID;
			}
			else if (category == CharacterFeatureCategoryEnum.Clothes)
			{
				return ClothingID;
			}
			else if (category == CharacterFeatureCategoryEnum.Hat)
			{
				return HatID;
			}
			else if (category == CharacterFeatureCategoryEnum.Mask)
			{
				return MaskID;
            }
			else if (category == CharacterFeatureCategoryEnum.AccessoryHand)
			{
				return AccessoryInHandID;
			}
			else
            {
				return 0;
            }
		}

		public void SetFeatureByCategory(CharacterFeatureCategoryEnum category, int featureID)
		{
			switch (category)
			{
				case CharacterFeatureCategoryEnum.Eyebrows:
					EyebrowsID = featureID;
					break;
				case CharacterFeatureCategoryEnum.Eyes:
					EyesID = featureID;
					break;
				case CharacterFeatureCategoryEnum.Hair:
					HairID = featureID;
					break;
				case CharacterFeatureCategoryEnum.Nose:
					NoseID = featureID;
					break;
				case CharacterFeatureCategoryEnum.Mouth:
					MouthID = featureID;
					break;
				case CharacterFeatureCategoryEnum.Clothes:
					ClothingID = featureID;
					break;
				case CharacterFeatureCategoryEnum.Hat:
					HatID = featureID;
					break;
				case CharacterFeatureCategoryEnum.Mask:
					MaskID = featureID;
					break;
				case CharacterFeatureCategoryEnum.AccessoryHand:
					AccessoryInHandID = featureID;
					break;
				case CharacterFeatureCategoryEnum.Skin:
					Age = (AgeSetting)featureID;
					break;
			}
		}
		public void SetColorByCategory(CharacterColorCategory colorCategory, Color colorItem)
		{
			if (colorCategory == CharacterColorCategory.Eyebrows)
			{
				EyebrowsColor = colorItem;
			}
			else if (colorCategory == CharacterColorCategory.Eyes)
			{
				EyesColor = colorItem;
			}
			else if (colorCategory == CharacterColorCategory.Hair)
			{
				HairColor = colorItem;
			}
			else if (colorCategory == CharacterColorCategory.Skin)
			{
				SkinColor = colorItem;
			}
		}
		public void SetIdColorByCategory(CharacterColorCategory colorCategory, int id)
		{
			if (colorCategory == CharacterColorCategory.Eyebrows)
			{
				EyebrowsColorId = id;
			}
			else if (colorCategory == CharacterColorCategory.Eyes)
			{
				EyesColorId = id;
			}
			else if (colorCategory == CharacterColorCategory.Hair)
			{
				HairColorId = id;
			}
			else if (colorCategory == CharacterColorCategory.Skin)
			{
				SkinColorId = id;
			}
		}
		public Color? GetColorByCategory(CharacterColorCategory colorCategory)
		{

			if (colorCategory == CharacterColorCategory.Eyebrows)
			{
				return EyebrowsColor;
			}
			else if (colorCategory == CharacterColorCategory.Eyes)
			{
				return EyesColor;
			}
			else if (colorCategory == CharacterColorCategory.Hair)
			{
				return HairColor;
			}
			else if (colorCategory == CharacterColorCategory.Skin)
			{
				return SkinColor;
			}
            else
            {
                return null;
            }
        }
		public int? GetIDByCategory(CharacterFeatureCategoryEnum featureCategoryEnum)
		{

			if (featureCategoryEnum == CharacterFeatureCategoryEnum.Eyebrows)
			{
				return EyebrowsID;
			}
			else if (featureCategoryEnum == CharacterFeatureCategoryEnum.Eyes)
			{
				return EyesID;
			}
			else if (featureCategoryEnum == CharacterFeatureCategoryEnum.Hair)
			{
				return HairID;
			}
			else if (featureCategoryEnum == CharacterFeatureCategoryEnum.Mouth)
			{
				return MouthID;
			}
			else if (featureCategoryEnum == CharacterFeatureCategoryEnum.Nose)
			{
				return NoseID;
			}
			else if (featureCategoryEnum == CharacterFeatureCategoryEnum.Hat)
			{
				return HatID;
			}
			else if (featureCategoryEnum == CharacterFeatureCategoryEnum.AccessoryHand)
			{
				return AccessoryInHandID;
			}
			else if (featureCategoryEnum == CharacterFeatureCategoryEnum.Clothes)
			{
				return ClothingID;
			}
			else if (featureCategoryEnum == CharacterFeatureCategoryEnum.Mask)
			{
				return MaskID;
			}
            else if (featureCategoryEnum == CharacterFeatureCategoryEnum.Skin)
            {
                return (int)Age;
            }
            else
			{
				return 1;
			}
		}
	}


}
