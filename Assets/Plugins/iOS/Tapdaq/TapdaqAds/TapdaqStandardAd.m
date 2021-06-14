#import "TapdaqStandardAd.h"

@implementation TapdaqStandardAd

- (NSString*) type {
    return @"unknown";
}

- (BOOL) isReady { return false; }
- (BOOL) isReadyForPlacementTag:(NSString*)tag { return false; }
- (void) load {}
- (void) loadForPlacementTag:(NSString *)tag {}
- (void) show {}
- (void) showForPlacementTag:(NSString *)tag {}
- (void) showForPlacementTag:(NSString *)tag withHashedUserId:(NSString *)hashedUserId {}

- (void)handleDidLoadAdRequest:(TDAdRequest *)adRequest {
    [self send: @"_didLoad" adType: [self type] tag: [[adRequest placement] tag] message: @""];
}

#pragma mark - TDAdRequestDelegate
- (void)adRequest:(TDAdRequest * _Nonnull)adRequest didFailToLoadWithError:(TDError * _Nullable)error {
    [self send: @"_didFailToLoad" adType: [self type] tag: [[adRequest placement] tag] message: @"" error:error];
}

- (void)adRequest:(TDAdRequest * _Nonnull)adRequest didFailToDisplayWithError:(TDError * _Nullable)error {
    [self send: @"_didFailToDisplay" adType: [self type] tag: [[adRequest placement] tag] message: @"" error:error];
}

- (void)willDisplayAdRequest:(TDAdRequest * _Nonnull)adRequest {
    [self send: @"_willDisplay" adType: [self type] tag: [[adRequest placement] tag] message: @""];
}

- (void)didDisplayAdRequest:(TDAdRequest * _Nonnull)adRequest {
    [self send: @"_didDisplay" adType: [self type] tag: [[adRequest placement] tag] message: @""];
}

- (void)didCloseAdRequest:(TDAdRequest * _Nonnull)adRequest {
    [self send: @"_didClose" adType: [self type] tag: [[adRequest placement] tag] message: @""];
}

- (void)didClickAdRequest:(TDAdRequest * _Nonnull)adRequest {
    [self send: @"_didClick" adType: [self type] tag: [[adRequest placement] tag] message: @""];
}

@end
