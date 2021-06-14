//
//  TapdaqUnityIOS.h
//  TapdaqUnity
//
//  Created by Rheo Violenes on 05/05/15.
//  Copyright (c) 2015 Nerd. All rights reserved.
//
#import <Foundation/Foundation.h>
#import <Tapdaq/Tapdaq.h>

void _LaunchMediationDebugger();

void _ConfigureTapdaq(const char* appIdChar,
                      const char* clientKeyChar,
                      const char* testDevices,
                      bool isDebugMode,
                      bool autoReloadAds,
                      const char* pluginVersion,
                      int isUserSubjectToGDPR,
                      int isConsentGiven,
                      int isAgeRestrictedUser,
                      const char* userIdChar,
                      bool forwardUserId);

void _SetDelegate();

bool _IsInitialised();

void _SetUserSubjectToGDPR(int userSubjectToGDPR);
int _UserSubjectToGDPR();

void _SetGdprConsent(int gdprConsent);
int _GdprConsent();

void _SetAgeRestrictedUser(int ageRestrictedUser);
int _AgeRestrictedUser();

void _SetUserSubjectToUSPrivacy(int userSubjectToUSPrivacy);
int _UserSubjectToUSPrivacy();

void _SetUSPrivacy(int usPrivacy);
int _USPrivacy();

void _SetAdMobContentRating(const char* rating);
const char* _GetAdMobContentRating();

void _SetAdvertiserTracking(int status);
int _AdvertiserTracking();

void _SetUserId(const char* userId);
const char* _GetUserId();

void _SetForwardUserId(bool forwardUserId);
bool _ShouldForwardUserId();

void _SetMuted(bool muted);
bool _IsMuted();

void _SetUserDataString(const char* key, const char* value);
void _SetUserDataInteger(const char* key, int value);
void _SetUserDataBoolean(const char* key, bool value);

const char* _GetUserDataString(const char* key);
int _GetUserDataInteger(const char* key);
bool _GetUserDataBoolean(const char* key);

const char* _GetAllUserData();

void _RemoveUserData(const char* key);

const char * _GetNetworkStatuses();
// banner

/**
 * Loads a banner.
 * @param size A string. Must be one of following values: TDMBannerStandard, TDMBannerLarge, TDMBannerMedium, TDMBannerFull, TDMBannerLeaderboard, TDMBannerSmartPortrait, TDMBannerSmartLandscape
 */
void _LoadBannerForSize(const char* tagChar, const char* sizeChar);

void _LoadBannerWithSize(const char* tagChar, int width, int height);

bool _IsBannerReady(const char* tagChar);

void _ShowBanner(const char* tagChar, const char* position);

void _ShowBannerWithPosition(const char* tagChar, int x, int y);

void _HideBanner(const char* tagChar);

void _DestroyBanner(const char* tagChar);

// interstitial

void _LoadInterstitialWithTag(const char *tagChar);

bool _IsInterstitialReadyWithTag(const char *tagChar);

const char * _GetInterstitialFrequencyCapError(const char *tagChar);

void _ShowInterstitialWithTag(const char* tagChar);

// video

void _LoadVideoWithTag(const char *tagChar);

bool _IsVideoReadyWithTag(const char *tagChar);

const char * _GetVideoFrequencyCapError(const char *tagChar);

void _ShowVideoWithTag(const char* tagChar);

// reward video

void _LoadRewardedVideoWithTag(const char *tagChar);

bool _IsRewardedVideoReadyWithTag(const char *tagChar);

const char * _GetRewardedVideoFrequencyCapError(const char *tagChar);

void _ShowRewardedVideoWithTag(const char* tagChar, const char* hashedUserIdChar);

void _LaunchMediationDebugger();

bool _isEmpty(const char *str);

// stats

void _SendIAP(const char* transationId, const char* productId, const char* name, double price, const char* currency, const char* locale);

// rewards

const char *_GetRewardId(const char* tag);

@interface TapdaqUnityIOS : NSObject

+ (instancetype)sharedInstance;

- (void)initWithApplicationId:(NSString *)appID
                    clientKey:(NSString *)clientKey
                  testDevices:(NSString *)testDevices
                  isDebugMode:(bool)isDebugMode
                autoReloadAds:(bool)autoReloadAds
                pluginVersion:(NSString *)pluginVersion
          isUserSubjectToGDPR:(int)isUserSubjectToGDPR
               isConsentGiven:(int)isConsentGiven
          isAgeRestrictedUser:(int)isAgeRestrictedUser
                       userId:(NSString*)userId
          shouldForwardUserId:(bool)forwardUserId;
    
- (void) setDelegate;

-(BOOL) IsInitialised;

- (void) setUserSubjectToGDPR:(int)userSubjectToGDPR;

- (int) userSubjectToGDPR;

- (void) setGdprConsent:(int)gdprConsent;

- (int) gdprConsent;

- (void) setAgeRestrictedUser:(int)ageRestrictedUser;

- (int) ageRestrictedUser;

- (void) setUserSubjectToUSPrivacy:(int)userSubjectToUSPrivacy;

- (int) userSubjectToUSPrivacy;

- (void) setUSPrivacy:(int)usPrivacy;

- (int) usPrivacy;

- (void) setAdMobContentRating:(NSString *)rating;

- (NSString *) getAdMobContentRating;

- (void) setUserId:(NSString *)userId;

- (NSString *) getUserId;

- (void) setForwardUserId:(BOOL)forwardUserId;

- (BOOL) shouldForwardUserId;

- (void) setMuted:(BOOL)muted;

- (BOOL) isMuted;

- (void) setUserDataString:(NSString*)value forKey:(NSString*)key;
    
- (void) setUserDataBoolean:(BOOL)value forKey:(NSString*)key;

- (void) setUserDataInteger:(NSInteger)value forKey:(NSString*)key;

- (NSString*) userDataStringForKey:(NSString*) key;

- (NSInteger) userDataIntegerForKey:(NSString*) key;

- (BOOL) userDataBooleanForKey:(NSString*) key;

- (NSString*) userData;

- (void) removeUserDataForKey:(NSString*) key;
@end
