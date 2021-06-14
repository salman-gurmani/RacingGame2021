#import "TapdaqStandardAd.h"

@implementation TapdaqVideoAd

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
    return @"video_interstitial";
}

- (void)loadForPlacementTag:(NSString *)tag {
    [Tapdaq.sharedSession loadVideoForPlacementTag:tag delegate:self];
}

- (BOOL)isReadyForPlacementTag:(NSString *)tag {
    return [[Tapdaq sharedSession] isVideoReadyForPlacementTag:tag];
}

- (void)showForPlacementTag:(NSString *)tag {
    [[Tapdaq sharedSession] showVideoForPlacementTag:tag delegate:self];
}

- (void)didLoadInterstitialAdRequest:(TDInterstitialAdRequest *)adRequest {
    [self handleDidLoadAdRequest:adRequest];
}
@end
