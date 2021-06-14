#import <Foundation/Foundation.h>
#import <Tapdaq/Tapdaq.h>

@interface JsonHelper : NSObject

+ (NSString *) toJsonString:(NSDictionary *) dict;
+ (NSString *) arrayToJsonString:(NSArray *) array;
+ (NSDictionary *) fromJsonString:(NSString *) json;

@end
