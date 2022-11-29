import { Inject,Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { environment } from '../environments/environment';
import { TwitterMetricsDto } from '../models/twitterMetricsDto';
import { catchError as _observableCatch, mergeMap as _observableMergeMap } from 'rxjs/operators';
import { Observable, of as _observableOf, throwError } from 'rxjs';
@Injectable({
    providedIn: 'root'
})
export class TweetMetricsService {
    constructor(@Inject(HttpClient) protected http: HttpClient) {
}
    public getMetrics(): Observable<TwitterMetricsDto> {
        let requestUrl = environment.ApiBase+'/twitterMetrics';
        const defaultRequestOptions: any = {
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Accept': 'application/json',
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': 'GET,OPTIONS',
                'Access-Control-Allow-Headers': 'Content-Type'
            })
        }
        
        return this.http.request('GET', requestUrl, defaultRequestOptions).pipe(_observableMergeMap((response_: any) => {
            return this.processResponse(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processResponse(<any>response_);
                } catch (e) {
                    return <Observable<TwitterMetricsDto>><any>throwError(e);
                }
            } else {
                return <Observable<TwitterMetricsDto>><any>throwError(response_);
            }
        }));
    }


    protected processResponse(response: HttpResponseBase): Observable<TwitterMetricsDto> {
        const status = response.status;
        const responseBlob = response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        if (status === 200) {
            return this.blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : JSON.parse(_responseText);
                result200 = resultData200 ?  TwitterMetricsDto.fromJS(resultData200) : <any>null;
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return this.blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                throw new Error('An unexpected server error occurred.'); 
            }));
        }
        return _observableOf<TwitterMetricsDto>(<any>null);
    }

    private blobToText(blob: any): Observable<string> {
        return new Observable<string>((observer: any) => {
            if (!blob) {
                observer.next('');
                observer.complete();
            } else {
                const reader = new FileReader();
                reader.onload = function () {
                    observer.next(this.result);
                    observer.complete();
                };
                reader.readAsText(blob);
            }
        });
    }

     

}

 
