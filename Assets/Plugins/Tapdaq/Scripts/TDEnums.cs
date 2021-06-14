namespace Tapdaq {
	public enum TDAdType {
        TDAdTypeNone,
		TDAdTypeInterstitial,
		TDAdTypeVideo,
		TDAdTypeRewardedVideo,
		TDAdTypeBanner
	}

	public enum TDMBannerSize {
		TDMBannerStandard = 0,
		TDMBannerLarge = 1,
		TDMBannerMedium = 2,
		TDMBannerFull = 3,
		TDMBannerLeaderboard = 4,
		TDMBannerSmart = 5,
        TDMBannerCustom = 6
	}

	public enum TDOrientation {
		portrait = 0,
		landscape = 1,
		universal = 2
	}

	public enum TDLogSeverity {
		debug = 0,
		warning = 1,
		error = 2
	}

	public enum TDBannerPosition {
		Top = 0,
		Bottom = 1,
        Custom = 2
	}

	public enum TDStatus {
		FALSE = 0,
		TRUE = 1,
		UNKNOWN = 2
	}
}

