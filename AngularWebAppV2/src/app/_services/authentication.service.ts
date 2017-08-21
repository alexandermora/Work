import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
// tslint:disable-next-line:import-blacklist
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
import { GlobalVarService } from './global-var.service';

@Injectable()
export class AuthenticationService
{
    credential: string;
    public token: string;
    public username: string;
    constructor(private http: Http, private globalVarService: GlobalVarService)
    {   // set token if saved in local storage
        const currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.token = currentUser && currentUser.token;
        this.username = currentUser && currentUser.username;
    }
    login(username: string, password: string, country: string, group: string): Observable<boolean>
    {   // username = btoa(username); password = btoa(password);
        const credential = new URLSearchParams();
        credential.set('Username', username);
        credential.set('Password', password);
        if (username === 'admin')
        {
            country = null;
            group = null;
        }
        else
        {   // country = btoa(country); group = btoa(group);
            credential.set('Group', group);
            credential.set('Country', country);
        }
        const body = credential.toString();
        const headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded');
        const options = new RequestOptions({ headers: headers });
        return this.http.post(this.globalVarService.urlRoot + '/api/jwt', body, options).map((response: Response) =>
        {
            // login successful if there's a jwt token in the response
            const token = response.json() && response.json().access_token;
            if (token)
            {
                // set token property
                this.token = token;
                // store username and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify({ username: username, token: token }));
                // return true to indicate successful login
                return true;
            }
            else
            {
                // return false to indicate failed login
                return false;
            }
        });
    }
    logout(): void
    {
        // clear token remove user from local storage to log user out
        this.token = null;
        localStorage.removeItem('currentUser');
        this.globalVarService.username = null;
    }
}
