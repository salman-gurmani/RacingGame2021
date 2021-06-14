#import "TapdaqStandardAd.h"
#import <Tapdaq/Tapdaq+PluginSupport.h>

extern UIViewController *UnityGetGLViewController();
extern UIView *UnityGetGLView();

static NSString *const kTDUnityBannerStandard = @"TDMBannerStandard";
static NSString *const kTDUnityBannerLarge = @"TDMBannerLarge";
static NSString *const kTDUnityBannerMedium = @"TDMBannerMedium";
static NSString *const kTDUnityBannerFull = @"TDMBannerFull";
static NSString *const kTDUnityBannerLeaderboard = @"TDMBannerLeaderboard";
static NSString *const kTDUnityBannerSmart = @"TDMBannerSmart";

#pragma mark - Banner Native
@interface TapdaqBannerAd ()
@end

@implementation TapdaqBannerAd

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
    return @"banner";
}

- (BOOL)isReadyForPlacementTag:(NSString *)tag {
    return [Tapdaq.sharedSession isBannerReadyBannerForPlacementTag:tag];
}

- (void)loadForPlacementTag:(NSString*)tag withSize:(NSString *)sizeStr {
    TDMBannerSize bannerSize = [self bannerSizeFromString:sizeStr];
    [Tapdaq.sharedSession loadPluginBannerForPlacementTag:tag withSize:bannerSize delegate:self];
}

- (void)loadForPlacementTag:(NSString*)tag withCustomSize:(CGSize)size {
    [Tapdaq.sharedSession loadPluginBannerForPlacementTag:tag withTargetSize:size delegate:self];
}

- (void)showForPlacementTag:(NSString*)tag withPosition:(const char *)position {
    NSString * posString = [NSString stringWithUTF8String: position];
    TDBannerPosition p;
    if([posString isEqualToString:@"Top"]) {
        p = TDBannerPositionMake(TDBannerPositionTypeTop);
    } else {
        p = TDBannerPositionMake(TDBannerPositionTypeBottom);
    }
    
    UIView *unityView = UnityGetGLView();
    [Tapdaq.sharedSession showBannerForPlacementTag:tag atPosition:p inView:unityView];
}

- (void)showForPlacementTag:(NSString*)tag atPositionX:(int)x atPositionY:(int)y {
    TDBannerPosition position = TDBannerPositionMakeCustom(CGPointMake(x,y));
    
    UIView *unityView = UnityGetGLView();
    [Tapdaq.sharedSession showBannerForPlacementTag:tag atPosition:position inView:unityView];
}

- (void)hideForPlacementTag:(NSString*)tag {
    [Tapdaq.sharedSession hideBannerForPlacementTag:tag];
}

- (void)destroyForPlacementTag:(NSString*)tag {
    [Tapdaq.sharedSession destroyBannerForPlacementTag:tag];
}

- (TDMBannerSize)bannerSizeFromString:(NSString *)sizeStr
{
    TDMBannerSize bannerSize = TDMBannerStandard;
    
    if ([sizeStr isEqualToString:kTDUnityBannerStandard]) {
        bannerSize = TDMBannerStandard;
    } else if ([sizeStr isEqualToString:kTDUnityBannerLarge]) {
        bannerSize = TDMBannerLarge;
    } else if ([sizeStr isEqualToString:kTDUnityBannerMedium]) {
        bannerSize = TDMBannerMedium;
    } else if ([sizeStr isEqualToString:kTDUnityBannerFull]) {
        bannerSize = TDMBannerFull;
    } else if ([sizeStr isEqualToString:kTDUnityBannerLeaderboard]) {
        bannerSize = TDMBannerLeaderboard;
    } else if ([sizeStr isEqualToString:kTDUnityBannerSmart]) {
        bannerSize = TDMBannerSmart;
    }
    
    return bannerSize;
}

- (void)didLoadBannerAdRequest:(TDBannerAdRequest *)adRequest {
    [self handleDidLoadAdRequest:adRequest];
}

- (void)didRefreshBannerForAdRequest:(TDBannerAdRequest *)adRequest {
    [self send: @"_didRefresh" adType: [self type] tag: [[adRequest placement] tag] message: @""];
}

- (void)didFailToRefreshBannerForAdRequest:(TDBannerAdRequest * _Nonnull)adRequest withError:(TDError * _Nullable)error {
    [self send: @"_didFailToRefresh" adType: [self type] tag: [[adRequest placement] tag] message: @"" error:error];
}

@end

