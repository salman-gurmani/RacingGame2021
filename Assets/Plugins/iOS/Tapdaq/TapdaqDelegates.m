#import "TapdaqDelegates.h"

@implementation TapdaqDelegates

+ (instancetype)sharedInstance
{
    static dispatch_once_t once;
    static id sharedInstance;
    dispatch_once(&once, ^{
        sharedInstance = [[self alloc] init];
    });
    return sharedInstance;
}

#pragma mark - TapdaqDelegate

- (void)didLoadConfig
{
    [self send:@"_didLoadConfig" message:@""];
}

- (void)didFailToLoadConfigWithError:(TDError *)error
{
    [self send:@"_didFailToLoadConfig" error:error];
}

- (void)didFailToDisplayRewardedVideoForPlacementTag:(NSString *)placementTag withError:(NSError *)error
{
    [self send: @"_didFailToDisplay" adType: @"rewarded_video_interstitial" tag: placementTag message: @"" error:error];
}
@end

