//
//  TapdaqUnityIOS.h
//  TapdaqUnity
//
//  Created by Rheo Violenes on 05/05/15.
//  Copyright (c) 2015 Nerd. All rights reserved.
//
#import <Foundation/Foundation.h>
#import <Tapdaq/Tapdaq.h>
#import "TDUnityDelegateBase.h"

@interface TapdaqDelegates : TDUnityDelegateBase <TapdaqDelegate>
+ (instancetype)sharedInstance;
@end
