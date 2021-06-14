#import <Foundation/Foundation.h>
#import <Tapdaq/Tapdaq.h>
#import "TDUnityDelegateBase.h"

@interface TapdaqStandardAd : TDUnityDelegateBase <TDAdRequestDelegate>

- (NSString*) type;
- (BOOL)isReady;
- (void)show;
- (void)load;

- (void)loadForPlacementTag:(NSString *)tag;
- (BOOL)isReadyForPlacementTag:(NSString *)tag;
- (void)showForPlacementTag:(NSString *)tag;
- (void)showForPlacementTag:(NSString *) tag withHashedUserId:(NSString *) hashedUserId;
- (void)handleDidLoadAdRequest:(TDAdRequest *)adRequest;
@end

@interface TapdaqVideoAd : TapdaqStandardAd <TDInterstitialAdRequestDelegate>
@end

@interface TapdaqRewardedVideoAd : TapdaqStandardAd<TDRewardedVideoAdRequestDelegate>
@end

@interface TapdaqInterstitialAd : TapdaqStandardAd <TDInterstitialAdRequestDelegate>
@end

@interface TapdaqBannerAd : TapdaqStandardAd<TDBannerAdRequestDelegate>

- (void)loadForPlacementTag:(NSString*)tag withSize:(NSString *)sizeStr;
- (void)loadForPlacementTag:(NSString*)tag withCustomSize:(CGSize)size;
- (void)hideForPlacementTag:(NSString*)tag;
- (void)destroyForPlacementTag:(NSString*)tag;
- (void)showForPlacementTag:(NSString*)tag withPosition:(const char *)position;
- (void)showForPlacementTag:(NSString*)tag atPositionX:(int)x atPositionY:(int)y;

@end
