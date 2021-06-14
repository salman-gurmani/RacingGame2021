#import "TapdaqStandardAd.h"

@implementation TapdaqInterstitialAd

+ (instancetype)sharedInstance
{
    static dispatch_once_t once;
    static id sharedInstance;
    dispatch_once(&once, ^{
        sharedInstance = [[self alloc] init];
    });
    return sharedInstance;
}

- (NSString*) type {
    return @"static_interstitial";
}

- (void)loadForPlacementTag:(NSString *)tag
{
    [Tapdaq.sharedSession loadInterstitialForPlacementTag:tag delegate:self];
}

- (BOOL)isReadyForPlacementTag:(NSString *)tag
{
    return [[Tapdaq sharedSession] isInterstitialReadyForPlacementTag:tag];
}

- (void)showForPlacementTag:(NSString *)tag
{
    [[Tapdaq sharedSession] showInterstitialForPlacementTag:tag delegate:self];
}

- (void)didLoadInterstitialAdRequest:(TDInterstitialAdRequest *)adRequest {
    [self handleDidLoadAdRequest:adRequest];
}
@end
