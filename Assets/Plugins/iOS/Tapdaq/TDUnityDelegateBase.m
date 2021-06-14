#import "TDUnityDelegateBase.h"
#import "JsonHelper.h"

@implementation TDUnityDelegateBase

+ (instancetype)sharedInstance
{
    static dispatch_once_t once;
    static id sharedInstance;
    dispatch_once(&once, ^{
        sharedInstance = [[self alloc] init];
    });
    return sharedInstance;
}

- (NSDictionary *)errorDictWithError:(NSError *)error {
    if (error != nil) {
        NSMutableDictionary *subErrorDicts = [NSMutableDictionary dictionary];
        if ([error isKindOfClass:TDError.class]) {
            for (NSString *networkName in [(TDError *)error subErrors].allKeys) {
                NSArray *errors = [(TDError *)error subErrors][networkName];
                NSMutableArray *networkErrors = NSMutableArray.array;
                
                for (NSError *networkError in errors) {
                    NSDictionary *dict = @{
                                           @"code": @(networkError.code),
                                           @"message": (networkError.localizedDescription == nil ? @"" : networkError.localizedDescription)
                                           };
                    [networkErrors addObject:dict];
                }
                subErrorDicts[networkName] = networkErrors;
            }
        }
        return @{
                 @"code": @(error.code),
                 @"message": (error.localizedDescription == nil ? @"" : error.localizedDescription),
                 @"subErrors": subErrorDicts
                 };
    }
    return nil;
}

- (void) send:(NSString *) methodName adType:(NSString *) adType tag:(NSString *) tag message: (NSString *) message
{
    [self send:methodName adType:adType tag:tag message:message error:nil];
}

- (void) send:(NSString *) methodName adType:(NSString *) adType tag:(NSString *) tag message: (NSString *) message error:(NSError *)error
{
    NSMutableDictionary* dict = [@{
                                   @"adType": adType,
                                   @"tag": tag,
                                   @"message": message
                                   } mutableCopy];
    if (error != nil) {
        dict[@"error"] = [self errorDictWithError:error];
    }
    [self send: methodName dictionary: dict];
}

- (void) send:(NSString *) methodName error:(NSError *)error
{
    NSDictionary * errorDict = [self errorDictWithError:error];
    [self send: methodName dictionary: errorDict != nil ? errorDict : @{}];
}

- (void) send:(NSString *) methodName dictionary:(NSDictionary *) dictionary
{
    [self send: methodName message: [JsonHelper toJsonString: dictionary]];
}

- (void) send:(NSString *) methodName message:(NSString *) message
{
    UnitySendMessage("TapdaqV1", [methodName UTF8String], [message UTF8String]);
}
@end

