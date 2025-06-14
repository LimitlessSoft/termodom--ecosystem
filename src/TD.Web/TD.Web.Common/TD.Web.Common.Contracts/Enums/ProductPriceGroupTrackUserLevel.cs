namespace TD.Web.Common.Contracts.Enums;

/// <summary>
/// Used to tell if given group should alert admin if user has the lowest prices on it.
/// Based on value here, FE will color user different colors.
/// </summary>
public enum ProductPriceGroupTrackUserLevel
{
	Track,
	DoNotTrack,
	LowLevelTrack
}
