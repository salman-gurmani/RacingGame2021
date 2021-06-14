//
//  TapdaqUnityIOS.h
//  TapdaqUnity
//
//  Created by Rheo Violenes on 05/05/15.
//  Copyright (c) 2015 Nerd. All rights reserved.
//
#import <Foundation/Foundation.h>
#import <Tapdaq/Tapdaq.h>

@interface TDUnityDelegateBase : NSObject

+ (instancetype)sharedInstance;

- (NSDictionary *)errorDictWithError:(NSError *)error;
- (void) send:(NSString *) methodName adType:(NSString *) adType tag:(NSString *) tag message: (NSString *) message;
- (void) send:(NSString *) methodName adType:(NSString *) adType tag:(NSString *) tag message: (NSString *) message error:(NSError *)error;
- (void) send:(NSString *) methodName error:(NSError *)error;
- (void) send:(NSString *) methodName dictionary:(NSDictionary *) dictionary;
- (void) send:(NSString *) methodName message:(NSString *) message;
@end
