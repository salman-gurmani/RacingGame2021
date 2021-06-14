//
//  TapdaqUnityIOS.m
//  TapdaqUnity
//
//  Created by Rheo Violenes on 05/05/15.
//  Copyright (c) 2015 Nerd. All rights reserved.
//
#import "TapdaqUnityIOS.h"
#import "TapdaqDelegates.h"
#import "TapdaqStandardAd.h"
#import "JsonHelper.h"

static NSString *const kTapdaqLogPrefix = @"[TapdaqUnity]";

// Helper method to create C string copy
char* makeStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

void _ConfigureTapdaq(const char* appIdChar,
                      const char* clientKeyChar,
                      const char* testDevicesChar,
                      bool isDebugMode,
                      bool autoReloadAds,
                      const char* pluginVersion,
                      int isUserSubjectToGDPR,
                      int isConsentGiven,
                      int isAgeRestrictedUser,
                      const char* userIdChar,
                      bool forwardUserId) {
    
    bool isValid = true;
    
    if (_isEmpty(appIdChar)) {
        NSLog(@"%@ iOS App ID is empty", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (_isEmpty(clientKeyChar)) {
        NSLog(@"%@ iOS Client Key is empty", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (!isValid) {
        NSLog(@"%@ Tapdaq did not initialise", kTapdaqLogPrefix);
        return;
    }
    
    NSString *appId = [[NSString stringWithUTF8String:appIdChar] copy];
    NSString *clientKey = [[NSString stringWithUTF8String:clientKeyChar] copy];
    NSString *testDevices = [[NSString stringWithUTF8String:testDevicesChar] copy];
    NSString *version = [[NSString stringWithUTF8String:pluginVersion] copy];
    
    NSString *userId = nil;
    
    if (!_isEmpty(userIdChar)) {
        userId = [[NSString stringWithUTF8String:userIdChar] copy];
    }
    
    
    TDProperties *properties = [[Tapdaq sharedSession] properties];
    [properties setPluginVersion:version];
    properties.isDebugEnabled = isDebugMode;
    TDLogger.logLevel = isDebugMode ? TDLogLevelDebug : TDLogLevelInfo;
    [properties setAutoReloadAds:autoReloadAds];
    properties.userId = userId;
    properties.forwardUserId = forwardUserId;

    [properties.privacySettings setUserSubjectToGdpr:(TDPrivacyStatus)isUserSubjectToGDPR];
    [properties.privacySettings setGdprConsentGiven:(TDPrivacyStatus)isConsentGiven];
    [properties.privacySettings setAgeRestrictedUser:(TDPrivacyStatus)isAgeRestrictedUser];
    
    NSData *data = [testDevices dataUsingEncoding:NSUTF8StringEncoding];
    
    NSError *error = nil;
    NSDictionary *testDevicesDictionary = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
    
    if(error == nil) {
        NSArray* amArray = testDevicesDictionary[@"adMobDevices"];
        NSArray* fbArray = testDevicesDictionary[@"facebookDevices"];
        
        if(amArray != nil) {
            TDTestDevices *amTestDevices = [[TDTestDevices alloc] initWithNetwork:TDMAdMob testDevices:amArray];
            [properties registerTestDevices: amTestDevices];
        }
        
        if(fbArray != nil) {
            TDTestDevices *fbTestDevices = [[TDTestDevices alloc] initWithNetwork:TDMFacebookAudienceNetwork testDevices:fbArray];
            [properties registerTestDevices: fbTestDevices];
        }
    }
    
    [[Tapdaq sharedSession] setApplicationId:appId
                                   clientKey:clientKey
                                  properties:properties];
}

void _SetDelegate() {
    Tapdaq.sharedSession.delegate = TapdaqDelegates.sharedInstance;
}

bool _IsInitialised() {
    return Tapdaq.sharedSession.isInitialised;
}

void _SetUserSubjectToGDPR(int userSubjectToGDPR) {
    Tapdaq.sharedSession.properties.privacySettings.userSubjectToGdpr = (TDPrivacyStatus)userSubjectToGDPR;
}

int _UserSubjectToGDPR() {
    return (int)Tapdaq.sharedSession.properties.privacySettings.userSubjectToGdpr;
}

void _SetGdprConsent(int gdprConsent) {
    Tapdaq.sharedSession.properties.privacySettings.gdprConsentGiven = (TDPrivacyStatus)gdprConsent;
}

int _GdprConsent() {
    return (int)Tapdaq.sharedSession.properties.privacySettings.gdprConsentGiven;
}

void _SetAgeRestrictedUser(int ageRestrictedUser) {
    Tapdaq.sharedSession.properties.privacySettings.ageRestrictedUser = (TDPrivacyStatus)ageRestrictedUser;
}

int _AgeRestrictedUser() {
    return (int)Tapdaq.sharedSession.properties.privacySettings.ageRestrictedUser;
}

void _SetUserSubjectToUSPrivacy(int userSubjectToUSPrivacy) {
    Tapdaq.sharedSession.properties.privacySettings.userSubjectToUSPrivacy = (TDPrivacyStatus)userSubjectToUSPrivacy;
}

int _UserSubjectToUSPrivacy() {
    return (int)Tapdaq.sharedSession.properties.privacySettings.userSubjectToUSPrivacy;
}

void _SetUSPrivacy(int usPrivacy) {
    Tapdaq.sharedSession.properties.privacySettings.usPrivacyDoNotSell = (TDPrivacyStatus)usPrivacy;
}

int _USPrivacy() {
    return (int)Tapdaq.sharedSession.properties.privacySettings.usPrivacyDoNotSell;
}

void _SetAdMobContentRating(const char * rating) {
    NSString *ratingString = [NSString stringWithUTF8String:rating].copy;
    Tapdaq.sharedSession.properties.adMobContentRating = ratingString;
}

const char * _GetAdMobContentRating() {
    NSString * ratingStr = Tapdaq.sharedSession.properties.adMobContentRating;
    return makeStringCopy([ratingStr UTF8String]);
}

void _SetAdvertiserTracking(int status) {
    Tapdaq.sharedSession.properties.privacySettings.advertiserTracking = (TDPrivacyStatus)status;
}

int _AdvertiserTracking() {
    return (int)Tapdaq.sharedSession.properties.privacySettings.advertiserTracking;
}

void _SetUserId(const char * userId) {
    NSString *userIdStrting = nil;
    if (!_isEmpty(userId)) {
        userIdStrting = [[NSString stringWithUTF8String:userId] copy];
    }
    Tapdaq.sharedSession.properties.userId = userIdStrting;
}

const char * _GetUserId() {
    NSString * userIdStr = Tapdaq.sharedSession.properties.userId;
    return makeStringCopy([userIdStr UTF8String]);
}

void _SetForwardUserId(bool forwardUserId) {
    Tapdaq.sharedSession.properties.forwardUserId = forwardUserId;
}

bool _ShouldForwardUserId() {
    return (bool)Tapdaq.sharedSession.properties.forwardUserId;
}

void _SetMuted(bool muted) {
    Tapdaq.sharedSession.properties.muted = muted;
}

bool _IsMuted() {
    return (bool)Tapdaq.sharedSession.properties.isMuted;
}

void _SetUserDataString(const char* key, const char* value) {
    NSString *keyStr = nil;
    NSString *valueStr = nil;
    if (!_isEmpty(key) && !_isEmpty(value)) {
        keyStr = [NSString stringWithUTF8String:key];
        valueStr = [NSString stringWithUTF8String:value];
    }
    [Tapdaq.sharedSession.properties setUserDataString:valueStr forKey:keyStr];
}

void _SetUserDataInteger(const char* key, int value) {
    NSString *keyStr = nil;
    if (!_isEmpty(key)) {
        keyStr = [NSString stringWithUTF8String:key];
    }
    [Tapdaq.sharedSession.properties setUserDataInteger:value forKey:keyStr];
}

void _SetUserDataBoolean(const char* key, bool value) {
    NSString *keyStr = nil;
    if (!_isEmpty(key)) {
        keyStr = [NSString stringWithUTF8String:key];
    }
    [Tapdaq.sharedSession.properties setUserDataBool:value forKey:keyStr];
}

const char* _GetUserDataString(const char* key) {
    NSString *keyStr = nil;
    if (!_isEmpty(key)) {
        keyStr = [[NSString stringWithUTF8String:key] copy];
    }
    
    NSString * dataStr = [Tapdaq.sharedSession.properties userDataStringForKey:keyStr];;
    return makeStringCopy([dataStr UTF8String]);
}

int _GetUserDataInteger(const char* key) {
    NSString *keyStr = nil;
    if (!_isEmpty(key)) {
        keyStr = [[NSString stringWithUTF8String:key] copy];
    }
    
    return (int)[Tapdaq.sharedSession.properties userDataIntegerForKey:keyStr];
}

bool _GetUserDataBoolean(const char* key) {
    NSString *keyStr = nil;
    if (!_isEmpty(key)) {
        keyStr = [[NSString stringWithUTF8String:key] copy];
    }
    
    return (bool)[Tapdaq.sharedSession.properties userDataBoolForKey:keyStr];
}

const char* _GetAllUserData() {
    NSString * dataStr = [JsonHelper toJsonString:Tapdaq.sharedSession.properties.userData];
    return makeStringCopy([dataStr UTF8String]);
}

void _RemoveUserData(const char* key) {
    NSString *keyStr = nil;
    if (!_isEmpty(key)) {
        keyStr = [[NSString stringWithUTF8String:key] copy];
    }
    [Tapdaq.sharedSession.properties removeUserDataForKey:keyStr];
}

const char* _GetNetworkStatuses() {
    NSArray* networkStatuses = [[Tapdaq sharedSession] networkStatusesDictionary];
    if (networkStatuses != nil && [networkStatuses count] > 0) {
        NSString * dataStr = [JsonHelper arrayToJsonString:networkStatuses];
        return makeStringCopy([dataStr UTF8String]);
    }
    return "";
}

#pragma mark - Banner (Bridge)

void _LoadBannerForSize(const char* tagChar, const char* sizeChar) {
    
    if (_isEmpty(sizeChar)) {
        NSLog(@"%@ No banner size specified, cannot load banner", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    NSString *sizeStr = [[NSString stringWithUTF8String:sizeChar] copy];
    [[TapdaqBannerAd sharedInstance] loadForPlacementTag:tagStr withSize:sizeStr];
}

void _LoadBannerWithSize(const char* tagChar, int width, int height) {
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, cannot load banner", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    CGSize size = CGSizeMake(width, height);
    
    [[TapdaqBannerAd sharedInstance] loadForPlacementTag:tagStr withCustomSize:size];
}

bool _IsBannerReady(const char* tagChar) {
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    return (bool) [[TapdaqBannerAd sharedInstance] isReadyForPlacementTag:tagStr];
    
}

void _ShowBanner(const char* tagChar, const char* position) {
    
    if (_isEmpty(position)) {
        NSLog(@"%@ No banner position given, failed to show banner", kTapdaqLogPrefix);
        return;
    }
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    [[TapdaqBannerAd sharedInstance] showForPlacementTag:tagStr withPosition:position];
    
}

void _ShowBannerWithPosition(const char* tagChar, int x, int y) {
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show banner", kTapdaqLogPrefix);
        return;
    }
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    [[TapdaqBannerAd sharedInstance] showForPlacementTag:tagStr atPositionX:x atPositionY:y];
}

void _HideBanner(const char* tagChar) {
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    [[TapdaqBannerAd sharedInstance] hideForPlacementTag:tagStr];
}

void _DestroyBanner(const char* tagChar) {
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    [[TapdaqBannerAd sharedInstance] destroyForPlacementTag:tagStr];
}

#pragma mark - Interstitial (Bridge)

void _LoadInterstitialWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to load interstitial", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqInterstitialAd sharedInstance] loadForPlacementTag:tag];
    
}

bool _IsInterstitialReadyWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to check if interstitial is ready", kTapdaqLogPrefix);
        return false;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    return (bool) [[TapdaqInterstitialAd sharedInstance] isReadyForPlacementTag:tag];
    
}

const char *_GetInterstitialFrequencyCapError(const char* tag) {
    NSString *placementTag = [NSString stringWithUTF8String:tag];
    TDError* error = [[Tapdaq sharedSession] interstitialCappedForPlacementTag:placementTag];
    
    NSString* result = [JsonHelper toJsonString:@{
        @"code": @(error.code),
        @"message": (error.localizedDescription == nil ? @"" : error.localizedDescription)
    }];
    
    return makeStringCopy([result UTF8String]);
}

void _ShowInterstitialWithTag(const char* tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show interstitial", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqInterstitialAd sharedInstance] showForPlacementTag:tag];
    
}

#pragma mark - Video (Bridge)

void _LoadVideoWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to load video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqVideoAd sharedInstance] loadForPlacementTag:tag];
    
}

bool _IsVideoReadyWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to check if video is ready", kTapdaqLogPrefix);
        return false;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    return (bool) [[TapdaqVideoAd sharedInstance] isReadyForPlacementTag:tag];
    
}

const char *_GetVideoFrequencyCapError(const char* tag) {
    NSString *placementTag = [NSString stringWithUTF8String:tag];
    TDError* error = [[Tapdaq sharedSession] videoCappedForPlacementTag:placementTag];
    
    NSString* result = [JsonHelper toJsonString:@{
        @"code": @(error.code),
        @"message": (error.localizedDescription == nil ? @"" : error.localizedDescription)
    }];
    
    return makeStringCopy([result UTF8String]);
}

void _ShowVideoWithTag(const char* tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqVideoAd sharedInstance] showForPlacementTag:tag];
    
}

#pragma mark - Rewarded Video (Bridge)

void _LoadRewardedVideoWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to load rewarded video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqRewardedVideoAd sharedInstance] loadForPlacementTag:tag];
    
}

bool _IsRewardedVideoReadyWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to check if rewarded video is ready", kTapdaqLogPrefix);
        return false;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    return (bool) [[TapdaqRewardedVideoAd sharedInstance] isReadyForPlacementTag:tag];
    
}

const char *_GetRewardedVideoFrequencyCapError(const char* tag) {
    NSString *placementTag = [NSString stringWithUTF8String:tag];
    TDError* error = [[Tapdaq sharedSession] rewardedVideoCappedForPlacementTag:placementTag];
    
    NSString* result = [JsonHelper toJsonString:@{
        @"code": @(error.code),
        @"message": (error.localizedDescription == nil ? @"" : error.localizedDescription)
    }];
    
    return makeStringCopy([result UTF8String]);
}

void _ShowRewardedVideoWithTag(const char* tagChar, const char* hashedUserIdChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show rewarded video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    NSString *hashedUserId = (hashedUserIdChar != nil ? [NSString stringWithUTF8String:hashedUserIdChar] : nil);
    
    [[TapdaqRewardedVideoAd sharedInstance] showForPlacementTag:tag withHashedUserId:hashedUserId];
    
}

#pragma mart - Stats

void _SendIAP(const char* transationId, const char* productId, const char* name, double price, const char* currency, const char* locale) {
    NSString *transactionIdString;
    NSString *productIdString;
    NSString *currencyString;
    NSString *nameString;
    NSString *localeString;
    
    if (transationId != NULL) {
        transactionIdString = [NSString stringWithUTF8String:transationId];
    }
    
    if (productId != NULL) {
        productIdString = [NSString stringWithUTF8String:productId];
    }
    
    if (currency != NULL) {
        currencyString = [NSString stringWithUTF8String:currency];
    }
    
    if (name != NULL) {
        nameString = [NSString stringWithUTF8String:name];
    }
    
    if (locale != NULL) {
        localeString = [NSString stringWithUTF8String:locale];
    }
    
    [[Tapdaq sharedSession] trackPurchaseForProductName:nameString
                                                  price:price
                                            priceLocale:localeString
                                               currency:currencyString
                                          transactionId:transactionIdString
                                              productId:productIdString];
}

const char *_GetRewardId(const char* tag) {
    NSString *placementTag = [NSString stringWithUTF8String:tag];
    NSString *rewardId = [[Tapdaq sharedSession] rewardIdForPlacementTag:placementTag];
    return [rewardId cStringUsingEncoding:NSUTF8StringEncoding];
}

void _LaunchMediationDebugger() {
    [[Tapdaq sharedSession] presentDebugViewController];
}

bool _isEmpty(const char* str) {
    return str == NULL;
}
