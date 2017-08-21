import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, RequestOptionsArgs } from '@angular/http';
import * as stringDecoder from 'string_decoder';
import 'rxjs/add/operator/map';
import { MethodPay } from '../_models/method-pay';
import { AuthenticationService } from './authentication.service';
import { GlobalVarService } from './global-var.service';

@Injectable()
export class WebServiceClientService
{
    private baseUrl = this.globalVarService.urlRoot + '/api/';

    constructor(private http: Http, private authenticationService: AuthenticationService, private globalVarService: GlobalVarService)
    {

    }
    getListWitoutAuth(route: string)
    {
        const headers = new Headers({ 'Access-Control-Allow-Origin' : '*'});
        const options = new RequestOptions({ headers: headers });
        return this.http.get(this.baseUrl + route, options).map(res => res.json());
    }
    getWithAuth(route: string)
    {
        const headers = new Headers({ 'Authorization' : 'Bearer ' + this.authenticationService.token });
        headers.append('Access-Control-Allow-Origin', '*');
        headers.append('Content-Type', 'application/json');
        const options = new RequestOptions({ headers: headers });
        return this.http.get(`${this.baseUrl}${route}`, options).map(res => res.json());
    }
    uploadSupport(route: string, photo)
    {
        const headers = new Headers({ 'Authorization' : 'Bearer ' + this.authenticationService.token });
        headers.append('Access-Control-Allow-Origin', '*');
        const options = new RequestOptions({ headers: headers });
        const formData = new FormData();
        formData.append('file', photo);
        return this.http.post(`${this.baseUrl}${route}`, formData, options).map(res => res.json());
    }
    postData(route: string, body)
    {
        const headers = new Headers({ 'Authorization' : 'Bearer ' + this.authenticationService.token });
        headers.append('Access-Control-Allow-Origin', '*');
        headers.append('Content-Type', 'application/json');
        const options = new RequestOptions({ headers: headers });
        return this.http.post(`${this.baseUrl}${route}`, body, options).map(res => res.json());
    }
    putData(route: string, bodyNew)
    {
        const headers = new Headers({ 'Authorization' : 'Bearer ' + this.authenticationService.token });
        headers.append('Access-Control-Allow-Origin', '*');
        headers.append('Content-Type', 'application/json');
        const options = new RequestOptions({ headers: headers });
        return this.http.put(`${this.baseUrl}${route}`, bodyNew, options).map(res => res.json());
    }
    deleteData(route: string)
    {
        const headers = new Headers({ 'Authorization' : 'Bearer ' + this.authenticationService.token });
        headers.append('Access-Control-Allow-Origin', '*');
        headers.append('Content-Type', 'application/json');
        const options = new RequestOptions({ headers: headers });
        return this.http.delete(`${this.baseUrl}${route}`, options).map(res => res.json());
    }
}
