
export class TwitterMetricsDto {
    totalTweets: number = 0;
    topHashtags: string[] = [];
    init(_data?: any) {        
        if (_data) {
            this.totalTweets = _data["totalTweets"];
            this.topHashtags = _data["topHashtags"];
        }
    }

    static fromJS(data: any): TwitterMetricsDto {
        data = typeof data === 'object' ? data : {};
        let result = new TwitterMetricsDto();
        result.init(data);
        return result;
    }
}