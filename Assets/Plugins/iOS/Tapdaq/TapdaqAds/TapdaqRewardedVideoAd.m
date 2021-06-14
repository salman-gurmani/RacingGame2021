#import "TapdaqStandardAd.h"
#import "JsonHelper.h"

@implementation TapdaqRewardedVideoAd

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
    return @"rewarded_video_interstitial";
}

- (void)loadForPlacementTag:(NSString *)tag {
    [Tapdaq.sharedSession loadRewardedVideoForPlacementTag:tag delegate:self];
}

- (BOOL)isReadyForPlacementTag:(NSString *)tag {
    return [[Tapdaq sharedSession] isRewardedVideoReadyForPlacementTag:tag];
}

- (void)showForPlacementTag:(NSString *)tag {
    [[Tapdaq sharedSession] showRewardedVideoForPlacementTag:tag delegate:self];
}

- (void)showForPlacementTag:(NSString *)tag withHashedUserId:(NSString *)hashedUserId {
    [[Tapdaq sharedSession] showRewardedVideoForPlacementTag:tag hashedUserId:hashedUserId];
}
#pragma mark TDRewardedVideoAdRequestDelegate
- (void)didLoadInterstitialAdRequest:(TDInterstitialAdRequest *)adRequest {
    [self handleDidLoadAdRequest:adRequest];
}

- (void)adRequest:(TDAdRequest * _Nonnull)adRequest didValidateReward:(TDReward * _Nonnull)reward {
     [self handleDidVerifyReward:reward];
}

- (void)adRequest:(TDAdRequest * _Nonnull)adRequest didFailToValidateReward:(TDReward * _Nonnull)reward {
     [self handleDidVerifyReward:reward];
}

- (void)handleDidVerifyReward:(TDReward *)reward {
    NSMutableDictionary* dict = [[NSMutableDictionary alloc] init];
    
    dict[@"RewardName"] = reward.name;
    dict[@"RewardAmount"] = @(reward.value);
    dict[@"RewardValid"] = @(reward.isValid);
    dict[@"Tag"] = reward.tag;
    dict[@"EventId"] = reward.eventId;
    
    if (reward.customJson != nil) {
        dict[@"RewardJson"] = [JsonHelper toJsonString:reward.customJson];
    } else {
        dict[@"RewardJson"] = [JsonHelper toJsonString:[NSDictionary dictionary]];
    }
    [self send: @"_didVerify" dictionary: dict];
}
@end
